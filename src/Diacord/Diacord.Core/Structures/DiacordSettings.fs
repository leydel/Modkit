namespace Modkit.Diacord.Core.Structures

open System.Text.Json.Serialization

type DiacordSettings = {
    [<JsonPropertyName "strict_roles">] StrictRoles: bool option
    [<JsonPropertyName "strict_emojis">] StrictEmojis: bool option
    [<JsonPropertyName "strict_stickers">] StrictStickers: bool option
    [<JsonPropertyName "strict_channels">] StrictChannels: bool option
}
