namespace Discordfs.Gateway.Types

open Discordfs.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<GatewaySendEventConverter>)>]
type GatewaySendEvent =
    | IDENTIFY                  of GatewayEventPayload<IdentifySendEvent>
    | RESUME                    of GatewayEventPayload<ResumeSendEvent>
    | HEARTBEAT                 of GatewayEventPayload<HeartbeatSendEvent>
    | REQUEST_GUILD_MEMBERS     of GatewayEventPayload<RequestGuildMembersSendEvent>
    | REQUEST_SOUNDBOARD_SOUNDS of GatewayEventPayload<RequestSoundboardSoundsSendEvent>
    | UPDATE_VOICE_STATE        of GatewayEventPayload<UpdateVoiceStateSendEvent>
    | UPDATE_PRESENCE           of GatewayEventPayload<UpdatePresenceSendEvent>

and GatewaySendEventConverter () =
    inherit JsonConverter<GatewaySendEvent> ()

    override __.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue(&reader)
        if not success then raise (JsonException())

        let opcode =
            document.RootElement.GetProperty "op"
            |> _.GetInt32()
            |> enum<GatewayOpcode>

        let json = document.RootElement.GetRawText()

        match opcode with
        | GatewayOpcode.IDENTIFY -> IDENTIFY <| Json.deserializeF json
        | GatewayOpcode.RESUME -> RESUME <| Json.deserializeF json
        | GatewayOpcode.HEARTBEAT -> HEARTBEAT <| Json.deserializeF json
        | GatewayOpcode.REQUEST_GUILD_MEMBERS -> REQUEST_GUILD_MEMBERS <| Json.deserializeF json
        | GatewayOpcode.REQUEST_SOUNDBOARD_SOUNDS -> REQUEST_SOUNDBOARD_SOUNDS <| Json.deserializeF json
        | GatewayOpcode.VOICE_STATE_UPDATE -> UPDATE_VOICE_STATE <| Json.deserializeF json
        | GatewayOpcode.PRESENCE_UPDATE -> UPDATE_PRESENCE <| Json.deserializeF json
        | _ -> failwith "Unexpected GatewayOpcode provided" // TODO: Handle gracefully for unfamiliar events
                
    override __.Write (writer, value, options) =
        match value with
        | IDENTIFY i -> Json.serializeF i |> writer.WriteRawValue
        | RESUME r -> Json.serializeF r |> writer.WriteRawValue
        | HEARTBEAT h -> Json.serializeF h |> writer.WriteRawValue
        | REQUEST_GUILD_MEMBERS r -> Json.serializeF r |> writer.WriteRawValue
        | REQUEST_SOUNDBOARD_SOUNDS r -> Json.serializeF r |> writer.WriteRawValue
        | UPDATE_VOICE_STATE u -> Json.serializeF u |> writer.WriteRawValue
        | UPDATE_PRESENCE u -> Json.serializeF u |> writer.WriteRawValue
