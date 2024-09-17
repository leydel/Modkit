namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
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
    member _.write (event: GatewayEvent<'a>) = task {
        Console.WriteLine $"SENDING | Opcode: {event.Opcode}, Event Name: {event.EventName}"
        Console.WriteLine $"SENDING | {FsJson.serialize event}"

        let cts = new CancellationTokenSource ()
        cts.CancelAfter(TimeSpan.FromSeconds 5)

        let bytes = event |> FsJson.serialize |> Encoding.UTF8.GetBytes

        let size = 4096

        let mutable isEndOfMessage = false
        let mutable offset = 0

        try
            while not isEndOfMessage do
                isEndOfMessage <- offset + size >= bytes.Length
                let count = Math.Min(size, bytes.Length - offset)
                let buffer = ArraySegment(bytes, offset, count)

                do! _ws.SendAsync(buffer, WebSocketMessageType.Text, isEndOfMessage, cts.Token)

                offset <- offset + size

            return Ok ()
        with _ ->
            return Error "Unexpected error occurred when attempting to write message"
    }

    interface IDiscordGatewayActions with
        member this.Identify payload = task {
            let message =
                GatewayEvent.build(
                    Opcode = GatewayOpcode.IDENTIFY,
                    Data = payload
                )
            
            return! this.write message
        }
            
        member this.Resume payload = task {
            let message =
                GatewayEvent.build(
                    Opcode = GatewayOpcode.RESUME,
                    Data = payload
                )
            
            return! this.write message
        }

        member this.Heartbeat payload = task {
            let message =
                GatewayEvent.build(
                    Opcode = GatewayOpcode.HEARTBEAT,
                    Data = payload
                )
            
            return! this.write message
        }

        member this.RequestGuildMembers payload = task {
            let message =
                GatewayEvent.build(
                    Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS,
                    Data = payload
                )
            
            return! this.write message
        }

        member this.UpdateVoiceState payload = task {
            let message =
                GatewayEvent.build(
                    Opcode = GatewayOpcode.VOICE_STATE_UPDATE,
                    Data = payload
                )
            
            return! this.write message
        }

        member this.UpdatePresence payload = task {
            let message =
                GatewayEvent.build(
                    Opcode = GatewayOpcode.PRESENCE_UPDATE,
                    Data = payload
                )
            
            return! this.write message
        }
