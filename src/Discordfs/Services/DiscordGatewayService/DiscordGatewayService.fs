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
    member val private _ws: ClientWebSocket = new ClientWebSocket()

    member this.mapLifecycle (handler: GatewayEvent -> Task<unit>) (message: GatewayEvent) = task {
        System.Console.WriteLine $"Opcode: {message.Opcode}, Event Name: {message.EventName}"

        match message.Opcode with
        | GatewayOpcode.HELLO ->
            // initiate heartbeat (gives interval)
            // on interval: if doesnt receive heartbeat ack, close connection and reconnect

            return () // TODO: Handle
        | GatewayOpcode.HEARTBEAT_ACK ->
            // on first ack, send identify

            return () // TODO: Handle
        | GatewayOpcode.DISPATCH when message.EventName = Some "Ready" -> // TODO: Check correct event name
            // send heartbeat back (or reverse)

            return () // TODO: Handle
        | _ -> 
            return! handler message
    }

    member this.handle (handler: GatewayEvent -> Task<unit>) = task {
        let buffer = Array.zeroCreate<byte> 4096
        let! res = this._ws.ReceiveAsync(ArraySegment buffer, CancellationToken.None)

        match res.MessageType with
        | WebSocketMessageType.Text
        | WebSocketMessageType.Binary ->
            let message = Encoding.UTF8.GetString(buffer, 0, res.Count) |> Json.deserialize<GatewayEvent>
            do! this.mapLifecycle handler message

            return true
        | _ ->
            return false
    }

    interface IDiscordGatewayService with
        member this.Actions = DiscordGatewayActions(this._ws)

        member this.Connect handler = task {
            try
                let! gateway = discordHttpService.Gateway.GetGateway "10" GatewayEncoding.JSON None

                do! this._ws.ConnectAsync(Uri gateway.Url, CancellationToken.None)
                while! this.handle handler do ()

                // TODO: Handle gateway disconnect and resuming

                return Ok ()
            with
            | _ -> 
                return Error "Unexpected error occurred with gateway connection"
        }

        member this.Disconnect () = task {
            try
                do! this._ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None)
                return Ok ()
            with
            | _ -> 
                return Error "Unexpected error occurred attempting to disconnect from the gateway"
        }
