namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Types
open System
open System.IO
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

    let rec heartbeat (jitter: bool) (interval: int) = task {
        if jitter then
            do! int ((float interval) * Random().NextDouble()) |> Task.Delay
        else
            do! Task.Delay interval

        match lastHeartbeatAcked with
        | false ->
            return Error "Last gateway heartbeat was not acked"
        | true ->
            lastHeartbeatAcked <- false
            heartbeatCount <- heartbeatCount + 1

            do! _actions.Heartbeat sequenceId :> Task
            do! Task.Delay interval
            return! heartbeat false interval

        // TODO: Handle graceful disconnect
    }

    member _.read () = task {
        use ms = new MemoryStream() 
        let mutable isEndOfMessage = false
        let mutable messageType: WebSocketMessageType = WebSocketMessageType.Text

        while not isEndOfMessage do
            let buffer = Array.zeroCreate<byte> 4096 |> ArraySegment
            let! res = _ws.ReceiveAsync(buffer, CancellationToken.None)
            Console.WriteLine($"Close status: {res.CloseStatus}")
            ms.Write(buffer.Array, buffer.Offset, res.Count)
            isEndOfMessage <- res.EndOfMessage
            messageType <- res.MessageType

        ms.Seek(0, SeekOrigin.Begin) |> ignore

        use sr = new StreamReader(ms)
        let! message = sr.ReadToEndAsync()
        return (messageType, message)
    }

    interface IDiscordGatewayService with
        member val Actions = _actions

        member this.Connect identify handler = task {
            try
                let! gateway = discordHttpService.Gateway.GetGateway "10" GatewayEncoding.JSON None

                do! _ws.ConnectAsync(Uri gateway.Url, CancellationToken.None)

                let rec loop (sequenceId: int option): Task<Result<int option, string>> = task {
                    Console.WriteLine("Awaiting new message...")

                    let! messageType, message = this.read ()

                    Console.WriteLine("Message received!")

                    match messageType with
                    | WebSocketMessageType.Close ->
                        Console.WriteLine($"RECEIVED | {message}")
                        return Error "Gateway connection closed"
                    | _ ->
                        match GatewayEventIdentifier.getType message with
                        | None ->
                            return Error "Unexpected payload received from gateway"
                        | Some identifier ->
                            Console.WriteLine $"RECEIVED | Opcode: {identifier.Opcode}, Event Name: {identifier.EventName}"
                            Console.WriteLine $"RECEIVED | {message}"

                            match identifier.Opcode with
                            | GatewayOpcode.HELLO ->
                                let event = GatewayEvent<Hello>.deserializeF message

                                _actions.Identify identify |> ignore
                                heartbeat true event.Data.HeartbeatInterval |> ignore

                                return! loop sequenceId
                            | GatewayOpcode.HEARTBEAT ->
                                _actions.Heartbeat sequenceId |> ignore
                            
                                return! loop sequenceId
                            | GatewayOpcode.HEARTBEAT_ACK ->
                                lastHeartbeatAcked <- true
                            
                                return! loop sequenceId
                            | _ ->
                                do! handler message

                                match GatewaySequencer.getSequenceNumber message with
                                | None -> return! loop sequenceId
                                | Some s -> return! loop (Some s)
                }
                            
                let! close = loop None

                match close with
                | Error message -> Console.WriteLine($"Error occurred: {message}")
                | Ok sequenceId -> Console.WriteLine($"Closed on sequence ID {sequenceId}")

                // TODO: Handle gateway disconnect and resuming

                return close |> Result.map (fun _ -> ())
            with
            | _ ->
                Console.WriteLine("Error occurred")
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
