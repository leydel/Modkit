namespace Discordfs.Gateway.Modules

open Discordfs.Gateway.Types
open Discordfs.Types
open System.Text.Json

type GatewayReadResponse =
    | Message of Opcode: GatewayOpcode * EventName: string option * Message: string
    | Close of GatewayCloseEventCode option

module Gateway =
    let readNext ws = task {
        let! res = Websocket.readNext ws

        match res with
        | WebsocketReadResponse.Close code ->
            return GatewayReadResponse.Close (Option.map enum<GatewayCloseEventCode> code)
        | WebsocketReadResponse.Message message ->
            let identifier = Json.deserializeF<GatewayEventIdentifier> message
            return GatewayReadResponse.Message (identifier.Opcode, identifier.EventName, message)
    }

    let identify (payload: Identify) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.IDENTIFY, Data = payload) |> Json.serializeF
        )
        
    let resume (payload: Resume) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.RESUME, Data = payload) |> Json.serializeF
        )

    let heartbeat (payload: Heartbeat) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.HEARTBEAT, Data = payload) |> Json.serializeF
        )

    let requestGuildMembers (payload: RequestGuildMembers) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS, Data = payload) |> Json.serializeF
        )

    let updateVoiceState (payload: UpdateVoiceState) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.VOICE_STATE_UPDATE, Data = payload) |> Json.serializeF
        )

    let updatePresence (payload: UpdatePresence) ws =
        ws |> Websocket.write (
            GatewayEvent.build(Opcode = GatewayOpcode.PRESENCE_UPDATE, Data = payload) |> Json.serializeF
        )
    
    let shouldReconnect (code: GatewayCloseEventCode option) =
        match code with
        | Some GatewayCloseEventCode.UNKNOWN_ERROR -> true
        | Some GatewayCloseEventCode.UNKNOWN_OPCODE -> true
        | Some GatewayCloseEventCode.DECODE_ERROR -> true
        | Some GatewayCloseEventCode.NOT_AUTHENTICATED -> true
        | Some GatewayCloseEventCode.AUTHENTICATION_FAILED -> false
        | Some GatewayCloseEventCode.ALREADY_AUTHENTICATED -> true
        | Some GatewayCloseEventCode.INVALID_SEQ -> true
        | Some GatewayCloseEventCode.RATE_LIMITED -> true
        | Some GatewayCloseEventCode.SESSION_TIMED_OUT -> true
        | Some GatewayCloseEventCode.INVALID_SHARD -> false
        | Some GatewayCloseEventCode.SHARDING_REQUIRED -> false
        | Some GatewayCloseEventCode.INVALID_API_VERSION -> false
        | Some GatewayCloseEventCode.INVALID_INTENTS -> false
        | Some GatewayCloseEventCode.DISALLOWED_INTENTS -> false
        | None -> true
        | _ -> false
