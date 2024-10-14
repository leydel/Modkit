namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetGlobalApplicationCommandsResponse =
    | Ok of ApplicationCommand list
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
