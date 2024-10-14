namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Text.Json.Serialization

type CreateInteractionResponsePayload (
    ``type``: InteractionCallbackType,
    ?data: InteractionCallbackData,
    ?files: IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                required "type" ``type``
                optional "data" data
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type CreateInteractionResponseOkResponse = {
    [<JsonPropertyName "interaction">] Interaction: InteractionCallback
    [<JsonPropertyName "resource">] Resource: InteractionCallbackResource
}

type CreateInteractionResponseResponse =
    | Ok of CreateInteractionResponseOkResponse
    | NoContent
    | BadRequest of ErrorResponse
    | Conflict of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Interaction =
    let createInteractionResponse
        (interactionId: string)
        (interactionToken: string)
        (withResponse: bool option)
        (content: CreateInteractionResponsePayload)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"interactions/{interactionId}/{interactionToken}/callback"
                bot botToken
                query "with_response" (withResponse >>. _.ToString())
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map CreateInteractionResponseResponse.Ok (Http.toJson res)
                | HttpStatusCode.NoContent -> return CreateInteractionResponseResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map CreateInteractionResponseResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateInteractionResponseResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateInteractionResponseResponse.Other status
            })
