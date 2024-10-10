namespace Discordfs.Gateway.Clients

open Discordfs.Gateway.Modules
open Discordfs.Gateway.Types
open Discordfs.Types
open System
open System.Net.WebSockets
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

type IGatewayClient =
    abstract member Connect:
        gatewayUrl: string ->
        identify: Identify ->
        handler: (string -> Task<unit>) ->
        Task<unit>

    abstract member RequestGuildMembers:
        RequestGuildMembers ->
        Task<unit>

    abstract member UpdateVoiceState:
        UpdateVoiceState ->
        Task<unit>

    abstract member UpdatePresence:
        UpdatePresence ->
        Task<unit>

type GatewayClient () =
    let mutable _ws: ClientWebSocket = new ClientWebSocket()

    interface IGatewayClient with
        member _.Connect gatewayUrl identify handler = task {
            let rec loop (state: GatewayLoopState) resumed (identify: Identify) handler ws = task {
                let now = DateTime.UtcNow
                let freshHeartbeat = state.Interval |> Option.map (fun i -> now.AddMilliseconds(i))

                let timeout =
                    match state.Heartbeat with
                    | Some h -> Task.Delay (h.Subtract(now))
                    | None -> Task.Delay Timeout.InfiniteTimeSpan

                let readNext = ws |> Gateway.readNext

                let! winner = Task.WhenAny(timeout, readNext)

                if winner = timeout then
                    match state.HeartbeatAcked with
                    | false ->
                        return ConnectionCloseBehaviour.Resume state.ResumeGatewayUrl
                    | true ->
                        ws |> Gateway.heartbeat state.SequenceId |> ignore
                        return! loop { state with HeartbeatAcked = false } resumed identify handler ws
                else
                    match readNext.Result with
                    | GatewayReadResponse.Close code ->
                        match Gateway.shouldReconnect code with
                        | true -> return ConnectionCloseBehaviour.Resume state.ResumeGatewayUrl
                        | false -> return ConnectionCloseBehaviour.Close

                    | GatewayReadResponse.Message (opcode, eventName, message) ->
                        match opcode, eventName with
                        | GatewayOpcode.HELLO, _ ->
                            let event = GatewayEvent<Hello>.deserializeF message

                            match resumed, state.SessionId, state.SequenceId with
                            | true, Some ses, Some seq -> Gateway.resume (Resume.build(identify.Token, ses, seq))
                            | _ -> Gateway.identify identify
                            <| ws |> ignore

                            let newState = { state with Interval = Some event.Data.HeartbeatInterval }
                            return! loop newState resumed identify handler ws

                        | GatewayOpcode.HEARTBEAT, _ ->
                            ws |> Gateway.heartbeat state.SequenceId |> ignore

                            return! loop { state with Heartbeat = freshHeartbeat } resumed identify handler ws

                        | GatewayOpcode.HEARTBEAT_ACK, _ ->
                            return! loop { state with HeartbeatAcked = true } resumed identify handler ws

                        | GatewayOpcode.DISPATCH, Some "READY" ->
                            let event = GatewayEvent<Ready>.deserializeF message
                            let resumeGatewayUrl = Some event.Data.ResumeGatewayUrl
                            let sessionId = Some event.Data.SessionId

                            let newState = { state with ResumeGatewayUrl = resumeGatewayUrl; SessionId = sessionId }
                            return! loop newState resumed identify handler ws

                        | GatewayOpcode.DISPATCH, Some "RESUMED" ->
                            return! loop state resumed identify handler ws

                        | GatewayOpcode.RECONNECT, _ ->
                            return ConnectionCloseBehaviour.Resume state.ResumeGatewayUrl

                        | GatewayOpcode.INVALID_SESSION, _ ->
                            let event = GatewayEvent<InvalidSession>.deserializeF message

                            match event.Data with
                            | true -> return! loop state resumed identify handler ws
                            | false -> return ConnectionCloseBehaviour.Reconnect

                        | _ ->
                            handler message |> ignore

                            match GatewaySequencer.getSequenceNumber message with
                            | None -> return! loop state resumed identify handler ws
                            | Some s -> return! loop { state with SequenceId = Some s } resumed identify handler ws
            }

            let rec resume cachedUrl (resumeGatewayUrl: string option) identify handler (ws: ClientWebSocket) = task {
                let gatewayUrl = Uri (Option.defaultValue cachedUrl resumeGatewayUrl)
                let resumed = resumeGatewayUrl.IsSome

                do! ws.ConnectAsync(gatewayUrl, CancellationToken.None)

                let initialState = {
                    SequenceId = None;
                    Interval = None;
                    Heartbeat = None;
                    HeartbeatAcked = true;
                    ResumeGatewayUrl = None;
                    SessionId = None;
                }

                let! close = loop initialState resumed identify handler ws

                match close with
                | ConnectionCloseBehaviour.Resume (Some resumeGatewayUrl) ->
                    do! ws.CloseAsync(WebSocketCloseStatus.Empty, "Resuming", CancellationToken.None)
                    return! resume cachedUrl (Some resumeGatewayUrl) identify handler ws

                | ConnectionCloseBehaviour.Resume None
                | ConnectionCloseBehaviour.Reconnect ->
                    do! ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Reconnecting", CancellationToken.None)
                    return! resume cachedUrl None identify handler ws

                | ConnectionCloseBehaviour.Close ->
                    do! ws.CloseAsync(WebSocketCloseStatus.InternalServerError, "Closing", CancellationToken.None)
            }

            do! resume gatewayUrl None identify handler _ws
        }

        member _.RequestGuildMembers payload =
            Gateway.requestGuildMembers payload _ws

        member _.UpdateVoiceState payload =
            Gateway.updateVoiceState payload _ws

        member _.UpdatePresence payload =
            Gateway.updatePresence payload _ws
