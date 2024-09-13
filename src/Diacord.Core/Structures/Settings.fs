namespace Modkit.Diacord.Core.Structures

type DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute
open System.Text.Json.Serialization

type Settings = {
    [<JsonPropertyName("strict_roles")>]
    [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>] // TODO: Test this
    [<DefaultValue(false)>]
    StrictRoles: bool
}
