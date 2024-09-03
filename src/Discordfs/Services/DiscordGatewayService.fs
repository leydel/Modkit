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

    // TODO: Define possible events here, like how done in DiscordHttpService

type DiscordGatewayService (discordHttpService: IDiscordHttpService) =
    static member GatewayVersion: string = "10"

    static member GatewayEncoding: GatewayEncoding = GatewayEncoding.JSON

    static member GatewayCompression: GatewayCompression option = None

    member val private _ws: ClientWebSocket option = None with get, set
    
    member _.GetGatewayUrl () = task {
        let! gateway = (discordHttpService.GetGateway
            DiscordGatewayService.GatewayVersion
            DiscordGatewayService.GatewayEncoding
            DiscordGatewayService.GatewayCompression)

        return gateway.Url
    }

    member this.Send (message: string) = task {
        // TODO: Correctly handle sending events (https://discord.com/developers/docs/topics/gateway#sending-events)

        let cts = new CancellationTokenSource ()
        cts.CancelAfter(TimeSpan.FromSeconds 5)

        match this._ws with
        | None -> ()
        | Some ws ->
            let buffer = Encoding.UTF8.GetBytes message

            let rec loop buffer offset = task {
                let count = 1024
                let segment = ArraySegment(buffer, offset, count)
                let isEndOfMessage = offset + count >= buffer.Length

                do! ws.SendAsync(segment, WebSocketMessageType.Text, isEndOfMessage, cts.Token)
            }

            do! loop buffer 0
    }

    interface IDiscordGatewayService with 
        member this.Connect () = task {
            let! url = this.GetGatewayUrl()

            let cts = new CancellationTokenSource ()
            cts.CancelAfter(TimeSpan.FromSeconds 5)

            let ws = new ClientWebSocket()
            do! ws.ConnectAsync(Uri url, cts.Token)
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

        // TODO: Handle lifecycle (https://discord.com/developers/docs/topics/gateway#connection-lifecycle)
