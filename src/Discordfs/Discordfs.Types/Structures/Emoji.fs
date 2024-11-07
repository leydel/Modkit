namespace Discordfs.Types

open System.Text.Json.Serialization

type Emoji = {
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "roles">] Roles: string list option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "require_colons">] RequireColons: bool option
    [<JsonPropertyName "managed">] Managed: bool option
    [<JsonPropertyName "animated">] Animated: bool option
    [<JsonPropertyName "available">] Available: bool option
}

type PartialEmoji = Emoji // All emoji properties are already optional
