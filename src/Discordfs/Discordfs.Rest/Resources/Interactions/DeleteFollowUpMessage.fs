namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open System.Net
open System.Net.Http

type DeleteFollowUpMessageResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Interaction =
    let deleteFollowUpMessage
        (applicationId: string)
        (interactionToken: string)
        (messageId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteFollowUpMessageResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteFollowUpMessageResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteFollowUpMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteFollowUpMessageResponse.Other status
            })
