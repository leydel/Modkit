namespace Discordfs.Types

open System.Text.Json.Serialization

type AvatarDecorationData = {
    [<JsonPropertyName "asset">] Asset: string
    [<JsonPropertyName "sku_id">] SkuId: string
}

type User = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "username">] Username: string
    [<JsonPropertyName "discriminator">] Discriminator: string
    [<JsonPropertyName "global_name">] GlobalName: string option
    [<JsonPropertyName "avatar">] Avatar: string option
    [<JsonPropertyName "bot">] Bot: bool option
    [<JsonPropertyName "system">] System: bool option
    [<JsonPropertyName "mfa_enabled">] MfaEnabled: bool option
    [<JsonPropertyName "banner">] Banner: string option
    [<JsonPropertyName "accent_color">] AccentColor: int option
    [<JsonPropertyName "locale">] Locale: string option
    [<JsonPropertyName "verified">] Verified: bool option
    [<JsonPropertyName "email">] Email: string option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "premium_type">] PremiumType: UserPremiumType option
    [<JsonPropertyName "public_flags">] PublicFlags: int option
    [<JsonPropertyName "avatar_decoration_data">] AvatarDecorationData: AvatarDecorationData option
}

type PartialUser = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "username">] Username: string option
    [<JsonPropertyName "discriminator">] Discriminator: string option
    [<JsonPropertyName "global_name">] GlobalName: string option
    [<JsonPropertyName "avatar">] Avatar: string option
    [<JsonPropertyName "bot">] Bot: bool option
    [<JsonPropertyName "system">] System: bool option
    [<JsonPropertyName "mfa_enabled">] MfaEnabled: bool option
    [<JsonPropertyName "banner">] Banner: string option
    [<JsonPropertyName "accent_color">] AccentColor: int option
    [<JsonPropertyName "locale">] Locale: string option
    [<JsonPropertyName "verified">] Verified: bool option
    [<JsonPropertyName "email">] Email: string option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "premium_type">] PremiumType: UserPremiumType option
    [<JsonPropertyName "public_flags">] PublicFlags: int option
    [<JsonPropertyName "avatar_decoration_data">] AvatarDecorationData: AvatarDecorationData option
}
