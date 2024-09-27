namespace Discordfs.Gateway.Types

open Discordfs.Types
open System
open System.Text.Json.Serialization

#nowarn "49"

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
    [<JsonName "guilds">] Guilds: UnavailableGuild list
    [<JsonName "session_id">] SessionId: string
    [<JsonName "resume_gateway_url">] ResumeGatewayUrl: string
    [<JsonName "shard">] Shard: (int * int) option
    [<JsonName "application">] Application: PartialApplication
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
