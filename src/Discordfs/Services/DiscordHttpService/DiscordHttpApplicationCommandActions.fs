namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks

type CreateGlobalApplicationCommand (
    name:                        string,
    ?description:                string,
    ?name_localizations:         IDictionary<string, string> option,
    ?description_localizations:  IDictionary<string, string> option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?dm_permission:              bool option,
    ?default_permission:         bool,
    ?integration_types:          ApplicationIntegrationType list,
    ?``type``:                   ApplicationCommandType,
    ?nsfw:                       bool
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            required "name" name
            optional "name_localizations" name_localizations
            optional "description" description
            optional "description_localizations" description_localizations
            optional "options" options
            optional "default_member_permissions" default_member_permissions
            optional "dm_permission" dm_permission
            optional "default_permission" default_permission
            optional "integration_types" integration_types
            optional "type" ``type``
            optional "nsfw" nsfw
        }

type BulkOverwriteGlobalApplicationCommands (
    commands: ApplicationCommand list
) =
    inherit Payload(Json) with
        override _.Serialize () =
            FsJson.serialize commands

type IDiscordHttpApplicationCommandActions =
    abstract member CreateGlobalApplicationCommand:
        applicationId: string ->
        content: CreateGlobalApplicationCommand ->
        Task<ApplicationCommand>

    abstract member BulkOverwriteGlobalApplicationCommands:
        applicationId: string -> 
        content: BulkOverwriteGlobalApplicationCommands ->
        Task<ApplicationCommand list>

    // TODO: Implement remaining endpoints

type DiscordHttpApplicationCommandActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpApplicationCommandActions with
        member _.CreateGlobalApplicationCommand applicationId content =
            req {
                post $"applications/{applicationId}/commands"
                bot token
                payload Json content
            }
            |> Http.send httpClientFactory
            |> Http.toJson

        member _.BulkOverwriteGlobalApplicationCommands applicationId content =
            req {
                put $"applications/{applicationId}/commands"
                bot token
                payload Json content
            }
            |> Http.send httpClientFactory
            |> Http.toJson
