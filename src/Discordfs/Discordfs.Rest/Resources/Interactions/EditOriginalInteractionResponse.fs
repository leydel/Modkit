namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Collections.Generic
open System.Net.Http

type EditOriginalInteractionResponsePayload (
    ?content: string,
    ?embeds: Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components: Component list,
    ?attachments: Attachment list,
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

module Interaction =
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
            |> Task.mapT Http.toJson<Message>
