namespace Modkit.Roles.Types

open Discordfs.Types
open System.Collections.Generic
open System.Text.Json.Serialization

type Condition = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "applicationId">] ApplicationId: string
    [<JsonPropertyName "type">] Type: ApplicationRoleConnectionMetadataType
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "nameLocalizations">] NameLocalizations: IDictionary<string, string> option
    [<JsonPropertyName "descriptionLocalizations">] DescriptionLocalizations: IDictionary<string, string> option
}

module Condition =
    let toApplicationRoleConnectionMetadata (condition: Condition): ApplicationRoleConnectionMetadata = {
        Type = condition.Type
        Key = condition.Id
        Name = condition.Name
        Description = condition.Description
        NameLocalizations = condition.NameLocalizations
        DescriptionLocalizations = condition.DescriptionLocalizations
    }
