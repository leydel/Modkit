namespace Modkit.Diacord.Core.Structures

open System.Text.Json.Serialization

type DiacordSettings = {
    [<JsonName "strict_roles">]
    [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>] // TODO: Test this
    [<System.ComponentModel.DefaultValue false>]
    StrictRoles: bool

    [<JsonName "strict_emojis">]
    [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>] // TODO: Test this
    [<System.ComponentModel.DefaultValue false>]
    StrictEmojis: bool

    [<JsonName "strict_stickers">]
    [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>] // TODO: Test this
    [<System.ComponentModel.DefaultValue false>]
    StrictStickers: bool

    [<JsonName "strict_channels">]
    [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>] // TODO: Test this
    [<System.ComponentModel.DefaultValue false>]
    StrictChannels: bool
}
