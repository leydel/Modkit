namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Net.Http

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
    | Conflict of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
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
                | HttpStatusCode.Conflict -> return! Task.map CreateGlobalApplicationCommandResponse.Conflict (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGlobalApplicationCommandResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGlobalApplicationCommandResponse.Other status
            })
