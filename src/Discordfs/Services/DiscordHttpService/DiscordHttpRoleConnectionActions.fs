namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpRoleConnectionActions =
    abstract member GetApplicationRoleConnectionMetadataRecords:
        applicationId: string ->
        Task<ApplicationRoleConnectionMetadata list>
        
    // TODO: Double check the payload type for this
    abstract member UpdateApplicationRoleConnectionMetadataRecords:
        applicationId: string ->
        payload: ApplicationRoleConnectionMetadata list ->
        Task<ApplicationRoleConnectionMetadata list>
        
    abstract member GetCurrentUserApplicationRoleConnection:
        applicationId: string ->
        oauth2AccessToken: string ->
        Task<ApplicationRoleConnection>

    // TODO: Double check the payload type for this
    abstract member UpdateCurrentUserApplicationRoleConnection:
        applicationId: string ->
        oauth2AccessToken: string ->
        payload: ApplicationRoleConnection ->
        Task<ApplicationRoleConnection>

 type DiscordHttpRoleConnectionActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpRoleConnectionActions with
        member _.GetApplicationRoleConnectionMetadataRecords applicationId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"/applications/{applicationId}/role-connections/metadata"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.UpdateApplicationRoleConnectionMetadataRecords applicationId payload =
            Req.create
                HttpMethod.Put
                Constants.DISCORD_API_URL
                $"/applications/{applicationId}/role-connections/metadata"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.GetCurrentUserApplicationRoleConnection applicationId token =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"/users/@me/applications/{applicationId}/role-connection"
            |> Req.oauth token
            |> Req.send httpClientFactory
            |> Res.body

        member _.UpdateCurrentUserApplicationRoleConnection applicationId token payload =
            Req.create
                HttpMethod.Put
                Constants.DISCORD_API_URL
                $"/users/@me/applications/{applicationId}/role-connection"
            |> Req.oauth token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body
