namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Net.Http

type GetGlobalApplicationCommandsResponse =
    | Ok of ApplicationCommand list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateGlobalApplicationCommandPayload (
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
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGlobalApplicationCommandResponse =
    | Ok of ApplicationCommand
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EditGlobalApplicationCommandPayload (
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

type BulkOverwriteGlobalApplicationCommandsPayload (
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

type CreateGuildApplicationCommandPayload (
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
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildApplicationCommandResponse =
    | Ok of ApplicationCommand
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EditGuildApplicationCommandPayload (
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

module ApplicationCommand =
    let getGlobalApplicationCommands
        (applicationId: string)
        (withLocalizations: bool option)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/commands"
                bot botToken
                query "with_localizations" (withLocalizations >>. _.ToString())
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGlobalApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGlobalApplicationCommandsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGlobalApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGlobalApplicationCommandsResponse.Other status
            })
            
    let createGlobalApplicationCommand
        (applicationId: string)
        (content: CreateGlobalApplicationCommandPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"applications/{applicationId}/commands"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map CreateGlobalApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.Created -> return! Task.map CreateGlobalApplicationCommandResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateGlobalApplicationCommandResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGlobalApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGlobalApplicationCommandResponse.Other status
            })
            
    let getGlobalApplicationCommand
        (applicationId: string)
        (commandId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/commands/{commandId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGlobalApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGlobalApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGlobalApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGlobalApplicationCommandResponse.Other status
            })
            
    let editGlobalApplicationCommand
        (applicationId: string)
        (commandId: string)
        (content: EditGlobalApplicationCommandPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                patch $"applications/{applicationId}/commands/{commandId}"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditGlobalApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditGlobalApplicationCommandResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditGlobalApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditGlobalApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return EditGlobalApplicationCommandResponse.Other status
            })
            
    let deleteGlobalApplicationCommand
        (applicationId: string)
        (commandId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"applications/{applicationId}/commands/{commandId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteGlobalApplicationCommandResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteGlobalApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteGlobalApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteGlobalApplicationCommandResponse.Other status
            })
            
    let bulkOverwriteGlobalApplicationCommands
        (applicationId: string)
        (content: BulkOverwriteGlobalApplicationCommandsPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                put $"applications/{applicationId}/commands"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map BulkOverwriteGlobalApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map BulkOverwriteGlobalApplicationCommandsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map BulkOverwriteGlobalApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return BulkOverwriteGlobalApplicationCommandsResponse.Other status
            })
            
    let getGuildApplicationCommands
        (applicationId: string)
        (guildId: string)
        (withLocalizations: bool option)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands"
                bot botToken
                query "with_localizations" (withLocalizations >>. _.ToString())
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandsResponse.Other status
            })
            
    let createGuildApplicationCommand
        (applicationId: string)
        (guildId: string)
        (content: CreateGuildApplicationCommandPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"applications/{applicationId}/guilds/{guildId}/commands"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map CreateGuildApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.Created -> return! Task.map CreateGuildApplicationCommandResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateGuildApplicationCommandResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGuildApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGuildApplicationCommandResponse.Other status
            })
            
    let getGuildApplicationCommand
        (applicationId: string)
        (guildId: string)
        (commandId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandResponse.Other status
            })
            
    let editGuildApplicationCommand
        (applicationId: string)
        (guildId: string)
        (commandId: string)
        (content: EditGuildApplicationCommandPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                patch $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditGuildApplicationCommandResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditGuildApplicationCommandResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditGuildApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditGuildApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return EditGuildApplicationCommandResponse.Other status
            })
            
    let deleteGuildApplicationCommand
        (applicationId: string)
        (guildId: string)
        (commandId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteGuildApplicationCommandResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteGuildApplicationCommandResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteGuildApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteGuildApplicationCommandResponse.Other status
            })
            
    let bulkOverwriteGuildApplicationCommands
        (applicationId: string)
        (guildId: string)
        (content: BulkOverwriteGlobalApplicationCommandsPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                put $"applications/{applicationId}/guilds/{guildId}/commands"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map BulkOverwriteGuildApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map BulkOverwriteGuildApplicationCommandsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map BulkOverwriteGuildApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return BulkOverwriteGuildApplicationCommandsResponse.Other status
            })
            
    let getGuildApplicationCommandsPermissions
        (applicationId: string)
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands/permissions"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandsPermissionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandsPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandsPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandsPermissionsResponse.Other status
            })
            
    let getGuildApplicationCommandPermissions
        (applicationId: string)
        (guildId: string)
        (commandId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandPermissionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandPermissionsResponse.Other status
            })
            
    let editApplicationCommandPermissions
        (applicationId: string)
        (guildId: string)
        (commandId: string)
        (content: EditApplicationCommandPermissions)
        oauthToken
        (httpClient: HttpClient) =
            req {
                put $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
                oauth oauthToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditApplicationCommandPermissionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditApplicationCommandPermissionsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditApplicationCommandPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditApplicationCommandPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return EditApplicationCommandPermissionsResponse.Other status
            })
