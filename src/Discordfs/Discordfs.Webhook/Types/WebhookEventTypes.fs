namespace Discordfs.Webhook.Types

open Discordfs.Types
open System.Text.Json.Serialization

// https://discord.com/developers/docs/events/webhook-events#application-authorized-application-authorized-structure
type ApplicationAuthorizedEvent = {
    [<JsonPropertyName "integration_type">] IntegrationType: ApplicationIntegrationType option
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "scopes">] Scopes: OAuth2Scope list
    [<JsonPropertyName "guild">] Guild: Guild option
}

// https://discord.com/developers/docs/events/webhook-events#entitlement-create-entitlement-create-structure
type EntitlementCreateEvent = Entitlement
