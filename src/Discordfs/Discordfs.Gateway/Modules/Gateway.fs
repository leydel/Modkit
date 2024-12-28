namespace Discordfs.Gateway.Modules

open Discordfs.Gateway.Types
open Discordfs.Types
open System
open System.Net.WebSockets
open System.Text.Json
open System.Threading
open System.Threading.Tasks

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
        
    type LifecycleContinuation =
        | Resume of ResumeGatewayUrl: string
        | Reconnect
        | Close

    type LifecycleState = {
        SequenceId: int option
        Interval: int option
        Heartbeat: DateTime option
        HeartbeatAcked: bool
        ResumeGatewayUrl: string option
        SessionId: string option
    }

    let rec loop (state: LifecycleState) (id: IdentifySendEvent) handler ws (ct: CancellationToken) = task {
        match ct.IsCancellationRequested with
        | true -> return LifecycleContinuation.Close
        | false ->
            let timeout =
                match state.Heartbeat with
                | Some h -> h.Subtract DateTime.UtcNow
                | None -> Timeout.InfiniteTimeSpan
                |> Task.Delay

            let event =
                ws |> Websocket.readNext ?> function
                | WebsocketReadResponse.Close code -> Error (Option.map enum<GatewayCloseEventCode> code)
                | WebsocketReadResponse.Message message -> Ok (Json.deserializeF<GatewayReceiveEvent> message, message)

            let! winner = Task.WhenAny(timeout, event)

            match winner, state.HeartbeatAcked with
            | winner, false when winner = timeout ->
                match state.ResumeGatewayUrl with
                | Some resumeGatewayUrl -> return LifecycleContinuation.Resume resumeGatewayUrl
                | None -> return LifecycleContinuation.Reconnect

            | winner, true when winner = timeout ->
                ws |> heartbeat state.SequenceId |> ignore

                return! loop { state with HeartbeatAcked = false } id handler ws ct
            
            | _ ->
                match event.Result with
                | Error code ->
                    match shouldReconnect code, state.ResumeGatewayUrl with
                    | true, Some resumeGatewayUrl -> return LifecycleContinuation.Resume resumeGatewayUrl
                    | true, None -> return LifecycleContinuation.Reconnect
                    | false, _ -> return LifecycleContinuation.Close

                | Ok (GatewayReceiveEvent.HELLO event, _) ->
                    let interval = Some event.Data.HeartbeatInterval

                    let sendEvent =
                        match state.SessionId, state.SequenceId with
                        | Some sessionId, Some sequenceId -> resume (ResumeSendEvent.build(id.Token, sessionId, sequenceId))
                        | _ -> identify id

                    ws |> sendEvent |> ignore

                    return! loop { state with Interval = interval } id handler ws ct

                | Ok (GatewayReceiveEvent.HEARTBEAT _, _) ->
                    let freshHeartbeat = state.Interval |> Option.map (fun i -> DateTime.UtcNow.AddMilliseconds(i))

                    ws |> heartbeat state.SequenceId |> ignore

                    return! loop { state with Heartbeat = freshHeartbeat } id handler ws ct

                | Ok (GatewayReceiveEvent.HEARTBEAT_ACK _, _) ->
                    return! loop { state with HeartbeatAcked = true } id handler ws ct

                | Ok (GatewayReceiveEvent.READY event, _) ->
                    let resumeGatewayUrl = Some event.Data.ResumeGatewayUrl
                    let sessionId = Some event.Data.SessionId

                    return! loop { state with ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId } id handler ws ct

                | Ok (GatewayReceiveEvent.RESUMED _, _) ->
                    return! loop state id handler ws ct

                | Ok (GatewayReceiveEvent.RECONNECT _, _) ->
                    match state.ResumeGatewayUrl with
                    | Some resumeGatewayUrl -> return LifecycleContinuation.Resume resumeGatewayUrl
                    | None -> return LifecycleContinuation.Reconnect

                | Ok (GatewayReceiveEvent.INVALID_SESSION event, _) ->
                    match event.Data, state.ResumeGatewayUrl with
                    | true, Some resumeGatewayUrl -> return LifecycleContinuation.Resume resumeGatewayUrl
                    | _ -> return LifecycleContinuation.Reconnect

                | Ok (_, event) ->
                    handler event |> ignore

                    match JsonDocument.Parse(event).RootElement.TryGetProperty "s" with
                    | true, t -> return! loop { state with SequenceId = t.GetInt32() |> Some } id handler ws ct
                    | _ -> return! loop state id handler ws ct
    }

    let rec connect (cachedUrl: string) (resumeGatewayUrl: string option) identify handler (ws: ClientWebSocket option ref) (ct: CancellationToken) = task {
        let socket = new ClientWebSocket()
        ws.Value <- Some socket
        do! socket.ConnectAsync(Uri (resumeGatewayUrl >>? cachedUrl), CancellationToken.None)

        let initialState = {
            SequenceId = None
            Interval = None
            Heartbeat = None
            HeartbeatAcked = true
            ResumeGatewayUrl = resumeGatewayUrl
            SessionId = None
        }

        match! loop initialState identify handler socket ct with
        | LifecycleContinuation.Resume resumeGatewayUrl ->
            do! socket.CloseAsync(WebSocketCloseStatus.Empty, "Resuming", CancellationToken.None)
            return! connect cachedUrl (Some resumeGatewayUrl) identify handler ws ct

        | LifecycleContinuation.Reconnect ->
            do! socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Reconnecting", CancellationToken.None)
            return! connect cachedUrl None identify handler ws ct

        | LifecycleContinuation.Close ->
            do! socket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Closing", CancellationToken.None)
    }
