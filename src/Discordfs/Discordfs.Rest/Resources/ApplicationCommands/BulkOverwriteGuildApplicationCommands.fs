namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type BulkOverwriteGuildApplicationCommands (
    commands: BulkOverwriteApplicationCommand list
) =
    inherit Payload() with
        override _.Content = JsonListPayload commands

type BulkOverwriteGuildApplicationCommandsResponse =
    | Ok of ApplicationCommand list
    | BadRequest of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
    let bulkOverwriteGuildApplicationCommands
        (applicationId: string)
        (guildId: string)
        (content: BulkOverwriteGlobalApplicationCommandsPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                put $"applications/{applicationId}/guilds/{guildId}/commands"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map BulkOverwriteGuildApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map BulkOverwriteGuildApplicationCommandsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map BulkOverwriteGuildApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return BulkOverwriteGuildApplicationCommandsResponse.Other status
            })
