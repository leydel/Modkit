namespace Modkit.Discordfs.Resources

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks

type IRoleConnectionResource =
    abstract member GetApplicationRoleConnectionMetadataRecords:
        applicationId: string ->
        Task<ApplicationRoleConnectionMetadata list>
        
    // TODO: Double check the payload type for this
    abstract member UpdateApplicationRoleConnectionMetadataRecords:
        applicationId: string ->
        ``type``: ApplicationRoleConnectionMetadataType ->
        key: string ->
        name: string ->
        nameLocalizations: IDictionary<string, string> option ->
        description: string ->
        descriptionLocalizations: IDictionary<string, string> option ->
        Task<ApplicationRoleConnectionMetadata list>
        
    abstract member GetCurrentUserApplicationRoleConnection:
        applicationId: string ->
        oauth2AccessToken: string ->
        Task<ApplicationRoleConnection>

    // TODO: Double check the payload type for this
    abstract member UpdateCurrentUserApplicationRoleConnection:
        applicationId: string ->
        oauth2AccessToken: string ->
        platformName: string option ->
        platformUsername: string option ->
        ``type``: ApplicationRoleConnectionMetadataType ->
        key: string ->
        name: string ->
        nameLocalizations: IDictionary<string, string> option ->
        description: string ->
        descriptionLocalizations: IDictionary<string, string> option ->
        Task<ApplicationRoleConnection>

    // TODO: CurrentUser endpoints not showing in documentation, may have been moved/removed?

 type RoleConnectionResource (httpClientFactory: IHttpClientFactory, token: string) =
    interface IRoleConnectionResource with
        member _.GetApplicationRoleConnectionMetadataRecords
            applicationId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"/applications/{applicationId}/role-connections/metadata"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.UpdateApplicationRoleConnectionMetadataRecords
            applicationId ``type`` key name nameLocalizations description descriptionLocalizations =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"/applications/{applicationId}/role-connections/metadata"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.property "type" ``type``
                    |> Dto.property "key" key
                    |> Dto.property "name" name
                    |> Dto.propertyIf "name_localizations" nameLocalizations
                    |> Dto.property "description" description
                    |> Dto.propertyIf "description_localizations" descriptionLocalizations
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetCurrentUserApplicationRoleConnection
            applicationId token =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"/users/@me/applications/{applicationId}/role-connection"
                |> Req.oauth token
                |> Req.send httpClientFactory
                |> Res.json

        member _.UpdateCurrentUserApplicationRoleConnection
            applicationId token platformName platformUsername ``type`` key name nameLocalizations description
            descriptionLocalizations =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"/users/@me/applications/{applicationId}/role-connection"
                |> Req.oauth token
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "platform_name" platformName
                    |> Dto.propertyIf "platform_username" platformUsername
                    |> Dto.property
                        "metadata"
                        {
                            Type = ``type``
                            Key = key
                            Name = name
                            NameLocalizations = nameLocalizations
                            Description = description
                            DescriptionLocalizations = descriptionLocalizations
                        }
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json
