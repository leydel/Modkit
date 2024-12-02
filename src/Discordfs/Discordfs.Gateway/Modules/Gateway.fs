namespace Discordfs.Gateway.Modules

open Discordfs.Gateway.Types
open Discordfs.Types
open System
open System.Net.WebSockets
open System.Text.Json
open System.Threading
open System.Threading.Tasks

type ConnectionCloseBehaviour =
    | Resume of ResumeGatewayUrl: string option
    | Reconnect
    | Close

type GatewayLoopState = {
    SequenceId: int option
    Interval: int option
    Heartbeat: DateTime option
    HeartbeatAcked: bool
    ResumeGatewayUrl: string option
    SessionId: string option
}

module Gateway =
    let identify (payload: IdentifySendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.IDENTIFY, Data = payload) |> Json.serializeF
        )
        
    let resume (payload: ResumeSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.RESUME, Data = payload) |> Json.serializeF
        )

    let heartbeat (payload: HeartbeatSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.HEARTBEAT, Data = payload) |> Json.serializeF
        )

    let requestGuildMembers (payload: RequestGuildMembersSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS, Data = payload) |> Json.serializeF
        )

    let requestSoundboardSounds (payload: RequestSoundboardSoundsSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.REQUEST_SOUNDBOARD_SOUNDS, Data = payload) |> Json.serializeF
        )

    let updateVoiceState (payload: UpdateVoiceStateSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.VOICE_STATE_UPDATE, Data = payload) |> Json.serializeF
        )

    let updatePresence (payload: UpdatePresenceSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.PRESENCE_UPDATE, Data = payload) |> Json.serializeF
        )
    
    let shouldReconnect (code: GatewayCloseEventCode option) =
        match code with
        | Some GatewayCloseEventCode.UNKNOWN_ERROR -> true
        | Some GatewayCloseEventCode.UNKNOWN_OPCODE -> true
        | Some GatewayCloseEventCode.DECODE_ERROR -> true
        | Some GatewayCloseEventCode.NOT_AUTHENTICATED -> true
        | Some GatewayCloseEventCode.AUTHENTICATION_FAILED -> false
        | Some GatewayCloseEventCode.ALREADY_AUTHENTICATED -> true
        | Some GatewayCloseEventCode.INVALID_SEQ -> true
        | Some GatewayCloseEventCode.RATE_LIMITED -> true
        | Some GatewayCloseEventCode.SESSION_TIMED_OUT -> true
        | Some GatewayCloseEventCode.INVALID_SHARD -> false
        | Some GatewayCloseEventCode.SHARDING_REQUIRED -> false
        | Some GatewayCloseEventCode.INVALID_API_VERSION -> false
        | Some GatewayCloseEventCode.INVALID_INTENTS -> false
        | Some GatewayCloseEventCode.DISALLOWED_INTENTS -> false
        | None -> true
        | _ -> false

    let rec loop (state: GatewayLoopState) resumed (id: IdentifySendEvent) handler ws = task {
        let now = DateTime.UtcNow
        let freshHeartbeat = state.Interval |> Option.map (fun i -> now.AddMilliseconds(i))

        let timeout =
            match state.Heartbeat with
            | Some h -> Task.Delay (h.Subtract(now))
            | None -> Task.Delay Timeout.InfiniteTimeSpan

        let readNext =
            Websocket.readNext ws
            ?> fun res ->
                match res with
                | WebsocketReadResponse.Close code -> Error (Option.map enum<GatewayCloseEventCode> code)
                | WebsocketReadResponse.Message message -> Ok (Json.deserializeF<GatewayReceiveEvent> message)

        let! winner = Task.WhenAny(timeout, readNext)

        if winner = timeout then
            match state.HeartbeatAcked with
            | false ->
                return ConnectionCloseBehaviour.Resume state.ResumeGatewayUrl
            | true ->
                ws |> heartbeat state.SequenceId |> ignore
                return! loop { state with HeartbeatAcked = false } resumed id handler ws
        else
            match readNext.Result with
            | Error code ->
                match shouldReconnect code with
                | true -> return ConnectionCloseBehaviour.Resume state.ResumeGatewayUrl
                | false -> return ConnectionCloseBehaviour.Close

            | Ok (GatewayReceiveEvent.HELLO event) ->
                match resumed, state.SessionId, state.SequenceId with
                | true, Some ses, Some seq -> resume (ResumeSendEvent.build(id.Token, ses, seq))
                | _ -> identify id
                <| ws |> ignore

                let newState = { state with Interval = Some event.Data.HeartbeatInterval }
                return! loop newState resumed id handler ws

            | Ok (GatewayReceiveEvent.HEARTBEAT _) ->
                ws |> heartbeat state.SequenceId |> ignore

                return! loop { state with Heartbeat = freshHeartbeat } resumed id handler ws

            | Ok (GatewayReceiveEvent.HEARTBEAT_ACK _) ->
                return! loop { state with HeartbeatAcked = true } resumed id handler ws

            | Ok (GatewayReceiveEvent.READY event) ->
                let resumeGatewayUrl = Some event.Data.ResumeGatewayUrl
                let sessionId = Some event.Data.SessionId

                let newState = { state with ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId }
                return! loop newState resumed id handler ws

            | Ok (GatewayReceiveEvent.RESUMED _) ->
                return! loop state resumed id handler ws

            | Ok (GatewayReceiveEvent.RECONNECT _) ->
                return ConnectionCloseBehaviour.Resume state.ResumeGatewayUrl

            | Ok (GatewayReceiveEvent.INVALID_SESSION event) ->
                match event.Data with
                | true -> return! loop state resumed id handler ws
                | false -> return ConnectionCloseBehaviour.Reconnect

            | Ok event ->
                handler event |> ignore

                match GatewayReceiveEvent.getSequenceNumber event with
                | None -> return! loop state resumed id handler ws
                | Some s -> return! loop { state with SequenceId = Some s } resumed id handler ws
    }

    let rec reconnect (cachedUrl: string) (resumeGatewayUrl: string option) identify handler (ws: ClientWebSocket option ref) = task {
        let gatewayUrl = Uri (Option.defaultValue cachedUrl resumeGatewayUrl)
        let resumed = resumeGatewayUrl.IsSome

        let socket = new ClientWebSocket()
        ws.Value <- Some socket
        do! socket.ConnectAsync(gatewayUrl, CancellationToken.None)

        let initialState = {
            SequenceId = None;
            Interval = None;
            Heartbeat = None;
            HeartbeatAcked = true;
            ResumeGatewayUrl = None;
            SessionId = None;
        }

        let! close = loop initialState resumed identify handler socket

        match close with
        | ConnectionCloseBehaviour.Resume (Some resumeGatewayUrl) ->
            do! socket.CloseAsync(WebSocketCloseStatus.Empty, "Resuming", CancellationToken.None)
            return! reconnect cachedUrl (Some resumeGatewayUrl) identify handler ws

        | ConnectionCloseBehaviour.Resume None
        | ConnectionCloseBehaviour.Reconnect ->
            do! socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Reconnecting", CancellationToken.None)
            return! reconnect cachedUrl None identify handler ws

        | ConnectionCloseBehaviour.Close ->
            do! socket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Closing", CancellationToken.None)
    }
