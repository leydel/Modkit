namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System
open System.Net
open System.Net.Http
open System.Text.Json.Serialization
open System.Threading.Tasks

type CreateGuildPayload(
    name:                           string,
    ?icon:                          string,
    ?verification_level:            GuildVerificationLevel,
    ?default_message_notifications: GuildMessageNotificationLevel,
    ?explicit_content_filter:       GuildExplicitContentFilterLevel,
    ?roles:                         Role list,
    ?channels:                      PartialChannel list,
    ?afk_channel_id:                string,
    ?afk_timeout:                   int,
    ?system_channel_id:             string,
    ?system_channel_flags:          int
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "icon" icon
            optional "verification_level" verification_level
            optional "default_message_notifications" default_message_notifications
            optional "explicit_content_filter" explicit_content_filter
            optional "roles" roles
            optional "channels" channels
            optional "afk_channel_id" afk_channel_id
            optional "afk_timeout" afk_timeout
            optional "system_channel_id" system_channel_id
            optional "system_channel_flags" system_channel_flags
        }

type CreateGuildResponse =
    | Created of Guild
    | BadRequest of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildResponse =
    | Ok of Guild
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildPreviewResponse =
    | Ok of GuildPreview
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildPayload(
    ?name:                          string,
    ?verification_level:            GuildVerificationLevel option,
    ?default_message_notifications: GuildMessageNotificationLevel option,
    ?explicit_content_filter:       GuildExplicitContentFilterLevel option,
    ?afk_channel_id:                string option,
    ?afk_timeout:                   int,
    ?icon:                          string option,
    ?owner_id:                      string,
    ?splash:                        string option,
    ?discovery_splash:              string option,
    ?banner:                        string option,
    ?system_channel_id:             string option,
    ?system_channel_flags:          int,
    ?rules_channel_id:              string option,
    ?public_updates_channel_id:     string option,
    ?preferred_locale:              string option,
    ?features:                      GuildFeature list,
    ?description:                   string option,
    ?premium_progress_bar_enabled:  bool,
    ?safety_alerts_channel_id:      string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "verification_level" verification_level
            optional "default_message_notifications" default_message_notifications
            optional "explicit_content_filter" explicit_content_filter
            optional "afk_channel_id" afk_channel_id
            optional "afk_timeout" afk_timeout
            optional "icon" icon
            optional "owner_id" owner_id
            optional "splash" splash
            optional "discovery_splash" discovery_splash
            optional "banner" banner
            optional "system_channel_id" system_channel_id
            optional "system_channel_flags" system_channel_flags
            optional "rules_channel_id" rules_channel_id
            optional "public_updates_channel_id" public_updates_channel_id
            optional "preferred_locale" preferred_locale
            optional "features" (features >>. List.map _.ToString())
            optional "description" description
            optional "premium_progress_bar_enabled" premium_progress_bar_enabled
            optional "safety_alerts_channel_id" safety_alerts_channel_id
        }

type ModifyGuildResponse =
    | Ok of Guild
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteGuildResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildChannelsResponse =
    | Ok of Channel list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateGuildChannelPayload(
    name:                                string,
    ?``type``:                           ChannelType option,
    ?topic:                              string option,
    ?bitrate:                            int option,
    ?user_limit:                         int option,
    ?rate_limit_per_user:                int option,
    ?position:                           int option,
    ?permission_overwrites:              PartialPermissionOverwrite list option,
    ?parent_id:                          string option,
    ?nsfw:                               bool option,
    ?rtc_region:                         string option,
    ?video_quality_mode:                 VideoQualityMode option,
    ?default_auto_archive_duration:      int option,
    ?default_reaction_emoji:             DefaultReaction option,
    ?available_tags:                     ChannelTag list option,
    ?default_sort_order:                 ChannelSortOrder option,
    ?default_forum_layout:               ChannelForumLayout option,
    ?default_thread_rate_limit_per_user: int option
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "type" ``type``
            optional "topic" topic
            optional "bitrate" bitrate
            optional "user_limit" user_limit
            optional "rate_limit_per_user" rate_limit_per_user
            optional "position" position
            optional "permission_overwrites" permission_overwrites
            optional "parent_id" parent_id
            optional "nsfw" nsfw
            optional "rtc_region" rtc_region
            optional "video_quality_mode" video_quality_mode
            optional "default_auto_archive_duration" default_auto_archive_duration
            optional "default_reaction_emoji" default_reaction_emoji
            optional "available_tags" available_tags
            optional "default_sort_order" default_sort_order
            optional "default_forum_layout" default_forum_layout
            optional "default_thread_rate_limit_per_user" default_thread_rate_limit_per_user
        }

type CreateGuildChannelResponse =
    | Created of Channel
    | BadRequest of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildChannelPosition = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "lock_permissions">] LockPermissions: bool option
    [<JsonPropertyName "parent_id">] ParentId: string option
}

type ModifyGuildChannelPositionsPayload(
    positions: ModifyGuildChannelPosition list
) =
    inherit Payload() with
        override _.Content =
            JsonListPayload positions

type ModifyGuildChannelPositionsResponse =
    | NoContent
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ListActiveGuildThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: GuildMember list
}

type ListActiveGuildThreadsResponse =
    | Ok of ListActiveGuildThreadsOkResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildMemberResponse =
    | Ok of GuildMember
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ListGuildMembersResponse =
    | Ok of GuildMember list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type SearchGuildMembersResponse =
    | Ok of GuildMember list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type AddGuildMemberPayload(
    access_token: string,
    ?nick:        string,
    ?roles:       string list,
    ?mute:        bool,
    ?deaf:        bool
) =
    inherit Payload() with
        override _.Content = json {
            required "access_token" access_token
            optional "nick" nick
            optional "roles" roles
            optional "mute" mute
            optional "deaf" deaf
        }

type AddGuildMemberResponse =
    | Created of GuildMember
    | NoContent
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
type ModifyGuildMemberPayload(
    ?nick:                         string option,
    ?roles:                        string list option,
    ?mute:                         bool option,
    ?deaf:                         bool option,
    ?channel_id:                   string option,
    ?communication_disabled_until: DateTime option,
    ?flags:                        int option
) =
    inherit Payload() with
        override _.Content = json {
            optional "nick" nick
            optional "roles" roles
            optional "mute" mute
            optional "deaf" deaf
            optional "channel_id" channel_id
            optional "communication_disabled_until" communication_disabled_until
            optional "flags" flags
        }

type ModifyGuildMemberResponse =
    | Ok of GuildMember
    | NoContent
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyCurrentMemberPayload(
    ?nick: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "nick" nick
        }

type ModifyCurrentMemberResponse =
    | Ok of GuildMember
    | NoContent
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type AddGuildMemberRoleResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type RemoveGuildMemberRoleResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type RemoveGuildMemberResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildBansResponse =
    | Ok of GuildBan list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildBanResponse =
    | Ok of GuildBan
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateGuildBanPayload(
    ?delete_message_days:    int,
    ?delete_message_seconds: int
) =
    inherit Payload() with
        override _.Content = json {
            optional "delete_message_days" delete_message_days
            optional "delete_message_seconds" delete_message_seconds
        }

type CreateGuildBanResponse =
    | NoContent
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type RemoveGuildBanResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type BulkGuildBanPayload(
    user_ids:                string list,
    ?delete_message_seconds: int
) =
    inherit Payload() with
        override _.Content = json {
            required "user_ids" user_ids
            optional "delete_message_seconds" delete_message_seconds
        }

type BulkGuildBanOkResponse = {
    [<JsonPropertyName "banned_users">] BannedUsers: string list
    [<JsonPropertyName "failed_users">] FailedUsers: string list
}

type BulkGuildBanResponse =
    | Ok of BulkGuildBanOkResponse
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildRolesResponse =
    | Ok of Role list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildRoleResponse =
    | Ok of Role
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
type CreateGuildRolePayload(
    ?name:          string,
    ?permissions:   string,
    ?color:         int,
    ?hoist:         bool,
    ?icon:          string option,
    ?unicode_emoji: string option,
    ?mentionable:   bool
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "permissions" permissions
            optional "color" color
            optional "hoist" hoist
            optional "icon" icon
            optional "unicode_emoji" unicode_emoji
            optional "mentionable" mentionable
        }

type CreateGuildRoleResponse =
    | Ok of Role
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildRolePosition = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "position">] Position: int option
}

type ModifyGuildRolePositionsPayload(
    positions: ModifyGuildRolePosition list
) =
    inherit Payload() with
        override _.Content =
            JsonListPayload positions

type ModifyGuildRolePositionsResposne =
    | Ok of Role list
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildRolePayload(
    ?name:          string option,
    ?permissions:   string option,
    ?color:         int option,
    ?hoist:         bool option,
    ?icon:          string option,
    ?unicode_emoji: string option,
    ?mentionable:   bool option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "permissions" permissions
            optional "color" color
            optional "hoist" hoist
            optional "icon" icon
            optional "unicode_emoji" unicode_emoji
            optional "mentionable" mentionable
        }

type ModifyGuildRoleResponse =
    | Ok of Role
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildMfaLevelPayload(
    level: GuildMfaLevel
) =
    inherit Payload() with
        override _.Content = json {
            required "level" level
        }

type ModifyGuildMfaLevelResponse =
    | Ok of GuildMfaLevel
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteGuildRoleResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildPruneCountOkResponse = {
    [<JsonPropertyName "pruned">] Pruned: int
}

type GetGuildPruneCountResponse =
    | Ok of GetGuildPruneCountOkResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type BeginGuildPrunePayload(
    ?days: int,
    ?compute_prune_count: bool,
    ?include_roles: string list,
    ?reason: string
) =
    inherit Payload() with
        override _.Content = json {
            optional "days" days
            optional "compute_prune_count" compute_prune_count
            optional "include_roles" include_roles
            optional "reason" reason
        }

type BeginGuildPruneOkResponse = {
    [<JsonPropertyName "pruned">] Pruned: int option
}

type BeginGuildPruneResponse =
    | Ok of BeginGuildPruneOkResponse
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildVoiceRegionsResponse =
    | Ok of VoiceRegion list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildInvitesResponse =
    | Ok of InviteWithMetadata list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildIntegrationsResponse =
    | Ok of GuildIntegration list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteGuildIntegrationResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildWidgetSettingsResponse =
    | Ok of GuildWidgetSettings
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildWidgetPayload(
    ?enabled:    bool,
    ?channel_id: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "enabled" enabled
            optional "channel_id" channel_id
        }

type ModifyGuildWidgetResponse =
    | Ok of GuildWidgetSettings
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildWidgetResponse =
    | Ok of GuildWidget
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildVanityUrlOkResponse = {
    [<JsonPropertyName "code">] Code: string option
    [<JsonPropertyName "uses">] Uses: int
}

type GetGuildVanityUrlResponse =
    | Ok of GetGuildVanityUrlOkResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildWidgetImageResponse =
    | Ok of string
    | NotFound of ErrorResponse
    | Other of HttpStatusCode

type GetGuildWelcomeScreenResponse =
    | Ok of WelcomeScreen
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildWelcomeScreenPayload(
    ?enabled:          bool option,
    ?welcome_channels: WelcomeScreenChannel list option,
    ?description:      string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "enabled" enabled
            optional "welcome_channels" welcome_channels
            optional "description" description
        }

type ModifyGuildWelcomeScreenResponse =
    | Ok of WelcomeScreen
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildOnboardingResponse =
    | Ok of GuildOnboarding
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildOnboardingPayload(
    prompts:             GuildOnboardingPrompt list,
    default_channel_ids: string list,
    enabled:             bool,
    mode:                OnboardingMode
) =
    inherit Payload() with
        override _.Content = json {
            required "prompts" prompts
            required "default_channel_ids" default_channel_ids
            required "enabled" enabled
            required "mode" mode
        }

type ModifyGuildOnboardingResponse =
    | Ok of GuildOnboarding
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Guild =
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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map CreateGuildResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateGuildResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGuildResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGuildResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildResponse.Other status
            })

    let getGuildPreview
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/preview"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildPreviewResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildPreviewResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildPreviewResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildPreviewResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map ModifyGuildResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildResponse.Other status
            })

    let deleteGuild
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"guilds/{guildId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return DeleteGuildResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteGuildResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteGuildResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteGuildResponse.Other status
            })

    let getGuildChannels
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/channels"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildChannelsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildChannelsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildChannelsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildChannelsResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map CreateGuildChannelResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateGuildChannelResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGuildChannelResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGuildChannelResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return ModifyGuildChannelPositionsResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildChannelPositionsResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildChannelPositionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildChannelPositionsResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildChannelPositionsResponse.Other status
            })

    let listActiveGuildThreads
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/threads/active"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListActiveGuildThreadsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ListActiveGuildThreadsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListActiveGuildThreadsResponse.TooManyRequests (Http.toJson res)
                | status -> return ListActiveGuildThreadsResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildMemberResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildMemberResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildMemberResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildMemberResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListGuildMembersResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ListGuildMembersResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListGuildMembersResponse.TooManyRequests (Http.toJson res)
                | status -> return ListGuildMembersResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map SearchGuildMembersResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map SearchGuildMembersResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map SearchGuildMembersResponse.TooManyRequests (Http.toJson res)
                | status -> return SearchGuildMembersResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map AddGuildMemberResponse.Created (Http.toJson res)
                | HttpStatusCode.NoContent -> return AddGuildMemberResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map AddGuildMemberResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map AddGuildMemberResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map AddGuildMemberResponse.TooManyRequests (Http.toJson res)
                | status -> return AddGuildMemberResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyGuildMemberResponse.Ok (Http.toJson res)
                | HttpStatusCode.NoContent -> return ModifyGuildMemberResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildMemberResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildMemberResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildMemberResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildMemberResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyCurrentMemberResponse.Ok (Http.toJson res)
                | HttpStatusCode.NoContent -> return ModifyCurrentMemberResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map ModifyCurrentMemberResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyCurrentMemberResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyCurrentMemberResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyCurrentMemberResponse.Other status
            })
        
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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return AddGuildMemberRoleResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map AddGuildMemberRoleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map AddGuildMemberRoleResponse.TooManyRequests (Http.toJson res)
                | status -> return AddGuildMemberRoleResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return RemoveGuildMemberRoleResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map RemoveGuildMemberRoleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map RemoveGuildMemberRoleResponse.TooManyRequests (Http.toJson res)
                | status -> return RemoveGuildMemberRoleResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return RemoveGuildMemberResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map RemoveGuildMemberResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map RemoveGuildMemberResponse.TooManyRequests (Http.toJson res)
                | status -> return RemoveGuildMemberResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildBansResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildBansResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildBansResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildBansResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildBanResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildBanResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildBanResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildBanResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return CreateGuildBanResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map CreateGuildBanResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map CreateGuildBanResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGuildBanResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGuildBanResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return RemoveGuildBanResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map RemoveGuildBanResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map RemoveGuildBanResponse.TooManyRequests (Http.toJson res)
                | status -> return RemoveGuildBanResponse.Other status
            })

    let buildGuildBan
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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map BulkGuildBanResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map BulkGuildBanResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map BulkGuildBanResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map BulkGuildBanResponse.TooManyRequests (Http.toJson res)
                | status -> return BulkGuildBanResponse.Other status
            })

    let getGuildRoles
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/roles"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildRolesResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildRolesResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildRolesResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildRolesResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildRoleResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildRoleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildRoleResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildRoleResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map CreateGuildRoleResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateGuildRoleResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map CreateGuildRoleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGuildRoleResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGuildRoleResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyGuildRolePositionsResposne.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildRolePositionsResposne.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildRolePositionsResposne.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildRolePositionsResposne.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildRolePositionsResposne.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyGuildRoleResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildRoleResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildRoleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildRoleResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildRoleResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyGuildMfaLevelResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildMfaLevelResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildMfaLevelResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildMfaLevelResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildMfaLevelResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteGuildRoleResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteGuildRoleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteGuildRoleResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteGuildRoleResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildPruneCountResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildPruneCountResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildPruneCountResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildPruneCountResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map BeginGuildPruneResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map BeginGuildPruneResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map BeginGuildPruneResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map BeginGuildPruneResponse.TooManyRequests (Http.toJson res)
                | status -> return BeginGuildPruneResponse.Other status
            })

    let getGuildVoiceRegions
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/regions"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildVoiceRegionsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildVoiceRegionsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildVoiceRegionsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildVoiceRegionsResponse.Other status
            })

    let getGuildInvites
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/invites"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildInvitesResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildInvitesResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildInvitesResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildInvitesResponse.Other status
            })

    let getGuildIntegrations
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/integrations"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildIntegrationsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildIntegrationsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildIntegrationsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildIntegrationsResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteGuildIntegrationResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteGuildIntegrationResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteGuildIntegrationResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteGuildIntegrationResponse.Other status
            })

    let getGuildWidgetSettings
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/widget"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildWidgetSettingsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildWidgetSettingsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildWidgetSettingsResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildWidgetSettingsResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyGuildWidgetResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildWidgetResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildWidgetResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildWidgetResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildWidgetResponse.Other status
            })

    let getGuildWidget
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/widget.json"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildWidgetResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildWidgetResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildWidgetResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildWidgetResponse.Other status
            })

    let getGuildVanityUrl
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/vanity-url"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildVanityUrlResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildVanityUrlResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildVanityUrlResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildVanityUrlResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildWidgetImageResponse.Ok (Http.toRaw res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildWidgetImageResponse.NotFound (Http.toJson res)
                | status -> return GetGuildWidgetImageResponse.Other status
            })

    let getGuildWelcomeScreen
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/welcome-screen"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildWelcomeScreenResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildWelcomeScreenResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildWelcomeScreenResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildWelcomeScreenResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyGuildWelcomeScreenResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildWelcomeScreenResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildWelcomeScreenResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildWelcomeScreenResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildWelcomeScreenResponse.Other status
            })

    let getGuildOnboarding
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/onboarding"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildOnboardingResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildOnboardingResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildOnboardingResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildOnboardingResponse.Other status
            })

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
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyGuildOnboardingResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildOnboardingResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildOnboardingResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildOnboardingResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildOnboardingResponse.Other status
            })
