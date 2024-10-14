namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Net.Http

type EditFollowUpMessagePayload (
    ?content: string option,
    ?embeds: Embed list option,
    ?allowed_mentions: AllowedMentions option,
    ?components: Component list option,
    ?attachments: Attachment list option,
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

module Interaction =
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
