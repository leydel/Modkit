namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Net.Http

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
    | Conflict of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
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
                | HttpStatusCode.Conflict -> return! Task.map CreateGuildApplicationCommandResponse.Conflict (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGuildApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGuildApplicationCommandResponse.Other status
            })
