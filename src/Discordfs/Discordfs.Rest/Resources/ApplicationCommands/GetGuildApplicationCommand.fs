namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetGuildApplicationCommandResponse =
    | Ok of ApplicationCommand
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
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
