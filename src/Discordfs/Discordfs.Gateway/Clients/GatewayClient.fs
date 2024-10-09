namespace Discordfs.Gateway.Clients

open Discordfs.Gateway.Modules
open Discordfs.Gateway.Types
open Discordfs.Types
open System
open System.Net.WebSockets
open System.Threading
open System.Threading.Tasks

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

    let handleHeartbeat jitter interval (sequenceId: int option) lastHeartbeatAcked ws = async {
        let delay =
            if jitter then int ((float interval) * Random().NextDouble())
            else interval

        do! Task.Delay delay |> Async.AwaitTask // TODO: FIX

        match lastHeartbeatAcked with
        | false -> 
            // TODO: Terminate connection with any close code besides 1000 and 1001, then reconnect and attempt to resume
            return ()
        | true ->
            Gateway.heartbeat sequenceId ws |> ignore

        // TODO: Handle graceful disconnect
    }

    let handleLifecycle (identify: Identify) sequenceId ws = async {
        let! res = Gateway.readNext ws |> Async.AwaitTask

        match res with
        | GatewayReadResponse.Close close ->
            return None // TODO: Handle reconnecting
        | GatewayReadResponse.Message (opcode, eventName, message) ->
            match opcode with
            | GatewayOpcode.HELLO ->
                let event = GatewayEvent<Hello>.deserializeF message

                Gateway.identify identify |> ignore

                // TODO: Start heartbeat

                //heartbeat true event.Data.HeartbeatInterval |> ignore

                return sequenceId
            | GatewayOpcode.HEARTBEAT ->
                Gateway.heartbeat sequenceId |> ignore

                // TODO: This should reset the above timer to start with this
                            
                return sequenceId
            | GatewayOpcode.HEARTBEAT_ACK ->
                //lastHeartbeatAcked <- true
                            
                return sequenceId
            | GatewayOpcode.DISPATCH when eventName = Some "ready" ->
                let event = GatewayEvent<Ready>.deserializeF message

                // TODO: handle ready event

                return sequenceId
            | _ ->
                // do! handler message

                match GatewaySequencer.getSequenceNumber message with
                | None -> return sequenceId
                | Some s -> return Some s
    }

    interface IGatewayClient with
        member _.Connect gatewayUrl identify handler = task {
            do! _ws.ConnectAsync(Uri gatewayUrl, CancellationToken.None)

            let rec loop jitter interval sequenceId lastHeartbeatAcked = task {
                let lifecycleTask = handleLifecycle identify sequenceId _ws |> Async.StartAsTask
                let heartbeatTask = handleHeartbeat jitter interval sequenceId lastHeartbeatAcked _ws |> Async.StartAsTask

                let! winner = Task.WhenAny [lifecycleTask :> Task; heartbeatTask :> Task] |> Async.AwaitTask
                match winner with
                | w when w = lifecycleTask -> None
                | w when w = heartbeatTask -> None
            }

            // TODO: Loop

            return ()
        }

        member _.RequestGuildMembers payload =
            Gateway.requestGuildMembers payload _ws

        member _.UpdateVoiceState payload =
            Gateway.updateVoiceState payload _ws

        member _.UpdatePresence payload =
            Gateway.updatePresence payload _ws
