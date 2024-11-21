﻿module Discordfs.Rest.Rest

open Discordfs.Rest.Common
open Discordfs.Rest.Modules
open Discordfs.Rest.Types
open Discordfs.Types
open System
open System.Net.Http

// ----- Interaction -----

let createInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (withResponse: bool option)
    (content: CreateInteractionResponsePayload<'a>)
    (client: BotClient) =
        req {
            post $"interactions/{interactionId}/{interactionToken}/callback"
            query "with_response" (withResponse >>. _.ToString())
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asOptionalJson<InteractionCallbackResponse>
            
let getOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (client: BotClient) =
        req {
            get $"webhooks/{interactionId}/{interactionToken}/messages/@original"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
let editOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (content: EditOriginalInteractionResponsePayload)
    (client: BotClient) =
        req {
            patch $"webhooks/{interactionId}/{interactionToken}/messages/@original"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
let deleteOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (client: BotClient) =
        req {
            delete $"webhooks/{interactionId}/{interactionToken}/messages/@original"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty
            
let createFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (content: CreateFollowUpMessagePayload)
    (client: BotClient) =
        req {
            post $"webhooks/{applicationId}/{interactionToken}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty
            
let getFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    (client: BotClient) =
        req {
            get $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
let editFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    (content: EditFollowUpMessagePayload)
    (client: BotClient) =
        req {
            patch $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
let deleteFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    (client: BotClient) =
        req {
            delete $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Application Command -----

let getGlobalApplicationCommands
    (applicationId: string)
    (withLocalizations: bool option)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/commands"
            query "with_localizations" (withLocalizations >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let createGlobalApplicationCommand
    (applicationId: string)
    (content: CreateGlobalApplicationCommandPayload)
    (client: BotClient) =
        req {
            post $"applications/{applicationId}/commands"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let getGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/commands/{commandId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let editGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    (content: EditGlobalApplicationCommandPayload)
    (client: BotClient) =
        req {
            patch $"applications/{applicationId}/commands/{commandId}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let deleteGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    (client: BotClient) =
        req {
            delete $"applications/{applicationId}/commands/{commandId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty
            
let bulkOverwriteGlobalApplicationCommands
    (applicationId: string)
    (content: BulkOverwriteGlobalApplicationCommandsPayload)
    (client: BotClient) =
        req {
            put $"applications/{applicationId}/commands"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let getGuildApplicationCommands
    (applicationId: string)
    (guildId: string)
    (withLocalizations: bool option)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands"
            query "with_localizations" (withLocalizations >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let createGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (content: CreateGuildApplicationCommandPayload)
    (client: BotClient) =
        req {
            post $"applications/{applicationId}/guilds/{guildId}/commands"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let getGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let editGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (content: EditGuildApplicationCommandPayload)
    (client: BotClient) =
        req {
            patch $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let deleteGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (client: BotClient) =
        req {
            delete $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty
            
let bulkOverwriteGuildApplicationCommands
    (applicationId: string)
    (guildId: string)
    (content: BulkOverwriteGlobalApplicationCommandsPayload)
    (client: BotClient) =
        req {
            put $"applications/{applicationId}/guilds/{guildId}/commands"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let getGuildApplicationCommandsPermissions
    (applicationId: string)
    (guildId: string)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/permissions"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildApplicationCommandPermissions list>
            
let getGuildApplicationCommandPermissions
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildApplicationCommandPermissions>
            
let editApplicationCommandPermissions
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (content: EditApplicationCommandPermissions)
    (client: OAuthClient) =
        req {
            put $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildApplicationCommandPermissions>

// ----- Application -----

let getCurrentApplication
    botToken
    (httpClient: HttpClient) =
        req {
            get "applications/@me"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Application>

let editCurrentApplication
    (content: EditCurrentApplicationPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch "applications/@me"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Application>

let getApplicationActivityInstance
    (applicationId: string)
    (instanceId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"applications/{applicationId}/activity-instances/{instanceId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ActivityInstance>

// ----- Audit Log -----

let getGuildAuditLog
    (guildId: string)
    (userId: string option)
    (actionType: AuditLogEventType option)
    (before: string option)
    (after: string option)
    (limit: int option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/audit-logs"
            bot botToken
            query "user_id" userId
            query "action_type" (actionType >>. _.ToString())
            query "before" before
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<AuditLog>

// ----- Auto Moderation -----

let getAutoModerationRule
    (guildId: string)
    (autoModerationRuleId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<AutoModerationRule>

let createAutoModerationRule
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateAutoModerationRulePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/auto-moderation/rules"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<AutoModerationRule>

let modifyAutoModerationRule
    (guildId: string)
    (autoModerationRuleId: string)
    (auditLogReason: string option)
    (content: CreateAutoModerationRulePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<AutoModerationRule>

let deleteAutoModerationRule
    (guildId: string)
    (autoModerationRuleId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Channel -----

let getChannel
    (channelId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let modifyChannel
    (channelId: string)
    (auditLogReason: string option)
    (content: ModifyChannelPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"channels/{channelId}"
            bot botToken
            audit auditLogReason
            payload (content.Payload)
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let deleteChannel
    (channelId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let editChannelPermissions
    (channelId: string)
    (overwriteId: string)
    (auditLogReason: string option)
    (content: EditChannelPermissionsPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"channels/{channelId}/permissions/{overwriteId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let getChannelInvites
    (channelId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/invites"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<InviteWithMetadata list>

let createChannelInvite
    (channelId: string)
    (auditLogReason: string option)
    (content: CreateChannelInvitePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/invites"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asOptionalJson<InviteWithMetadata>

let deleteChannelPermission
    (channelId: string)
    (overwriteId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/permissions/{overwriteId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let followAnnouncementChannel
    (channelId: string)
    (auditLogReason: string option)
    (content: FollowAnnouncementChannelPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/followers"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<FollowedChannel>

let triggerTypingIndicator
    (channelId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/typing"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let getPinnedMessages
    (channelId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/pins"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message list>

let pinMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"channels/{channelId}/pins/{messageId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let unpinMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/pins/{messageId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let groupDmAddRecipient
    (channelId: string)
    (userId: string)
    (content: GroupDmAddRecipientPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"channels/{channelId}/recipients/{userId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asOptionalJson<Channel>

let groupDmRemoveRecipient
    (channelId: string)
    (userId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/recipients/{userId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let startThreadFromMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    (content: StartThreadFromMessagePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/messages/{messageId}/threads"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let startThreadWithoutMessage
    (channelId: string)
    (auditLogReason: string option)
    (content: StartThreadWithoutMessagePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/threads"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let startThreadInForumOrMediaChannel
    (channelId: string)
    (auditLogReason: string option)
    (content: StartThreadWithoutMessagePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/threads"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<StartThreadInForumOrMediaChannelOkResponse>

let joinThread
    (channelId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"channels/{channelId}/thread-members/@me"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let addThreadMember
    (channelId: string)
    (userId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"channels/{channelId}/thread-members/{userId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let leaveThread
    (channelId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/thread-members/@me"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let removeThreadMember
    (channelId: string)
    (userId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/thread-members/{userId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let getThreadMember
    (channelId: string)
    (userId: string)
    (withMember: bool option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/thread-members/{userId}"
            bot botToken
            query "with_member" (match withMember with | Some true -> Some "true" | _ -> None)
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ThreadMember>

let listThreadMembers
    (channelId: string)
    (withMember: bool option)
    (after: string option)
    (limit: int option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/thread-members"
            bot botToken
            query "with_member" (match withMember with | Some true -> Some "true" | _ -> None)
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ThreadMember list>

    // TODO: Test paginated response and implement for list thread members

let listPublicArchivedThreads
    (channelId: string)
    (before: DateTime option)
    (limit: int option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/threads/archived/public"
            bot botToken
            query "before" (before >>. _.ToString())
            query "limit" (limit >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ListPublicArchivedThreadsOkResponse>

let listPrivateArchivedThreads
    (channelId: string)
    (before: DateTime option)
    (limit: int option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/threads/archived/private"
            bot botToken
            query "before" (before >>. _.ToString())
            query "limit" (limit >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ListPrivateArchivedThreadsOkResponse>

let listJoinedPrivateArchivedThreads
    (channelId: string)
    (before: DateTime option)
    (limit: int option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/users/@me/threads/archived/private"
            bot botToken
            query "before" (before >>. _.ToString())
            query "limit" (limit >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ListJoinedPrivateArchivedThreadsOkResponse>

// ----- Emoji -----

let listGuildEmojis
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/emojis"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Emoji list>

let getGuildEmoji
    (guildId: string)
    (emojiId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/emojis/{emojiId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Emoji>

let createGuildEmoji
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildEmojiPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/emojis"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Emoji>
        
let modifyGuildEmoji
    (guildId: string)
    (emojiId: string)
    (auditLogReason: string option)
    (content: ModifyGuildEmojiPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/emojis/{emojiId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Emoji>

let deleteGuildEmoji
    (guildId: string)
    (emojiId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/emojis/{emojiId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty
        
let listApplicationEmojis
    (applicationId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/emojis"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ListApplicationEmojisOkResponse>

let getApplicationEmoji
    (applicationId: string)
    (emojiId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/emojis/{emojiId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Emoji>

let createApplicationEmoji
    (applicationId: string)
    (content: CreateApplicationEmojiPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"applications/{applicationId}/emojis"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Emoji>
        
let modifyApplicationEmoji
    (applicationId: string)
    (emojiId: string)
    (content: ModifyApplicationEmojiPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"applications/{applicationId}/emojis/{emojiId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Emoji>

let deleteApplicationEmoji
    (applicationId: string)
    (emojiId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"applications/{applicationId}/emojis/{emojiId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Entitlement -----

let listEntitlements
    (applicationId: string)
    (userId: string option)
    (skuIds: string list option)
    (before: string option)
    (after: string option)
    (limit: int option)
    (guildId: string option)
    (excludeEnded: bool option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/entitlements"
            bot botToken
            query "user_id" userId
            query "sku_ids" (skuIds >>. String.concat ",")
            query "before" before
            query "after" after
            query "limit" (limit >>. _.ToString())
            query "guild_id" guildId
            query "exclude_ended" (excludeEnded >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Entitlement list>

let consumeEntitlement
    (applicationId: string)
    (entitlementId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"applications/{applicationId}/entitlements/{entitlementId}/consume"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let createTestEntitlement
    (applicationId: string)
    (content: CreateTestEntitlementPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"applications/{applicationId}/entitlements"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Entitlement>

let deleteTestEntitlement
    (applicationId: string)
    (entitlementId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"applications/{applicationId}/entitlements/{entitlementId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Guild -----

let createGuild
    (content: CreateGuildPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post "guilds"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Guild>

let getGuild
    (guildId: string)
    (withCounts: bool option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}"
            bot botToken
            query "with_counts" (withCounts >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Guild>

let getGuildPreview
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/preview"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildPreview>

let modifyGuild
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Guild>

let deleteGuild
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildChannels
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/channels"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Channel list>

let createGuildChannel
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildChannelPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/channels"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let modifyGuildChannelPositions
    (guildId: string)
    (content: ModifyGuildChannelPositionsPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/channels"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let listActiveGuildThreads
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/threads/active"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ListActiveGuildThreadsOkResponse>

let getGuildMember
    (guildId: string)
    (userId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/members/{userId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildMember>

let listGuildMembers
    (guildId: string)
    (limit: int option)
    (after: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/members"
            bot botToken
            query "limit" (limit >>. _.ToString())
            query "after" (after >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildMember list>

let searchGuildMembers
    (guildId: string)
    (q: string) // query (cannot name same due to req ce)
    (limit: int option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/members/search"
            bot botToken
            query "query" q
            query "limit" (limit >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildMember list>

let addGuildMember
    (guildId: string)
    (userId: string)
    (content: AddGuildMemberPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"guilds/{guildId}/members/{userId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asOptionalJson<GuildMember> // TODO: Double check this has both 200 and 204

let modifyGuildMember
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (content: ModifyGuildMemberPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/members/{userId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asOptionalJson<GuildMember> // TODO: Double check this has both 200 and 204

let modifyCurrentMember
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyCurrentMemberPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/members/@me"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asOptionalJson<GuildMember> // TODO: Double check this has both 200 and 204
        
let addGuildMemberRole
    (guildId: string)
    (userId: string)
    (roleId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"guilds/{guildId}/members/{userId}/roles/{roleId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let removeGuildMemberRole
    (guildId: string)
    (userId: string)
    (roleId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/members/{userId}/roles/{roleId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let removeGuildMember
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/members/{userId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildBans
    (guildId: string)
    (limit: int option)
    (before: string option)
    (after: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/bans"
            bot botToken
            query "limit" (limit >>. _.ToString())
            query "before" before
            query "after" after
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildBan list>

let getGuildBan
    (guildId: string)
    (userId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/bans/{userId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildBan>

let createGuildBan
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (content: CreateGuildBanPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"guilds/{guildId}/bans/{userId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let removeGuildBan
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/bans/{userId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let bulkGuildBan
    (guildId: string)
    (auditLogReason: string option)
    (content: BulkGuildBanPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/bulk-ban"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<BulkGuildBanOkResponse>

let getGuildRoles
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/roles"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Role list>

let getGuildRole
    (guildId: string)
    (roleId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/roles/{roleId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Role>

let createGuildRole
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildRolePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/roles"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Role>

let modifyGuildRolePositions
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildRolePositionsPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/channels"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Role list>

let modifyGuildRole
    (guildId: string)
    (roleId: string)
    (auditLogReason: string option)
    (content: ModifyGuildRolePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/roles/{roleId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Role>

let modifyGuildMfaLevel
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildMfaLevelPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/mfa"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildMfaLevel>

let deleteGuildRole
    (guildId: string)
    (roleId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/roles/{roleId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildPruneCount
    (guildId: string)
    (days: int option)
    (includeRoles: string list option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/prune"
            bot botToken
            query "days" (days >>. _.ToString())
            query "include_roles" (includeRoles >>. String.concat ",")
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GetGuildPruneCountOkResponse>

let beginGuildPrune
    (guildId: string)
    (auditLogReason: string option)
    (content: BeginGuildPrunePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/prune"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<BeginGuildPruneOkResponse>

let getGuildVoiceRegions
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/regions"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<VoiceRegion list>

let getGuildInvites
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/invites"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<InviteWithMetadata list>

let getGuildIntegrations
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/integrations"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildIntegration list>

let deleteGuildIntegration
    (guildId: string)
    (integrationId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/integrations/{integrationId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildWidgetSettings
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/widget"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildWidgetSettings>

let modifyGuildWidget
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildWidgetPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/widget"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildWidgetSettings>

let getGuildWidget
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/widget.json"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildWidget>

let getGuildVanityUrl
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/vanity-url"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GetGuildVanityUrlOkResponse>

let getGuildWidgetImage
    (guildId: string)
    (style: GuildWidgetStyle option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/widget.png"
            bot botToken
            query "style" (style >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asRaw // TODO: Convert to png image format

let getGuildWelcomeScreen
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/welcome-screen"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<WelcomeScreen>

let modifyGuildWelcomeScreen
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildWelcomeScreenPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/welcome-screen"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<WelcomeScreen>

let getGuildOnboarding
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/onboarding"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildOnboarding>

let modifyGuildOnboarding
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildOnboardingPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"guilds/{guildId}/onboarding"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildOnboarding>

// ----- Guild Scheduled Event -----

let listGuildScheduledEvents
    (guildId: string)
    (withUserCount: bool option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/scheduled-events"
            bot botToken
            query "with_user_count" (withUserCount >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEvent list>

let createGuildScheduledEvent
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildScheduledEventPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/scheduled-events"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEvent>

let getGuildScheduledEvent
    (guildId: string)
    (guildScheduledEventId: string)
    (withUserCount: bool option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
            bot botToken
            query "with_user_count" (withUserCount >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEvent>

let modifyGuildScheduledEvent
    (guildId: string)
    (guildScheduledEventId: string)
    (auditLogReason: string option)
    (content: ModifyGuildScheduledEventPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEvent>

let deleteGuildScheduledEvent
    (guildId: string)
    (guildScheduledEventId: string)
    //(auditLogReason: string option) // TODO: Check if audit log is supposed to be available for this
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildScheduledEventUsers
    (guildId: string)
    (guildScheduledEventId: string)
    (limit: int option)
    (withMember: bool option)
    (before: string option)
    (after: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}/users"
            bot botToken
            query "limit" (limit >>. _.ToString())
            query "with_member" (withMember >>. _.ToString())
            query "before" before
            query "after" after
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEventUser list>

// ----- Guild Template -----

let getGuildTemplate
    (templateCode: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/templates/{templateCode}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

let createGuildFromTemplate
    (templateCode: string)
    (content: CreateGuildFromTemplatePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/templates/{templateCode}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Guild>

let getGuildTemplates
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/templates"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate list>

let createGuildTemplate
    (guildId: string)
    (content: CreateGuildTemplatePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/templates"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

let syncGuildTemplate
    (guildId: string)
    (templateCode: string)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"guilds/{guildId}/templates/{templateCode}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

let modifyGuildTemplate
    (guildId: string)
    (templateCode: string)
    (content: ModifyGuildTemplatePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/templates/{templateCode}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

let deleteGuildTemplate
    (guildId: string)
    (templateCode: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/templates/{templateCode}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

// ----- Invite -----

let getInvite
    (inviteCode: string)
    (withCounts: bool option)
    (withExpiration: bool option)
    (guildScheduledEventId: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"invites/{inviteCode}"
            bot botToken
            query "with_counts" (withCounts >>. _.ToString())
            query "with_expiration" (withExpiration >>. _.ToString())
            query "guild_scheduled_event_id" guildScheduledEventId
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Invite>

let deleteInvite
    (inviteCode: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"invites/{inviteCode}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Invite>

// ----- Message -----

let getChannelMessages
    (channelId: string)
    (around: string option)
    (before: string option)
    (after: string option)
    (limit: int option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/messages"
            bot botToken
            query "around" around
            query "before" before
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message list>

let getChannelMessage
    (channelId: string)
    (messageId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/messages/{messageId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>

let createMessage
    (channelId: string)
    (content: CreateMessagePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/messages"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>

let crosspostMessage
    (channelId: string)
    (messageId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/messages/{messageId}/crosspost"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>

let createReaction
    (channelId: string)
    (messageId: string)
    (emoji: string)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/@me"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let deleteOwnReaction
    (channelId: string)
    (messageId: string)
    (emoji: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/@me"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let deleteUserReaction
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (userId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/{userId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let getReactions
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (``type``: ReactionType option)
    (after: string option)
    (limit: int option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/messages/{messageId}/reactions/{emoji}"
            bot botToken
            query "type" (``type`` >>. int >>. _.ToString())
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<User list>

let deleteAllReactions
    (channelId: string)
    (messageId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let deleteAllReactionsForEmoji
    (channelId: string)
    (messageId: string)
    (emoji: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let editMessage
    (channelId: string)
    (messageId: string)
    (content: EditMessagePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"channels/{channelId}/messages/{messageId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>

let deleteMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let bulkDeleteMessages
    (channelId: string)
    (auditLogReason: string option)
    (content: BulkDeleteMessagesPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/messages/bulk-delete"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Poll -----

let getAnswerVoters
    (channelId: string)
    (messageId: string)
    (answerId: string)
    (after: string option)
    (limit: int option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/polls/{messageId}/answers/{answerId}"
            bot botToken
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GetAnswerVotersOkResponse>

let endPoll
    (channelId: string)
    (messageId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/polls/{messageId}/expire"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>

// ----- Role Connection -----

let getApplicationRoleConnectionMetadataRecords
    (applicationId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/role-connections/metadata"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationRoleConnectionMetadata list>

let updateApplicationRoleConnectionMetadataRecords
    (applicationId: string)
    (content: UpdateApplicationRoleConnectionMetadataRecordsPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"applications/{applicationId}/role-connections/metadata"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationRoleConnectionMetadata list>

// ----- Sku -----

let listSkus
    (applicationId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/skus"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Sku list>

// ----- Soundboard -----

let sendSoundboardSound
    (channelId: string)
    (content: SendSoundboardSoundPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/send-soundboard-sound"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let listDefaultSoundboardSounds
    botToken
    (httpClient: HttpClient) =
        req {
            get "soundboard-default-sounds"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound list>

let listGuildSoundboardSounds
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/soundboard-sounds"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound list>

let getGuildSoundboardSound
    (guildId: string)
    (soundId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/soundboard-sounds/{soundId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound>

let createGuildSoundboardSound
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildSoundboardSoundPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/soundboard-sounds"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound>

let modifyGuildSoundboardSound
    (guildId: string)
    (soundId: string)
    (auditLogReason: string option)
    (content: ModifyGuildSoundboardSoundPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/soundboard-sounds/{soundId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound>

let deleteGuildSoundboardSound
    (guildId: string)
    (soundId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/soundboard-sounds/{soundId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Stage Instance -----

let createStageInstance
    (auditLogReason: string option)
    (content: CreateStageInstancePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post "stage_instances"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<StageInstance>

let getStanceInstance
    (channelId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"stage-instances/{channelId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<StageInstance>

let modifyStageInstance
    (channelId: string)
    (auditLogReason: string option)
    (content: ModifyStageInstancePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"stage-instances/{channelId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<StageInstance>

let deleteStageInstance
    (channelId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"stage-instances/{channelId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Sticker -----

let listStickerPacks
    botToken
    (httpClient: HttpClient) =
        req {
            get "sticker-packs"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ListStickerPacksOkResponse>

let getStickerPack
    (packId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"sticker-packs/{packId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<StickerPack>

let listGuildStickers
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/stickers"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Sticker list>

let getGuildSticker
    (guildId: string)
    (stickerId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/stickers/{stickerId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Sticker>

let createGuildSticker
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildStickerPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"guilds/{guildId}/stickers"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Sticker>

let modifyGuildSticker
    (guildId: string)
    (stickerId: string)
    (auditLogReason: string option)
    (content: CreateGuildStickerPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/stickers/{stickerId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Sticker>

let deleteGuildSticker
    (guildId: string)
    (stickerId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"guilds/{guildId}/stickers/{stickerId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Subscription -----

let listSkuSubscriptions
    (skuId: string)
    (before: string option)
    (after: string option)
    (limit: int option)
    (userId: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"skus/{skuId}/subscriptions"
            bot botToken
            query "before" before
            query "after" after
            query "limit" (limit >>. _.ToString())
            query "userId" userId
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Subscription list>

let getSkuSubscription
    (skuId: string)
    (subscriptionId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"skus/{skuId}/subscriptions/{subscriptionId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Subscription>

// ----- User -----

let getCurrentUser
    (token: DiscordAccessToken)
    (httpClient: HttpClient) =
        match token with
        | DiscordAccessToken.OAUTH oauthToken ->
            req {
                get "users/@me"
                oauth oauthToken
            }
        | DiscordAccessToken.BOT botToken ->
            req {
                get "users/@me"
                bot botToken
            }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<User>

let getUser
    (userId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"users/{userId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<User>

let modifyCurrentUser
    (content: ModifyCurrentUserPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch "users/@me"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<User>

let getCurrentUserGuilds
    (before: string option)
    (after: string option)
    (limit: int option)
    (withCounts: bool option)
    (token: DiscordAccessToken)
    (httpClient: HttpClient) =
        match token with
        | DiscordAccessToken.OAUTH oauthToken ->
            req {
                get "users/@me/guilds"
                oauth oauthToken
                query "before" before
                query "after" after
                query "limit" (limit >>. _.ToString())
                query "with_counts" (withCounts >>. _.ToString())
            }
        | DiscordAccessToken.BOT botToken ->
            req {
                get "users/@me/guilds"
                bot botToken
                query "before" before
                query "after" after
                query "limit" (limit >>. _.ToString())
                query "with_counts" (withCounts >>. _.ToString())
            }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<PartialGuild list>

let getCurrentUserGuildMember
    (guildId: string)
    (token: DiscordAccessToken)
    (httpClient: HttpClient) =
        match token with
        | DiscordAccessToken.OAUTH oauthToken ->
            req {
                get $"users/@me/guilds/{guildId}/member"
                oauth oauthToken
            }
        | DiscordAccessToken.BOT botToken ->
            req {
                get $"users/@me/guilds/{guildId}/member"
                bot botToken
            }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildMember>

let leaveGuild
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"users/@me/guilds/{guildId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let createDm
    (content: CreateDmPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post "users/@me/channels"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let createGroupDm
    (content: CreateGroupDmPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post "users/@me/channels"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let getCurrentUserConnections
    oauthToken
    (httpClient: HttpClient) =
        req {
            get "users/@me/connections"
            oauth oauthToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Connection list>

let getCurrentUserApplicationRoleConnection
    (applicationId: string)
    oauthToken
    (httpClient: HttpClient) =
        req {
            get $"users/@me/applications/{applicationId}/role-connection"
            oauth oauthToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationRoleConnection>

let updateCurrentUserApplicationRoleConnection
    (applicationId: string)
    (content: UpdateCurrentUserApplicationRoleConnectionPayload)
    oauthToken
    (httpClient: HttpClient) =
        req {
            put $"users/@me/applications/{applicationId}/role-connection"
            oauth oauthToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationRoleConnection>

// ----- Voice -----

let listVoiceRegions
    botToken
    (httpClient: HttpClient) =
        req {
            get "voice/regions"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<VoiceRegion list>

let getCurrentUserVoiceState
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/voice-states/@me"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<VoiceState>

let getUserVoiceState
    (guildId: string)
    (userId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/voice-states/{userId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<VoiceState>

let modifyCurrentUserVoiceState
    (guildId: string)
    (content: ModifyCurrentUserVoiceStatePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/voice-states/@me"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let modifyUserVoiceState
    (guildId: string)
    (userId: string)
    (content: ModifyUserVoiceStatePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"guilds/{guildId}/voice-states/{userId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Webhook -----

let createWebhook
    (channelId: string)
    (auditLogReason: string option)
    (content: CreateWebhookPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"channels/{channelId}/webhooks"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let getChannelWebhooks
    (channelId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"channels/{channelId}/webhooks"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Webhook list>

let getGuildWebhooks
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"guilds/{guildId}/webhooks"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Webhook list>

let getWebhook
    (webhookId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"webhooks/{webhookId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let getWebhookWithToken
    (webhookId: string)
    (webhookToken: string)
    (httpClient: HttpClient) =
        req {
            get $"webhooks/{webhookId}/{webhookToken}"
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let modifyWebhook
    (webhookId: string)
    (auditLogReason: string option)
    (content: ModifyWebhookPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"webhooks/{webhookId}"
            bot botToken
            audit auditLogReason
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let modifyWebhookWithToken
    (webhookId: string)
    (webhookToken: string)
    (content: ModifyWebhookWithTokenPayload)
    (httpClient: HttpClient) =
        req {
            patch $"webhooks/{webhookId}/{webhookToken}"
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let deleteWebhook
    (webhookId: string)
    (auditLogReason: string option)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"webhooks/{webhookId}"
            bot botToken
            audit auditLogReason
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let deleteWebhookWithToken
    (webhookId: string)
    (webhookToken: string)
    (httpClient: HttpClient) =
        req {
            delete $"webhooks/{webhookId}/{webhookToken}"
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

let executeWebhook
    (webhookId: string)
    (webhookToken: string)
    (wait: bool option)
    (threadId: string option)
    (content: ExecuteWebhookPayload)
    (httpClient: HttpClient) =
        req {
            post $"webhooks/{webhookId}/{webhookToken}"
            query "wait" (wait >>. _.ToString())
            query "thread_id" threadId
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>

let getWebhookMessage
    (webhookId: string)
    (webhookToken: string)
    (messageId: string)
    (threadId: string option)
    (httpClient: HttpClient) =
        req {
            get $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
            query "thread_id" threadId
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>

let editWebhookMessage
    (webhookId: string)
    (webhookToken: string)
    (messageId: string)
    (threadId: string option)
    (content: EditWebhookMessagePayload)
    (httpClient: HttpClient) =
        req {
            patch $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
            query "thread_id" threadId
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>

let deleteWebhookMessage
    (webhookId: string)
    (webhookToken: string)
    (messageId: string)
    (threadId: string option)
    (httpClient: HttpClient) =
        req {
            patch $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
            query "thread_id" threadId
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Gateway -----

let getGateway
    (version: string)
    (encoding: GatewayEncoding)
    (compression: GatewayCompression option)
    (httpClient: HttpClient) =
        req {
            get "gateway"
            query "v" version
            query "encoding" (encoding.ToString())
            query "compress" (compression >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GetGatewayOkResponse>
        
let getGatewayBot
    (version: string)
    (encoding: GatewayEncoding)
    (compression: GatewayCompression option)
    botToken
    (httpClient: HttpClient) =
        req {
            get "gateway/bot"
            bot botToken
            query "v" version
            query "encoding" (encoding.ToString())
            query "compress" (compression >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GetGatewayBotOkResponse>
        

// ----- OAuth2 -----

let getCurrentBotApplicationInformation
    botToken // TODO: Check swagger for if this is meant to be bot or access token
    (httpClient: HttpClient) =
        req {
            get "oauth2/applications/@me"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Application>

let getCurrentAuthorizationInformation
    oauthToken
    (httpClient: HttpClient) =
        req {
            get "oauth2/@me"
            oauth oauthToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GetCurrentAuthorizationInformationOkResponse>
