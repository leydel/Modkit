namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Collections.Generic
open System.Net.Http

type CreateFollowUpMessagePayload (
    ?content: string,
    ?tts: bool,
    ?embeds: Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components: Component list,
    ?attachments: Attachment list,
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

module Interaction =
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
            |> Task.wait
