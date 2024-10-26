namespace Discordfs.Webhook.Types

open Discordfs.Types
open System
open System.Text.Json
open System.Text.Json.Serialization

// https://discord.com/developers/docs/events/webhook-events#webhook-types
type WebhookType =
    | PING = 0
    | EVENT = 1

// https://discord.com/developers/docs/events/webhook-events#event-types
type WebhookEventType =
    | APPLICATION_AUTHORIZED
    | ENTITLEMENT_CREATE
with
    override this.ToString () =
        match this with
        | WebhookEventType.APPLICATION_AUTHORIZED -> "APPLICATION_AUTHORIZED"
        | WebhookEventType.ENTITLEMENT_CREATE -> "ENTITLEMENT_CREATE"

    static member FromString (str: string) =
        match str with
        | "APPLICATION_AUTHORIZED" -> Some WebhookEventType.APPLICATION_AUTHORIZED
        | "ENTITLEMENT_CREATE" -> Some WebhookEventType.ENTITLEMENT_CREATE
        | _ -> None

// https://discord.com/developers/docs/events/webhook-events#event-body-object
type WebhookEventBody<'a> = {
    [<JsonPropertyName "type">] Type: WebhookEventType
    [<JsonPropertyName "timestamp">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] Timestamp: DateTime
    [<JsonPropertyName "data">] Data: 'a
}

// https://discord.com/developers/docs/events/webhook-events#payload-structure
type WebhookEventPayload<'a> = {
    [<JsonPropertyName "version">] Version: int
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "type">] Type: WebhookType
    [<JsonPropertyName "event">] Event: 'a
}

// https://discord.com/developers/docs/events/webhook-events#application-authorized-application-authorized-structure
type ApplicationAuthorized = {
    [<JsonPropertyName "integration_type">] IntegrationType: ApplicationIntegrationType option
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "scopes">] Scopes: OAuth2Scope list
    [<JsonPropertyName "guild">] Guild: Guild option
}

// https://discord.com/developers/docs/events/webhook-events#entitlement-create-entitlement-create-structure
type EntitlementCreate = Entitlement
