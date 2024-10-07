namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System
open System.Collections.Generic
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
    ?name:                               string,
    ?``type``:                           ChannelType,
    ?position:                           int option,
    ?topic:                              string option,
    ?nsfw:                               bool option,
    ?rate_limit_per_user:                int option,
    ?bitrate:                            int option,
    ?user_limit:                         int option,
    ?permission_overwrites:              PermissionOverwrite list option,
    ?parent_id:                          string option,
    ?rtc_region:                         string option,
    ?video_quality_mode:                 VideoQualityMode option,
    ?default_auto_archive_duration:      int option,
    ?flags:                              int,
    ?available_tags:                     ChannelTag list,
    ?default_reaction_emoji:             DefaultReaction option,
    ?default_thread_rate_limit_per_user: int,
    ?default_sort_order:                 ChannelSortOrder option,
    ?default_forum_layout:               ChannelForumLayout
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
    ?name:                  string,
    ?archived:              bool,
    ?auto_archive_duration: int,
    ?locked:                bool,
    ?invitable:             bool,
    ?rate_limit_per_user:   int option,
    ?flags:                 int,
    ?applied_tags:          string list
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
    ?allow:   string option,
    ?deny:    string option
) =
    inherit Payload() with
        override _.Content = json {
            required "type" ``type``
            optional "allow" allow
            optional "deny" deny
        }

type CreateChannelInvite (
    target_type:            InviteTargetType,
    ?max_age:               int,
    ?max_uses:              int,
    ?temporary:             bool,
    ?unique:                bool,
    ?target_user_id:        string,
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

type GroupDmAddRecipient (
    access_token: string,
    ?nick: string
) =
    inherit Payload() with
        override _.Content = json {
            required "access_token" access_token
            optional "nick" nick
        }

type StartThreadFromMessage (
    name:                   string,
    ?auto_archive_duration: int,
    ?rate_limit_per_user:   int option
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "auto_archive_duration" auto_archive_duration
            optional "rate_limit_per_user" rate_limit_per_user
        }

type StartThreadWithoutMessage (
    name:                   string,
    ?auto_archive_duration: int,
    ?``type``:              ThreadType,
    ?invitable:             bool,
    ?rate_limit_per_user:   int option
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "auto_archive_duration" auto_archive_duration
            optional "type" ``type``
            optional "invitable" invitable
            optional "rate_limit_per_user" rate_limit_per_user
        }


type StartThreadInForumOrMediaChannel (
    name:                   string,
    message:                ForumAndMediaThreadMessageParams,
    ?auto_archive_duration: int,
    ?applied_tags:          string list,
    ?files:                 IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                required "name" name
                optional "auto_archive_duration" auto_archive_duration
                required "message" message
                optional "applied_tags" applied_tags
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
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
        content: GroupDmAddRecipient ->
        Task<unit>

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
        content: StartThreadFromMessage ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/channel#start-thread-without-message
    abstract member StartThreadWithoutMessage:
        channelId: string ->
        auditLogReason: string option ->
        content: StartThreadWithoutMessage ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/channel#start-thread-in-forum-or-media-channel
    abstract member StartThreadInForumOrMediaChannel:
        channelId: string ->
        auditLogReason: string option ->
        content: StartThreadInForumOrMediaChannel ->
        Task<Channel>

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
        withMember: bool option ->
        Task<ThreadMember>

    // https://discord.com/developers/docs/resources/channel#list-thread-members
    abstract member ListThreadMembers:
        channelId: string ->
        withMember: bool option ->
        after: string option ->
        limit: int option ->
        Task<ThreadMember list> // TODO: Test paginated response and implement

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

        member _.GetPinnedMessages channelId =
            req {
                get $"channels/{channelId}/pins"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.PinMessage channelId messageId auditLogReason =
            req {
                put $"channels/{channelId}/pins/{messageId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.UnpinMessage channelId messageId auditLogReason =
            req {
                delete $"channels/{channelId}/pins/{messageId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.GroupDmAddRecipient channelId userId content =
            req {
                put $"channels/{channelId}/recipients/{userId}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.GroupDmRemoveRecipient channelId userId =
            req {
                delete $"channels/{channelId}/recipients/{userId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.StartThreadFromMessage channelId messageId auditLogReason content =
            req {
                post $"channels/{channelId}/messages/{messageId}/threads"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.StartThreadWithoutMessage channelId auditLogReason content =
            req {
                post $"channels/{channelId}/threads"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.StartThreadInForumOrMediaChannel channelId auditLogReason content =
            req {
                post $"channels/{channelId}/threads"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.JoinThread channelId =
            req {
                put $"channels/{channelId}/thread-members/@me"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.AddThreadMember channelId userId =
            req {
                put $"channels/{channelId}/thread-members/{userId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.LeaveThread channelId =
            req {
                delete $"channels/{channelId}/thread-members/@me"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.RemoveThreadMember channelId userId =
            req {
                delete $"channels/{channelId}/thread-members/{userId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait
            
        member _.GetThreadMember channelId userId withMember =
            req {
                get $"channels/{channelId}/thread-members/{userId}"
                bot token
                query "with_member" (match withMember with | Some true -> Some "true" | _ -> None)
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ListThreadMembers channelId withMember after limit =
            req {
                get $"channels/{channelId}/thread-members"
                bot token
                query "with_member" (match withMember with | Some true -> Some "true" | _ -> None)
                query "after" after
                query "limit" (limit >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ListPublicArchivedThreads channelId before limit =
            req {
                get $"channels/{channelId}/threads/archived/public"
                bot token
                query "before" (before >>. _.ToString())
                query "limit" (limit >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ListPrivateArchivedThreads channelId before limit =
            req {
                get $"channels/{channelId}/threads/archived/private"
                bot token
                query "before" (before >>. _.ToString())
                query "limit" (limit >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ListJoinedPrivateArchivedThreads channelId before limit =
            req {
                get $"channels/{channelId}/users/@me/threads/archived/private"
                bot token
                query "before" (before >>. _.ToString())
                query "limit" (limit >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
