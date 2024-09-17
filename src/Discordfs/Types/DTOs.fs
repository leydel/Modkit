namespace Modkit.Discordfs.Types

open FSharp.Json
open System
open System.Collections.Generic
open System.Text.Json.Serialization

#nowarn "49"

type CreateChannelInvite = {
    [<JsonField("max_age")>]
    MaxAge: int option
    
    [<JsonField("max_uses")>]
    MaxUses: int option
    
    [<JsonField("temporary")>]
    Temporary: bool option
    
    [<JsonField("unique")>]
    Unique: bool option
    
    [<JsonField("target_type", EnumValue = EnumMode.Value)>]
    TargetType: InviteTargetType option
    
    [<JsonField("target_user_id")>]
    TargetUserId: string option
    
    [<JsonField("target_application_id")>]
    TargetApplicationId: string option
}
with
    static member build(
        ?maxAge: int,
        ?maxUses: int,
        ?temporary: bool,
        ?unique: bool,
        ?targetType: InviteTargetType,
        ?targetUserId: string,
        ?targetApplicationId: string
    ) = {
        MaxAge = maxAge;
        MaxUses = maxUses;
        Temporary = temporary;
        Unique = unique;
        TargetType = targetType;
        TargetUserId = targetUserId;
        TargetApplicationId = targetApplicationId;
    }

type CreateGlobalApplicationCommand = {
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("name_localizations")>]
    NameLocalizations: Dictionary<string, string> option
    
    [<JsonField("description")>]
    Description: string option
    
    [<JsonField("description_localizations")>]
    DescriptionLocalizations: Dictionary<string, string> option
    
    [<JsonField("options")>]
    Options: ApplicationCommandOption list option
    
    [<JsonField("default_member_permissions")>]
    DefaultMemberPermissions: string option
    
    [<JsonField("dm_permissions")>]
    DmPermissions: bool option
    
    [<JsonField("name", EnumValue = EnumMode.Value)>]
    IntegrationTypes: ApplicationIntegrationType list option
    
    [<JsonField("contexts", EnumValue = EnumMode.Value)>]
    Contexts: InteractionContextType list option
    
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ApplicationCommandType option
    
    [<JsonField("nsfw")>]
    Nsfw: bool option
}
with
    static member build(
        Name: string,
        ?NameLocalizations: Dictionary<string, string>,
        ?Description: string,
        ?DescriptionLocalizations: Dictionary<string, string>,
        ?Options: ApplicationCommandOption list,
        ?DefaultMemberPermissions: string,
        ?DmPermissions: bool,
        ?IntegrationTypes: ApplicationIntegrationType list,
        ?Contexts: InteractionContextType list,
        ?Type: ApplicationCommandType,
        ?Nsfw: bool
    ) = {
        Name = Name;
        NameLocalizations = NameLocalizations;
        Description = Description;
        DescriptionLocalizations = DescriptionLocalizations;
        Options = Options;
        DefaultMemberPermissions = DefaultMemberPermissions;
        DmPermissions = DmPermissions;
        IntegrationTypes = IntegrationTypes;
        Contexts = Contexts;
        Type = Type;
        Nsfw = Nsfw;
    }

type ExecuteWebhook = {
    [<JsonField("content")>]
    Content: string option
    
    [<JsonField("username")>]
    Username: string
    
    [<JsonField("avatar_url")>]
    AvatarUrl: string
    
    [<JsonField("tts")>]
    Tts: bool
    
    [<JsonField("embeds")>]
    Embeds: Embed list option
    
    [<JsonField("allowed_mentions")>]
    AllowedMentions: AllowedMentions
    
    [<JsonField("components")>]
    Components: Component list option

    // TODO: Add `files`, `payload_json`, `attachments` support
    
    [<JsonField("flags")>]
    Flags: int
    
    [<JsonField("thread_name")>]
    ThreadName: string option
    
    [<JsonField("applied_tags")>]
    AppliedTags: string list option
    
    [<JsonField("poll")>]
    Poll: Poll option

    // TODO: Check what types should be `option`
}

type EditWebhookMessage = {
    [<JsonField("content")>]
    Content: string option
    
    [<JsonField("embeds")>]
    Embeds: Embed list option
    
    [<JsonField("allowed_mentions")>]
    AllowedMentions: AllowedMentions
    
    [<JsonField("components")>]
    Components: Component list option

    // TODO: Add `files`, `payload_json`, `attachments` support

    // TODO: Check what types should be `option`
}

type GetGateway = {
    [<JsonField("url")>]
    Url: string
}

type GetGatewayBot = {
    [<JsonField("url")>]
    Url: string

    [<JsonField("shards")>]
    Shards: int

    [<JsonField("session_start_limit")>]
    SessionStartLimit: SessionStartLimit
}

type VoiceChannelEffect = {
    [<JsonField("channel_id")>]
    ChannelId: string
    
    [<JsonField("guild_id")>]
    GuildId: string
    
    [<JsonField("user_id")>]
    UserId: string
    
    [<JsonField("emoji")>]
    Emoji: Emoji option
    
    [<JsonField("animation_type", EnumValue = EnumMode.Value)>]
    AnimationType: AnimationType option
    
    [<JsonField("animation_id")>]
    AnimationId: int option

    [<JsonField("sound_id", Transform = typeof<SoundboardSoundIdTransform>)>]
    SoundId: SoundboardSoundId option

    [<JsonField("sound_volume")>]
    SoundVolume: double option
}

type EditChannelPermissions = {
    [<JsonField("allow")>]
    Allow: string option

    [<JsonField("deny")>]
    Deny: string option

    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: EditChannelPermissionsType
}

type FollowAnnouncementChannel = {
    [<JsonField("webhook_channel_id")>]
    WebhookChannelId: string
}

type GroupDmAddRecipient = {
    [<JsonField("access_token")>]
    AccessToken: string

    [<JsonField("nick")>]
    Nick: string // TODO: Check if this is optional (documented as not, but I expect it is)
}

type StartThreadFromMessage = {
    [<JsonField("name")>]
    Name: string

    [<JsonField("auto_archive_duration", EnumValue = EnumMode.Value)>]
    AutoArchiveDuration: AutoArchiveDurationType option

    [<JsonField("rate_limit_per_user")>]
    RateLimitPerUser: int option
}

type StartThreadWithoutMessage = {
    [<JsonField("name")>]
    Name: string

    [<JsonField("auto_archive_duration", EnumValue = EnumMode.Value)>]
    AutoArchiveDuration: AutoArchiveDurationType option

    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ThreadType

    [<JsonField("invitable")>]
    Invitable: bool option

    [<JsonField("rate_limit_per_user")>]
    RateLimitPerUser: int option
}

type ListPublicArchivedThreadsResponse = {
    [<JsonField("threads")>]
    Threads: Channel list
    
    [<JsonField("members")>]
    Members: ThreadMember list
    
    [<JsonField("has_more")>]
    HasMore: bool
}

type ListPrivateArchivedThreadsResponse = {
    [<JsonField("threads")>]
    Threads: Channel list
    
    [<JsonField("members")>]
    Members: ThreadMember list
    
    [<JsonField("has_more")>]
    HasMore: bool
}

type ListJoinedPrivateArchivedThreadsResponse = {
    [<JsonField("threads")>]
    Threads: Channel list
    
    [<JsonField("members")>]
    Members: ThreadMember list
    
    [<JsonField("has_more")>]
    HasMore: bool
}

// https://discord.com/developers/docs/resources/auto-moderation#create-auto-moderation-rule-json-params
type CreateAutoModerationRule = {
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("event_type")>]
    EventType: AutoModerationEventType
    
    [<JsonField("trigger_type")>]
    TriggerType: AutoModerationTriggerType
    
    [<JsonField("trigger_metadata")>]
    TriggerMetadata: AutoModerationTriggerMetadata option
    
    [<JsonField("actions")>]
    Actions: AutoModerationAction list
    
    [<JsonField("enabled")>]
    Enabled: bool option
    
    [<JsonField("exempt_roles")>]
    ExemptRoles: string list option
    
    [<JsonField("exempt_channels")>]
    ExemptChannels: string list option
}

// https://discord.com/developers/docs/resources/auto-moderation#modify-auto-moderation-rule-json-params
type ModifyAutoModerationRule = {
    [<JsonField("name")>]
    Name: string option
    
    [<JsonField("event_type")>]
    EventType: AutoModerationEventType option
    
    [<JsonField("trigger_type")>]
    TriggerType: AutoModerationTriggerType option
    
    [<JsonField("trigger_metadata")>]
    TriggerMetadata: AutoModerationTriggerMetadata option
    
    [<JsonField("actions")>]
    Actions: AutoModerationAction list option
    
    [<JsonField("enabled")>]
    Enabled: bool option
    
    [<JsonField("exempt_roles")>]
    ExemptRoles: string list option
    
    [<JsonField("exempt_channels")>]
    ExemptChannels: string list option
}

// https://discord.com/developers/docs/resources/application#edit-current-application-json-params
type EditCurrentApplication = {
    [<JsonField("custom_install_url")>]
    CustomInstallUrl: string option
    
    [<JsonField("description")>]
    Description: string option
    
    [<JsonField("role_connection_verification_url")>]
    RoleConnectionVerificationUrl: string option
    
    [<JsonField("install_params")>]
    InstallParams: OAuth2InstallParams option
    
    [<JsonField("integration_types_config")>]
    IntegrationTypesConfig: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option
    
    [<JsonField("flags")>]
    Flags: int option
    
    [<JsonField("icon")>]
    Icon: string option
    
    [<JsonField("cover_image")>]
    CoverImage: string option
    
    [<JsonField("interactions_endpoint_url")>]
    InteractionsEndpointUrl: string option
    
    [<JsonField("tags")>]
    Tags: string list option
}

// https://discord.com/developers/docs/resources/emoji#create-guild-emoji-json-params
type CreateGuildEmoji = {
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("image")>]
    Image: string
    
    [<JsonField("roles")>]
    Roles: string list
}

// https://discord.com/developers/docs/resources/emoji#modify-guild-emoji-json-params
type ModifyGuildEmoji = {
    [<JsonField("name")>]
    Name: string option
    
    [<JsonField("roles")>]
    Roles: string list option
}

// https://discord.com/developers/docs/resources/emoji#list-application-emojis
type ListApplicationEmojisResponse = {
    [<JsonField("items")>]
    Items: Emoji list
}

// https://discord.com/developers/docs/resources/emoji#create-application-emoji-json-params
type CreateApplicationEmoji = {
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("image")>]
    Image: string
}

// https://discord.com/developers/docs/resources/emoji#modify-application-emoji-json-params
type ModifyApplicationEmoji = {
    [<JsonField("name")>]
    Name: string
}

// https://discord.com/developers/docs/resources/entitlement#create-test-entitlement-json-params
type CreateTestEntitlement = {
    [<JsonField("sku_id")>]
    SkuId: string
    
    [<JsonField("owner_id")>]
    OwnerId: string
    
    [<JsonField("owner_type", EnumValue = EnumMode.Value)>]
    OwnerType: EntitlementOwnerType
}

// https://discord.com/developers/docs/resources/guild#modify-guild-channel-positions-json-params
type ModifyGuildChannelPosition = {
    [<JsonName "id">] Id: string
    [<JsonName "position">] Position: int option
    [<JsonName "lock_permissions">] LockPermissions: bool option
    [<JsonName "parent_id">] ParentId: string option
}

// https://discord.com/developers/docs/resources/guild#list-active-guild-threads-response-body
type ListActiveGuildThreadsResponse = {
    [<JsonName "threads">] Threads: Channel list
    [<JsonName "members">] Members: GuildMember list
}

// https://discord.com/developers/docs/resources/guild#bulk-guild-ban-bulk-ban-response
type BulkGuildBanResponse = {
    [<JsonName "banned_users">] BannedUsers: string list
    [<JsonName "failed_users">] FailedUsers: string list
}

// https://discord.com/developers/docs/resources/guild#modify-guild-role-positions-json-params
type ModifyGuildRolePosition = {
    [<JsonName "id">] Id: string
    [<JsonName "position">] Position: int option
}

// https://discord.com/developers/docs/resources/guild#get-guild-prune-count
type GetGuildPruneCountResponse = {
    [<JsonName "pruned">] Pruned: int
}

// https://discord.com/developers/docs/resources/guild#begin-guild-prune
type BeginGuildPruneResponse = {
    [<JsonName "pruned">] Pruned: int option
}

// https://discord.com/developers/docs/resources/guild#get-guild-vanity-url
type GetGuildVanityUrlResponse = {
    [<JsonName "code">] Code: string option
    [<JsonName "uses">] Uses: int
}

// https://discord.com/developers/docs/topics/gateway-events#identify-identify-structure
type Identify = {
    [<JsonName "token">] Token: string
    [<JsonName "properties">] Properties: ConnectionProperties
    [<JsonName "compress">] Compress: bool option
    [<JsonName "large_threshold">] LargeThreshold: int option
    [<JsonName "shard">] Shard: (int * int) option
    [<JsonName "presence">] Presence: UpdatePresence option
    [<JsonName "intents">] Intents: int
}
with
    static member build(
        Token: string,
        Intents: int,
        Properties: ConnectionProperties,
        ?Compress: bool,
        ?LargeThreshold: int,
        ?Shard: int * int,
        ?Presence: UpdatePresence
    ) = {
        Token = Token;
        Intents = Intents;
        Properties = Properties;
        Compress = Compress;
        LargeThreshold = LargeThreshold;
        Shard = Shard;
        Presence = Presence;
    }

// https://discord.com/developers/docs/topics/gateway-events#resume-resume-structure
and Resume = {
    [<JsonName "token">] Token: string
    [<JsonName "session_id">] SessionId: string
    [<JsonName "seq">] Sequence: int
}
with
    static member build(
        Token: string,
        SessionId: string,
        Sequence: int
    ) = {
        Token = Token;
        SessionId = SessionId;
        Sequence = Sequence;
    }

// https://discord.com/developers/docs/topics/gateway-events#heartbeat-example-heartbeat
and Heartbeat = int option

// https://discord.com/developers/docs/topics/gateway-events#request-guild-members-request-guild-members-structure
and RequestGuildMembers = {
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "query">] Query: string option
    [<JsonName "limit">] Limit: int
    [<JsonName "presences">] Presences: bool option
    [<JsonName "user_ids">] UserIds: string list option
    [<JsonName "nonce">] Nonce: string option
}
with
    static member build(
        GuildId: string,
        Limit: int,
        ?Presences: bool,
        ?Query: string,
        ?UserIds: string list,
        ?Nonce: string
    ) = {
        GuildId = GuildId;
        Query = Query;
        Limit = Limit;
        Presences = Presences;
        UserIds = UserIds;
        Nonce = Nonce;
    }

// https://discord.com/developers/docs/topics/gateway-events#update-voice-state-gateway-voice-state-update-structure
and UpdateVoiceState = {
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "channel_id">] ChannelId: string option
    [<JsonName "self_mute">] SelfMute: bool
    [<JsonName "self_deaf">] SelfDeaf: bool
}
with
    static member build(
        GuildId: string,
        ChannelId: string option,
        SelfMute: bool,
        SelfDeaf: bool
    ) = {
        GuildId = GuildId;
        ChannelId = ChannelId;
        SelfMute = SelfMute;
        SelfDeaf = SelfDeaf;
    }

// https://discord.com/developers/docs/topics/gateway-events#update-presence-gateway-presence-update-structure
and UpdatePresence = {
    [<JsonName "since">] Since: int option
    [<JsonName "activities">] Activities: Activity list
    [<JsonName "status">] Status: StatusType
    [<JsonName "afk">] Afk: bool
}
with
    static member build(
        Since: int option,
        Activities: Activity list,
        Status: StatusType,
        Afk: bool
    ) = {
        Since = Since;
        Activities = Activities;
        Status = Status;
        Afk = Afk;
    }

// https://discord.com/developers/docs/topics/gateway#hello-event-example-hello-event
type Hello = {
    [<JsonName "heartbeat_interval">] HeartbeatInterval: int
}

// https://discord.com/developers/docs/topics/gateway-events#ready-ready-event-fields
type Ready = {
    [<JsonName "v">] Version: int
    [<JsonName "user">] User: User
    [<JsonName "guilds">] Guilds: Guild list // Unavailable guilds only (id, unavailable=true) (should this type be created)
    [<JsonName "session_id">] SessionId: string
    [<JsonName "resume_gateway_url">] ResumeGatewayUrl: string
    [<JsonName "shard">] Shard: (int * int) option
    [<JsonName "application">] Application: Application // partion, only contains id and flags (should this type be created)
}

// https://discord.com/developers/docs/topics/gateway-events#typing-start-typing-start-event-fields
type TypingStart = {
    [<JsonName "channel_id">] ChannelId: string
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "user_id">] UserId: string
    [<JsonName "timestamp">] Timestamp: DateTime
    [<JsonName "member">] Member: GuildMember
}
with
    static member build(
        ChannelId: string,
        UserId: string,
        Timestamp: DateTime,
        Member: GuildMember,
        ?GuildId: string
    ) = {
        ChannelId = ChannelId;
        GuildId = GuildId;
        UserId = UserId;
        Timestamp = Timestamp;
        Member = Member;
    }