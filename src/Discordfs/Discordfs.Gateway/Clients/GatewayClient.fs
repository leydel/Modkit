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
            let rec loop (sequenceId: int option) acked (interval: int option) (heartbeat: DateTime option) identify handler ws = task {
                let now = DateTime.UtcNow
                let freshInterval = Some (now.AddMilliseconds(interval.Value))

                let timeout =
                    match heartbeat with
                    | Some h -> Task.Delay (h.Subtract(now))
                    | None -> Task.Delay Timeout.InfiniteTimeSpan

                let readNext = ws |> Gateway.readNext

                let! winner = Task.WhenAny(timeout, readNext)

                if winner = timeout then
                    match acked with
                    | false ->
                        return ConnectionCloseBehaviour.Reconnect
                    | true ->
                        ws |> Gateway.heartbeat sequenceId |> ignore
                        return! loop sequenceId false interval freshInterval identify handler ws
                else
                    let res = readNext.Result

                    match readNext.Result with
                    | GatewayReadResponse.Close code ->
                        match Gateway.shouldReconnect code with
                        | true -> return ConnectionCloseBehaviour.Reconnect
                        | false -> return ConnectionCloseBehaviour.Close
                    | GatewayReadResponse.Message (opcode, _, message) ->
                        match opcode with
                        | GatewayOpcode.HELLO ->
                            let event = GatewayEvent<Hello>.deserializeF message
                            let helloInterval = Some event.Data.HeartbeatInterval

                            Gateway.identify identify |> ignore
                            return! loop sequenceId acked helloInterval heartbeat identify handler ws
                        | GatewayOpcode.HEARTBEAT ->
                            Gateway.heartbeat sequenceId |> ignore
                            return! loop sequenceId acked interval freshInterval identify handler ws
                        | GatewayOpcode.HEARTBEAT_ACK ->
                            return! loop sequenceId true interval heartbeat identify handler ws
                        | _ ->
                            handler message |> ignore

                            match GatewaySequencer.getSequenceNumber message with
                            | None -> return! loop sequenceId acked interval heartbeat identify handler ws
                            | Some s -> return! loop (Some s) acked interval heartbeat identify handler ws
            }
            
            do! _ws.ConnectAsync(Uri gatewayUrl, CancellationToken.None)
            let! close = loop None true None None identify handler _ws

            // TODO: Handle reconnecting/resuming with `close`

            return ()
        }

        member _.RequestGuildMembers payload =
            Gateway.requestGuildMembers payload _ws

        member _.UpdateVoiceState payload =
            Gateway.updateVoiceState payload _ws

        member _.UpdatePresence payload =
            Gateway.updatePresence payload _ws
