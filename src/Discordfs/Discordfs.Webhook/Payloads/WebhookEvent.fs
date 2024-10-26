namespace Discordfs.Webhook.Payloads

open Discordfs.Webhook.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<WebhookEventConverter>)>]
type WebhookEvent =
    | EntitlementCreate of WebhookEventPayload<EntitlementCreate>
    | ApplicationAuthorized of WebhookEventPayload<ApplicationAuthorized>

and WebhookEventConverter () =
    inherit JsonConverter<WebhookEvent> ()

    override __.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue(&reader)
        if not success then raise (JsonException())

        let webhookType =
            document.RootElement.GetProperty "data"
            |> _.GetProperty("type")
            |> _.GetString()
            |> WebhookEventType.FromString

        let json = document.RootElement.GetRawText()

        match webhookType with
        | Some ENTITLEMENT_CREATE -> EntitlementCreate <| Json.deserializeF json
        | Some APPLICATION_AUTHORIZED -> ApplicationAuthorized <| Json.deserializeF json
        | _ -> failwith "Unexpected WebhookEventType provided"
                
    override __.Write (writer, value, options) =
        match value with
        | EntitlementCreate e -> Json.serializeF e |> writer.WriteRawValue
        | ApplicationAuthorized a -> Json.serializeF a |> writer.WriteRawValue
        