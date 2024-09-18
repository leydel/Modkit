namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
open System
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpGuildActions =
    // https://discord.com/developers/docs/resources/guild#create-guild
    abstract member CreateGuild:
        name: string ->
        icon: string option ->
        verificationLevel: GuildVerificationLevel option ->
        defaultMessageNotifications: GuildMessageNotificationLevel option ->
        explicitContentFilter: GuildExplicitContentFilterLevel option ->
        roles: Role list option ->
        channels: Channel list option ->
        afkChannelId: string option ->
        afkTimeout: int option ->
        systemChannelId: string option ->
        systemChannelFlags: int option ->
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
        name: string option ->
        verificationLevel: GuildVerificationLevel option option ->
        defaultMessageNotifications: GuildMessageNotificationLevel option option ->
        explicitContentFilter: GuildExplicitContentFilterLevel option option ->
        afkChannelId: string option option ->
        afkTimeout: int option ->
        icon: string option option ->
        ownerId: string option ->
        splash: string option option ->
        discoverySplash: string option option ->
        banner: string option option ->
        systemChannelId: string option option ->
        systemChannelFlags: int option ->
        rulesChannelId: string option option ->
        publicUpdatesChannelId: string option option ->
        preferredLocale: string option option ->
        features: GuildFeature list option ->
        description: string option option ->
        premiumProgressBarEnabled: bool option ->
        safetyAlertsChannelId: string option option ->
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
        name: string ->
        channelType: ChannelType option ->
        topic: string option ->
        bitrate: int option ->
        userLimit: int option ->
        rateLimitPerUser: int option ->
        position: int option ->
        permissionOverwrites: PermissionOverwrite list option ->
        parentId: string option ->
        nsfw: bool option ->
        rtcRegion: string option ->
        videoQualityMode: VideoQualityMode option ->
        defaultAutoArchiveDuration: int option ->
        defaultReactionEmoji: DefaultReaction option ->
        availableTags: ChannelTag option ->
        defaultSortOrder: ChannelSortOrder option ->
        defaultForumLayout: ChannelForumLayout option ->
        defaultThreadRateLimitPerUser: int option ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/guild#modify-guild-channel-positions
    abstract member ModifyGuildChannelPositions:
        guildId: string ->
        payload: ModifyGuildChannelPosition list ->
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
        accessToken: string ->
        nick: string option ->
        roles: string list option ->
        mute: bool option ->
        deaf: bool option ->
        Task<GuildMember>

    // https://discord.com/developers/docs/resources/guild#modify-guild-member
    abstract member ModifyGuildMember:
        guildId: string ->
        userId: string ->
        auditLogReason: string option ->
        nick: string option option ->
        roles: string list option option ->
        mute: bool option option ->
        deaf: bool option option ->
        channelId: string option option ->
        communicationDisabledUntil: DateTime option option ->
        flags: int option option ->
        Task<GuildMember>

    // https://discord.com/developers/docs/resources/guild#modify-current-member
    abstract member ModifyCurrentMember:
        guildId: string ->
        auditLogReason: string option ->
        nick: string option option ->
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
        deleteMessageDays: int option ->
        deleteMessageSeconds: int option ->
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
        userIds: string list ->
        deleteMessageSeconds: int option ->
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
        name: string option ->
        permissions: string option ->
        color: int option ->
        hoist: bool option ->
        icon: string option option ->
        unicodeEmoji: string option option ->
        mentionable: bool option ->
        Task<Role>

    // https://discord.com/developers/docs/resources/guild#modify-guild-role-positions
    abstract member ModifyGuildRolePositions:
        guildId: string ->
        auditLogReason: string option ->
        payload: ModifyGuildRolePosition list ->
        Task<Role list>

    // https://discord.com/developers/docs/resources/guild#modify-guild-role
    abstract member ModifyGuildRole:
        guildId: string ->
        roleId: string ->
        auditLogReason: string option ->
        name: string option option ->
        permissions: string option option ->
        color: int option option ->
        hoist: bool option option ->
        icon: string option option ->
        unicodeEmoji: string option option ->
        mentionable: bool option option ->
        Task<Role>

    // https://discord.com/developers/docs/resources/guild#modify-guild-mfa-level
    abstract member ModifyGuildMfaLevel:
        guildId: string ->
        auditLogReason: string option ->
        level: GuildMfaLevel ->
        Task<GuildMfaLevel> // TODO: Test if this correctly deserializes the enum

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
        days: int option ->
        computePruneCount: bool option ->
        includeRoles: string list option ->
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
        enabled: bool option ->
        channelId: string option option ->
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
        enabled: bool option option ->
        welcomeChannels: WelcomeScreenChannel list option option ->
        description: string option option ->
        Task<WelcomeScreen>

    // https://discord.com/developers/docs/resources/guild#get-guild-onboarding
    abstract member GetGuildOnboarding:
        guildId: string ->
        Task<GuildOnboarding>

    // https://discord.com/developers/docs/resources/guild#modify-guild-onboarding
    abstract member ModifyGuildOnboarding:
        guildId: string ->
        auditLogReason: string option ->
        prompts: GuildOnboardingPrompt list ->
        defaultChannelIds: string list ->
        enabled: bool ->
        mode: OnboardingMode ->
        Task<GuildOnboarding>

type DiscordHttpGuildActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpGuildActions with
        member _.CreateGuild
            name icon verificationLevel defaultMessageNotifications explicitContentFilter roles channels afkChannelId
            afkTimeout systemChannelId systemChannelFlags =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    "guilds"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.property   "name" name
                    |> Dto.propertyIf "icon" icon
                    |> Dto.propertyIf "verification_level" verificationLevel
                    |> Dto.propertyIf "default_message_notifications" defaultMessageNotifications
                    |> Dto.propertyIf "explicit_ccontent_filter" explicitContentFilter
                    |> Dto.propertyIf "roles" roles
                    |> Dto.propertyIf "channels" channels
                    |> Dto.propertyIf "afk_channel_id" afkChannelId
                    |> Dto.propertyIf "afk_timeout" afkTimeout
                    |> Dto.propertyIf "system_channel_id" systemChannelId
                    |> Dto.propertyIf "system_channel_flags" systemChannelFlags
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuild
            guildId withCounts =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}"
                |> Req.bot token
                |> Req.queryOpt "with_counts" (Option.map _.ToString() withCounts)
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildPreview
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/preview"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.ModifyGuild
            guildId auditLogReason name verificationLevel defaultMessageNotifications
            explicitContentFilter afkChannelId afkTimeout icon ownerId splash discoverySplash banner systemChannelId
            systemChannelFlags rulesChannelId publicUpdatesChannelId preferredLocale features description
            premiumProgressBarEnabled safetyAlertsChannelId =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "name" name
                    |> Dto.propertyIf "verification_level" verificationLevel
                    |> Dto.propertyIf "default_message_notifications" defaultMessageNotifications
                    |> Dto.propertyIf "explicit_content_filter" explicitContentFilter
                    |> Dto.propertyIf "afk_channel_id" afkChannelId
                    |> Dto.propertyIf "afk_timeout" afkTimeout
                    |> Dto.propertyIf "icon" icon
                    |> Dto.propertyIf "owner_id" ownerId
                    |> Dto.propertyIf "splash" splash
                    |> Dto.propertyIf "discovery_splash" discoverySplash
                    |> Dto.propertyIf "banner" banner
                    |> Dto.propertyIf "system_channel_id" systemChannelId
                    |> Dto.propertyIf "system_channel_flags" systemChannelFlags
                    |> Dto.propertyIf "rules_channel_id" rulesChannelId
                    |> Dto.propertyIf "public_updates_channel_id" publicUpdatesChannelId
                    |> Dto.propertyIf "preferred_locale" preferredLocale
                    |> Dto.propertyIf "features" features // TODO: Setup this to serialize using GuildFeatureConverter
                    |> Dto.propertyIf "description" description
                    |> Dto.propertyIf "premium_progress_bar_enabled" premiumProgressBarEnabled
                    |> Dto.propertyIf "safety_alerts_channel_id" safetyAlertsChannelId
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteGuild
            guildId =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.GetGuildChannels
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/channels"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.CreateGuildChannel
            guildId auditLogReason name channelType topic bitrate userLimit rateLimitPerUser position
            permissionOverwrites parentId nsfw rtcRegion videoQualityMode defaultAutoArchiveDuration
            defaultReactionEmoji availableTags defaultSortOrder defaultForumLayout defaultThreadRateLimitPerUser =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/channels"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property   "name" name
                    |> Dto.propertyIf "type" channelType
                    |> Dto.propertyIf "topic" topic
                    |> Dto.propertyIf "bitrate" bitrate
                    |> Dto.propertyIf "user_limit" userLimit
                    |> Dto.propertyIf "rate_limit_per_user" rateLimitPerUser 
                    |> Dto.propertyIf "position" position
                    |> Dto.propertyIf "permission_overwrites" permissionOverwrites
                    |> Dto.propertyIf "parent_id" parentId
                    |> Dto.propertyIf "nsfw" nsfw
                    |> Dto.propertyIf "rtc_region" rtcRegion
                    |> Dto.propertyIf "video_quality_mode" videoQualityMode
                    |> Dto.propertyIf "default_auto_archive_duration" defaultAutoArchiveDuration
                    |> Dto.propertyIf "default_reaction_emoji" defaultReactionEmoji
                    |> Dto.propertyIf "available_tags" availableTags
                    |> Dto.propertyIf "default_sort_order" defaultSortOrder
                    |> Dto.propertyIf "default_forum_layout" defaultForumLayout
                    |> Dto.propertyIf "default_thread_rate_limit_per_user" defaultThreadRateLimitPerUser
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.ModifyGuildChannelPositions
            guildId payload =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/channels"
                |> Req.bot token
                |> Req.json (FsJson.serialize payload)
                |> Req.send httpClientFactory
                |> Res.ignore                

        member _.ListActiveGuildThreads
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/threads/active"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json
                
        member _.GetGuildMember
            guildId userId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/members/{userId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.ListGuildMembers
            guildId limit after =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/members"
                |> Req.bot token
                |> Req.queryOpt "limit" (Option.map _.ToString() limit)
                |> Req.queryOpt "after" after
                |> Req.send httpClientFactory
                |> Res.json
                
        member _.SearchGuildMembers
            guildId query limit =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/members/search"
                |> Req.bot token
                |> Req.query "query" query
                |> Req.queryOpt "limit" (Option.map _.ToString() limit)
                |> Req.send httpClientFactory
                |> Res.json

        member _.AddGuildMember
            guildId userId accessToken nick roles mute deaf =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/members/{userId}"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.property   "access_token" accessToken
                    |> Dto.propertyIf "nick" nick
                    |> Dto.propertyIf "roles" roles
                    |> Dto.propertyIf "mute" mute
                    |> Dto.propertyIf "deaf" deaf
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.ModifyGuildMember
            guildId userId auditLogReason nick roles mute deaf channelId communicationDisabledUntil flags =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/members/{userId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "nick" nick
                    |> Dto.propertyIf "roles" roles
                    |> Dto.propertyIf "mute" mute
                    |> Dto.propertyIf "deaf" deaf
                    |> Dto.propertyIf "channel_id" channelId
                    |> Dto.propertyIf "communication_disabled_until" communicationDisabledUntil
                    |> Dto.propertyIf "flags" flags
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json
                
        member _.ModifyCurrentMember
            guildId auditLogReason nick =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/members/@me"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "nick" nick
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json
                
        member _.AddGuildMemberRole
            guildId userId roleId auditLogReason =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/members/{userId}/roles/{roleId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.RemoveGuildMemberRole
            guildId userId roleId auditLogReason =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/members/{userId}/roles/{roleId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore
                
        member _.RemoveGuildMember
            guildId userId auditLogReason =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/members/{userId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.GetGuildBans
            guildId limit before after =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/bans"
                |> Req.bot token
                |> Req.queryOpt "limit" (Option.map _.ToString() limit)
                |> Req.queryOpt "before" before
                |> Req.queryOpt "after" after
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildBan
            guildId userId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/bans/{userId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.CreateGuildBan
            guildId userId auditLogReason deleteMessageDays deleteMessageSeconds =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/bans/{userId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "delete_message_days" deleteMessageDays
                    |> Dto.propertyIf "delete_message_seconds" deleteMessageSeconds
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.RemoveGuildBan
            guildId userId auditLogReason =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/bans/{userId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.BulkGuildBan
            guildId auditLogReason userIds deleteMessageSeconds =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/bulk-ban"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property "user_ids" userIds
                    |> Dto.propertyIf "delete_message_seconds" deleteMessageSeconds
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildRoles
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/roles"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildRole
            guildId roleId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/roles/{roleId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.CreateGuildRole
            guildId auditLogReason name permissions color hoist icon unicodeEmoji mentionable =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/roles"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "name" name
                    |> Dto.propertyIf "permissions" permissions
                    |> Dto.propertyIf "color" color
                    |> Dto.propertyIf "hoist" hoist
                    |> Dto.propertyIf "icon" icon
                    |> Dto.propertyIf "unicode_emoji" unicodeEmoji
                    |> Dto.propertyIf "mentionable" mentionable
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.ModifyGuildRolePositions
            guildId auditLogReason payload =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/channels"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (FsJson.serialize payload)
                |> Req.send httpClientFactory
                |> Res.json   

        member _.ModifyGuildRole
            guildId roleId auditLogReason name permissions color hoist icon unicodeEmoji mentionable =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/roles/{roleId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "name" name
                    |> Dto.propertyIf "permissions" permissions
                    |> Dto.propertyIf "color" color
                    |> Dto.propertyIf "hoist" hoist
                    |> Dto.propertyIf "icon" icon
                    |> Dto.propertyIf "unicode_emoji" unicodeEmoji
                    |> Dto.propertyIf "mentionable" mentionable
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json
                
        member _.ModifyGuildMfaLevel
            guildId auditLogReason level =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/mfa"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property "level" (int level)
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteGuildRole
            guildId roleId auditLogReason =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/roles/{roleId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildPruneCount
            guildId days includeRoles =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/prune"
                |> Req.bot token
                |> Req.queryOpt "days" (Option.map _.ToString() days)
                |> Req.queryOpt "include_roles" (Option.map (String.concat ",") includeRoles)
                |> Req.send httpClientFactory
                |> Res.json

        member _.BeginGuildPrune
            guildId auditLogReason days computePruneCount includeRoles =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/prune"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.queryOpt "days" (Option.map _.ToString() days)
                |> Req.queryOpt "compute_prune_count" (Option.map _.ToString() computePruneCount)
                |> Req.queryOpt "include_roles" (Option.map (String.concat ",") includeRoles)
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildVoiceRegions
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/regions"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json
                
        member _.GetGuildInvites
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/invites"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildIntegrations
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/integrations"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteGuildIntegration
            guildId integrationId auditLogReason =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/integrations/{integrationId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.GetGuildWidgetSettings
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/widget"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json
                
        member _.ModifyGuildWidget
            guildId auditLogReason enabled channelId =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/widget"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "enabled" enabled
                    |> Dto.propertyIf "channel_id" channelId
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildWidget
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/widget.json"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildVanityUrl
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/vanity-url"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json
                
        member _.GetGuildWidgetImage
            guildId style =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/widget.png"
                |> Req.bot token
                |> Req.queryOpt "style" (Option.map _.ToString() style)
                |> Req.send httpClientFactory
                |> Res.raw
                
        member _.GetGuildWelcomeScreen
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/welcome-screen"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.ModifyGuildWelcomeScreen
            guildId auditLogReason enabled welcomeChannels description =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/welcome-screen"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "enabled" enabled
                    |> Dto.propertyIf "welcome_channels" welcomeChannels
                    |> Dto.propertyIf "description" description
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json
                
        member _.GetGuildOnboarding
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/onboarding"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json
        
        member _.ModifyGuildOnboarding
            guildId auditLogReason prompts defaultChannelIds enabled mode =
                Req.create
                    HttpMethod.Put
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/onboarding"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property "prompts" prompts
                    |> Dto.property "default_channel_ids" defaultChannelIds
                    |> Dto.property "enabled" enabled
                    |> Dto.property "mode" (int mode)
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json
