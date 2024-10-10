namespace Discordfs.Gateway.Clients

open Discordfs.Gateway.Modules
open Discordfs.Gateway.Types
open Discordfs.Types
open System
open System.Net.WebSockets
open System.Threading
open System.Threading.Tasks

type ConnectionCloseBehaviour =
    | Reconnect
    | Close

type GatewayLoopState = {
    SequenceId: int option
    Acked: bool
    Interval: int option
    Heartbeat: DateTime option
    ResumeGatewayUrl: string option
    Identify: Identify
    Handler: string -> Task<unit>
    Websocket: ClientWebSocket
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
    let _ws: ClientWebSocket = new ClientWebSocket()

    interface IGatewayClient with
        member _.Connect gatewayUrl identify handler = task {
            let rec loop (state: GatewayLoopState) = task {
                let ws = state.Websocket

                let now = DateTime.UtcNow
                let freshHeartbeat = state.Interval |> Option.map (fun i -> now.AddMilliseconds(i))

                let timeout =
                    match state.Heartbeat with
                    | Some h -> Task.Delay (h.Subtract(now))
                    | None -> Task.Delay Timeout.InfiniteTimeSpan

                let readNext = ws |> Gateway.readNext

                let! winner = Task.WhenAny(timeout, readNext)

                if winner = timeout then
                    match state.Acked with
                    | false ->
                        return ConnectionCloseBehaviour.Reconnect
                    | true ->
                        ws |> Gateway.heartbeat state.SequenceId |> ignore
                        return! loop { state with Acked = false }
                else
                    let res = readNext.Result

                    match readNext.Result with
                    | GatewayReadResponse.Close code ->
                        match Gateway.shouldReconnect code with
                        | true -> return ConnectionCloseBehaviour.Reconnect
                        | false -> return ConnectionCloseBehaviour.Close
                    | GatewayReadResponse.Message (opcode, eventName, message) ->
                        match opcode with
                        | GatewayOpcode.HELLO ->
                            Gateway.identify identify |> ignore
                            let event = GatewayEvent<Hello>.deserializeF message
                            return! loop { state with Interval = Some event.Data.HeartbeatInterval }
                        | GatewayOpcode.HEARTBEAT ->
                            Gateway.heartbeat state.SequenceId |> ignore
                            return! loop { state with Heartbeat = freshHeartbeat }
                        | GatewayOpcode.HEARTBEAT_ACK ->
                            return! loop { state with Acked = true }
                        | GatewayOpcode.DISPATCH when eventName = Some "READY" ->
                            let event = GatewayEvent<Ready>.deserializeF message
                            return! loop { state with ResumeGatewayUrl = Some event.Data.ResumeGatewayUrl }
                        | _ ->
                            handler message |> ignore

                            match GatewaySequencer.getSequenceNumber message with
                            | None -> return! loop state
                            | Some s -> return! loop { state with SequenceId = Some s }
            }
            
            do! _ws.ConnectAsync(Uri gatewayUrl, CancellationToken.None)

            let! close = loop {
                SequenceId = None;
                Acked = true;
                Interval = None;
                Heartbeat = None;
                ResumeGatewayUrl = None;
                Identify = identify;
                Handler = handler
                Websocket = _ws
            }

            // TODO: Handle reconnecting/resuming with `close`

            return ()
        }

        member _.RequestGuildMembers payload =
            Gateway.requestGuildMembers payload _ws

        member _.UpdateVoiceState payload =
            Gateway.updateVoiceState payload _ws

        member _.UpdatePresence payload =
            Gateway.updatePresence payload _ws
