﻿namespace Discordfs.Gateway.Modules

open Discordfs.Gateway.Types
open Discordfs.Types
open System.Text.Json

type GatewayReadResponse =
    | Message of Opcode: GatewayOpcode * EventName: string option * Message: string
    | Close of GatewayCloseEventCode

module Gateway =
    let readNext ws = task {
        let! res = Websocket.readNext ws

        match res with
        | WebsocketReadResponse.Close code ->
            return GatewayReadResponse.Close (enum<GatewayCloseEventCode> code)
        | WebsocketReadResponse.Message message ->
            let identifier = GatewayEventIdentifier.deserializeF message
            return GatewayReadResponse.Message (identifier.Opcode, identifier.EventName, message)
    }

    let identify (payload: Identify) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.IDENTIFY, Data = payload) |> JsonSerializer.Serialize
        )
        
    let resume (payload: Resume) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.RESUME, Data = payload) |> JsonSerializer.Serialize
        )

    let heartbeat (payload: Heartbeat) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.HEARTBEAT, Data = payload) |> JsonSerializer.Serialize
        )

    let requestGuildMembers (payload: RequestGuildMembers) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS, Data = payload) |> JsonSerializer.Serialize
        )

    let updateVoiceState (payload: UpdateVoiceState) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.VOICE_STATE_UPDATE, Data = payload) |> JsonSerializer.Serialize
        )

    let updatePresence (payload: UpdatePresence) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.PRESENCE_UPDATE, Data = payload) |> JsonSerializer.Serialize
        )
    