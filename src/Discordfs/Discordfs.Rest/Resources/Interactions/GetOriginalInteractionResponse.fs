namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetOriginalInteractionResponseResponse =
    | Ok of Message
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Interaction =
    let getOriginalInteractionResponse
        (interactionId: string)
        (interactionToken: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"webhooks/{interactionId}/{interactionToken}/messages/@original"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetOriginalInteractionResponseResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetOriginalInteractionResponseResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetOriginalInteractionResponseResponse.TooManyRequests (Http.toJson res)
                | status -> return GetOriginalInteractionResponseResponse.Other status
            })
