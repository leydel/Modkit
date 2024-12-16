module Discordfs.Rest.Rest

open Discordfs.Rest.Modules
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
    (client: BotClient) =
        req {
            get "applications/@me"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Application>

let editCurrentApplication
    (content: EditCurrentApplicationPayload)
    (client: BotClient) =
        req {
            patch "applications/@me"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Application>

let getApplicationActivityInstance
    (applicationId: string)
    (instanceId: string)
    (client: BotClient) =
        req {
            patch $"applications/{applicationId}/activity-instances/{instanceId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ActivityInstance>

// ----- Audit Log -----

let getGuildAuditLog
    (guildId: string)
    (userId: string option)
    (actionType: AuditLogEventType option)
    (before: string option)
    (after: string option)
    (limit: int option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/audit-logs"
            query "user_id" userId
            query "action_type" (actionType >>. _.ToString())
            query "before" before
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<AuditLog>

// ----- Auto Moderation -----

let getAutoModerationRule
    (guildId: string)
    (autoModerationRuleId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<AutoModerationRule>

let createAutoModerationRule
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateAutoModerationRulePayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/auto-moderation/rules"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<AutoModerationRule>

let modifyAutoModerationRule
    (guildId: string)
    (autoModerationRuleId: string)
    (auditLogReason: string option)
    (content: CreateAutoModerationRulePayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<AutoModerationRule>

let deleteAutoModerationRule
    (guildId: string)
    (autoModerationRuleId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Channel -----

let getChannel
    (channelId: string)
    (client: BotClient) =
        req {
            get $"channels/{channelId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let modifyChannel
    (channelId: string)
    (auditLogReason: string option)
    (content: ModifyChannelPayload)
    (client: BotClient) =
        req {
            patch $"channels/{channelId}"
            payload (content.Payload)
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let deleteChannel
    (channelId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let editChannelPermissions
    (channelId: string)
    (overwriteId: string)
    (auditLogReason: string option)
    (content: EditChannelPermissionsPayload)
    (client: BotClient) =
        req {
            put $"channels/{channelId}/permissions/{overwriteId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let getChannelInvites
    (channelId: string)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/invites"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<InviteWithMetadata list>

let createChannelInvite
    (channelId: string)
    (auditLogReason: string option)
    (content: CreateChannelInvitePayload)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/invites"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asOptionalJson<InviteWithMetadata>

let deleteChannelPermission
    (channelId: string)
    (overwriteId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/permissions/{overwriteId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let followAnnouncementChannel
    (channelId: string)
    (auditLogReason: string option)
    (content: FollowAnnouncementChannelPayload)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/followers"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<FollowedChannel>

let triggerTypingIndicator
    (channelId: string)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/typing"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let getPinnedMessages
    (channelId: string)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/pins"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message list>

let pinMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            put $"channels/{channelId}/pins/{messageId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let unpinMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/pins/{messageId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let groupDmAddRecipient
    (channelId: string)
    (userId: string)
    (content: GroupDmAddRecipientPayload)
    (client: BotClient) =
        req {
            put $"channels/{channelId}/recipients/{userId}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asOptionalJson<Channel>

let groupDmRemoveRecipient
    (channelId: string)
    (userId: string)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/recipients/{userId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let startThreadFromMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    (content: StartThreadFromMessagePayload)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/messages/{messageId}/threads"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let startThreadWithoutMessage
    (channelId: string)
    (auditLogReason: string option)
    (content: StartThreadWithoutMessagePayload)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/threads"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let startThreadInForumOrMediaChannel
    (channelId: string)
    (auditLogReason: string option)
    (content: StartThreadWithoutMessagePayload)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/threads"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<StartThreadInForumOrMediaChannelOkResponse>

let joinThread
    (channelId: string)
    (client: BotClient) =
        req {
            put $"channels/{channelId}/thread-members/@me"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let addThreadMember
    (channelId: string)
    (userId: string)
    (client: BotClient) =
        req {
            put $"channels/{channelId}/thread-members/{userId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let leaveThread
    (channelId: string)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/thread-members/@me"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let removeThreadMember
    (channelId: string)
    (userId: string)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/thread-members/{userId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let getThreadMember
    (channelId: string)
    (userId: string)
    (withMember: bool option)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/thread-members/{userId}"
            query "with_member" (match withMember with | Some true -> Some "true" | _ -> None)
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ThreadMember>

let listThreadMembers
    (channelId: string)
    (withMember: bool option)
    (after: string option)
    (limit: int option)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/thread-members"
            query "with_member" (match withMember with | Some true -> Some "true" | _ -> None)
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ThreadMember list>

    // TODO: Test paginated response and implement for list thread members

let listPublicArchivedThreads
    (channelId: string)
    (before: DateTime option)
    (limit: int option)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/threads/archived/public"
            query "before" (before >>. _.ToString())
            query "limit" (limit >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ListPublicArchivedThreadsOkResponse>

let listPrivateArchivedThreads
    (channelId: string)
    (before: DateTime option)
    (limit: int option)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/threads/archived/private"
            query "before" (before >>. _.ToString())
            query "limit" (limit >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ListPrivateArchivedThreadsOkResponse>

let listJoinedPrivateArchivedThreads
    (channelId: string)
    (before: DateTime option)
    (limit: int option)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/users/@me/threads/archived/private"
            query "before" (before >>. _.ToString())
            query "limit" (limit >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ListJoinedPrivateArchivedThreadsOkResponse>

// ----- Emoji -----

let listGuildEmojis
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/emojis"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Emoji list>

let getGuildEmoji
    (guildId: string)
    (emojiId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/emojis/{emojiId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Emoji>

let createGuildEmoji
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildEmojiPayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/emojis"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Emoji>
        
let modifyGuildEmoji
    (guildId: string)
    (emojiId: string)
    (auditLogReason: string option)
    (content: ModifyGuildEmojiPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/emojis/{emojiId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Emoji>

let deleteGuildEmoji
    (guildId: string)
    (emojiId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/emojis/{emojiId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty
        
let listApplicationEmojis
    (applicationId: string)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/emojis"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ListApplicationEmojisOkResponse>

let getApplicationEmoji
    (applicationId: string)
    (emojiId: string)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/emojis/{emojiId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Emoji>

let createApplicationEmoji
    (applicationId: string)
    (content: CreateApplicationEmojiPayload)
    (client: BotClient) =
        req {
            post $"applications/{applicationId}/emojis"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Emoji>
        
let modifyApplicationEmoji
    (applicationId: string)
    (emojiId: string)
    (content: ModifyApplicationEmojiPayload)
    (client: BotClient) =
        req {
            patch $"applications/{applicationId}/emojis/{emojiId}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Emoji>

let deleteApplicationEmoji
    (applicationId: string)
    (emojiId: string)
    (client: BotClient) =
        req {
            delete $"applications/{applicationId}/emojis/{emojiId}"
        }
        |> client.SendAsync
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
    (excludeDeleted: bool option)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/entitlements"
            query "user_id" userId
            query "sku_ids" (skuIds >>. String.concat ",")
            query "before" before
            query "after" after
            query "limit" (limit >>. _.ToString())
            query "guild_id" guildId
            query "exclude_ended" (excludeEnded >>. _.ToString())
            query "exclude_deleted" (excludeDeleted >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Entitlement list>

let getEntitlement
    (applicationId: string)
    (entitlementId: string)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/entitlements/{entitlementId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Entitlement>

let consumeEntitlement
    (applicationId: string)
    (entitlementId: string)
    (client: BotClient) =
        req {
            post $"applications/{applicationId}/entitlements/{entitlementId}/consume"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let createTestEntitlement
    (applicationId: string)
    (content: CreateTestEntitlementPayload)
    (client: BotClient) =
        req {
            post $"applications/{applicationId}/entitlements"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Entitlement>

let deleteTestEntitlement
    (applicationId: string)
    (entitlementId: string)
    (client: BotClient) =
        req {
            delete $"applications/{applicationId}/entitlements/{entitlementId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Guild -----

let createGuild
    (content: CreateGuildPayload)
    (client: BotClient) =
        req {
            post "guilds"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Guild>

let getGuild
    (guildId: string)
    (withCounts: bool option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}"
            query "with_counts" (withCounts >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Guild>

let getGuildPreview
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/preview"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildPreview>

let modifyGuild
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Guild>

let deleteGuild
    (guildId: string)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildChannels
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/channels"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Channel list>

let createGuildChannel
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildChannelPayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/channels"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let modifyGuildChannelPositions
    (guildId: string)
    (content: ModifyGuildChannelPositionsPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/channels"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let listActiveGuildThreads
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/threads/active"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ListActiveGuildThreadsOkResponse>

let getGuildMember
    (guildId: string)
    (userId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/members/{userId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildMember>

let listGuildMembers
    (guildId: string)
    (limit: int option)
    (after: string option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/members"
            query "limit" (limit >>. _.ToString())
            query "after" (after >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildMember list>

let searchGuildMembers
    (guildId: string)
    (q: string) // query (cannot name same due to req ce)
    (limit: int option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/members/search"
            query "query" q
            query "limit" (limit >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildMember list>

let addGuildMember
    (guildId: string)
    (userId: string)
    (content: AddGuildMemberPayload)
    (client: BotClient) =
        req {
            put $"guilds/{guildId}/members/{userId}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asOptionalJson<GuildMember> // TODO: Double check this has both 200 and 204

let modifyGuildMember
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (content: ModifyGuildMemberPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/members/{userId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asOptionalJson<GuildMember> // TODO: Double check this has both 200 and 204

let modifyCurrentMember
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyCurrentMemberPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/members/@me"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asOptionalJson<GuildMember> // TODO: Double check this has both 200 and 204
        
let addGuildMemberRole
    (guildId: string)
    (userId: string)
    (roleId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            put $"guilds/{guildId}/members/{userId}/roles/{roleId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let removeGuildMemberRole
    (guildId: string)
    (userId: string)
    (roleId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/members/{userId}/roles/{roleId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let removeGuildMember
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/members/{userId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildBans
    (guildId: string)
    (limit: int option)
    (before: string option)
    (after: string option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/bans"
            query "limit" (limit >>. _.ToString())
            query "before" before
            query "after" after
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildBan list>

let getGuildBan
    (guildId: string)
    (userId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/bans/{userId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildBan>

let createGuildBan
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (content: CreateGuildBanPayload)
    (client: BotClient) =
        req {
            put $"guilds/{guildId}/bans/{userId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let removeGuildBan
    (guildId: string)
    (userId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/bans/{userId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let bulkGuildBan
    (guildId: string)
    (auditLogReason: string option)
    (content: BulkGuildBanPayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/bulk-ban"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<BulkGuildBanOkResponse>

let getGuildRoles
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/roles"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Role list>

let getGuildRole
    (guildId: string)
    (roleId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/roles/{roleId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Role>

let createGuildRole
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildRolePayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/roles"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Role>

let modifyGuildRolePositions
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildRolePositionsPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/channels"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Role list>

let modifyGuildRole
    (guildId: string)
    (roleId: string)
    (auditLogReason: string option)
    (content: ModifyGuildRolePayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/roles/{roleId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Role>

let modifyGuildMfaLevel
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildMfaLevelPayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/mfa"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildMfaLevel>

let deleteGuildRole
    (guildId: string)
    (roleId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/roles/{roleId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildPruneCount
    (guildId: string)
    (days: int option)
    (includeRoles: string list option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/prune"
            query "days" (days >>. _.ToString())
            query "include_roles" (includeRoles >>. String.concat ",")
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GetGuildPruneCountOkResponse>

let beginGuildPrune
    (guildId: string)
    (auditLogReason: string option)
    (content: BeginGuildPrunePayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/prune"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<BeginGuildPruneOkResponse>

let getGuildVoiceRegions
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/regions"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<VoiceRegion list>

let getGuildInvites
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/invites"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<InviteWithMetadata list>

let getGuildIntegrations
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/integrations"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildIntegration list>

let deleteGuildIntegration
    (guildId: string)
    (integrationId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/integrations/{integrationId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildWidgetSettings
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/widget"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildWidgetSettings>

let modifyGuildWidget
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildWidgetPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/widget"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildWidgetSettings>

let getGuildWidget
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/widget.json"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildWidget>

let getGuildVanityUrl
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/vanity-url"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GetGuildVanityUrlOkResponse>

let getGuildWidgetImage
    (guildId: string)
    (style: GuildWidgetStyle option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/widget.png"
            query "style" (style >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asRaw // TODO: Convert to png image format

let getGuildWelcomeScreen
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/welcome-screen"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<WelcomeScreen>

let modifyGuildWelcomeScreen
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildWelcomeScreenPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/welcome-screen"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<WelcomeScreen>

let getGuildOnboarding
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/onboarding"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildOnboarding>

let modifyGuildOnboarding
    (guildId: string)
    (auditLogReason: string option)
    (content: ModifyGuildOnboardingPayload)
    (client: BotClient) =
        req {
            put $"guilds/{guildId}/onboarding"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildOnboarding>

// ----- Guild Scheduled Event -----

let listGuildScheduledEvents
    (guildId: string)
    (withUserCount: bool option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/scheduled-events"
            query "with_user_count" (withUserCount >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEvent list>

let createGuildScheduledEvent
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildScheduledEventPayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/scheduled-events"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEvent>

let getGuildScheduledEvent
    (guildId: string)
    (guildScheduledEventId: string)
    (withUserCount: bool option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
            query "with_user_count" (withUserCount >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEvent>

let modifyGuildScheduledEvent
    (guildId: string)
    (guildScheduledEventId: string)
    (auditLogReason: string option)
    (content: ModifyGuildScheduledEventPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEvent>

let deleteGuildScheduledEvent
    (guildId: string)
    (guildScheduledEventId: string)
    //(auditLogReason: string option) // TODO: Check if audit log is supposed to be available for this
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let getGuildScheduledEventUsers
    (guildId: string)
    (guildScheduledEventId: string)
    (limit: int option)
    (withMember: bool option)
    (before: string option)
    (after: string option)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}/users"
            query "limit" (limit >>. _.ToString())
            query "with_member" (withMember >>. _.ToString())
            query "before" before
            query "after" after
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildScheduledEventUser list>

// ----- Guild Template -----

let getGuildTemplate
    (templateCode: string)
    (client: BotClient) =
        req {
            get $"guilds/templates/{templateCode}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

let createGuildFromTemplate
    (templateCode: string)
    (content: CreateGuildFromTemplatePayload)
    (client: BotClient) =
        req {
            post $"guilds/templates/{templateCode}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Guild>

let getGuildTemplates
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/templates"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate list>

let createGuildTemplate
    (guildId: string)
    (content: CreateGuildTemplatePayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/templates"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

let syncGuildTemplate
    (guildId: string)
    (templateCode: string)
    (client: BotClient) =
        req {
            put $"guilds/{guildId}/templates/{templateCode}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

let modifyGuildTemplate
    (guildId: string)
    (templateCode: string)
    (content: ModifyGuildTemplatePayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/templates/{templateCode}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

let deleteGuildTemplate
    (guildId: string)
    (templateCode: string)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/templates/{templateCode}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildTemplate>

// ----- Invite -----

let getInvite
    (inviteCode: string)
    (withCounts: bool option)
    (withExpiration: bool option)
    (guildScheduledEventId: string option)
    (client: BotClient) =
        req {
            get $"invites/{inviteCode}"
            query "with_counts" (withCounts >>. _.ToString())
            query "with_expiration" (withExpiration >>. _.ToString())
            query "guild_scheduled_event_id" guildScheduledEventId
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Invite>

let deleteInvite
    (inviteCode: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"invites/{inviteCode}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Invite>

// ----- Message -----

let getChannelMessages
    (channelId: string)
    (around: string option)
    (before: string option)
    (after: string option)
    (limit: int option)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/messages"
            query "around" around
            query "before" before
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message list>

let getChannelMessage
    (channelId: string)
    (messageId: string)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/messages/{messageId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>

let createMessage
    (channelId: string)
    (content: CreateMessagePayload)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/messages"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>

let crosspostMessage
    (channelId: string)
    (messageId: string)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/messages/{messageId}/crosspost"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>

let createReaction
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/@me"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let deleteOwnReaction
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/@me"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let deleteUserReaction
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (userId: string)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}/{userId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let getReactions
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (``type``: ReactionType option)
    (after: string option)
    (limit: int option)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/messages/{messageId}/reactions/{emoji}"
            query "type" (``type`` >>. int >>. _.ToString())
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<User list>

let deleteAllReactions
    (channelId: string)
    (messageId: string)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let deleteAllReactionsForEmoji
    (channelId: string)
    (messageId: string)
    (emoji: string)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}/reactions/{emoji}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let editMessage
    (channelId: string)
    (messageId: string)
    (content: EditMessagePayload)
    (client: BotClient) =
        req {
            patch $"channels/{channelId}/messages/{messageId}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>

let deleteMessage
    (channelId: string)
    (messageId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"channels/{channelId}/messages/{messageId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let bulkDeleteMessages
    (channelId: string)
    (auditLogReason: string option)
    (content: BulkDeleteMessagesPayload)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/messages/bulk-delete"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Poll -----

let getAnswerVoters
    (channelId: string)
    (messageId: string)
    (answerId: string)
    (after: string option)
    (limit: int option)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/polls/{messageId}/answers/{answerId}"
            query "after" after
            query "limit" (limit >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GetAnswerVotersOkResponse>

let endPoll
    (channelId: string)
    (messageId: string)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/polls/{messageId}/expire"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>

// ----- Role Connection -----

let getApplicationRoleConnectionMetadataRecords
    (applicationId: string)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/role-connections/metadata"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationRoleConnectionMetadata list>

let updateApplicationRoleConnectionMetadataRecords
    (applicationId: string)
    (content: UpdateApplicationRoleConnectionMetadataRecordsPayload)
    (client: BotClient) =
        req {
            put $"applications/{applicationId}/role-connections/metadata"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationRoleConnectionMetadata list>

// ----- Sku -----

let listSkus
    (applicationId: string)
    (client: BotClient) =
        req {
            get $"applications/{applicationId}/skus"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Sku list>

// ----- Soundboard -----

let sendSoundboardSound
    (channelId: string)
    (content: SendSoundboardSoundPayload)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/send-soundboard-sound"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let listDefaultSoundboardSounds
    (client: BotClient) =
        req {
            get "soundboard-default-sounds"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound list>

let listGuildSoundboardSounds
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/soundboard-sounds"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound list>

let getGuildSoundboardSound
    (guildId: string)
    (soundId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/soundboard-sounds/{soundId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound>

let createGuildSoundboardSound
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildSoundboardSoundPayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/soundboard-sounds"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound>

let modifyGuildSoundboardSound
    (guildId: string)
    (soundId: string)
    (auditLogReason: string option)
    (content: ModifyGuildSoundboardSoundPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/soundboard-sounds/{soundId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<SoundboardSound>

let deleteGuildSoundboardSound
    (guildId: string)
    (soundId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/soundboard-sounds/{soundId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Stage Instance -----

let createStageInstance
    (auditLogReason: string option)
    (content: CreateStageInstancePayload)
    (client: BotClient) =
        req {
            post "stage_instances"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<StageInstance>

let getStanceInstance
    (channelId: string)
    (client: BotClient) =
        req {
            get $"stage-instances/{channelId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<StageInstance>

let modifyStageInstance
    (channelId: string)
    (auditLogReason: string option)
    (content: ModifyStageInstancePayload)
    (client: BotClient) =
        req {
            patch $"stage-instances/{channelId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<StageInstance>

let deleteStageInstance
    (channelId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"stage-instances/{channelId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Sticker -----

let listStickerPacks
    (client: BotClient) =
        req {
            get "sticker-packs"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ListStickerPacksOkResponse>

let getStickerPack
    (packId: string)
    (client: BotClient) =
        req {
            get $"sticker-packs/{packId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<StickerPack>

let listGuildStickers
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/stickers"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Sticker list>

let getGuildSticker
    (guildId: string)
    (stickerId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/stickers/{stickerId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Sticker>

let createGuildSticker
    (guildId: string)
    (auditLogReason: string option)
    (content: CreateGuildStickerPayload)
    (client: BotClient) =
        req {
            post $"guilds/{guildId}/stickers"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Sticker>

let modifyGuildSticker
    (guildId: string)
    (stickerId: string)
    (auditLogReason: string option)
    (content: CreateGuildStickerPayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/stickers/{stickerId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Sticker>

let deleteGuildSticker
    (guildId: string)
    (stickerId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"guilds/{guildId}/stickers/{stickerId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Subscription -----

let listSkuSubscriptions
    (skuId: string)
    (before: string option)
    (after: string option)
    (limit: int option)
    (userId: string option)
    (client: BotClient) =
        req {
            get $"skus/{skuId}/subscriptions"
            query "before" before
            query "after" after
            query "limit" (limit >>. _.ToString())
            query "userId" userId
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Subscription list>

let getSkuSubscription
    (skuId: string)
    (subscriptionId: string)
    (client: BotClient) =
        req {
            get $"skus/{skuId}/subscriptions/{subscriptionId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Subscription>

// ----- User -----

let getCurrentUser
    (client: DiscordClient) =
        req {
            get "users/@me"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<User>

let getUser
    (userId: string)
    (client: BotClient) =
        req {
            get $"users/{userId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<User>

let modifyCurrentUser
    (content: ModifyCurrentUserPayload)
    (client: BotClient) =
        req {
            patch "users/@me"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<User>

let getCurrentUserGuilds
    (before: string option)
    (after: string option)
    (limit: int option)
    (withCounts: bool option)
    (client: DiscordClient) =
        req {
            get "users/@me/guilds"
            query "before" before
            query "after" after
            query "limit" (limit >>. _.ToString())
            query "with_counts" (withCounts >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<PartialGuild list>

let getCurrentUserGuildMember
    (guildId: string)
    (client: DiscordClient) =
        req {
            get $"users/@me/guilds/{guildId}/member"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GuildMember>

let leaveGuild
    (guildId: string)
    (client: BotClient) =
        req {
            delete $"users/@me/guilds/{guildId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let createDm
    (content: CreateDmPayload)
    (client: BotClient) =
        req {
            post "users/@me/channels"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let createGroupDm
    (content: CreateGroupDmPayload)
    (client: BotClient) =
        req {
            post "users/@me/channels"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Channel>

let getCurrentUserConnections
    (client: OAuthClient) =
        req {
            get "users/@me/connections"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Connection list>

let getCurrentUserApplicationRoleConnection
    (applicationId: string)
    (client: OAuthClient) =
        req {
            get $"users/@me/applications/{applicationId}/role-connection"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationRoleConnection>

let updateCurrentUserApplicationRoleConnection
    (applicationId: string)
    (content: UpdateCurrentUserApplicationRoleConnectionPayload)
    (client: OAuthClient) =
        req {
            put $"users/@me/applications/{applicationId}/role-connection"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ApplicationRoleConnection>

// ----- Voice -----

let listVoiceRegions
    (client: BotClient) =
        req {
            get "voice/regions"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<VoiceRegion list>

let getCurrentUserVoiceState
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/voice-states/@me"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<VoiceState>

let getUserVoiceState
    (guildId: string)
    (userId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/voice-states/{userId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<VoiceState>

let modifyCurrentUserVoiceState
    (guildId: string)
    (content: ModifyCurrentUserVoiceStatePayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/voice-states/@me"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let modifyUserVoiceState
    (guildId: string)
    (userId: string)
    (content: ModifyUserVoiceStatePayload)
    (client: BotClient) =
        req {
            patch $"guilds/{guildId}/voice-states/{userId}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Webhook -----

let createWebhook
    (channelId: string)
    (auditLogReason: string option)
    (content: CreateWebhookPayload)
    (client: BotClient) =
        req {
            post $"channels/{channelId}/webhooks"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let getChannelWebhooks
    (channelId: string)
    (client: BotClient) =
        req {
            get $"channels/{channelId}/webhooks"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Webhook list>

let getGuildWebhooks
    (guildId: string)
    (client: BotClient) =
        req {
            get $"guilds/{guildId}/webhooks"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Webhook list>

let getWebhook
    (webhookId: string)
    (client: BotClient) =
        req {
            get $"webhooks/{webhookId}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let getWebhookWithToken
    (webhookId: string)
    (webhookToken: string)
    (client: BotClient) =
        req {
            get $"webhooks/{webhookId}/{webhookToken}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let modifyWebhook
    (webhookId: string)
    (auditLogReason: string option)
    (content: ModifyWebhookPayload)
    (client: BotClient) =
        req {
            patch $"webhooks/{webhookId}"
            payload content
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let modifyWebhookWithToken
    (webhookId: string)
    (webhookToken: string)
    (content: ModifyWebhookWithTokenPayload)
    (client: BotClient) =
        req {
            patch $"webhooks/{webhookId}/{webhookToken}"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Webhook>

let deleteWebhook
    (webhookId: string)
    (auditLogReason: string option)
    (client: BotClient) =
        req {
            delete $"webhooks/{webhookId}"
        }
        |> DiscordRequest.withAuditLogReason auditLogReason
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let deleteWebhookWithToken
    (webhookId: string)
    (webhookToken: string)
    (client: BotClient) =
        req {
            delete $"webhooks/{webhookId}/{webhookToken}"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let executeWebhook
    (webhookId: string)
    (webhookToken: string)
    (wait: bool option)
    (threadId: string option)
    (content: ExecuteWebhookPayload)
    (client: BotClient) =
        req {
            post $"webhooks/{webhookId}/{webhookToken}"
            query "wait" (wait >>. _.ToString())
            query "thread_id" threadId
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>

let getWebhookMessage
    (webhookId: string)
    (webhookToken: string)
    (messageId: string)
    (threadId: string option)
    (client: BotClient) =
        req {
            get $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
            query "thread_id" threadId
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>

let editWebhookMessage
    (webhookId: string)
    (webhookToken: string)
    (messageId: string)
    (threadId: string option)
    (content: EditWebhookMessagePayload)
    (client: BotClient) =
        req {
            patch $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
            query "thread_id" threadId
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Message>

let deleteWebhookMessage
    (webhookId: string)
    (webhookToken: string)
    (messageId: string)
    (threadId: string option)
    (client: BotClient) =
        req {
            patch $"webhooks/{webhookId}/{webhookToken}/messages/{messageId}"
            query "thread_id" threadId
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Gateway -----

let getGateway
    (version: string)
    (encoding: GatewayEncoding)
    (compression: GatewayCompression option)
    (client: HttpClient) =
        req {
            get "gateway"
            query "v" version
            query "encoding" (encoding.ToString())
            query "compress" (compression >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GetGatewayOkResponse>
        
let getGatewayBot
    (version: string)
    (encoding: GatewayEncoding)
    (compression: GatewayCompression option)
    (client: BotClient) =
        req {
            get "gateway/bot"
            query "v" version
            query "encoding" (encoding.ToString())
            query "compress" (compression >>. _.ToString())
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GetGatewayBotOkResponse>
        

// ----- OAuth2 -----

let getCurrentBotApplicationInformation
    (client: OAuthClient) =
        req {
            get "oauth2/applications/@me"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<Application>

let getCurrentAuthorizationInformation
    (client: OAuthClient) =
        req {
            get "oauth2/@me"
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<GetCurrentAuthorizationInformationOkResponse>

let authorizationCodeGrant
    (content: AuthorizationCodeGrantPayload)
    (client: BasicClient) =
        req {
            post "oauth2/token"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<AuthorizationCodeGrantResponse>

let refreshTokenGrant
    (content: RefreshTokenGrantPayload)
    (client: BasicClient) =
        req {
            post "oauth2/token"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<RefreshTokenGrantResponse>

let revokeToken
    (content: RevokeTokenPayload)
    (client: BasicClient) =
        req {
            post "oauth2/token/revoke"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asEmpty

let clientCredentialsGrant
    (content: ClientCredentialsGrantPayload)
    (client: BasicClient) =
        req {
            post "oauth2/token"
            payload content
        }
        |> client.SendAsync
        ?>> DiscordResponse.asJson<ClientCredentialsGrantResponse>
