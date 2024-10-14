namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Net.Http

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

module ApplicationCommand =
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
