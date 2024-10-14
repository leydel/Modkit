namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetGuildApplicationCommandsPermissionsResponse =
    | Ok of GuildApplicationCommandPermissions list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
    let getGuildApplicationCommandsPermissions
        (applicationId: string)
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/guilds/{guildId}/commands/permissions"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildApplicationCommandsPermissionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildApplicationCommandsPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildApplicationCommandsPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildApplicationCommandsPermissionsResponse.Other status
            })
