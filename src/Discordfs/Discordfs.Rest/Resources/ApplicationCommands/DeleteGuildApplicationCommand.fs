namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open System.Net
open System.Net.Http

type DeleteGuildApplicationCommandResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
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
