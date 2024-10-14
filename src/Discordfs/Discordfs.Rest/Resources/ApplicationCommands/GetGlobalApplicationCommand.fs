namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetGlobalApplicationCommandResponse =
    | Ok of ApplicationCommand
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
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
