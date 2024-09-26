﻿namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System
open System.Net.Http
open System.Threading.Tasks

type IChannelResource =
    // https://discord.com/developers/docs/resources/channel#get-channel
    abstract member GetChannel:
        channelId: string ->
        Task<Channel>
    
    // https://discord.com/developers/docs/resources/channel#modify-channel
    // TODO: Implement modify channel endpoint

    // https://discord.com/developers/docs/resources/channel#deleteclose-channel
    abstract member DeleteChannel:
        channelId: string ->
        auditLogReason: string option ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/channel#edit-channel-permissions
    abstract member EditChannelPermissions:
        channelId: string ->
        overwriteId: string ->
        auditLogReason: string option ->
        allow: string option ->
        deny: string option ->
        ``type``: EditChannelPermissionsType ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#get-channel-invites
    abstract member GetChannelInvites:
        channelId: string ->
        Task<InviteWithMetadata list>

    // https://discord.com/developers/docs/resources/channel#create-channel-invite
    abstract member CreateChannelInvite:
        channelId: string ->
        auditLogReason: string option ->
        maxAge: int option ->
        maxUses: int option ->
        temporary: bool option ->
        unique: bool option ->
        targetType: InviteTargetType option ->
        targetUserId: string option ->
        targetApplicationId: string option ->
        Task<Invite>

    // https://discord.com/developers/docs/resources/channel#delete-channel-permission
    abstract member DeleteChannelPermission:
        channelId: string ->
        overwriteId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#follow-announcement-channel
    abstract member FollowAnnouncementChannel:
        channelId: string ->
        auditLogReason: string option ->
        webhookChannelId: string ->
        Task<FollowedChannel>

    // https://discord.com/developers/docs/resources/channel#trigger-typing-indicator
    abstract member TriggerTypingIndicator:
        channelId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#get-pinned-messages
    abstract member GetPinnedMessages:
        channelId: string ->
        Task<Message list>

    // https://discord.com/developers/docs/resources/channel#pin-message
    abstract member PinMessage:
        channelId: string ->
        messageId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#unpin-message
    abstract member UnpinMessage:
        channelId: string ->
        messageId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#group-dm-add-recipient
    abstract member GroupDmAddRecipient:
        channelId: string ->
        recipientId: string ->
        accessToken: string ->
        nick: string option ->
        Task<unit> // According to openapi sec, appears to return 201 or 204 (remove this comment once status codes handled properly)

    // https://discord.com/developers/docs/resources/channel#group-dm-remove-recipient
    abstract member GroupDmRemoveRecipient:
        channelId: string ->
        recipientId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#start-thread-from-message
    abstract member StartThreadFromMessage:
        channelId: string ->
        messageId: string ->
        auditLogReason: string option ->
        name: string ->
        autoArchiveDuration: AutoArchiveDurationType option ->
        rateLimitPerUser: int option ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/channel#start-thread-without-message
    abstract member StartThreadWithoutMessage:
        channelId: string ->
        auditLogReason: string option ->
        name: string ->
        autoArchiveDuration: AutoArchiveDurationType option ->
        ``type``: ThreadType ->
        invitable: bool option ->
        rateLimitPerUser: int option ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/channel#start-thread-in-forum-or-media-channel
    // TODO: Implement start thread in forum or media channel endpoint

    // https://discord.com/developers/docs/resources/channel#join-thread
    abstract member JoinThread:
        channelId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#add-thread-member
    abstract member AddThreadMember:
        channelId: string ->
        userId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#leave-thread
    abstract member LeaveThread:
        channelId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#remove-thread-member
    abstract member RemoveThreadMember:
        channelId: string ->
        userId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#get-thread-member
    abstract member GetThreadMember:
        channelId: string ->
        userId: string ->
        withMember: bool ->
        Task<ThreadMember>

    // https://discord.com/developers/docs/resources/channel#list-thread-members
    // TODO: Implement list thread members endpoint

    // https://discord.com/developers/docs/resources/channel#list-public-archived-threads
    abstract member ListPublicArchivedThreads:
        channelId: string ->
        before: DateTime option ->
        limit: int option ->
        Task<ListPublicArchivedThreadsResponse>

    // https://discord.com/developers/docs/resources/channel#list-private-archived-threads
    abstract member ListPrivateArchivedThreads:
        channelId: string ->
        before: DateTime option ->
        limit: int option ->
        Task<ListPrivateArchivedThreadsResponse>

    // https://discord.com/developers/docs/resources/channel#list-joined-private-archived-threads
    abstract member ListJoinedPrivateArchivedThreads:
        channelId: string ->
        before: DateTime option ->
        limit: int option ->
        Task<ListJoinedPrivateArchivedThreadsResponse>

type ChannelResource (httpClientFactory: IHttpClientFactory, token: string) =
    interface IChannelResource with
        member _.GetChannel
            channelId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteChannel
            channelId auditLogReason =
                Req.create
                        HttpMethod.Delete
                        Constants.DISCORD_API_URL
                        $"channels/{channelId}"
                    |> Req.bot token
                    |> Req.audit auditLogReason
                    |> Req.send httpClientFactory
                    |> Res.json

        member _.EditChannelPermissions
            channelId overwriteId auditLogReason allow deny ``type`` =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/permissions/{overwriteId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "allow" allow
                    |> Dto.propertyIf "deny" deny
                    |> Dto.property "type" ``type``
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.GetChannelInvites
            channelId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/invites"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.CreateChannelInvite
            channelId auditLogReason maxAge maxUses temporary unique targetType targetUserId targetApplicationId =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/invites"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "max_age" maxAge
                    |> Dto.propertyIf "max_uses" maxUses
                    |> Dto.propertyIf "temporary" temporary
                    |> Dto.propertyIf "unique" unique
                    |> Dto.propertyIf "target_type" targetType
                    |> Dto.propertyIf "target_user_id" targetUserId
                    |> Dto.propertyIf "target_application_id" targetApplicationId
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteChannelPermission
            channelId overwriteId auditLogReason =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/permissions/{overwriteId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.FollowAnnouncementChannel
            channelId auditLogReason webhookChannelId =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/followers"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property "webhook_channel_id" webhookChannelId
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.TriggerTypingIndicator
            channelId =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/typing"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.GetPinnedMessages
            channelId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/pins"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.PinMessage
            channelId messageId auditLogReason =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/pins/{messageId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.UnpinMessage
            channelId messageId auditLogReason =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/pins/{messageId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.GroupDmAddRecipient
            channelId userId accessToken nick =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/recipients/{userId}"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.property "access_token" accessToken
                    |> Dto.propertyIf "nick" nick
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.GroupDmRemoveRecipient
            channelId userId =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/recipients/{userId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.StartThreadFromMessage
            channelId messageId auditLogReason name autoArchiveDuration rateLimitPerUser =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/messages/{messageId}/threads"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property "name" name
                    |> Dto.propertyIf "auto_archive_duration" autoArchiveDuration
                    |> Dto.propertyIf "rate_limit_per_user" rateLimitPerUser
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.StartThreadWithoutMessage
            channelId auditLogReason name autoArchiveDuration ``type`` invitable rateLimitPerUser =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/threads"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property "name" name
                    |> Dto.propertyIf "auto_archive_duration" autoArchiveDuration
                    |> Dto.property "type" ``type``
                    |> Dto.propertyIf "invitable" invitable
                    |> Dto.propertyIf "rate_limit_per_user" rateLimitPerUser
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.JoinThread
            channelId =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/thread-members/@me"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.AddThreadMember
            channelId userId =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/thread-members/{userId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.LeaveThread
            channelId =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/thread-members/@me"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore
            
        member _.RemoveThreadMember
            channelId userId =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/thread-members/{userId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.GetThreadMember
            channelId userId withMember =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/thread-members/{userId}"
                |> Req.bot token
                |> Req.queryOpt "with_member" (if withMember then Some "true" else None)
                |> Req.send httpClientFactory
                |> Res.json

        member _.ListPublicArchivedThreads
            channelId before limit =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/threads/archived/public"
                |> Req.bot token
                |> Req.queryOpt "before" (Option.map _.ToString() before)
                |> Req.queryOpt "limit" (Option.map _.ToString() limit)
                |> Req.send httpClientFactory
                |> Res.json

        member _.ListPrivateArchivedThreads
            channelId before limit =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/threads/archived/private"
                |> Req.bot token
                |> Req.queryOpt "before" (Option.map _.ToString() before)
                |> Req.queryOpt "limit" (Option.map _.ToString() limit)
                |> Req.send httpClientFactory
                |> Res.json

        member _.ListJoinedPrivateArchivedThreads
            channelId before limit =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"channels/{channelId}/users/@me/threads/archived/private"
                |> Req.bot token
                |> Req.queryOpt "before" (Option.map _.ToString() before)
                |> Req.queryOpt "limit" (Option.map _.ToString() limit)
                |> Req.send httpClientFactory
                |> Res.json