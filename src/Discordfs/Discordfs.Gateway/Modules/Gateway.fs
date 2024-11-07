namespace Discordfs.Gateway.Modules

open Discordfs.Gateway.Types
open Discordfs.Types
open System.Text.Json

module Gateway =
    let readNext ws = task {
        let! res = Websocket.readNext ws

        match res with
        | WebsocketReadResponse.Close code -> return Error (Option.map enum<GatewayCloseEventCode> code)
        | WebsocketReadResponse.Message message -> return Ok (Json.deserializeF<GatewayReceiveEvent> message)
    }

    let identify (payload: IdentifySendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.IDENTIFY, Data = payload) |> Json.serializeF
        )
        
    let resume (payload: ResumeSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.RESUME, Data = payload) |> Json.serializeF
        )

    let heartbeat (payload: HeartbeatSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.HEARTBEAT, Data = payload) |> Json.serializeF
        )

    let requestGuildMembers (payload: RequestGuildMembersSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.REQUEST_GUILD_MEMBERS, Data = payload) |> Json.serializeF
        )

    let requestSoundboardSounds (payload: RequestSoundboardSoundsSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.REQUEST_SOUNDBOARD_SOUNDS, Data = payload) |> Json.serializeF
        )

    let updateVoiceState (payload: UpdateVoiceStateSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.VOICE_STATE_UPDATE, Data = payload) |> Json.serializeF
        )

    let updatePresence (payload: UpdatePresenceSendEvent) ws =
        ws |> Websocket.write (
            GatewayEventPayload.build(Opcode = GatewayOpcode.PRESENCE_UPDATE, Data = payload) |> Json.serializeF
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
