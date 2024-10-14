namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type BulkOverwriteGlobalApplicationCommandsPayload (
    commands: BulkOverwriteApplicationCommand list
) =
    inherit Payload() with
        override _.Content = JsonListPayload commands

type BulkOverwriteGlobalApplicationCommandsResponse =
    | Ok of ApplicationCommand list
    | BadRequest of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module ApplicationCommand =
    let bulkOverwriteGlobalApplicationCommands
        (applicationId: string)
        (content: BulkOverwriteGlobalApplicationCommandsPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                put $"applications/{applicationId}/commands"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map BulkOverwriteGlobalApplicationCommandsResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map BulkOverwriteGlobalApplicationCommandsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map BulkOverwriteGlobalApplicationCommandsResponse.TooManyRequests (Http.toJson res)
                | status -> return BulkOverwriteGlobalApplicationCommandsResponse.Other status
            })
