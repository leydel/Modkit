namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Collections.Generic
open System.Threading.Tasks

type UpdateApplicationRoleConnectionMetadataRecords (
    ``type``: ApplicationRoleConnectionMetadataType,
    key: string,
    name: string,
    description: string,
    ?name_localizations: IDictionary<string, string>,
    ?description_localizations: IDictionary<string, string>
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            required "type" ``type``
            required "key" key
            required "name" name
            optional "name_localizations" name_localizations
            required "description" description
            optional "description_localizations" description_localizations
        }

type IRoleConnectionResource =
    abstract member GetApplicationRoleConnectionMetadataRecords:
        applicationId: string ->
        Task<ApplicationRoleConnectionMetadata list>
        
    abstract member UpdateApplicationRoleConnectionMetadataRecords:
        applicationId: string ->
        content: UpdateApplicationRoleConnectionMetadataRecords ->
        Task<ApplicationRoleConnectionMetadata list>

 type RoleConnectionResource (httpClientFactory, token) =
    interface IRoleConnectionResource with
        member _.GetApplicationRoleConnectionMetadataRecords applicationId =
            req {
                get $"applications/{applicationId}/role-connections/metadata"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.UpdateApplicationRoleConnectionMetadataRecords applicationId content =
            req {
                put $"applications/{applicationId}/role-connections/metadata"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
