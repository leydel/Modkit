namespace Modkit.Discordfs.Services

open FSharp.Json
open Modkit.Discordfs.Types
open System
open System.Net.WebSockets
open System.Text
open System.Threading
open System.Threading.Tasks

type IDiscordGatewayService =
    abstract member Actions: IDiscordGatewayActions

    abstract member Connect:
        (GatewayEvent -> Task<unit>) ->
        Task<Result<unit, string>>

    abstract member Disconnect:
        unit ->
        Task<Result<unit, string>>

type DiscordGatewayService (discordHttpService: IDiscordHttpService) =
    member val private _ws: ClientWebSocket option = None with get, set

    member this.lifecycle (message: GatewayEvent) = task {
        // TODO: Check if message is lifecycle event, if so handle here and return true, otherwise return false

        // hello, heartbeat, ready

        // TODO: Handle gateway disconnect and resuming

        return false
    }

    interface IDiscordGatewayService with
        member this.Actions = DiscordGatewayActions(this._ws) // TODO: Check if this correctly updates ws client when created

        member this.Connect handler = task {
            match this._ws with
            | Some _ ->
                return Error "Cannot disconnect because no websocket is connected"
            | None ->
                let! gateway = discordHttpService.Gateway.GetGateway "10" GatewayEncoding.JSON None

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
                
            // TODO: Clean up this code
        }

        member this.Disconnect () = task {
            match this._ws with
            | None ->
                return Error "Cannot disconnect because no websocket is connected"
            | Some ws ->
                do! ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None)
                return Ok ()
        }
