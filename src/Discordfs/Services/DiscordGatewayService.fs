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

    // TODO: Define possible events here, like how done in DiscordHttpService

type DiscordGatewayService (discordHttpService: IDiscordHttpService) =
    member val private _ws: ClientWebSocket option = None with get, set

    member this.Send (message: string) = task {
        // TODO: Correctly handle sending events (https://discord.com/developers/docs/topics/gateway#sending-events)

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
                        do! handler (Encoding.UTF8.GetString(buffer, 0, res.Count))
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

        // TODO: Handle lifecycle (https://discord.com/developers/docs/topics/gateway#connection-lifecycle)
