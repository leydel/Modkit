namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open System.Net.Http

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
            |> Task.wait
