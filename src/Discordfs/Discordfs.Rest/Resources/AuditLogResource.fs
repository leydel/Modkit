namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetGuildAuditLogResponse =
    | Ok of AuditLog
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module AuditLog =
    let getGuildAuditLog
        (guildId: string)
        (userId: string option)
        (actionType: AuditLogEventType option)
        (before: string option)
        (after: string option)
        (limit: int option)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/audit-logs"
                bot botToken
                query "user_id" userId
                query "action_type" (actionType >>. _.ToString())
                query "before" before
                query "after" after
                query "limit" (limit >>. _.ToString())
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildAuditLogResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildAuditLogResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildAuditLogResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildAuditLogResponse.Other status
            })
