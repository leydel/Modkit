namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Collections.Generic
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
            |> Task.mapT Http.toJson
            