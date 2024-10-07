namespace Discordfs.Gateway.Types

open Discordfs.Types
open System
open System.Text.Json.Serialization

#nowarn "49"

// https://discord.com/developers/docs/topics/gateway-events#identify-identify-structure
type Identify = {
    [<JsonPropertyName "token">] Token: string
    [<JsonPropertyName "properties">] Properties: ConnectionProperties
    [<JsonPropertyName "compress">] Compress: bool option
    [<JsonPropertyName "large_threshold">] LargeThreshold: int option
    [<JsonPropertyName "shard">] Shard: (int * int) option
    [<JsonPropertyName "presence">] Presence: UpdatePresence option
    [<JsonPropertyName "intents">] Intents: int
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
    [<JsonPropertyName "token">] Token: string
    [<JsonPropertyName "session_id">] SessionId: string
    [<JsonPropertyName "seq">] Sequence: int
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
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "query">] Query: string option
    [<JsonPropertyName "limit">] Limit: int
    [<JsonPropertyName "presences">] Presences: bool option
    [<JsonPropertyName "user_ids">] UserIds: string list option
    [<JsonPropertyName "nonce">] Nonce: string option
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
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "self_mute">] SelfMute: bool
    [<JsonPropertyName "self_deaf">] SelfDeaf: bool
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
    [<JsonPropertyName "since">] Since: int option
    [<JsonPropertyName "activities">] Activities: Activity list
    [<JsonPropertyName "status">] Status: StatusType
    [<JsonPropertyName "afk">] Afk: bool
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
    [<JsonPropertyName "heartbeat_interval">] HeartbeatInterval: int
}

// https://discord.com/developers/docs/topics/gateway-events#ready-ready-event-fields
type Ready = {
    [<JsonPropertyName "v">] Version: int
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "guilds">] Guilds: UnavailableGuild list
    [<JsonPropertyName "session_id">] SessionId: string
    [<JsonPropertyName "resume_gateway_url">] ResumeGatewayUrl: string
    [<JsonPropertyName "shard">] Shard: (int * int) option
    [<JsonPropertyName "application">] Application: PartialApplication
}

// https://discord.com/developers/docs/topics/gateway-events#typing-start-typing-start-event-fields
type TypingStart = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "timestamp">] Timestamp: DateTime
    [<JsonPropertyName "member">] Member: GuildMember
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
