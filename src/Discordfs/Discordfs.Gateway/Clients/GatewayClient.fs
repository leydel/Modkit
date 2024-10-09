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

type GatewayState = {
    Interval: int option
    SequenceId: int option
    LastHeartbeatAcked: bool
}

type HeartbeatResult =
    | Ok of GatewayState
    | MissedAck
    | Skip

type GatewayClient () =
    let _ws: ClientWebSocket = new ClientWebSocket()

    let handleHeartbeat jitter state ws = async {
        match state.Interval with
        | None ->
            return HeartbeatResult.Skip
        | Some interval ->
            let delay =
                if jitter then int ((float interval) * Random().NextDouble())
                else interval

            do! Task.Delay delay |> Async.AwaitTask

            match state.LastHeartbeatAcked with
            | false -> 
                // TODO: Terminate connection with any close code besides 1000 and 1001, then reconnect and attempt to resume
                return HeartbeatResult.MissedAck
            | true ->
                Gateway.heartbeat state.SequenceId ws |> ignore
                return HeartbeatResult.Ok { state with LastHeartbeatAcked = false }

            // TODO: Handle graceful disconnect
    }

    let handleLifecycle (identify: Identify) (handler: string -> Task<unit>) state ws = async {
        let! res = Gateway.readNext ws |> Async.AwaitTask

        match res with
        | GatewayReadResponse.Close close ->
             // TODO: Handle reconnecting
            return state
        | GatewayReadResponse.Message (opcode, _, message) ->
            match opcode with
            | GatewayOpcode.HELLO ->
                let event = GatewayEvent<Hello>.deserializeF message
                let interval = Some event.Data.HeartbeatInterval

                Gateway.identify identify |> ignore

                return { state with GatewayState.Interval = interval }
            | GatewayOpcode.HEARTBEAT ->
                Gateway.heartbeat state.SequenceId |> ignore

                // TODO: This should reset the above timer to start with this
                            
                return state
            | GatewayOpcode.HEARTBEAT_ACK ->
                return { state with LastHeartbeatAcked = true }
            | _ ->
                do! handler message |> Async.AwaitTask

                match GatewaySequencer.getSequenceNumber message with
                | None -> return state
                | Some s -> return { state with SequenceId = Some s }
    }

    interface IGatewayClient with
        member _.Connect gatewayUrl identify handler = task {
            do! _ws.ConnectAsync(Uri gatewayUrl, CancellationToken.None)

            let rec loop jitter state = task {
                use lifecycleCts = new CancellationTokenSource()
                use heartbeatCts = new CancellationTokenSource()

                let lifecycle = async {
                    let! res = handleLifecycle identify handler state _ws
                    heartbeatCts.Cancel()
                    return res
                }

                let heartbeat = async {
                    let! res = handleHeartbeat jitter state _ws

                    match res with
                    | HeartbeatResult.Ok _ -> lifecycleCts.Cancel()
                    | HeartbeatResult.MissedAck -> () // TODO: Reconnect
                    | HeartbeatResult.Skip -> ()

                }

                do! Task.WhenAll(
                    Async.StartAsTask(lifecycle, TaskCreationOptions.None, lifecycleCts.Token) :> Task,
                    Async.StartAsTask(heartbeat, TaskCreationOptions.None, heartbeatCts.Token) :> Task
                )

                // TODO: When heartbeat restarts it doesnt continue with remaining interval

                // TODO: Probably rewrite all of this, it's pretty disgusting
                // If mid-processing one, I think this approach completely cancels it regardless. Bad!
            }

            return! loop true { Interval = None; SequenceId = None; LastHeartbeatAcked = true }
        }

        member _.RequestGuildMembers payload =
            Gateway.requestGuildMembers payload _ws

        member _.UpdateVoiceState payload =
            Gateway.updateVoiceState payload _ws

        member _.UpdatePresence payload =
            Gateway.updatePresence payload _ws
