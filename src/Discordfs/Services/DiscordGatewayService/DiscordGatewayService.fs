namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Types
open System
open System.Net.WebSockets
open System.Text
open System.Threading
open System.Threading.Tasks

type IDiscordGatewayService =
    abstract member Actions: IDiscordGatewayActions

    abstract member Connect:
        identify: Identify ->
        handler: (string -> Task<unit>) ->
        Task<Result<unit, string>>

    abstract member Disconnect:
        unit ->
        Task<Result<unit, string>>

type DiscordGatewayService (discordHttpService: IDiscordHttpService) =
    let _ws: ClientWebSocket = new ClientWebSocket()

    let _actions: IDiscordGatewayActions = DiscordGatewayActions(_ws)

    let mutable lastHeartbeatAcked = true
    let mutable heartbeatCount = 0
    let mutable sequenceId: int option = None

    member this.heartbeat (interval: int) = task {
        match lastHeartbeatAcked with
        | false ->
            return Error "Last gateway heartbeat was not acked"
        | true ->
            lastHeartbeatAcked <- false
            heartbeatCount <- heartbeatCount + 1

            do! _actions.Heartbeat sequenceId :> Task
            do! Task.Delay interval
            return! this.heartbeat interval

        // TODO: Handle graceful disconnect
    }

    member this.mapLifecycle (identify: Identify) (handler: string -> Task<unit>) (message: string) = task {
        match GatewayEventIdentifier.getType message with
        | None -> failwith "Unexpected payload received from gateway"
        | Some identifier ->
            Console.WriteLine $"RECEIVED | Opcode: {identifier.Opcode}, Event Name: {identifier.EventName}"
            Console.WriteLine $"RECEIVED | ${message}"

            match identifier.Opcode with
            | GatewayOpcode.HELLO ->
                let event = GatewayEvent<Hello>.deserializeF message

                do! _actions.Identify identify :> Task

                let random = Random()
                let jitter = int ((float event.Data.HeartbeatInterval) * random.NextDouble())
                do! Task.Delay jitter

                Console.WriteLine $"JITTER ${jitter}"

                this.heartbeat event.Data.HeartbeatInterval |> ignore
                return ()
            | GatewayOpcode.HEARTBEAT ->
                do! _actions.Heartbeat sequenceId :> Task
                return ()
            | GatewayOpcode.HEARTBEAT_ACK ->
                lastHeartbeatAcked <- true
                return ()
            | GatewayOpcode.DISPATCH when identifier.EventName = Some "ready" -> // TODO: Check correct event name
                let event = GatewayEvent<Ready>.deserializeF message

                // TODO: Handle ready event however needed
                return ()
            | _ ->
                sequenceId <-
                    match GatewaySequencer.getSequenceNumber message with
                    | None -> sequenceId
                    | Some seq -> Some seq

                return! handler message
    }

    member this.handle (identify: Identify) (handler: string -> Task<unit>) = task {
        Console.WriteLine("Awaiting new message")

        let buffer = Array.zeroCreate<byte> 4096
        let! res = _ws.ReceiveAsync(ArraySegment buffer, CancellationToken.None)

        Console.WriteLine("RECEIVED | Message type: {0}", res.MessageType)
        Console.WriteLine("RECEIVED | Content: {0}", Encoding.UTF8.GetString(buffer, 0, res.Count))

        match res.MessageType with
        | WebSocketMessageType.Text
        | WebSocketMessageType.Binary ->
            do! this.mapLifecycle identify handler (Encoding.UTF8.GetString(buffer, 0, res.Count))
            Console.WriteLine("Finished lifecycle")
            return true
        | _ ->
            return false
    }

    interface IDiscordGatewayService with
        member val Actions = _actions

        member this.Connect identify handler = task {
            try
                let! gateway = discordHttpService.Gateway.GetGateway "10" GatewayEncoding.JSON None

                Console.WriteLine gateway.Url
                Console.WriteLine (Uri gateway.Url)

                do! _ws.ConnectAsync(Uri gateway.Url, CancellationToken.None)
                while! this.handle identify handler do
                    Console.WriteLine("Do while loop entered")

                Console.WriteLine("Do while loop exited")

                // TODO: Handle gateway disconnect and resuming

                return Ok ()
            with
            | _ -> 
                return Error "Unexpected error occurred with gateway connection"
        }

        member this.Disconnect () = task {
            try
                do! _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None)
                return Ok ()
            with
            | _ -> 
                return Error "Unexpected error occurred attempting to disconnect from the gateway"
        }
