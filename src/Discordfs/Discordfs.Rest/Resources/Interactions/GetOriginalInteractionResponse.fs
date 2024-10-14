namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Net.Http

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
            |> Task.mapT Http.toJson<Message>
