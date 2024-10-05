namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System
open System.Threading.Tasks

type CreateGuild(
    name:                           string,
    ?icon:                          string,
    ?verification_level:            GuildVerificationLevel,
    ?default_message_notifications: GuildMessageNotificationLevel,
    ?explicit_content_filter:       GuildExplicitContentFilterLevel,
    ?roles:                         Role list,
    ?channels:                      Channel list,
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

type ModifyGuild(
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
    ?features:                      GuildFeature list, // TODO: Setup this to serialize using GuildFeatureConverter
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
            optional "features" features
            optional "description" description
            optional "premium_progress_bar_enabled" premium_progress_bar_enabled
            optional "safety_alerts_channel_id" safety_alerts_channel_id
        }

type CreateGuildChannel(
    name:                                string,
    ?``type``:                           ChannelType option,
    ?topic:                              string option,
    ?bitrate:                            int option,
    ?user_limit:                         int option,
    ?rate_limit_per_user:                int option,
    ?position:                           int option,
    ?permission_overwrites:              PermissionOverwrite list option,
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

type ModifyGuildChannelPositions(
    positions: ModifyGuildChannelPosition list
) =
    inherit Payload() with
        override _.Content =
            JsonListPayload positions

type AddGuildMember(
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

type ModifyGuildMember(
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

type ModifyCurrentMember(
    ?nick: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "nick" nick
        }

type CreateGuildBan(
    ?delete_message_days:    int,
    ?delete_message_seconds: int
) =
    inherit Payload() with
        override _.Content = json {
            optional "delete_message_days" delete_message_days
            optional "delete_message_seconds" delete_message_seconds
        }

type BulkGuildBan(
    user_ids:                string list,
    ?delete_message_seconds: int
) =
    inherit Payload() with
        override _.Content = json {
            required "user_ids" user_ids
            optional "delete_message_seconds" delete_message_seconds
        }

type CreateGuildRole(
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

type ModifyGuildRolePositions(
    positions: ModifyGuildRolePosition list
) =
    inherit Payload() with
        override _.Content =
            JsonListPayload positions

type ModifyGuildRole(
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

type ModifyGuildMfaLevel(
    level: GuildMfaLevel
) =
    inherit Payload() with
        override _.Content = json {
            required "level" level
        }

type BeginGuildPrune(
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

type ModifyGuildWidget(
    ?enabled:    bool,
    ?channel_id: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "enabled" enabled
            optional "channel_id" channel_id
        }

type ModifyGuildWelcomeScreen(
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

type ModifyGuildOnboarding(
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

type IGuildResource =
    // https://discord.com/developers/docs/resources/guild#create-guild
    abstract member CreateGuild:
        content: CreateGuild ->
        Task<Guild>

    // https://discord.com/developers/docs/resources/guild#get-guild
    abstract member GetGuild:
        guildId: string ->
        withCounts: bool option ->
        Task<Guild>

    // https://discord.com/developers/docs/resources/guild#get-guild-preview
    abstract member GetGuildPreview:
        guildId: string ->
        Task<GuildPreview>

    // https://discord.com/developers/docs/resources/guild#modify-guild
    abstract member ModifyGuild:
        guildId: string ->
        auditLogReason: string option ->
        content: ModifyGuild ->
        Task<Guild>

    // https://discord.com/developers/docs/resources/guild#delete-guild
    abstract member DeleteGuild:
        guildId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/guild#get-guild-channels
    abstract member GetGuildChannels:
        guildId: string ->
        Task<Channel list>

    // https://discord.com/developers/docs/resources/guild#create-guild-channel
    abstract member CreateGuildChannel:
        guildId: string ->
        auditLogReason: string option ->
        content: CreateGuildChannel ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/guild#modify-guild-channel-positions
    abstract member ModifyGuildChannelPositions:
        guildId: string ->
        content: ModifyGuildChannelPositions ->
        Task<unit>

    // https://discord.com/developers/docs/resources/guild#list-active-guild-threads
    abstract member ListActiveGuildThreads:
        guildId: string ->
        Task<ListActiveGuildThreadsResponse>

    // https://discord.com/developers/docs/resources/guild#get-guild-member
    abstract member GetGuildMember:
        guildId: string ->
        userId: string ->
        Task<GuildMember>

    // https://discord.com/developers/docs/resources/guild#list-guild-members
    abstract member ListGuildMembers:
        guildId: string ->
        limit: int option ->
        after: string option ->
        Task<GuildMember list>

    // https://discord.com/developers/docs/resources/guild#search-guild-members
    abstract member SearchGuildMembers:
        guildId: string ->
        query: string ->
        limit: int option ->
        Task<GuildMember list>

    // https://discord.com/developers/docs/resources/guild#add-guild-member
    abstract member AddGuildMember:
        guildId: string ->
        userId: string ->
        content: AddGuildMember ->
        Task<GuildMember>

    // https://discord.com/developers/docs/resources/guild#modify-guild-member
    abstract member ModifyGuildMember:
        guildId: string ->
        userId: string ->
        auditLogReason: string option ->
        content: ModifyGuildMember ->
        Task<GuildMember>

    // https://discord.com/developers/docs/resources/guild#modify-current-member
    abstract member ModifyCurrentMember:
        guildId: string ->
        auditLogReason: string option ->
        content: ModifyCurrentMember ->
        Task<GuildMember>

    // https://discord.com/developers/docs/resources/guild#add-guild-member-role
    abstract member AddGuildMemberRole:
        guildId: string ->
        userId: string ->
        roleId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/guild#remove-guild-member-role
    abstract member RemoveGuildMemberRole:
        guildId: string ->
        userId: string ->
        roleId: string ->
        auditLogReason: string option ->
        Task<unit>
        
    // https://discord.com/developers/docs/resources/guild#remove-guild-member
    abstract member RemoveGuildMember:
        guildId: string ->
        userId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/guild#get-guild-bans
    abstract member GetGuildBans:
        guildId: string ->
        limit: int option ->
        before: string option ->
        after: string option ->
        Task<GuildBan list>

    // https://discord.com/developers/docs/resources/guild#get-guild-ban
    abstract member GetGuildBan:
        guildId: string ->
        userId: string ->
        Task<GuildBan>

    // https://discord.com/developers/docs/resources/guild#create-guild-ban
    abstract member CreateGuildBan:
        guildId: string ->
        userId: string ->
        auditLogReason: string option ->
        content: CreateGuildBan ->
        Task<unit>

    // https://discord.com/developers/docs/resources/guild#remove-guild-ban
    abstract member RemoveGuildBan:
        guildId: string ->
        userId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/guild#bulk-guild-ban
    abstract member BulkGuildBan:
        guildId: string ->
        auditLogReason: string option ->
        content: BulkGuildBan ->
        Task<BulkGuildBanResponse>

    // https://discord.com/developers/docs/resources/guild#get-guild-roles
    abstract member GetGuildRoles:
        guildId: string ->
        Task<Role list>

    // https://discord.com/developers/docs/resources/guild#get-guild-role
    abstract member GetGuildRole:
        guildId: string ->
        roleId: string ->
        Task<Role>

    // https://discord.com/developers/docs/resources/guild#create-guild-role
    abstract member CreateGuildRole:
        guildId: string ->
        auditLogReason: string option ->
        content: CreateGuildRole ->
        Task<Role>

    // https://discord.com/developers/docs/resources/guild#modify-guild-role-positions
    abstract member ModifyGuildRolePositions:
        guildId: string ->
        auditLogReason: string option ->
        content: ModifyGuildRolePositions ->
        Task<Role list>

    // https://discord.com/developers/docs/resources/guild#modify-guild-role
    abstract member ModifyGuildRole:
        guildId: string ->
        roleId: string ->
        auditLogReason: string option ->
        content: ModifyGuildRole ->
        Task<Role>

    // https://discord.com/developers/docs/resources/guild#modify-guild-mfa-level
    abstract member ModifyGuildMfaLevel:
        guildId: string ->
        auditLogReason: string option ->
        content: ModifyGuildMfaLevel ->
        Task<GuildMfaLevel>

    // https://discord.com/developers/docs/resources/guild#delete-guild-role
    abstract member DeleteGuildRole:
        guildId: string ->
        roleId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/guild#get-guild-prune-count
    abstract member GetGuildPruneCount:
        guildId: string ->
        days: int option ->
        includeRoles: string list option ->
        Task<GetGuildPruneCountResponse>

    // https://discord.com/developers/docs/resources/guild#begin-guild-prune
    abstract member BeginGuildPrune:
        guildId: string ->
        auditLogReason: string option ->
        content: BeginGuildPrune ->
        Task<BeginGuildPruneResponse>

    // https://discord.com/developers/docs/resources/guild#get-guild-voice-regions
    abstract member GetGuildVoiceRegions:
        guildId: string ->
        Task<VoiceRegion list>

    // https://discord.com/developers/docs/resources/guild#get-guild-invites
    abstract member GetGuildInvites:
        guildId: string ->
        Task<InviteWithMetadata list>

    // https://discord.com/developers/docs/resources/guild#get-guild-integrations
    abstract member GetGuildIntegrations:
        guildId: string ->
        Task<GuildIntegration list>

    // https://discord.com/developers/docs/resources/guild#delete-guild-integration
    abstract member DeleteGuildIntegration:
        guildId: string ->
        integrationId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/guild#get-guild-widget-settings
    abstract member GetGuildWidgetSettings:
        guildId: string ->
        Task<GuildWidgetSettings>

    // https://discord.com/developers/docs/resources/guild#modify-guild-widget
    abstract member ModifyGuildWidget:
        guildId: string ->
        auditLogReason: string option ->
        content: ModifyGuildWidget ->
        Task<GuildWidgetSettings>

    // https://discord.com/developers/docs/resources/guild#get-guild-widget
    abstract member GetGuildWidget:
        guildId: string ->
        Task<GuildWidget>

    // https://discord.com/developers/docs/resources/guild#get-guild-vanity-url
    abstract member GetGuildVanityUrl:
        guildId: string ->
        Task<GetGuildVanityUrlResponse>

    // https://discord.com/developers/docs/resources/guild#get-guild-widget-image
    abstract member GetGuildWidgetImage:
        guildId: string ->
        style: GuildWidgetStyle option ->
        Task<string>

    // https://discord.com/developers/docs/resources/guild#get-guild-welcome-screen
    abstract member GetGuildWelcomeScreen:
        guildId: string ->
        Task<WelcomeScreen>

    // https://discord.com/developers/docs/resources/guild#modify-guild-welcome-screen
    abstract member ModifyGuildWelcomeScreen:
        guildId: string ->
        auditLogReason: string option ->
        content: ModifyGuildWelcomeScreen ->
        Task<WelcomeScreen>

    // https://discord.com/developers/docs/resources/guild#get-guild-onboarding
    abstract member GetGuildOnboarding:
        guildId: string ->
        Task<GuildOnboarding>

    // https://discord.com/developers/docs/resources/guild#modify-guild-onboarding
    abstract member ModifyGuildOnboarding:
        guildId: string ->
        auditLogReason: string option ->
        content: ModifyGuildOnboarding ->
        Task<GuildOnboarding>

type GuildResource (httpClientFactory, token) =
    interface IGuildResource with
        member _.CreateGuild content =
            req {
                post "guilds"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuild guildId withCounts =
            req {
                get $"guilds/{guildId}"
                bot token
                query "with_counts" (withCounts >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildPreview guildId =
            req {
                get $"guilds/{guildId}/preview"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuild guildId auditLogReason content =
            req {
                patch $"guilds/{guildId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteGuild guildId =
            req {
                delete $"guilds/{guildId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildChannels guildId =
            req {
                get $"guilds/{guildId}/channels"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateGuildChannel guildId auditLogReason content =
            req {
                post $"guilds/{guildId}/channels"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildChannelPositions guildId content =
            req {
                patch $"guilds/{guildId}/channels"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.wait
            
        member _.ListActiveGuildThreads guildId =
            req {
                get $"guilds/{guildId}/threads/active"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildMember guildId userId =
            req {
                get $"guilds/{guildId}/members/{userId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ListGuildMembers guildId limit after =
            req {
                get $"guilds/{guildId}/members"
                bot token
                query "limit" (limit >>. _.ToString())
                query "after" (after >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.SearchGuildMembers guildId queryProp limit =
            req {
                get $"guilds/{guildId}/members/search"
                bot token
                query "query" queryProp
                query "limit" (limit >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
                
        member _.AddGuildMember guildId userId content =
            req {
                put $"guilds/{guildId}/members/{userId}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildMember guildId userId auditLogReason content =
            req {
                patch $"guilds/{guildId}/members/{userId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyCurrentMember guildId auditLogReason content =
            req {
                patch $"guilds/{guildId}/members/@me"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.AddGuildMemberRole guildId userId roleId auditLogReason =
            req {
                put $"guilds/{guildId}/members/{userId}/roles/{roleId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.RemoveGuildMemberRole guildId userId roleId auditLogReason =
            req {
                delete $"guilds/{guildId}/members/{userId}/roles/{roleId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait 

        member _.RemoveGuildMember guildId userId auditLogReason =
            req {
                delete $"guilds/{guildId}/members/{userId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.GetGuildBans guildId limit before after =
            req {
                get $"guilds/{guildId}/bans"
                bot token
                query "limit" (limit >>. _.ToString())
                query "before" before
                query "after" after
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildBan guildId userId =
            req {
                get $"guilds/{guildId}/bans/{userId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateGuildBan guildId userId auditLogReason content =
            req {
                put $"guilds/{guildId}/bans/{userId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.RemoveGuildBan guildId userId auditLogReason =
            req {
                delete $"guilds/{guildId}/bans/{userId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.BulkGuildBan guildId auditLogReason content =
            req {
                post $"guilds/{guildId}/bulk-ban"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildRoles guildId =
            req {
                get $"guilds/{guildId}/roles"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildRole guildId roleId =
            req {
                get $"guilds/{guildId}/roles/{roleId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateGuildRole guildId auditLogReason content =
            req {
                post $"guilds/{guildId}/roles"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildRolePositions guildId auditLogReason content =
            req {
                patch $"guilds/{guildId}/channels"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
            
        member _.ModifyGuildRole guildId roleId auditLogReason content =
            req {
                patch $"guilds/{guildId}/roles/{roleId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildMfaLevel guildId auditLogReason content =
            req {
                post $"guilds/{guildId}/mfa"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteGuildRole guildId roleId auditLogReason =
            req {
                delete $"guilds/{guildId}/roles/{roleId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildPruneCount guildId days includeRoles =
            req {
                get $"guilds/{guildId}/prune"
                bot token
                query "days" (days >>. _.ToString())
                query "include_roles" (includeRoles >>. String.concat ",")
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.BeginGuildPrune guildId auditLogReason content =
            req {
                post $"guilds/{guildId}/prune"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildVoiceRegions guildId =
            req {
                get $"guilds/{guildId}/regions"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildInvites guildId =
            req {
                get $"guilds/{guildId}/invites"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildIntegrations guildId =
            req {
                get $"guilds/{guildId}/integrations"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteGuildIntegration guildId integrationId auditLogReason =
            req {
                delete $"guilds/{guildId}/integrations/{integrationId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.GetGuildWidgetSettings guildId =
            req {
                get $"guilds/{guildId}/widget"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildWidget guildId auditLogReason content =
            req {
                patch $"guilds/{guildId}/widget"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildWidget guildId =
            req {
                get $"guilds/{guildId}/widget.json"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildVanityUrl guildId =
            req {
                get $"guilds/{guildId}/vanity-url"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildWidgetImage guildId style =
            req {
                get $"guilds/{guildId}/widget.png"
                bot token
                query "style" (style >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toRaw

        member _.GetGuildWelcomeScreen guildId =
            req {
                get $"guilds/{guildId}/welcome-screen"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildWelcomeScreen guildId auditLogReason content =
            req {
                patch $"guilds/{guildId}/welcome-screen"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildOnboarding guildId =
            req {
                get $"guilds/{guildId}/onboarding"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildOnboarding guildId auditLogReason content =
            req {
                put $"guilds/{guildId}/onboarding"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
        