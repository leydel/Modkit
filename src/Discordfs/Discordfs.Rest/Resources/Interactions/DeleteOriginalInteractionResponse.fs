namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open System.Net
open System.Net.Http

type DeleteOriginalInteractionResponseResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Interaction =
    let deleteOriginalInteractionResponse
        (interactionId: string)
        (interactionToken: string)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"webhooks/{interactionId}/{interactionToken}/messages/@original"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteOriginalInteractionResponseResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteOriginalInteractionResponseResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteOriginalInteractionResponseResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteOriginalInteractionResponseResponse.Other status
            })
