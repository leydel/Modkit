namespace Modkit.Discordfs.Endpoints

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks

type Nullable<'a> = 'a option
type Optional<'a> = 'a option

type CreateGlobalApplicationCommandPayload (
    name, ?description, ?name_localizations, ?description_localizations, ?options, ?default_member_permissions,
    ?dm_permission, ?default_permission, ?integration_types, ?``type``, ?nsfw
) =
    member val Name: string = name with get, set
    member val NameLocalizations: Optional<Nullable<IDictionary<string, string>>> = name_localizations with get, set
    member val Description: Optional<string> = description with get, set
    member val DescriptionLocalizations: Optional<Nullable<IDictionary<string, string>>> = description_localizations with get, set
    member val Options: Optional<ApplicationCommandOption list> = options with get, set
    member val DefaultMemberPermissions: Optional<Nullable<string>> = default_member_permissions with get, set
    member val DmPermission: Optional<Nullable<bool>> = dm_permission with get, set
    member val DefaultPermission: Optional<bool> = default_permission with get, set
    member val IntegrationTypes: Optional<ApplicationIntegrationType list> = integration_types with get, set
    member val Type: Optional<ApplicationCommandType> = ``type`` with get, set
    member val Nsfw: Optional<bool> = nsfw with get, set
    
    override this.ToString () =
        Dto()
        |> Dto.property   (nameof(name)) this.Name
        |> Dto.propertyIf (nameof(name_localizations)) this.NameLocalizations
        |> Dto.propertyIf (nameof(description)) this.Description
        |> Dto.propertyIf (nameof(description_localizations)) this.DescriptionLocalizations
        |> Dto.propertyIf (nameof(options)) this.Options
        |> Dto.propertyIf (nameof(default_member_permissions)) this.DefaultMemberPermissions
        |> Dto.propertyIf (nameof(dm_permission)) this.DmPermission
        |> Dto.propertyIf (nameof(default_permission)) this.DefaultPermission
        |> Dto.propertyIf (nameof(integration_types)) this.IntegrationTypes
        |> Dto.propertyIf (nameof(``type``)) this.Type
        |> Dto.propertyIf (nameof(nsfw)) this.Nsfw
        |> Dto.json

type ICreateGlobalApplicationCommand =
    abstract member Run:
        applicationId: string ->
        payload: CreateGlobalApplicationCommandPayload ->
        Task<ApplicationCommand>

type CreateGlobalApplicationCommand (httpClientFactory, token) =
    interface ICreateGlobalApplicationCommand with
        member _.Run
            applicationId payload =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/commands"
                |> Req.bot token
                |> Req.json (payload.ToString())
                |> Req.send httpClientFactory
                |> Res.json
