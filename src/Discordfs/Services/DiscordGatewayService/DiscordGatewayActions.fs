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

type DiscordGatewayActions (private _ws: ClientWebSocket option) =
    member this.Send (message: GatewayEvent) = task {
        let cts = new CancellationTokenSource ()
        cts.CancelAfter(TimeSpan.FromSeconds 5)

        match _ws with
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

        // TODO: Clean up this code
    }

    interface IDiscordGatewayActions with
        member this.Identify payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.IDENTIFY,
                Data = payload
            ))
            
            do! this.Send message :> Task
        }
            
        member this.Resume payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.RESUME,
                Data = payload
            ))
            
            do! this.Send message :> Task
        }

        member this.Heartbeat payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.HEARTBEAT,
                Data = payload
            ))
            
            do! this.Send message :> Task
        }

        member this.RequestGuildMembers payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS,
                Data = payload
            ))
            
            do! this.Send message :> Task
        }

        member this.UpdateVoiceState payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.VOICE_STATE_UPDATE,
                Data = payload
            ))
            
            do! this.Send message :> Task
        }

        member this.UpdatePresence payload = task {
            let message = (GatewayEvent.build(
                Opcode = GatewayOpcode.PRESENCE_UPDATE,
                Data = payload
            ))

            do! this.Send message :> Task
        }
