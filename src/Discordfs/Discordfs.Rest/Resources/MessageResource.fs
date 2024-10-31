namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Collections.Generic
open System.Threading.Tasks

type CreateMessage (
    ?content:           string,
    ?nonce:             MessageNonce,
    ?tts:               bool,
    ?embeds:            Embed list,
    ?allow_mentions:    AllowedMentions,
    ?message_reference: MessageReference,
    ?components:        Component list,
    ?sticker_ids:       string list,
    ?attachments:       Attachment list, // TODO: Partial (with filename and description)
    ?flags:             int,
    ?enforce_nonce:     bool,
    ?poll:              Poll,
    ?files:             IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "nonce" (match nonce with | Some (MessageNonce.Number n) -> Some n | _ -> None)
                optional "nonce" (match nonce with | Some (MessageNonce.String s) -> Some s | _ -> None)
                optional "tts" tts
                optional "embeds" embeds
                optional "allowed_mentions" allow_mentions
                optional "message_reference" message_reference
                optional "components" components
                optional "sticker_ids" sticker_ids
                optional "attachments" attachments
                optional "flags" flags
                optional "enforce_nonce" enforce_nonce
                optional "poll" poll
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type EditMessage (
    ?content:        string option,
    ?embeds:         Embed list option,
    ?flags:          int option,
    ?allow_mentions: AllowedMentions option,
    ?components:     Component list option,
    ?attachments:    Attachment list option, // TODO: Partial (with filename and description)
    ?files:          IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "embeds" embeds
                optional "flags" flags
                optional "allowed_mentions" allow_mentions
                optional "components" components
                optional "attachments" attachments
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type BulkDeleteMessages (
    messages: string list
) =
    inherit Payload() with
        override _.Content = json {
            required "messages" messages
        }

type IMessageResource =
    // https://discord.com/developers/docs/resources/message#get-channel-messages
    abstract member GetChannelMessages:
        channelId: string ->
        around: string option ->
        before: string option ->
        after: string option ->
        limit: int option ->
        Task<Message list>

    // https://discord.com/developers/docs/resources/message#get-channel-message
    abstract member GetChannelMessage:
        channelId: string ->
        messageId: string ->
        Task<Message>

    // https://discord.com/developers/docs/resources/message#create-message
    abstract member CreateMessage:
        channelId: string ->
        content: CreateMessage ->
        Task<Message>

    // https://discord.com/developers/docs/resources/message#crosspost-message
    abstract member CrosspostMessage:
        channelId: string ->
        messageId: string ->
        Task<Message>

    // https://discord.com/developers/docs/resources/message#create-reaction
    abstract member CreateReaction:
        channelId: string ->
        messageId: string ->
        emoji: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/message#delete-own-reaction
    abstract member DeleteOwnReaction:
        channelId: string ->
        messageId: string ->
        emoji: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/message#delete-user-reaction
    abstract member DeleteUserReaction:
        channelId: string ->
        messageId: string ->
        emoji: string ->
        userId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/message#get-reactions
    abstract member GetReactions:
        channelId: string ->
        messageId: string ->
        emoji: string ->
        ``type``: ReactionType option ->
        after: string option ->
        limit: int option ->
        Task<User list>

    // https://discord.com/developers/docs/resources/message#delete-all-reactions
    abstract member DeleteAllReactions:
        channelId: string ->
        messageId: string ->
        Task<unit>
        
    // https://discord.com/developers/docs/resources/message#delete-all-reactions-for-emoji
    abstract member DeleteAllReactionsForEmoji:
        channelId: string ->
        messageId: string ->
        emoji: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/message#edit-message
    abstract member EditMessage:
        channelId: string ->
        messageId: string ->
        content: EditMessage ->
        Task<Message>

    // https://discord.com/developers/docs/resources/message#delete-message
    abstract member DeleteMessage:
        channelId: string ->
        messageId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/message#bulk-delete-messages
    abstract member BulkDeleteMessages:
        channelId: string ->
        auditLogReason: string option ->
        content: BulkDeleteMessages ->
        Task<unit>

type MessageResource (httpClientFactory, token) =
    interface IMessageResource with
        member _.GetChannelMessages channelId around before after limit =
            req {
                get $"channels/{channelId}/messages"
                bot token
                query "around" around
                query "before" before
                query "after" after
                query "limit" (limit >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetChannelMessage channelId messageId =
            req {
                get $"channels/{channelId}/messages/{messageId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateMessage channelId content =
            req {
                post $"channels/{channelId}/messages"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CrosspostMessage channelId messageId =
            req {
                post $"channels/{channelId}/messages/{messageId}/crosspost"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateReaction channelId messageId emoji =
            req {
                post $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/@me"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.DeleteOwnReaction channelId messageId emoji =
            req {
                delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/@me"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.DeleteUserReaction channelId messageId emoji userId =
            req {
                delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/{userId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.GetReactions channelId messageId emoji ``type`` after limit =
            req {
                get $"channels/{channelId}/messages/{messageId}/reactions/{emoji}"
                bot token
                query "type" (``type`` >>. int >>. _.ToString())
                query "after" after
                query "limit" (limit >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteAllReactions channelId messageId =
            req {
                delete $"channels/{channelId}/messages/{messageId}/reactions"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.DeleteAllReactionsForEmoji channelId messageId emoji =
            req {
                delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.EditMessage channelId messageId content =
            req {
                patch $"channels/{channelId}/messages/{messageId}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteMessage channelId messageId auditLogReason =
            req {
                delete $"channels/{channelId}/messages/{messageId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.BulkDeleteMessages channelId auditLogReason content =
            req {
                post $"channels/{channelId}/messages/bulk-delete"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.wait
            