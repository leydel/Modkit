namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Types
open System
open System.Net.WebSockets
open System.Text
open System.Threading
open System.Threading.Tasks

type IDiscordGatewayService =
    abstract member Connect:
        (string -> Task<unit>) ->
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

    member _.lifecycle (message: string) = task {
        // TODO: Check if message is lifecycle event, if so handle here and return true, otherwise return false

        // hello, heartbeat, ready

        // TODO: Handle gateway disconnect and resuming

        return false
    }

    member this.Send (message: string) = task {
        // TODO: Change payload from message to gateway payload object

        let cts = new CancellationTokenSource ()
        cts.CancelAfter(TimeSpan.FromSeconds 5)

        match this._ws with
        | None ->
            return Error "Unable to send data as no websocket is connected"
        | Some ws ->
            let buffer = Encoding.UTF8.GetBytes message

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
            do! ws.ConnectAsync(Uri gateway.Url, cts.Token)
            this._ws <- Some ws

            let rec loop buffer = task {
                try
                    let! res = ws.ReceiveAsync(ArraySegment buffer, CancellationToken.None)

                    match res.MessageType with
                    | WebSocketMessageType.Text ->
                        let message = Encoding.UTF8.GetString(buffer, 0, res.Count)
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
        }

        member this.Disconnect () = task {
            match this._ws with
            | None ->
                return Error "Cannot disconnect because no websocket is connected"
            | Some ws ->
                do! ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None)
                return Ok ()
        }

        member this.Identify payload =
            Task.FromResult () // TODO
            
        member this.Resume payload =
            Task.FromResult () // TODO

        member this.Heartbeat payload =
            Task.FromResult () // TODO

        member this.RequestGuildMembers payload =
            Task.FromResult () // TODO

        member this.UpdateVoiceState payload =
            Task.FromResult () // TODO

        member this.UpdatePresence payload =
            Task.FromResult () // TODO
