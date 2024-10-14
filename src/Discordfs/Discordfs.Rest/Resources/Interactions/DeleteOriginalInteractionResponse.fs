namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open System.Net.Http

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
            |> Task.wait
