namespace Discordfs.Types

open System.Collections.Generic
open System.Text.Json.Serialization

#nowarn "49"

type ApplicationRoleConnectionMetadata = {
    [<JsonPropertyName "type">] Type: ApplicationRoleConnectionMetadataType
    [<JsonPropertyName "key">] Key: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: IDictionary<string, string> option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "description_localizations">] DescriptionLocalizations: IDictionary<string, string> option
}
with
    static member build(
        Type: ApplicationRoleConnectionMetadataType,
        Key: string,
        Name: string,
        Description: string,
        ?NameLocalizations: IDictionary<string, string>,
        ?DescriptionLocalizations: IDictionary<string, string>
    ) = {
        Type = Type;
        Key = Key;
        Name = Name;
        NameLocalizations = NameLocalizations;
        Description = Description;
        DescriptionLocalizations = DescriptionLocalizations;
    }

type ApplicationRoleConnection = {
    [<JsonPropertyName "platform_name">] PlatformName: string option
    [<JsonPropertyName "platform_username">] PlatformUsername: string option
    [<JsonPropertyName "metadata">] Metadata: ApplicationRoleConnectionMetadata
}
