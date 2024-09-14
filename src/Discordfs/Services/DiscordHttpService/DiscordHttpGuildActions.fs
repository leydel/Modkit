namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpGuildActions =
    // https://discord.com/developers/docs/resources/guild#create-guild
    abstract member CreateGuild:
        payload: CreateGuild ->
        Task<Guild>

    // https://discord.com/developers/docs/resources/guild#get-guild
    abstract member GetGuild:
        guildId: string ->
        withCounts: bool option ->
        Task<Guild>

    // https://discord.com/developers/docs/resources/guild#get-guild-preview
    abstract member GetGuildPreview:
        guildId: string ->
        Task<Guild>

    // https://discord.com/developers/docs/resources/guild#modify-guild
    abstract member ModifyGuild:
        guildId: string ->
        auditLogReason: string option ->
        payload: ModifyGuild ->
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
        payload: CreateGuildChannel ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/guild#modify-guild-channel-positions
    abstract member ModifyGuildChannelPositions:
        guildId: string ->
        payload: ModifyGuildChannelPositions ->
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
        payload: AddGuildMember ->
        Task<GuildMember>

    // https://discord.com/developers/docs/resources/guild#modify-guild-member
    abstract member ModifyGuildMember:
        guildId: string ->
        userId: string ->
        auditLogReason: string option ->
        payload: ModifyGuildMember ->
        Task<GuildMember>

    // https://discord.com/developers/docs/resources/guild#modify-current-member
    abstract member ModifyCurrentMember:
        guildId: string ->
        auditLogReason: string option ->
        payload: ModifyCurrentMember ->
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
        payload: CreateGuildBan ->
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
        payload: BulkGuildBan ->
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
        payload: CreateGuildRole ->
        Task<Role>

    // https://discord.com/developers/docs/resources/guild#modify-guild-role-positions
    abstract member ModifyGuildRolePositions:
        guildId: string ->
        auditLogReason: string option ->
        payload: ModifyGuildRolePositions list ->
        Task<Role list>
    
    // https://discord.com/developers/docs/resources/guild#modify-guild-role
    abstract member ModifyGuildRole:
        guildId: string ->
        roleId: string ->
        auditLogReason: string option ->
        payload: ModifyGuildRole ->
        Task<Role>

    // https://discord.com/developers/docs/resources/guild#modify-guild-mfa-level
    abstract member ModifyGuildMfaLevel:
        guildId: string ->
        auditLogReason: string option ->
        payload: ModifyGuildMfaLevel ->
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
        payload: BeginGuildPrune ->
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
    abstract member ModifyGuildWidgetSettings:
        guildId: string ->
        auditLogReason: string option ->
        payload: ModifyGuildWidgetSettings ->
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
        style: GuildWidgetStyle ->
        Task<unit> // TODO: Handle png response type

    // https://discord.com/developers/docs/resources/guild#get-guild-welcome-screen
    abstract member GetGuildWelcomeScreen:
        guildId: string ->
        Task<WelcomeScreen>

    // https://discord.com/developers/docs/resources/guild#modify-guild-welcome-screen
    abstract member ModifyGuildWelcomeScreen:
        guildId: string ->
        auditLogReason: string option ->
        payload: ModifyGuildWelcomeScreen ->
        Task<WelcomeScreen>

    // https://discord.com/developers/docs/resources/guild#get-guild-onboarding
    abstract member GetGuildOnboarding:
        guildId: string ->
        Task<GuildOnboarding>

    // https://discord.com/developers/docs/resources/guild#modify-guild-onboarding
    abstract member ModifyGuildOnboarding:
        guildId: string ->
        auditLogReason: string option ->
        payload: ModifyGuildOnboarding ->
        Task<GuildOnboarding>

type DiscordHttpGuildActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpGuildActions with
        member _.GetGuildRoles guildId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/roles"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        // TODO: Implement other methods
