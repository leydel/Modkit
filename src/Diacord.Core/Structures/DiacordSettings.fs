namespace Modkit.Diacord.Core.Structures

open System.Text.Json.Serialization

type DiacordSettings = {
    [<JsonName "strict_roles">] StrictRoles: bool
    [<JsonName "strict_emojis">] StrictEmojis: bool
    [<JsonName "strict_stickers">] StrictStickers: bool
    [<JsonName "strict_channels">] StrictChannels: bool
}
