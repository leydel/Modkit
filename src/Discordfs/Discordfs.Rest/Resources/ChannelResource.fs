namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System
open System.Net.Http
open System.Threading.Tasks

type ModifyChannel =
    | GroupDm of ModifyGroupDmChannel
    | Guild of ModifyGuildChannel
    | Thread of ModifyThreadChannel
with
    member this.Payload =
        match this with
        | ModifyChannel.GroupDm groupdm -> groupdm :> Payload
        | ModifyChannel.Guild guild -> guild :> Payload
        | ModifyChannel.Thread thread -> thread :> Payload

and ModifyGroupDmChannel(
    ?name: string,
    ?icon: string
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "icon" icon
        }

and ModifyGuildChannel(
    ?name: string,
    ?``type``: ChannelType,
    ?position: int option,
    ?topic: string option,
    ?nsfw: bool option,
    ?rate_limit_per_user: int option,
    ?bitrate: int option,
    ?user_limit: int option,
    ?permission_overwrites: PermissionOverwrite list option,
    ?parent_id: string option,
    ?rtc_region: string option,
    ?video_quality_mode: VideoQualityMode option,
    ?default_auto_archive_duration: int option,
    ?flags: int,
    ?available_tags: ChannelTag list,
    ?default_reaction_emoji: DefaultReaction option,
    ?default_thread_rate_limit_per_user: int,
    ?default_sort_order: ChannelSortOrder option,
    ?default_forum_layout: ChannelForumLayout
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "type" ``type``
            optional "position" position
            optional "topic" topic
            optional "nsfw" nsfw
            optional "rate_limit_per_user" rate_limit_per_user
            optional "bitrate" bitrate
            optional "user_limit" user_limit
            optional "permission_overwrites" permission_overwrites
            optional "parent_id" parent_id
            optional "rtc_region" rtc_region
            optional "video_quality_mode" video_quality_mode
            optional "default_auto_archive_duration" default_auto_archive_duration
            optional "flags" flags
            optional "available_tags" available_tags
            optional "default_reaction_emoji" default_reaction_emoji
            optional "default_thread_rate_limit_per_user" default_thread_rate_limit_per_user
            optional "default_sort_order" default_sort_order
            optional "default_forum_layout" default_forum_layout
        }

and ModifyThreadChannel (
    ?name: string,
    ?archived: bool,
    ?auto_archive_duration: int,
    ?locked: bool,
    ?invitable: bool,
    ?rate_limit_per_user: int option,
    ?flags: int,
    ?applied_tags: string list
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "archived" archived
            optional "auto_archive_duration" auto_archive_duration
            optional "locked" locked
            optional "invitable" invitable
            optional "rate_limit_per_user" rate_limit_per_user
            optional "flags" flags
            optional "applied_tags" applied_tags
        }

type EditChannelPermissions (
    ``type``: EditChannelPermissionsType,
    ?allow: string option,
    ?deny: string option
) =
    inherit Payload() with
        override _.Content = json {
            required "type" ``type``
            optional "allow" allow
            optional "deny" deny
        }

type CreateChannelInvite (
    target_type: InviteTargetType,
    ?max_age: int,
    ?max_uses: int,
    ?temporary: bool,
    ?unique: bool,
    ?target_user_id: string,
    ?target_application_id: string
) =
    inherit Payload() with
        override _.Content = json {
            optional "max_age" max_age
            optional "max_uses" max_uses
            optional "temporary" temporary
            optional "unique" unique
            required "target_type" target_type
            optional "target_user_id" target_user_id
            optional "target_application_id" target_application_id
        }

type FollowAnnouncementChannel (
    webhook_channel_id: string
) =
    inherit Payload() with
        override _.Content = json {
            required "webhook_channel_id" webhook_channel_id
        }

type IChannelResource =
    // https://discord.com/developers/docs/resources/channel#get-channel
    abstract member GetChannel:
        channelId: string ->
        Task<Channel>
    
    // https://discord.com/developers/docs/resources/channel#modify-channel
    abstract member ModifyChannel:
        channelId: string ->
        auditLogReason: string option ->
        content: ModifyChannel ->
        Task<Channel>

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
        content: EditChannelPermissions ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#get-channel-invites
    abstract member GetChannelInvites:
        channelId: string ->
        Task<InviteWithMetadata list>

    // https://discord.com/developers/docs/resources/channel#create-channel-invite
    abstract member CreateChannelInvite:
        channelId: string ->
        auditLogReason: string option ->
        content: CreateChannelInvite ->
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
        content: FollowAnnouncementChannel ->
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
        member _.GetChannel channelId =
            req {
                get $"channels/{channelId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyChannel channelId auditLogReason content =
            req {
                patch $"channels/{channelId}"
                bot token
                audit auditLogReason
                payload (content.Payload)
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteChannel channelId auditLogReason =
            req {
                delete $"channels/{channelId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.EditChannelPermissions channelId overwriteId auditLogReason content =
            req {
                put $"channels/{channelId}/permissions/{overwriteId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.GetChannelInvites channelId =
            req {
                get $"channels/{channelId}/invites"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateChannelInvite channelId auditLogReason content =
            req {
                post $"channels/{channelId}/invites"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteChannelPermission channelId overwriteId auditLogReason =
            req {
                delete $"channels/{channelId}/permissions/{overwriteId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.FollowAnnouncementChannel channelId auditLogReason content =
            req {
                post $"channels/{channelId}/followers"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.TriggerTypingIndicator channelId =
            req {
                post $"channels/{channelId}/typing"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

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
