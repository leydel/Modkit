namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open System.Net
open System.Net.Http

type DeleteGlobalApplicationCommandResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
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
