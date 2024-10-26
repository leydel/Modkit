namespace Discordfs.Webhook.Payloads

open Discordfs.Webhook.Common
open Discordfs.Webhook.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<WebhookEventConverter>)>]
type WebhookEvent =
    | Ping of WebhookEventPayload<Empty>
    | EntitlementCreate of WebhookEventPayload<WebhookEventBody<EntitlementCreate>>
    | ApplicationAuthorized of WebhookEventPayload<WebhookEventBody<ApplicationAuthorized>>

and WebhookEventConverter () =
    inherit JsonConverter<WebhookEvent> ()

    override __.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue(&reader)
        if not success then raise (JsonException())

        let webhookType =
            document.RootElement.GetProperty "type"
            |> _.GetInt32()
            |> enum<WebhookType>

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
        | WebhookType.PING, _ -> Ping <| Json.deserializeF json
        | WebhookType.EVENT, Some ENTITLEMENT_CREATE -> EntitlementCreate <| Json.deserializeF json
        | WebhookType.EVENT, Some APPLICATION_AUTHORIZED -> ApplicationAuthorized <| Json.deserializeF json
        | _ -> failwith "Unexpected WebhookType and/or WebhookEventType provided"
                
    override __.Write (writer, value, options) =
        match value with
        | Ping p -> Json.serializeF p |> writer.WriteRawValue
        | EntitlementCreate e -> Json.serializeF e |> writer.WriteRawValue
        | ApplicationAuthorized a -> Json.serializeF a |> writer.WriteRawValue
        