namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Text.Json.Serialization

type CreateInteractionResponseData =
    | Message of MessageInteractionCallback
    | Autocomplete of AutocompleteInteractionCallback
    | Modal of ModalInteractionCallback

type CreateInteractionResponsePayload (
    ``type``: InteractionCallbackType,
    ?data: CreateInteractionResponseData,
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
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetOriginalInteractionResponseResponse =
    | Ok of Message
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EditOriginalInteractionResponsePayload (
    ?content: string,
    ?embeds: Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components: Component list,
    ?attachments: Attachment list, // TODO: Partial (with filename and description)
    ?poll: Poll,
    ?files: IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "poll" poll
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type EditOriginalInteractionResponseResponse =
    | Ok of Message
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteOriginalInteractionResponseResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateFollowUpMessagePayload (
    ?content: string,
    ?tts: bool,
    ?embeds: Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components: Component list,
    ?attachments: Attachment list, // TODO: Partial (with filename and description)
    ?flags: int,
    ?poll: Poll,
    ?files: IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "tts" tts
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "flags" flags
                optional "poll" poll                
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type CreateFollowUpMessageResponse =
    | NoContent
    | BadRequest of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetFollowUpMessageResponse =
    | Ok of Message
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EditFollowUpMessagePayload (
    ?content: string option,
    ?embeds: Embed list option,
    ?allowed_mentions: AllowedMentions option,
    ?components: Component list option,
    ?attachments: Attachment list option, // TODO: Partial (with filename and description)
    ?poll: Poll option,
    ?files: IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "poll" poll                
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type EditFollowUpMessageResponse =
    | Ok of Message
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteFollowUpMessageResponse =
    | NoContent
    | NotFound of ErrorResponse
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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditOriginalInteractionResponseResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditOriginalInteractionResponseResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditOriginalInteractionResponseResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditOriginalInteractionResponseResponse.TooManyRequests (Http.toJson res)
                | status -> return EditOriginalInteractionResponseResponse.Other status
            })
            
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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return CreateFollowUpMessageResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map CreateFollowUpMessageResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateFollowUpMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateFollowUpMessageResponse.Other status
            })
            
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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetFollowUpMessageResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetFollowUpMessageResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetFollowUpMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return GetFollowUpMessageResponse.Other status
            })
            
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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditFollowUpMessageResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditFollowUpMessageResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditFollowUpMessageResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditFollowUpMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return EditFollowUpMessageResponse.Other status
            })
            
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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteFollowUpMessageResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteFollowUpMessageResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteFollowUpMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteFollowUpMessageResponse.Other status
            })
