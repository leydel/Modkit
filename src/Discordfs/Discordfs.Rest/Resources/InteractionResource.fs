namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Threading.Tasks

type CreateInteractionResponse (
    ``type``: InteractionCallbackType,
    ?data: InteractionCallbackData,
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

type EditOriginalInteractionResponse (
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

type CreateFollowUpMessage (
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

type EditFollowUpMessage (
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
            
type IInteractionResource =
    abstract member CreateInteractionResponse:
        interactionId: string ->
        interactionToken: string ->
        withResponse: bool option ->
        content: CreateInteractionResponse ->
        Task<InteractionCallbackResponse option>

    abstract member GetOriginalInteractionResponse:
        interactionId: string ->
        interactionToken: string ->
        Task<Message>

    abstract member EditOriginalInteractionResponse:
        interactionId: string ->
        interactionToken: string ->
        content: EditOriginalInteractionResponse ->
        Task<Message>

    abstract member DeleteOriginalInteractionResponse:
        interactionId: string ->
        interactionToken: string ->
        Task<unit>

    abstract member CreateFollowUpMessage:
        applicationId: string ->
        interactionToken: string ->
        content: CreateFollowUpMessage ->
        Task<unit>

    abstract member GetFollowUpMessage:
        applicationId: string ->
        interactionToken: string ->
        messageId: string ->
        Task<Message>

    abstract member EditFollowUpMessage:
        applicationId: string ->
        interactionToken: string ->
        messageId: string ->
        content: EditFollowUpMessage ->
        Task<Message>

    abstract member DeleteFollowUpMessage:
        applicationId: string ->
        interactionToken: string ->
        messageId: string ->
        Task<unit>

type InteractionResource (httpClientFactory, token) =
    interface IInteractionResource with
        member _.CreateInteractionResponse interactionId interactionToken withResponse content =
            let req = req {
                post $"interactions/{interactionId}/{interactionToken}/callback"
                bot token
                query "with_response" (withResponse >>. _.ToString())
                payload content
            }
            let task = Http.send httpClientFactory req
            
            match withResponse with
            | Some true -> Task.mapT Http.toJson task
            | _ -> Task.map (fun _ -> None) task

        member _.GetOriginalInteractionResponse interactionId interactionToken =
            req {
                get $"webhooks/{interactionId}/{interactionToken}/messages/@original"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.EditOriginalInteractionResponse interactionId interactionToken content =
            req {
                patch $"webhooks/{interactionId}/{interactionToken}/messages/@original"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteOriginalInteractionResponse interactionId interactionToken =
            req {
                delete $"webhooks/{interactionId}/{interactionToken}/messages/@original"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.CreateFollowUpMessage applicationId interactionToken content =
            req {
                post $"webhooks/{applicationId}/{interactionToken}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.GetFollowUpMessage applicationId interactionToken messageId =
            req {
                get $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.EditFollowUpMessage applicatoinId interactionToken messageId content =
            req {
                patch $"webhooks/{applicatoinId}/{interactionToken}/messages/{messageId}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteFollowUpMessage applicationId interactionToken messageId =
            req {
                delete $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait
