namespace Modkit.Roles.Types

open System.Text.Json.Serialization

type RoleApp = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "token">] Token: string
}
