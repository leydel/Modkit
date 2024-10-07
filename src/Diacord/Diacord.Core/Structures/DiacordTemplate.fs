namespace Modkit.Diacord.Core.Structures

open System.Text.Json.Serialization

type DiacordTemplate = {
    [<JsonPropertyName "settings">] Settings: DiacordSettings option
    [<JsonPropertyName "roles">] Roles: DiacordRole list option
    [<JsonPropertyName "emojis">] Emojis: DiacordEmoji list option
    [<JsonPropertyName "stickers">] Stickers: DiacordSticker list option
    [<JsonPropertyName "channels">] Channels: DiacordGenericChannel list option
}
