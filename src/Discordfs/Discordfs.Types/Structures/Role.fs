namespace Discordfs.Types

open System.Text.Json
open System.Text.Json.Serialization

type RoleTags = {
    [<JsonPropertyName "bot_id">] BotId: string option
    [<JsonPropertyName "integration_id">] IntegrationId: string option
    [<JsonPropertyName "premium_subscriber">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] PremiumSubscriber: unit option
    [<JsonPropertyName "subscription_listing_id">] SubscriptionListingId: string option
    [<JsonPropertyName "available_for_purchase">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] AvailableForPurchase: unit option
    [<JsonPropertyName "guild_connections">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] GuildConnections: unit option
}

type Role = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "color">] Color: int
    [<JsonPropertyName "hoist">] Hoist: bool
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "unicode_emoji">] UnicodeEmoji: string option
    [<JsonPropertyName "position">] Position: int
    [<JsonPropertyName "permissions">] Permissions: string
    [<JsonPropertyName "managed">] Managed: bool
    [<JsonPropertyName "mentionable">] Mentionable: bool
    [<JsonPropertyName "tags">] Tags: RoleTags option
    [<JsonPropertyName "flags">] Flags: int
}
