namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetGuildApplicationCommandsResponse =
    | Ok of ApplicationCommand list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
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
