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
        ?>> DiscordResponse.asJson<Emoji list>

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

// TODO: Implement

// ----- Guild -----

// TODO: Implement

// ----- Guild Scheduled Event -----

// TODO: Implement

// ----- Guild Template -----

// TODO: Implement

// ----- Invite -----

// TODO: Implement

// ----- Message -----

// TODO: Implement

// ----- Poll -----

// TODO: Implement

// ----- Role Connection -----

// TODO: Implement

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

// TODO: Implement

// ----- OAuth2 -----

// TODO: Implement
