namespace Discordfs.Gateway.Payloads

open Discordfs.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<GatewaySendEventConverter>)>]
type GatewaySendEvent =
    | Identify of GatewayEventPayload<Identify>
    | Resume of GatewayEventPayload<Resume>
    | Heartbeat of GatewayEventPayload<Heartbeat>
    | RequestGuildMembers of GatewayEventPayload<RequestGuildMembers>
    | UpdateVoiceState of GatewayEventPayload<UpdateVoiceState>
    | UpdatePresence of GatewayEventPayload<UpdatePresence>

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
        | GatewayOpcode.IDENTIFY -> Identify <| Json.deserializeF json
        | GatewayOpcode.RESUME -> Resume <| Json.deserializeF json
        | GatewayOpcode.HEARTBEAT -> Heartbeat <| Json.deserializeF json
        | GatewayOpcode.REQUEST_GUILD_MEMBERS -> RequestGuildMembers <| Json.deserializeF json
        | GatewayOpcode.VOICE_STATE_UPDATE -> UpdateVoiceState <| Json.deserializeF json
        | GatewayOpcode.PRESENCE_UPDATE -> UpdatePresence <| Json.deserializeF json
        | _ -> failwith "Unexpected GatewayOpcode provided"
                
    override __.Write (writer, value, options) =
        match value with
        | Identify i -> Json.serializeF i |> writer.WriteRawValue
        | Resume r -> Json.serializeF r |> writer.WriteRawValue
        | Heartbeat h -> Json.serializeF h |> writer.WriteRawValue
        | RequestGuildMembers r -> Json.serializeF r |> writer.WriteRawValue
        | UpdateVoiceState u -> Json.serializeF u |> writer.WriteRawValue
        | UpdatePresence u -> Json.serializeF u |> writer.WriteRawValue

// TODO: Determine if this is needed (seems like possibly not, if not used in a while, feel free to delete)
