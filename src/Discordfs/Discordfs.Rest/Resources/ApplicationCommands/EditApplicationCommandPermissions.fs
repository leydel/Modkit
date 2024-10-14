namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type EditApplicationCommandPermissions (
    permissions: ApplicationCommandPermission list
) =
    inherit Payload() with
        override _.Content = json {
            required "permissions" permissions
        }

type EditApplicationCommandPermissionsResponse =
    | Ok of GuildApplicationCommandPermissions
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
    let editApplicationCommandPermissions
        (applicationId: string)
        (guildId: string)
        (commandId: string)
        (content: EditApplicationCommandPermissions)
        oauthToken
        (httpClient: HttpClient) =
            req {
                put $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
                oauth oauthToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditApplicationCommandPermissionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditApplicationCommandPermissionsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditApplicationCommandPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditApplicationCommandPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return EditApplicationCommandPermissionsResponse.Other status
            })
