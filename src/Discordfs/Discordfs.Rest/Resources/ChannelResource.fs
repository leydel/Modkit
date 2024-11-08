namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Text.Json
open System.Text.Json.Serialization
open System.Threading.Tasks

[<RequireQualifiedAccess>]
type GetChannelResponse =
    | Ok of Channel
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyChannelPayload =
    | GroupDm of ModifyGroupDmChannelPayload
    | Guild of ModifyGuildChannelPayload
    | Thread of ModifyThreadChannelPayload
with
    member this.Payload =
        match this with
        | ModifyChannelPayload.GroupDm groupdm -> groupdm :> Payload
        | ModifyChannelPayload.Guild guild -> guild :> Payload
        | ModifyChannelPayload.Thread thread -> thread :> Payload

and ModifyGroupDmChannelPayload(
    ?name: string,
    ?icon: string
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "icon" icon
        }

and ModifyGuildChannelPayload(
    ?name:                               string,
    ?``type``:                           ChannelType,
    ?position:                           int option,
    ?topic:                              string option,
    ?nsfw:                               bool option,
    ?rate_limit_per_user:                int option,
    ?bitrate:                            int option,
    ?user_limit:                         int option,
    ?permission_overwrites:              PartialPermissionOverwrite list option,
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

and ModifyThreadChannelPayload (
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
        
[<RequireQualifiedAccess>]
type ModifyChannelResponse =
    | Ok of Channel
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type DeleteChannelResponse =
    | Ok of Channel
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EditChannelPermissionsPayload (
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
        
[<RequireQualifiedAccess>]
type EditChannelPermissionsResponse =
    | NoContent
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type GetChannelInvitesResponse =
    | Ok of InviteWithMetadata list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateChannelInvitePayload (
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
        
[<RequireQualifiedAccess>]
type CreateChannelInviteResponse =
    | Ok of InviteWithMetadata
    | NoContent
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type DeleteChannelPermissionResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type FollowAnnouncementChannelPayload (
    webhook_channel_id: string
) =
    inherit Payload() with
        override _.Content = json {
            required "webhook_channel_id" webhook_channel_id
        }
        
[<RequireQualifiedAccess>]
type FollowAnnouncementChannelResponse =
    | Ok of FollowedChannel
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type TriggerTypingIndicatorResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type GetPinnedMessagesResponse =
    | Ok of Message list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type PinMessageResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type UnpinMessageResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GroupDmAddRecipientPayload (
    access_token: string,
    ?nick: string
) =
    inherit Payload() with
        override _.Content = json {
            required "access_token" access_token
            optional "nick" nick
        }
        
[<RequireQualifiedAccess>]
type GroupDmAddRecipientResponse =
    | Created of Channel
    | NoContent
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type GroupDmRemoveRecipientResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type StartThreadFromMessagePayload (
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
        
[<RequireQualifiedAccess>]
type StartThreadFromMessageResponse =
    | Created of Channel
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type StartThreadWithoutMessagePayload (
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
        
[<RequireQualifiedAccess>]
type StartThreadWithoutMessageResponse =
    | Created of Channel
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ForumAndMediaThreadMessageParams = {
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "embeds">] Embeds: Embed list option
    [<JsonPropertyName "allowed_mentions">] AllowedMentions: AllowedMentions option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "sticker_ids">] StickerIds: string list option
    [<JsonPropertyName "attachments">] Attachments: PartialAttachment list option
    [<JsonPropertyName "flags">] Flags: int option
}

type StartThreadInForumOrMediaChannelPayload (
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

type StartThreadInForumOrMediaChannelOkResponseExtraFields = {
    [<JsonPropertyName "message">] Message: Message option
}

[<JsonConverter(typeof<StartThreadInForumOrMediaChannelOkResponseConverter>)>]
type StartThreadInForumOrMediaChannelOkResponse = {
    Channel: Channel
    ExtraFields: StartThreadInForumOrMediaChannelOkResponseExtraFields
}

and StartThreadInForumOrMediaChannelOkResponseConverter () =
    inherit JsonConverter<StartThreadInForumOrMediaChannelOkResponse> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            Channel = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let channel = Json.serializeF value.Channel
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge channel extraFields)
        
[<RequireQualifiedAccess>]
type StartThreadInForumOrMediaChannelResponse =
    | Created of StartThreadInForumOrMediaChannelOkResponse
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type JoinThreadResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type AddThreadMemberResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type LeaveThreadResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type RemoveThreadMemberResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type GetThreadMemberResponse =
    | Ok of ThreadMember
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
[<RequireQualifiedAccess>]
type ListThreadMembersResponse =
    | Ok of ThreadMember list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ListPublicArchivedThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
    [<JsonPropertyName "has_more">] HasMore: bool
}

[<RequireQualifiedAccess>]
type ListPublicArchivedThreadsResponse =
    | Ok of ListPublicArchivedThreadsOkResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ListPrivateArchivedThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
    [<JsonPropertyName "has_more">] HasMore: bool
}

[<RequireQualifiedAccess>]
type ListPrivateArchivedThreadsResponse =
    | Ok of ListPrivateArchivedThreadsOkResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ListJoinedPrivateArchivedThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
    [<JsonPropertyName "has_more">] HasMore: bool
}

[<RequireQualifiedAccess>]
type ListJoinedPrivateArchivedThreadsResponse =
    | Ok of ListJoinedPrivateArchivedThreadsOkResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Channel =
    let getChannel
        (channelId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"channels/{channelId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetChannelResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetChannelResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetChannelResponse.TooManyRequests (Http.toJson res)
                | status -> return GetChannelResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyChannelResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyChannelResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyChannelResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyChannelResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyChannelResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map DeleteChannelResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map DeleteChannelResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteChannelResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteChannelResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return EditChannelPermissionsResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map EditChannelPermissionsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditChannelPermissionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditChannelPermissionsResponse.TooManyRequests (Http.toJson res)
                | status -> return EditChannelPermissionsResponse.Other status
            })

    let getChannelInvites
        (channelId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"channels/{channelId}/invites"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetChannelInvitesResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetChannelInvitesResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetChannelInvitesResponse.TooManyRequests (Http.toJson res)
                | status -> return GetChannelInvitesResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map CreateChannelInviteResponse.Ok (Http.toJson res)
                | HttpStatusCode.NoContent -> return CreateChannelInviteResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map CreateChannelInviteResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map CreateChannelInviteResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateChannelInviteResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateChannelInviteResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteChannelPermissionResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteChannelPermissionResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteChannelPermissionResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteChannelPermissionResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map FollowAnnouncementChannelResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map FollowAnnouncementChannelResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map FollowAnnouncementChannelResponse.TooManyRequests (Http.toJson res)
                | status -> return FollowAnnouncementChannelResponse.Other status
            })

    let triggerTypingIndicator
        (channelId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"channels/{channelId}/typing"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return TriggerTypingIndicatorResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map TriggerTypingIndicatorResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map TriggerTypingIndicatorResponse.TooManyRequests (Http.toJson res)
                | status -> return TriggerTypingIndicatorResponse.Other status
            })

    let getPinnedMessages
        (channelId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"channels/{channelId}/pins"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetPinnedMessagesResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetPinnedMessagesResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetPinnedMessagesResponse.TooManyRequests (Http.toJson res)
                | status -> return GetPinnedMessagesResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return PinMessageResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map PinMessageResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map PinMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return PinMessageResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return UnpinMessageResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map UnpinMessageResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map UnpinMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return UnpinMessageResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map GroupDmAddRecipientResponse.Created (Http.toJson res)
                | HttpStatusCode.NoContent -> return GroupDmAddRecipientResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map GroupDmAddRecipientResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GroupDmAddRecipientResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GroupDmAddRecipientResponse.TooManyRequests (Http.toJson res)
                | status -> return GroupDmAddRecipientResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return GroupDmRemoveRecipientResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map GroupDmRemoveRecipientResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GroupDmRemoveRecipientResponse.TooManyRequests (Http.toJson res)
                | status -> return GroupDmRemoveRecipientResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map StartThreadFromMessageResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map StartThreadFromMessageResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map StartThreadFromMessageResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map StartThreadFromMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return StartThreadFromMessageResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map StartThreadWithoutMessageResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map StartThreadWithoutMessageResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map StartThreadWithoutMessageResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map StartThreadWithoutMessageResponse.TooManyRequests (Http.toJson res)
                | status -> return StartThreadWithoutMessageResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map StartThreadInForumOrMediaChannelResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map StartThreadInForumOrMediaChannelResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map StartThreadInForumOrMediaChannelResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map StartThreadInForumOrMediaChannelResponse.TooManyRequests (Http.toJson res)
                | status -> return StartThreadInForumOrMediaChannelResponse.Other status
            })

    let joinThread
        (channelId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                put $"channels/{channelId}/thread-members/@me"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return JoinThreadResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map JoinThreadResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map JoinThreadResponse.TooManyRequests (Http.toJson res)
                | status -> return JoinThreadResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return AddThreadMemberResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map AddThreadMemberResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map AddThreadMemberResponse.TooManyRequests (Http.toJson res)
                | status -> return AddThreadMemberResponse.Other status
            })

    let leaveThread
        (channelId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"channels/{channelId}/thread-members/@me"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return LeaveThreadResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map LeaveThreadResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map LeaveThreadResponse.TooManyRequests (Http.toJson res)
                | status -> return LeaveThreadResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return RemoveThreadMemberResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map RemoveThreadMemberResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map RemoveThreadMemberResponse.TooManyRequests (Http.toJson res)
                | status -> return RemoveThreadMemberResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetThreadMemberResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetThreadMemberResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetThreadMemberResponse.TooManyRequests (Http.toJson res)
                | status -> return GetThreadMemberResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListThreadMembersResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ListThreadMembersResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListThreadMembersResponse.TooManyRequests (Http.toJson res)
                | status -> return ListThreadMembersResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListPublicArchivedThreadsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ListPublicArchivedThreadsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListPublicArchivedThreadsResponse.TooManyRequests (Http.toJson res)
                | status -> return ListPublicArchivedThreadsResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListPrivateArchivedThreadsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ListPrivateArchivedThreadsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListPrivateArchivedThreadsResponse.TooManyRequests (Http.toJson res)
                | status -> return ListPrivateArchivedThreadsResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListJoinedPrivateArchivedThreadsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ListJoinedPrivateArchivedThreadsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListJoinedPrivateArchivedThreadsResponse.TooManyRequests (Http.toJson res)
                | status -> return ListJoinedPrivateArchivedThreadsResponse.Other status
            })
