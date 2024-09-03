namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Types
open System
open System.Net.WebSockets
open System.Text
open System.Threading
open System.Threading.Tasks

type IDiscordGatewayService =
    abstract member Connect:
        unit ->
        Task<unit>

    abstract member Disconnect:
        unit ->
        Task<unit>

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

                do! ws.SendAsync(segment, WebSocketMessageType.Text, isEndOfMessage, cts.Token)
            }

            do! loop buffer 0
            return Ok ()
    }

    interface IDiscordGatewayService with 
        member this.Connect () = task {
            let! gateway = discordHttpService.GetGateway "10" GatewayEncoding.JSON None

            let cts = new CancellationTokenSource()
            cts.CancelAfter(TimeSpan.FromSeconds 5)

            let ws = new ClientWebSocket()
            do! ws.ConnectAsync(Uri gateway.Url, cts.Token)
            this._ws <- Some ws

            let rec loop buffer = task {
                let! res = ws.ReceiveAsync(ArraySegment buffer, CancellationToken.None)

                match res.MessageType with
                | WebSocketMessageType.Close -> ()
                | _ ->
                    let message = Encoding.UTF8.GetString(buffer, 0, res.Count)

                    // TODO: Handle received message here

                    return! loop buffer
            }

            return! loop (Array.zeroCreate<byte> 4096)
        }

        member this.Disconnect () = task {
            match this._ws with
            | None -> ()
            | Some ws ->
                do! ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None)
        }

        // TODO: Handle lifecycle (https://discord.com/developers/docs/topics/gateway#connection-lifecycle)
