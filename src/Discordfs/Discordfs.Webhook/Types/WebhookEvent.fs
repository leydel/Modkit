namespace Discordfs.Webhook.Types

open Discordfs.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<WebhookEventConverter>)>]
type WebhookEvent =
    | PING                   of WebhookEventPayload<Empty>
    | ENTITLEMENT_CREATE     of WebhookEventPayload<WebhookEventBody<EntitlementCreateEvent>>
    | APPLICATION_AUTHORIZED of WebhookEventPayload<WebhookEventBody<ApplicationAuthorizedEvent>>

and WebhookEventConverter () =
    inherit JsonConverter<WebhookEvent> ()

    override __.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue(&reader)
        if not success then raise (JsonException())

        let webhookType =
            document.RootElement.GetProperty "type"
            |> _.GetInt32()
            |> enum<WebhookPayloadType>

        let webhookEventType =
            try
                document.RootElement.GetProperty "data"
                |> _.GetProperty("type")
                |> _.GetString()
                |> WebhookEventType.FromString
            with | _ ->
                None

        let json = document.RootElement.GetRawText()

        match webhookType, webhookEventType with
        | WebhookPayloadType.PING, _ -> PING <| Json.deserializeF json
        | WebhookPayloadType.EVENT, Some WebhookEventType.ENTITLEMENT_CREATE -> ENTITLEMENT_CREATE <| Json.deserializeF json
        | WebhookPayloadType.EVENT, Some WebhookEventType.APPLICATION_AUTHORIZED -> APPLICATION_AUTHORIZED <| Json.deserializeF json
        | _ -> failwith "Unexpected WebhookPayloadType and/or WebhookEventType provided" // TODO: Handle gracefully for unfamiliar events
                
    override __.Write (writer, value, options) =
        match value with
        | PING p -> Json.serializeF p |> writer.WriteRawValue
        | ENTITLEMENT_CREATE e -> Json.serializeF e |> writer.WriteRawValue
        | APPLICATION_AUTHORIZED a -> Json.serializeF a |> writer.WriteRawValue
        