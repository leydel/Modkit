namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http
open System.Text.Json.Serialization

type GetAnswerVotersOkResponse = {
    [<JsonPropertyName "users">] Users: User list
}

type GetAnswerVotersResponse =
    | Ok of GetAnswerVotersOkResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EndPollResponse =
    | Ok of Message
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Poll =
    let getAnswerVoters
        (channelId: string)
        (messageId: string)
        (answerId: string)
        (after: string option)
        (limit: int option)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"channels/{channelId}/polls/{messageId}/answers/{answerId}"
                bot botToken
                query "after" after
                query "limit" (limit >>. _.ToString())
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetAnswerVotersResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetAnswerVotersResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetAnswerVotersResponse.TooManyRequests (Http.toJson res)
                | status -> return GetAnswerVotersResponse.Other status
            })

    let endPoll
        (channelId: string)
        (messageId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"channels/{channelId}/polls/{messageId}/expire"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EndPollResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EndPollResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EndPollResponse.TooManyRequests (Http.toJson res)
                | status -> return EndPollResponse.Other status
            })
