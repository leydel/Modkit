namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetGuildApplicationCommandPermissionsResponse =
    | Ok of GuildApplicationCommandPermissions
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
    let getGuildApplicationCommandPermissions
        (applicationId: string)
        (guildId: string)
        (commandId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandPermissionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandPermissionsResponse.Other status
            })
