namespace Modkit.Discordfs.Services

open FSharp.Json
open Modkit.Discordfs.Types
open System
open System.Net.WebSockets
open System.Text
open System.Threading
open System.Threading.Tasks

type IDiscordGatewayService =
    abstract member Connect:
        (GatewayEvent -> Task<unit>) ->
        Task<Result<unit, string>>

    abstract member Disconnect:
        unit ->
        Task<Result<unit, string>>

    abstract member Identify:
        Identify ->
        Task<unit>

    abstract member Resume:
        Resume ->
        Task<unit>

    abstract member Heartbeat:
        Heartbeat ->
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

type DiscordGatewayService (discordHttpService: IDiscordHttpService) =
    member val private _ws: ClientWebSocket option = None with get, set

    member _.lifecycle (message: GatewayEvent) = task {
        // TODO: Check if message is lifecycle event, if so handle here and return true, otherwise return false

        // hello, heartbeat, ready

        // TODO: Handle gateway disconnect and resuming

        return false
    }

    // TODO: Clean up `Send` and `Connect` events by splitting into smaller functions

    member this.Send (message: GatewayEvent) = task {
        let cts = new CancellationTokenSource ()
        cts.CancelAfter(TimeSpan.FromSeconds 5)

        match this._ws with
        | None ->
            return Error "Unable to send data as no websocket is connected"
        | Some ws ->
            let buffer = message |> Json.serialize |> Encoding.UTF8.GetBytes

            let rec loop buffer offset = task {
                let count = 1024
                let segment = ArraySegment(buffer, offset, count)
                let isEndOfMessage = offset + count >= buffer.Length

                try
                    do! ws.SendAsync(segment, WebSocketMessageType.Text, isEndOfMessage, cts.Token)

                    if isEndOfMessage then
                        return Ok ()
                    else
                        return! loop buffer (offset + count)
                with
                    | _ ->
                        return Error "Unexpected error occurred when attempting to send message"
            }

            return! loop buffer 0
    }

    interface IDiscordGatewayService with 
        member this.Connect handler = task {
            let! gateway = discordHttpService.GetGateway "10" GatewayEncoding.JSON None

            let cts = new CancellationTokenSource()
            cts.CancelAfter(TimeSpan.FromSeconds 5)

            let ws = new ClientWebSocket()
            this._ws <- Some ws

            try
                do! ws.ConnectAsync(Uri gateway.Url, cts.Token)

                let rec loop buffer = task {
                    try
                        let! res = ws.ReceiveAsync(ArraySegment buffer, CancellationToken.None)

                        match res.MessageType with
                        | WebSocketMessageType.Text ->
                            let message = Encoding.UTF8.GetString(buffer, 0, res.Count) |> Json.deserialize<GatewayEvent>

                            // TODO: Figure out how to correctly serialize gateway data
                            //       Might need a custom transform that maps all events to their data

                            let! handledByLifecycle = this.lifecycle message

                            if not handledByLifecycle then
                                do! handler message

                            return! loop buffer
                        | WebSocketMessageType.Close ->
                            return Ok ()
                        | _ ->
                            return Error $"Unexpected message type received: {res.MessageType}"
                    with
                        | _ ->
                            return Error $"Unexpected error occurred when attempting to receive message"
                }

                return! loop (Array.zeroCreate<byte> 4096)
            with
                | _ -> 
                    return Error "Unexpected error occurred attempting to connect to the gateway"
        }

        member this.Disconnect () = task {
            match this._ws with
            | None ->
                return Error "Cannot disconnect because no websocket is connected"
            | Some ws ->
                do! ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None)
                return Ok ()
        }

        member this.Identify payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.IDENTIFY,
                Data = payload
            ))

            let! _ = this.Send message
            return ()
        }
            
        member this.Resume payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.RESUME,
                Data = payload
            ))

            let! _ = this.Send message
            return ()
        }

        member this.Heartbeat payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.HEARTBEAT,
                Data = payload
            ))

            let! _ = this.Send message
            return ()
        }

        member this.RequestGuildMembers payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS,
                Data = payload
            ))

            let! _ = this.Send message
            return ()
        }

        member this.UpdateVoiceState payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.VOICE_STATE_UPDATE,
                Data = payload
            ))

            let! _ = this.Send message
            return ()
        }

        member this.UpdatePresence payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.PRESENCE_UPDATE,
                Data = payload
            ))

            let! _ = this.Send message
            return ()
        }
