module Discordfs.Rest.Rest

open Discordfs.Rest.Common
open Discordfs.Types
open System
open System.Net.Http

// ----- Interaction -----

let createInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (withResponse: bool option)
    (content: CreateInteractionResponsePayload<'a>)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"interactions/{interactionId}/{interactionToken}/callback"
            bot botToken
            query "with_response" (withResponse >>. _.ToString())
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asOptionalJson<InteractionCallbackResponse>
            
let getOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"webhooks/{interactionId}/{interactionToken}/messages/@original"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
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
        ?>> DiscordResponse.asJson<Message>
            
let deleteOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"webhooks/{interactionId}/{interactionToken}/messages/@original"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty
            
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
        ?>> DiscordResponse.asEmpty
            
let getFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
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
        ?>> DiscordResponse.asJson<Message>
            
let deleteFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Application Command -----

let getGlobalApplicationCommands
    (applicationId: string)
    (withLocalizations: bool option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/commands"
            bot botToken
            query "with_localizations" (withLocalizations >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let createGlobalApplicationCommand
    (applicationId: string)
    (content: CreateGlobalApplicationCommandPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"applications/{applicationId}/commands"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let getGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/commands/{commandId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let editGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    (content: EditGlobalApplicationCommandPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"applications/{applicationId}/commands/{commandId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let deleteGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"applications/{applicationId}/commands/{commandId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty
            
let bulkOverwriteGlobalApplicationCommands
    (applicationId: string)
    (content: BulkOverwriteGlobalApplicationCommandsPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"applications/{applicationId}/commands"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let getGuildApplicationCommands
    (applicationId: string)
    (guildId: string)
    (withLocalizations: bool option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands"
            bot botToken
            query "with_localizations" (withLocalizations >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let createGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (content: CreateGuildApplicationCommandPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"applications/{applicationId}/guilds/{guildId}/commands"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let getGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let editGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (content: EditGuildApplicationCommandPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let deleteGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty
            
let bulkOverwriteGuildApplicationCommands
    (applicationId: string)
    (guildId: string)
    (content: BulkOverwriteGlobalApplicationCommandsPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"applications/{applicationId}/guilds/{guildId}/commands"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let getGuildApplicationCommandsPermissions
    (applicationId: string)
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/permissions"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildApplicationCommandPermissions list>
            
let getGuildApplicationCommandPermissions
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildApplicationCommandPermissions>
            
let editApplicationCommandPermissions
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (content: EditApplicationCommandPermissions)
    oauthToken
    (httpClient: HttpClient) =
        req {
            put $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
            oauth oauthToken
            payload content
        }
        |> httpClient.SendAsync
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

// TODO: Implement

// ----- Soundboard -----

// TODO: Implement

// ----- Stage Instance -----

// TODO: Implement

// ----- Sticker -----

// TODO: Implement

// ----- Subscription -----

// TODO: Implement

// ----- User -----

// TODO: Implement

// ----- Voice -----

// TODO: Implement

// ----- Webhook -----

// TODO: Implement

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
    botToken
    (httpClient: HttpClient) =
        req {
            get "oauth2/applications/@me"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Application>

let getCurrentAuthorizationInformation
    oauth2AccessToken
    (httpClient: HttpClient) =
        req {
            get "oauth2/@me"
            oauth oauth2AccessToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GetCurrentAuthorizationInformationOkResponse>
