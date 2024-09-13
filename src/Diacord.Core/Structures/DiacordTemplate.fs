namespace Modkit.Diacord.Core.Structures

open System.Text.Json.Serialization

type DiacordTemplate = {
    [<JsonPropertyName("settings")>]
    Settings: DiacordSettings option

    [<JsonPropertyName("roles")>]
    Roles: DiacordRole list option
}
