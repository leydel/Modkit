namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http
open System.Threading.Tasks

type GetInviteResponse =
    | Ok of Invite
    | NotFound of ErrorResponse
    | TooManyRequests of ErrorResponse
    | Other of HttpStatusCode

type DeleteInviteResponse =
    | Ok of Invite
    | NotFound of ErrorResponse
    | TooManyRequests of ErrorResponse
    | Other of HttpStatusCode

module Invite =
    let getInvite
        (inviteCode: string)
        (withCounts: bool option)
        (withExpiration: bool option)
        (guildScheduledEventId: string option)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"invites/{inviteCode}"
                bot botToken
                query "with_counts" (withCounts >>. _.ToString())
                query "with_expiration" (withExpiration >>. _.ToString())
                query "guild_scheduled_event_id" guildScheduledEventId
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetInviteResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetInviteResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetInviteResponse.TooManyRequests (Http.toJson res)
                | status -> return GetInviteResponse.Other status
            })
            
    let deleteInvite
        (inviteCode: string)
        (auditLogReason: string option)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"invites/{inviteCode}"
                bot botToken
                audit auditLogReason
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map DeleteInviteResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map DeleteInviteResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteInviteResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteInviteResponse.Other status
            })
