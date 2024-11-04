namespace Discordfs.Core

open Discordfs.Gateway.Payloads
open Discordfs.Webhook.Payloads
open System.Text.Json
open System.Text.Json.Serialization

type DiscordEventType =
    | GATEWAY = 0
    | INTERACTION = 1
    | WEBHOOK = 2

type DiscordEventPayload<'a> = {
    [<JsonPropertyName "type">] Type: DiscordEventType
    [<JsonPropertyName "data">] Data: 'a
}

type DiscordEvent =
    | GATEWAY     of DiscordEventPayload<GatewayReceiveEvent>
    | INTERACTION of DiscordEventPayload<InteractionReceiveEvent>
    | WEBHOOK     of DiscordEventPayload<WebhookEvent>

and DiscordEventConverter () =
    inherit JsonConverter<DiscordEvent> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue(&reader)
        if not success then raise (JsonException())

        let eventType =
            document.RootElement.GetProperty "type"
            |> _.GetInt32()
            |> enum<DiscordEventType>

        let json = document.RootElement.GetRawText()

        match eventType with
        | DiscordEventType.GATEWAY -> GATEWAY <| Json.deserializeF json
        | DiscordEventType.INTERACTION -> INTERACTION <| Json.deserializeF json
        | DiscordEventType.WEBHOOK -> WEBHOOK <| Json.deserializeF json
        | _ -> failwith "Unexpected DiscordEventType provided" // TODO: Handle gracefully for unfamiliar events

    override _.Write (writer, value, options) =
        match value with
        | GATEWAY g -> Json.serializeF g |> writer.WriteRawValue
        | INTERACTION i -> Json.serializeF i |> writer.WriteRawValue
        | WEBHOOK w -> Json.serializeF w |> writer.WriteRawValue
