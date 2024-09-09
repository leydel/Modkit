namespace Modkit.Discordfs.Services

open FSharp.Json
open Modkit.Discordfs.Types
open System
open System.Net.WebSockets
open System.Text
open System.Threading
open System.Threading.Tasks

type IDiscordGatewayActions =
    abstract member Identify:
        Identify ->
        Task<Result<unit, string>>

    abstract member Resume:
        Resume ->
        Task<Result<unit, string>>

    abstract member Heartbeat:
        Heartbeat ->
        Task<Result<unit, string>>

    abstract member RequestGuildMembers:
        RequestGuildMembers ->
        Task<Result<unit, string>>

    abstract member UpdateVoiceState:
        UpdateVoiceState ->
        Task<Result<unit, string>>

    abstract member UpdatePresence:
        UpdatePresence ->
        Task<Result<unit, string>>

type DiscordGatewayActions (private _ws: ClientWebSocket) =
    let rec send (buffer: byte array) (offset: int) = async {
        let cts = new CancellationTokenSource ()
        cts.CancelAfter(TimeSpan.FromSeconds 5)

        let count = 1024
        let segment = ArraySegment(buffer, offset, count)
        let isEndOfMessage = offset + count >= buffer.Length

        do! _ws.SendAsync(segment, WebSocketMessageType.Text, isEndOfMessage, cts.Token) |> Async.AwaitTask

        match isEndOfMessage with
        | false -> return! send buffer (offset + count)
        | true -> return Ok ()
    }

    member _.Send (message: GatewayEvent) = task {
        try
            let buffer = message |> Json.serialize |> Encoding.UTF8.GetBytes
            return! send buffer 0
        with
        | _ ->
            return Error "Unexpected error occurred when attempting to send message"
    }

    interface IDiscordGatewayActions with
        member this.Identify payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.IDENTIFY,
                Data = payload
            ))
            
            return! this.Send message
        }
            
        member this.Resume payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.RESUME,
                Data = payload
            ))
            
            return! this.Send message
        }

        member this.Heartbeat payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.HEARTBEAT,
                Data = payload
            ))
            
            return! this.Send message
        }

        member this.RequestGuildMembers payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS,
                Data = payload
            ))
            
            return! this.Send message
        }

        member this.UpdateVoiceState payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.VOICE_STATE_UPDATE,
                Data = payload
            ))
            
            return! this.Send message
        }

        member this.UpdatePresence payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.PRESENCE_UPDATE,
                Data = payload
            ))
            
            return! this.Send message
        }
