namespace Modkit.Discordfs.Types

open FSharp.Json
open System
open System.Collections.Generic

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
    
    [<JsonField("name")>]
    IntegrationTypes: ApplicationIntegrationType list option
    
    [<JsonField("contexts")>]
    Contexts: InteractionContextType list option
    
    [<JsonField("type")>]
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

type Identify = {
    [<JsonField("token")>]
    Token: string
    
    [<JsonField("properties")>]
    Properties: ConnectionProperties
    
    [<JsonField("compress")>]
    Compress: bool option
    
    [<JsonField("large_threshold")>]
    LargeThreshold: int option
    
    [<JsonField("shard")>]
    Shard: (int * int) option
    
    [<JsonField("presence")>]
    Presence: UpdatePresence option
    
    [<JsonField("intents")>]
    Intents: int
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

and Resume = {
    [<JsonField("token")>]
    Token: string
    
    [<JsonField("session_id")>]
    SessionId: string
    
    [<JsonField("seq")>]
    Sequence: int
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

and Heartbeat = int option

and RequestGuildMembers = {
    [<JsonField("guild_id")>]
    GuildId: string
    
    [<JsonField("query")>]
    Query: string option
    
    [<JsonField("limit")>]
    Limit: int
    
    [<JsonField("presences")>]
    Presences: bool option
    
    [<JsonField("user_ids")>]
    UserIds: string list option
    
    [<JsonField("nonce")>]
    Nonce: string option
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

and UpdateVoiceState = {
    [<JsonField("guild_id")>]
    GuildId: string
    
    [<JsonField("channel_id")>]
    ChannelId: string option
    
    [<JsonField("self_mute")>]
    SelfMute: bool
    
    [<JsonField("self_deaf")>]
    SelfDeaf: bool
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

and UpdatePresence = {
    [<JsonField("since")>]
    Since: int option
    
    [<JsonField("activities")>]
    Activities: Activity list
    
    [<JsonField("status", Transform = typeof<StatusTypeTransform>)>]
    Status: StatusType
    
    [<JsonField("afk")>]
    Afk: bool
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

type TypingStart = {
    [<JsonField("channel_id")>]
    ChannelId: string
    
    [<JsonField("guild_id")>]
    GuildId: string option
    
    [<JsonField("user_id")>]
    UserId: string
    
    [<JsonField("timestamp")>]
    Timestamp: DateTime
    
    [<JsonField("member")>]
    Member: GuildMember
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

type VoiceChannelEffect = {
    [<JsonField("channel_id")>]
    ChannelId: string
    
    [<JsonField("guild_id")>]
    GuildId: string
    
    [<JsonField("user_id")>]
    UserId: string
    
    [<JsonField("emoji")>]
    Emoji: Emoji option
    
    [<JsonField("animation_type")>]
    AnimationType: AnimationType option
    
    [<JsonField("animation_id")>]
    AnimationId: int option

    [<JsonField("sound_id", Transform = typeof<SoundboardSoundIdTransform>)>]
    SoundId: SoundboardSoundId option

    [<JsonField("sound_volume")>]
    SoundVolume: double option
}
