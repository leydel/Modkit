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
    ?username: string,
    ?avatar_url: string,
    ?tts: bool,
    ?embeds: Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components: Component list,
    ?attachments: Attachment list,
    ?flags: int,
    ?thread_name: string,
    ?applied_tags: string list,
    ?poll: Poll,
    ?files: IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "username" username
                optional "avatar_url" avatar_url
                optional "tts" tts
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "flags" flags
                optional "thread_name" thread_name
                optional "applied_tags" applied_tags
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
        threadId: string option ->
        Task<Message>

    abstract member EditOriginalInteractionResponse:
        interactionId: string ->
        interactionToken: string ->
        threadId: string option ->
        content: EditOriginalInteractionResponse ->
        Task<Message>

    abstract member DeleteOriginalInteractionResponse:
        interactionId: string ->
        interactionToken: string ->
        Task<unit>

    abstract member CreateFollowUpMessage:
        applicationId: string ->
        interactionToken: string ->
        threadId: string option ->
        content: CreateFollowUpMessage ->
        Task<unit>

    abstract member GetFollowUpMessage:
        applicationId: string ->
        interactionToken: string ->
        messageId: string ->
        threadId: string option ->
        Task<Message>

    abstract member EditFollowUpMessage:
        applicationId: string ->
        interactionToken: string ->
        messageId: string ->
        threadId: string option ->
        content: EditFollowUpMessage ->
        Task<Message>

    abstract member DeleteFollowUpMessage:
        applicationId: string ->
        interactionToken: string ->
        messageId: string ->
        threadId: string option -> // TODO: Check if thread_id is supposed to exist (exists in webhook delete but not mentioned in follow-up delete
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

        member _.GetOriginalInteractionResponse interactionId interactionToken threadId =
            req {
                get $"webhooks/{interactionId}/{interactionToken}/messages/@original"
                bot token
                query "thread_id" threadId
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.EditOriginalInteractionResponse interactionId interactionToken threadId content =
            req {
                patch $"webhooks/{interactionId}/{interactionToken}/messages/@original"
                bot token
                query "thread_id" threadId
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

        member _.CreateFollowUpMessage applicationId interactionToken threadId content =
            req {
                post $"webhooks/{applicationId}/{interactionToken}"
                bot token
                query "thread_id" threadId
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.GetFollowUpMessage applicationId interactionToken messageId threadId =
            req {
                get $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
                bot token
                query "thread_id" threadId
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.EditFollowUpMessage applicatoinId interactionToken messageId threadId content =
            req {
                patch $"webhooks/{applicatoinId}/{interactionToken}/messages/{messageId}"
                bot token
                query "thread_id" threadId
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteFollowUpMessage applicationId interactionToken threadId messageId =
            req {
                delete $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
                bot token
                query "thread_id" threadId
            }
            |> Http.send httpClientFactory
            |> Task.wait
