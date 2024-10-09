namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Threading.Tasks

type GetGlobalApplicationCommandsResponse =
    | Ok of ApplicationCommand list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateGlobalApplicationCommand (
    name:                        string,
    ?name_localizations:         IDictionary<string, string> option,
    ?description:                string,
    ?description_localizations:  IDictionary<string, string> option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?integration_types:          ApplicationIntegrationType list,
    ?contexts:                   InteractionContextType list,
    ?``type``:                   ApplicationCommandType,
    ?nsfw:                       bool
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "name_localizations" name_localizations
            optional "description" description
            optional "description_localizations" description_localizations
            optional "options" options
            optional "default_member_permissions" default_member_permissions
            optional "integration_types" integration_types
            optional "contexts" contexts
            optional "type" ``type``
            optional "nsfw" nsfw
        }

type CreateGlobalApplicationCommandResponse =
    | Ok of ApplicationCommand
    | Created of ApplicationCommand
    | BadRequest of ErrorResponse
    | Conflict of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGlobalApplicationCommandResponse =
    | Ok of ApplicationCommand
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EditGlobalApplicationCommand (
    ?name:                       string,
    ?name_localizations:         IDictionary<string, string> option,
    ?description:                string,
    ?description_localizations:  IDictionary<string, string> option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?integration_types:          ApplicationIntegrationType list,
    ?contexts:                   InteractionContextType list,
    ?nsfw:                       bool
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "name_localizations" name_localizations
            optional "description" description
            optional "description_localizations" description_localizations
            optional "options" options
            optional "default_member_permissions" default_member_permissions
            optional "integration_types" integration_types
            optional "contexts" contexts
            optional "nsfw" nsfw
        }

type EditGlobalApplicationCommandResponse =
    | Ok of ApplicationCommand
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteGlobalApplicationCommandResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type BulkOverwriteGlobalApplicationCommands (
    commands: BulkOverwriteApplicationCommand list
) =
    inherit Payload() with
        override _.Content = JsonListPayload commands

type BulkOverwriteGlobalApplicationCommandsResponse =
    | Ok of ApplicationCommand list
    | BadRequest of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildApplicationCommandsResponse =
    | Ok of ApplicationCommand list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateGuildApplicationCommand (
    name:                        string,
    ?name_localizations:         IDictionary<string, string> option,
    ?description:                string,
    ?description_localizations:  IDictionary<string, string> option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?``type``:                   ApplicationCommandType,
    ?nsfw:                       bool
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "name_localizations" name_localizations
            optional "description" description
            optional "description_localizations" description_localizations
            optional "options" options
            optional "default_member_permissions" default_member_permissions
            optional "type" ``type``
            optional "nsfw" nsfw
        }

type CreateGuildApplicationCommandResponse =
    | Ok of ApplicationCommand
    | Created of ApplicationCommand
    | BadRequest of ErrorResponse
    | Conflict of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildApplicationCommandResponse =
    | Ok of ApplicationCommand
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EditGuildApplicationCommand (
    ?name:                       string,
    ?name_localizations:         IDictionary<string, string> option,
    ?description:                string,
    ?description_localizations:  IDictionary<string, string> option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?nsfw:                       bool
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "name_localizations" name_localizations
            optional "description" description
            optional "description_localizations" description_localizations
            optional "options" options
            optional "default_member_permissions" default_member_permissions
            optional "nsfw" nsfw
        }

type EditGuildApplicationCommandResponse =
    | Ok of ApplicationCommand
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteGuildApplicationCommandResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type BulkOverwriteGuildApplicationCommands (
    commands: BulkOverwriteApplicationCommand list
) =
    inherit Payload() with
        override _.Content = JsonListPayload commands

type BulkOverwriteGuildApplicationCommandsResponse =
    | Ok of ApplicationCommand list
    | BadRequest of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildApplicationCommandsPermissionsResponse =
    | Ok of GuildApplicationCommandPermissions list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildApplicationCommandPermissionsResponse =
    | Ok of GuildApplicationCommandPermissions
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EditApplicationCommandPermissions (
    permissions: ApplicationCommandPermission list
) =
    inherit Payload() with
        override _.Content = json {
            required "permissions" permissions
        }

type EditApplicationCommandPermissionsResponse =
    | Ok of GuildApplicationCommandPermissions
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type IApplicationCommandResource =
    // https://discord.com/developers/docs/interactions/application-commands#get-global-application-commands
    abstract member GetGlobalApplicationCommands:
        applicationId: string ->
        withLocalizations: bool option ->
        Task<GetGlobalApplicationCommandsResponse>

    // https://discord.com/developers/docs/interactions/application-commands#create-global-application-command
    abstract member CreateGlobalApplicationCommand:
        applicationId: string ->
        content: CreateGlobalApplicationCommand ->
        Task<CreateGlobalApplicationCommandResponse>

    // https://discord.com/developers/docs/interactions/application-commands#get-global-application-command
    abstract member GetGlobalApplicationCommand:
        applicationId: string ->
        commandId: string ->
        Task<GetGlobalApplicationCommandResponse>

    // https://discord.com/developers/docs/interactions/application-commands#edit-global-application-command
    abstract member EditGlobalApplicationCommand:
        applicationId: string ->
        commandId: string ->
        content: EditGlobalApplicationCommand ->
        Task<EditGlobalApplicationCommandResponse>

    // https://discord.com/developers/docs/interactions/application-commands#delete-global-application-command
    abstract member DeleteGlobalApplicationCommand:
        applicationId: string ->
        commandId: string ->
        Task<DeleteGlobalApplicationCommandResponse>

    // https://discord.com/developers/docs/interactions/application-commands#bulk-overwrite-global-application-commands
    abstract member BulkOverwriteGlobalApplicationCommands:
        applicationId: string -> 
        content: BulkOverwriteGlobalApplicationCommands ->
        Task<BulkOverwriteGlobalApplicationCommandsResponse>

    // https://discord.com/developers/docs/interactions/application-commands#get-guild-application-commands
    abstract member GetGuildApplicationCommands:
        applicationId: string ->
        guildId: string ->
        withLocalizations: bool option ->
        Task<GetGuildApplicationCommandsResponse>

    // https://discord.com/developers/docs/interactions/application-commands#create-guild-application-command
    abstract member CreateGuildApplicationCommand:
        applicationId: string ->
        guildId: string ->
        content: CreateGuildApplicationCommand ->
        Task<CreateGuildApplicationCommandResponse>

    // https://discord.com/developers/docs/interactions/application-commands#get-guild-application-command
    abstract member GetGuildApplicationCommand:
        applicationId: string ->
        guildId: string ->
        commandId: string ->
        Task<GetGuildApplicationCommandResponse>

    // https://discord.com/developers/docs/interactions/application-commands#edit-guild-application-command
    abstract member EditGuildApplicationCommand:
        applicationId: string ->
        guildId: string ->
        commandId: string ->
        content: EditGuildApplicationCommand ->
        Task<EditGuildApplicationCommandResponse>

    // https://discord.com/developers/docs/interactions/application-commands#delete-guild-application-command
    abstract member DeleteGuildApplicationCommand:
        applicationId: string ->
        guildId: string ->
        commandId: string ->
        Task<DeleteGuildApplicationCommandResponse>

    // https://discord.com/developers/docs/interactions/application-commands#bulk-overwrite-guild-application-commands
    abstract member BulkOverwriteGuildApplicationCommands:
        applicationId: string ->
        guildId: string ->
        content: BulkOverwriteGuildApplicationCommands ->
        Task<BulkOverwriteGuildApplicationCommandsResponse>

    // https://discord.com/developers/docs/interactions/application-commands#get-guild-application-command-permissions
    abstract member GetGuildApplicationCommandsPermissions:
        applicationId: string ->
        guildId: string ->
        Task<GetGuildApplicationCommandsPermissionsResponse>

    // https://discord.com/developers/docs/interactions/application-commands#get-application-command-permissions
    abstract member GetGuildApplicationCommandPermissions:
        applicationId: string ->
        guildId: string ->
        commandId: string ->
        Task<GetGuildApplicationCommandPermissionsResponse>

    // https://discord.com/developers/docs/interactions/application-commands#edit-application-command-permissions
    abstract member EditApplicationCommandPermissions:
        applicationId: string ->
        guildId: string ->
        commandId: string ->
        oauth2AccessToken: string ->
        content: EditApplicationCommandPermissions ->
        Task<EditApplicationCommandPermissionsResponse>

type ApplicationCommandResource (httpClientFactory, token) =
    interface IApplicationCommandResource with
        member _.GetGlobalApplicationCommands applicationId withLocalizations =
            req {
                get $"applications/{applicationId}/commands"
                bot token
                query "with_localizations" (withLocalizations >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGlobalApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGlobalApplicationCommandsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGlobalApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGlobalApplicationCommandsResponse.Other status
            })

        member _.CreateGlobalApplicationCommand applicationId content =
            req {
                post $"applications/{applicationId}/commands"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map CreateGlobalApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.Created -> return! Task.map CreateGlobalApplicationCommandResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateGlobalApplicationCommandResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.Conflict -> return! Task.map CreateGlobalApplicationCommandResponse.Conflict (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGlobalApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGlobalApplicationCommandResponse.Other status
            })

        member _.GetGlobalApplicationCommand applicationId commandId =
            req {
                get $"applications/{applicationId}/commands/{commandId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGlobalApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGlobalApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGlobalApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGlobalApplicationCommandResponse.Other status
            })

        member _.EditGlobalApplicationCommand applicationId commandId content =
            req {
                patch $"applications/{applicationId}/commands/{commandId}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditGlobalApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditGlobalApplicationCommandResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditGlobalApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditGlobalApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return EditGlobalApplicationCommandResponse.Other status
            })

        member _.DeleteGlobalApplicationCommand applicationId commandId =
            req {
                delete $"applications/{applicationId}/commands/{commandId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteGlobalApplicationCommandResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteGlobalApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteGlobalApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteGlobalApplicationCommandResponse.Other status
            })

        member _.BulkOverwriteGlobalApplicationCommands applicationId content =
            req {
                put $"applications/{applicationId}/commands"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map BulkOverwriteGlobalApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map BulkOverwriteGlobalApplicationCommandsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map BulkOverwriteGlobalApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return BulkOverwriteGlobalApplicationCommandsResponse.Other status
            })

        member _.GetGuildApplicationCommands applicationId guildId withLocalizations =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands"
                bot token
                query "with_localizations" (withLocalizations >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandsResponse.Other status
            })

        member _.CreateGuildApplicationCommand applicationId guildId content =
            req {
                post $"applications/{applicationId}/guilds/{guildId}/commands"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map CreateGuildApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.Created -> return! Task.map CreateGuildApplicationCommandResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateGuildApplicationCommandResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.Conflict -> return! Task.map CreateGuildApplicationCommandResponse.Conflict (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGuildApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGuildApplicationCommandResponse.Other status
            })

        member _.GetGuildApplicationCommand applicationId guildId commandId =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandResponse.Other status
            })

        member _.EditGuildApplicationCommand applicationId guildId commandId content =
            req {
                patch $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditGuildApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditGuildApplicationCommandResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditGuildApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditGuildApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return EditGuildApplicationCommandResponse.Other status
            })

        member _.DeleteGuildApplicationCommand applicationId guildId commandId =
            req {
                delete $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteGuildApplicationCommandResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteGuildApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteGuildApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteGuildApplicationCommandResponse.Other status
            })

        member _.BulkOverwriteGuildApplicationCommands applicationId guildId content =
            req {
                put $"applications/{applicationId}/guilds/{guildId}/commands"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map BulkOverwriteGuildApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map BulkOverwriteGuildApplicationCommandsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map BulkOverwriteGuildApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return BulkOverwriteGuildApplicationCommandsResponse.Other status
            })

        member _.GetGuildApplicationCommandsPermissions applicationId guildId =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands/permissions"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandsPermissionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandsPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandsPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandsPermissionsResponse.Other status
            })

        member _.GetGuildApplicationCommandPermissions applicationId guildId commandId =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandPermissionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandPermissionsResponse.Other status
            })

        member _.EditApplicationCommandPermissions applicationId guildId commandId oauth2AccessToken content =
            req {
                put $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
                oauth oauth2AccessToken
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditApplicationCommandPermissionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditApplicationCommandPermissionsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditApplicationCommandPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditApplicationCommandPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return EditApplicationCommandPermissionsResponse.Other status
            })
