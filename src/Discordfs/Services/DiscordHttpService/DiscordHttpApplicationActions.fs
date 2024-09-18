namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpApplicationActions =
    // https://discord.com/developers/docs/resources/application#get-current-application
    abstract member GetCurrentApplication:
        unit ->
        Task<Application>

    // https://discord.com/developers/docs/resources/application#edit-current-application
    abstract member EditCurrentApplication:
        customInstallUrl: string option ->
        description: string option ->
        roleConnectionVerificationUrl: string option ->
        installParams: OAuth2InstallParams option ->
        integrationTypesConfig: IDictionary<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option ->
        flags: int option ->
        icon: string option ->
        coverImage: string option ->
        interactionsEndpointUrl: string option ->
        tags: string list option ->
        Task<Application>

    // https://discord.com/developers/docs/resources/application#get-application-activity-instance
    abstract member GetApplicationActivityInstance:
        applicationId: string ->
        instanceId: string ->
        Task<ActivityInstance>

type DiscordHttpApplicationActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpApplicationActions with
        member _.GetCurrentApplication
            () =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"applications/@me"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.EditCurrentApplication
            customInstallUrl description roleConnectionVerificationUrl installParams integrationTypesConfig flags icon
            coverImage interactionsEndpointUrl tags =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"applications/@me"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "custom_install_url" customInstallUrl
                    |> Dto.propertyIf "description" description
                    |> Dto.propertyIf "role_connection_verification_url" roleConnectionVerificationUrl
                    |> Dto.propertyIf "install_params" installParams
                    |> Dto.propertyIf "integration_types_config" integrationTypesConfig
                    |> Dto.propertyIf "flags" flags
                    |> Dto.propertyIf "icon" icon
                    |> Dto.propertyIf "cover_image" coverImage
                    |> Dto.propertyIf "interactions_endpoint_url" interactionsEndpointUrl
                    |> Dto.propertyIf "tags" tags
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetApplicationActivityInstance
            applicationId instanceId =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/activity-instances/{instanceId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json
            