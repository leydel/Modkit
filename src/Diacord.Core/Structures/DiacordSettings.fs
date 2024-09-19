namespace Modkit.Diacord.Core.Structures

open System.Text.Json.Serialization

type DiacordSettings = {
    [<JsonName "strict_roles">] StrictRoles: bool option
    [<JsonName "strict_emojis">] StrictEmojis: bool option
    [<JsonName "strict_stickers">] StrictStickers: bool option
    [<JsonName "strict_channels">] StrictChannels: bool option
}
