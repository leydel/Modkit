namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Collections.Generic
open System.Threading.Tasks

type CreateWebhook (
    name:    string,
    ?avatar: string option
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "avatar" avatar
        }

type ModifyWebhook (
    ?name:       string,
    ?avatar:     string option,
    ?channel_id: string
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "avatar" avatar
            optional "channel_id" channel_id
        }

type ModifyWebhookWithToken (
    ?name:   string,
    ?avatar: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "avatar" avatar
        }

type ExecuteWebhook (
    ?content: string,
    ?username: string,
    ?avatar_url: string,
    ?tts: bool,
    ?embeds: Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components: Component list,
    ?attachments: Attachment list, // TODO: Partial (with filename and description)
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

type EditWebhookMessage (
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

type IWebhookResource =
    // https://discord.com/developers/docs/resources/webhook#create-webhook
    abstract member CreateWebhook:
        channelId: string ->
        auditLogReason: string option ->
        content: CreateWebhook ->
        Task<Webhook>

    // https://discord.com/developers/docs/resources/webhook#get-channel-webhooks
    abstract member GetChannelWebhooks:
        channelId: string ->
        Task<Webhook list>

    // https://discord.com/developers/docs/resources/webhook#get-guild-webhooks
    abstract member GetGuildWebhooks:
        guildId: string ->
        Task<Webhook list>

    // https://discord.com/developers/docs/resources/webhook#get-webhook
    abstract member GetWebhook:
        webhookId: string ->
        Task<Webhook>

    // https://discord.com/developers/docs/resources/webhook#get-webhook-with-token
    abstract member GetWebhookWithToken:
        webhookId: string ->
        webhookToken: string ->
        Task<Webhook>

    // https://discord.com/developers/docs/resources/webhook#modify-webhook
    abstract member ModifyWebhook:
        webhookId: string ->
        auditLogReason: string option ->
        content: ModifyWebhook ->
        Task<Webhook>

    // https://discord.com/developers/docs/resources/webhook#modify-webhook-with-token
    abstract member ModifyWebhookWithToken:
        webhookId: string ->
        webhookToken: string ->
        content: ModifyWebhookWithToken ->
        Task<Webhook>

    // https://discord.com/developers/docs/resources/webhook#delete-webhook
    abstract member DeleteWebhook:
        webhookId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/webhook#delete-webhook-with-token
    abstract member DeleteWebhookWithToken:
        webhookId: string ->
        webhookToken: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/webhook#execute-webhook
    abstract member ExecuteWebhook:
        webhookId: string ->
        webhookToken: string ->
        wait: bool option ->
        threadId: string option ->
        content: ExecuteWebhook ->
        Task<Message>

    // https://discord.com/developers/docs/resources/webhook#get-webhook-message
    abstract member GetWebhookMessage:
        webhookId: string ->
        webhookToken: string ->
        messageId: string ->
        threadId: string option ->
        Task<Message>

    // https://discord.com/developers/docs/resources/webhook#edit-webhook-message
    abstract member EditWebhookMessage:
        webhookId: string ->
        webhookToken: string ->
        messageId: string ->
        threadId: string option ->
        content: EditWebhookMessage ->
        Task<Message>

    // https://discord.com/developers/docs/resources/webhook#delete-webhook-message
    abstract member DeleteWebhookMessage:
        webhookId: string ->
        webhookToken: string ->
        messageId: string ->
        threadId: string option ->
        Task<unit>

type WebhookResource (httpClientFactory, token) =
    interface IWebhookResource with
        member _.CreateWebhook channelId auditLogReason content =
            req {
                post $"channels/{channelId}/webhooks"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetChannelWebhooks channelId =
            req {
                get $"channels/{channelId}/webhooks"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildWebhooks guildId =
            req {
                get $"guilds/{guildId}/webhooks"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetWebhook webhookId =
            req {
                get $"webhooks/{webhookId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetWebhookWithToken webhookId webhookToken =
            req {
                get $"webhooks/{webhookId}/{webhookToken}"
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyWebhook webhookId auditLogReason content =
            req {
                patch $"webhooks/{webhookId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyWebhookWithToken webhookId webhookToken content =
            req {
                patch $"webhooks/{webhookId}/{webhookToken}"
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteWebhook webhookId auditLogReason =
            req {
                delete $"webhooks/{webhookId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.DeleteWebhookWithToken webhookId webhookToken =
            req {
                delete $"webhooks/{webhookId}/{webhookToken}"
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.ExecuteWebhook webhookId webhookToken wait threadId content =
            req {
                post $"webhooks/{webhookId}/{webhookToken}"
                query "wait" (wait >>. _.ToString())
                query "thread_id" threadId
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetWebhookMessage webhookId webhookToken messageId threadId =
            req {
                get $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
                query "thread_id" threadId
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.EditWebhookMessage webhookId webhookToken messageId threadId content =
            req {
                patch $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
                query "thread_id" threadId
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteWebhookMessage webhookId webhookToken messageId threadId =
            req {
                patch $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
                query "thread_id" threadId
            }
            |> Http.send httpClientFactory
            |> Task.wait
