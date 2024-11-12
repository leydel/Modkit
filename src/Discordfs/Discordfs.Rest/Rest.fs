module Discordfs.Rest.Rest

open Discordfs.Rest.Common
open Discordfs.Types
open System.Net.Http

let createInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (withResponse: bool option)
    (content: CreateInteractionResponsePayload<'a>)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"interactions/{interactionId}/{interactionToken}/callback"
            bot botToken
            query "with_response" (withResponse >>. _.ToString())
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asOptionalJson<InteractionCallbackResponse>
            
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
        ?>> DiscordResponse.asJson<Message>
            
let editOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (content: EditOriginalInteractionResponsePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"webhooks/{interactionId}/{interactionToken}/messages/@original"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
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
        ?>> DiscordResponse.asEmpty
            
let createFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (content: CreateFollowUpMessagePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"webhooks/{applicationId}/{interactionToken}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty
            
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
        ?>> DiscordResponse.asJson<Message>
            
let editFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    (content: EditFollowUpMessagePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
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
        ?>> DiscordResponse.asEmpty
