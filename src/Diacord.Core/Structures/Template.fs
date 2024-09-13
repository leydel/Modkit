namespace Modkit.Diacord.Core.Structures

open System.Text.Json.Serialization

type Template = {
    [<JsonPropertyName("settings")>]
    Settings: Settings option

    [<JsonPropertyName("roles")>]
    Roles: Role list option
}
