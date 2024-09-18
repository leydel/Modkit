namespace Modkit.Discordfs.Types

open FSharp.Json
open System
open System.Text.Json.Serialization

#nowarn "49"

type InteractionCallback = {
    [<JsonName "type">] Type: InteractionCallbackType
    [<JsonName "data">] Data: InteractionCallbackData option
}
with
    static member build(
        Type: InteractionCallbackType,
        ?Data: InteractionCallbackData
    ) = {
        Type = Type;
        Data = Data;
    }

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
    [<JsonName "channel_id">] ChannelId: string
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "user_id">] UserId: string
    [<JsonName "emoji">] Emoji: Emoji option
    [<JsonName "animation_type">] AnimationType: AnimationType option
    [<JsonName "animation_id">] AnimationId: int option
    [<JsonName "sound_id">] [<JsonConverter(typeof<SoundboardSoundIdConverter>)>] SoundId: SoundboardSoundId option
    [<JsonName "sound_volume">] SoundVolume: double option
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
    [<JsonName "status">] [<JsonConverter(typeof<StatusTypeConverter>)>] Status: StatusType
    [<JsonName "afk">] Afk: bool
}
with
    static member build(
        Status: StatusType,
        ?Activities: Activity list,
        ?Afk: bool,
        ?Since: int
    ) = {
        Since = Since;
        Activities = Option.defaultValue [] Activities;
        Status = Status;
        Afk = Option.defaultValue false Afk;
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