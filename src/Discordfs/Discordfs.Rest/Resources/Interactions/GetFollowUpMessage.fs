namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetFollowUpMessageResponse =
    | Ok of Message
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Interaction =
    let getFollowUpMessage
        (applicationId: string)
        (interactionToken: string)
        (messageId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetFollowUpMessageResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetFollowUpMessageResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetFollowUpMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return GetFollowUpMessageResponse.Other status
            })
