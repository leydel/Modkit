namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Net.Http

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
            |> Task.mapT Http.toJson<Message>
