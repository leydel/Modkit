namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net.Http

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

module Interaction =
    let createInteractionResponse
        (interactionId: string)
        (interactionToken: string)
        (withResponse: bool option)
        (content: CreateInteractionResponsePayload)
        botToken
        (httpClient: HttpClient) =
            let req = req {
                post $"interactions/{interactionId}/{interactionToken}/callback"
                bot botToken
                query "with_response" (withResponse >>. _.ToString())
                payload content
            }
            let task = httpClient.SendAsync(req)

            match withResponse with
            | Some true ->
                Task.mapT Http.toJson<InteractionCallbackResponse> task
                |> Task.map (fun res -> Some res)
            | _ ->
                Task.map (fun _ -> None) task
                