namespace Modkit.Roles.Types

open Discordfs.Types
open System.Text.Json.Serialization

type RoleApp = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "token">] Token: string
    [<JsonPropertyName "publicKey">] PublicKey: string
    [<JsonPropertyName "metadata">] Metadata: ApplicationRoleConnectionMetadata list
}
