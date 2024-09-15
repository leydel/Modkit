namespace Modkit.Diacord.Core.Structures

open System.Text.Json.Serialization

type DiacordTemplate = {
    [<JsonName "settings">]
    Settings: DiacordSettings option
    
    [<JsonName "roles">]
    Roles: DiacordRole list option
    
    [<JsonName "emojis">]
    Emojis: DiacordEmoji list option

    [<JsonName "stickers">]
    Stickers: DiacordSticker list option

    [<JsonName "channels">]
    Channels: DiacordChannel list option

    // TODO: Figure out way to allow channels to be defined within categories (discriminated union probably)
}
