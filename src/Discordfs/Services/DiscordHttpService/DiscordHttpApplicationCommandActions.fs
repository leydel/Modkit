namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpApplicationCommandActions =
    abstract member CreateGlobalApplicationCommand:
        applicationId: string ->
        name: string ->
        nameLocalizations: IDictionary<string, string> option ->
        description: string ->
        descriptionLocalizations: IDictionary<string, string> option ->
        options: ApplicationCommandOption list option ->
        defaultMemberPermissions: string option ->
        dmPermissions: bool option ->
        integrationTypes: ApplicationIntegrationType list option ->
        ``type``: ApplicationCommandType ->
        nsfw: bool option ->
        Task<ApplicationCommand>

    abstract member BulkOverwriteGlobalApplicationCommands:
        applicationId: string -> 
        payload: ApplicationCommand list ->
        Task<ApplicationCommand list>

    // TODO: Check above types for what needs to be `option` or not

    // TODO: Implement remaining endpoints

type DiscordHttpApplicationCommandActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpApplicationCommandActions with
        member _.CreateGlobalApplicationCommand
            applicationId name nameLocalizations description descriptionLocalizations options defaultMemberPermissions
            dmPermissions integrationTypes ``type`` nsfw =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/commands"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.property "name" name
                    |> Dto.propertyIf "name_localizations" nameLocalizations
                    |> Dto.property "description" description
                    |> Dto.propertyIf "description_localizations" descriptionLocalizations
                    |> Dto.propertyIf "options" options
                    |> Dto.propertyIf "default_member_permissions" defaultMemberPermissions
                    |> Dto.propertyIf "dm_permissions" dmPermissions
                    |> Dto.propertyIf "integration_types" integrationTypes
                    |> Dto.property "type" ``type``
                    |> Dto.propertyIf "nsfw" nsfw
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.BulkOverwriteGlobalApplicationCommands
            applicationId payload =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/commands"
                |> Req.bot token
                |> Req.json (FsJson.serialize payload)
                |> Req.send httpClientFactory
                |> Res.json
