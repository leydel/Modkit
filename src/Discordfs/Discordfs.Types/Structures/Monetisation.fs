namespace Discordfs.Types

open System
open System.Text.Json.Serialization

type Entitlement = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "sku_id">] SkuId: string
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "user_id">] UserId: string option
    [<JsonPropertyName "type">] Type: EntitlementType
    [<JsonPropertyName "deleted">] Deleted: bool
    [<JsonPropertyName "starts_at">] StartsAt: DateTime option
    [<JsonPropertyName "ends_at">] EndsAt: DateTime option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "consumed">] Consumed: bool option
}

// https://discord.com/developers/docs/resources/sku#sku-object-sku-structure
type Sku = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: SkuType
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "slug">] Slug: string
    [<JsonPropertyName "flags">] Flags: int
}

// https://discord.com/developers/docs/resources/subscription#subscription-object
type Subscription = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "sku_id">] SkuId: string
    [<JsonPropertyName "entitlement_ids">] EntitlmentIds: string list
    [<JsonPropertyName "current_period_start">] CurrentPeriodStart: DateTime
    [<JsonPropertyName "current_period_end">] CurrentPeriodEnd: DateTime
    [<JsonPropertyName "status">] Status: SubscriptionStatusType
    [<JsonPropertyName "created_at">] CanceledAt: DateTime option
    [<JsonPropertyName "country">] Country: string option
}
