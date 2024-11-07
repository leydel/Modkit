namespace Discordfs.Types

open System
open System.Text.Json.Serialization

type GuildMember = {
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "nick">] Nick: string option
    [<JsonPropertyName "avatar">] Avatar: string option
    [<JsonPropertyName "banner">] Banner: string option
    [<JsonPropertyName "roles">] Roles: string list
    [<JsonPropertyName "joined_at">] JoinedAt: DateTime option
    [<JsonPropertyName "premium_since">] PremiumSince: DateTime option
    [<JsonPropertyName "deaf">] Deaf: bool
    [<JsonPropertyName "mute">] Mute: bool
    [<JsonPropertyName "flags">] Flags: int
    [<JsonPropertyName "pending">] Pending: bool option
    [<JsonPropertyName "permissions">] Permissions: string option
    [<JsonPropertyName "communication_disabled_until">] CommunicationDisabledUntil: DateTime option
    [<JsonPropertyName "avatar_decoration_metadata">] AvatarDecorationData: AvatarDecorationData option
}

type PartialGuildMember = {
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "nick">] Nick: string option
    [<JsonPropertyName "avatar">] Avatar: string option
    [<JsonPropertyName "banner">] Banner: string option
    [<JsonPropertyName "roles">] Roles: string list option
    [<JsonPropertyName "joined_at">] JoinedAt: DateTime option
    [<JsonPropertyName "premium_since">] PremiumSince: DateTime option
    [<JsonPropertyName "deaf">] Deaf: bool option
    [<JsonPropertyName "mute">] Mute: bool option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "pending">] Pending: bool option
    [<JsonPropertyName "permissions">] Permissions: string option
    [<JsonPropertyName "communication_disabled_until">] CommunicationDisabledUntil: DateTime option
    [<JsonPropertyName "avatar_decoration_metadata">] AvatarDecorationData: AvatarDecorationData option
}
