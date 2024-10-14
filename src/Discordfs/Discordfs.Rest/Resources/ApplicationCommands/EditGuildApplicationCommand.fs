namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Net.Http

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

module ApplicationCommand =
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
