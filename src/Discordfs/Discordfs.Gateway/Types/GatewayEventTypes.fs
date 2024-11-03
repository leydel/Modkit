namespace Discordfs.Gateway.Types

open Discordfs.Types
open System
open System.Text.Json
open System.Text.Json.Serialization

#nowarn "49"

// https://discord.com/developers/docs/topics/gateway-events#identify-identify-structure
type IdentifySendEvent = {
    [<JsonPropertyName "token">] Token: string
    [<JsonPropertyName "properties">] Properties: ConnectionProperties
    [<JsonPropertyName "compress">] Compress: bool option
    [<JsonPropertyName "large_threshold">] LargeThreshold: int option
    [<JsonPropertyName "shard">] Shard: (int * int) option
    [<JsonPropertyName "presence">] Presence: UpdatePresenceSendEvent option
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
        ?Presence: UpdatePresenceSendEvent
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
and ResumeSendEvent = {
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
and HeartbeatSendEvent = int option

// https://discord.com/developers/docs/topics/gateway-events#request-guild-members-request-guild-members-structure
and RequestGuildMembersSendEvent = {
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
and UpdateVoiceStateSendEvent = {
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

// https://discord.com/developers/docs/events/gateway-events#request-soundboard-sounds
and RequestSoundboardSoundsSendEvent = {
    [<JsonPropertyName "guild_ids">] GuildIds: string list 
}
with
    static member build(
        GuildIds: string list
    ) = {
        GuildIds = GuildIds;
    }

// https://discord.com/developers/docs/topics/gateway-events#update-presence-gateway-presence-update-structure
and UpdatePresenceSendEvent = {
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

and PartialUpdatePresenceSendEvent = {
    [<JsonPropertyName "since">] Since: int option
    [<JsonPropertyName "activities">] Activities: Activity list option
    [<JsonPropertyName "status">] Status: StatusType option
    [<JsonPropertyName "afk">] Afk: bool option
}

// https://discord.com/developers/docs/events/gateway#connection-lifecycle
type HeartbeatReceiveEvent = Empty

// https://discord.com/developers/docs/events/gateway#connection-lifecycle
type HeartbeatAckReceiveEvent = Empty

// https://discord.com/developers/docs/topics/gateway#hello-event-example-hello-event
type HelloReceiveEvent = {
    [<JsonPropertyName "heartbeat_interval">] HeartbeatInterval: int
}

// https://discord.com/developers/docs/topics/gateway-events#ready-ready-event-fields
type ReadyReceiveEvent = {
    [<JsonPropertyName "v">] Version: int
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "guilds">] Guilds: UnavailableGuild list
    [<JsonPropertyName "session_id">] SessionId: string
    [<JsonPropertyName "resume_gateway_url">] ResumeGatewayUrl: string
    [<JsonPropertyName "shard">] Shard: (int * int) option
    [<JsonPropertyName "application">] Application: PartialApplication
}

// https://discord.com/developers/docs/events/gateway-events#resumed
type ResumedReceiveEvent = Empty

// https://discord.com/developers/docs/events/gateway-events#reconnect
type ReconnectReceiveEvent = Empty

// https://discord.com/developers/docs/topics/gateway-events#invalid-session
type InvalidSessionReceiveEvent = bool

// https://discord.com/developers/docs/events/gateway-events#application-command-permissions-update
type ApplicationCommandPermissionsUpdateReceiveEvent = ApplicationCommandPermission

// https://discord.com/developers/docs/events/gateway-events#auto-moderation-rule-create
type AutoModerationRuleCreateReceiveEvent = AutoModerationRule

// https://discord.com/developers/docs/events/gateway-events#auto-moderation-rule-update
type AutoModerationRuleUpdateReceiveEvent = AutoModerationRule

// https://discord.com/developers/docs/events/gateway-events#auto-moderation-rule-delete
type AutoModerationRuleDeleteReceiveEvent = AutoModerationRule

// https://discord.com/developers/docs/events/gateway-events#auto-moderation-action-execution-auto-moderation-action-execution-event-fields
type AutoModerationActionExecutionReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "action">] Action: AutoModerationAction
    [<JsonPropertyName "rule_id">] RuleId: string
    [<JsonPropertyName "rule_trigger_type">] RuleTriggerType: AutoModerationTriggerType
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "message_id">] MessageId: string option
    [<JsonPropertyName "alert_system_message_id">] AlertSystemMessageId: string option
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "matched_keyword">] MatchedKeyword: string option
    [<JsonPropertyName "matched_content">] MatchedContent: string option
}

// https://discord.com/developers/docs/events/gateway-events#channel-create
type ChannelCreateReceiveEvent = Channel

// https://discord.com/developers/docs/events/gateway-events#channel-update
type ChannelUpdateReceiveEvent = Channel

// https://discord.com/developers/docs/events/gateway-events#channel-delete
type ChannelDeleteReceiveEvent = Channel

// https://discord.com/developers/docs/events/gateway-events#thread-create
[<JsonConverter(typeof<ThreadCreateReceiveEventConverter>)>]
type ThreadCreateReceiveEvent = {
    Channel: Channel
    ExtraFields: ThreadCreateReceiveEventExtraFields
}

and ThreadCreateReceiveEventExtraFields = {
    [<JsonPropertyName "newly_created">] NewlyCreated: bool option
    [<JsonPropertyName "thread_member">] ThreadMember: ThreadMember option
}

and ThreadCreateReceiveEventConverter () =
    inherit JsonConverter<ThreadCreateReceiveEvent> ()

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

// https://discord.com/developers/docs/events/gateway-events#thread-update
type ThreadUpdateReceiveEvent = Channel

// https://discord.com/developers/docs/events/gateway-events#thread-delete
type ThreadDeleteReceiveEvent = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "parent_id">] ParentId: string
    [<JsonPropertyName "type">] Type: ChannelType
}

// https://discord.com/developers/docs/events/gateway-events#thread-list-sync-thread-list-sync-event-fields
type ThreadListSyncReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "channel_ids">] ChannelIds: string list option
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
}

// https://discord.com/developers/docs/events/gateway-events#thread-member-update
[<JsonConverter(typeof<ThreadMemberUpdateEventConverter>)>]
type ThreadMemberUpdateReceiveEvent = {
    ThreadMember: ThreadMember
    ExtraFields: ThreadMemberUpdateEventExtraFields
}

and ThreadMemberUpdateEventExtraFields = {
    [<JsonPropertyName "guild_id">] GuildId: string
}

and ThreadMemberUpdateEventConverter () =
    inherit JsonConverter<ThreadMemberUpdateReceiveEvent> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            ThreadMember = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let threadMember = Json.serializeF value.ThreadMember
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge threadMember extraFields)

// https://discord.com/developers/docs/events/gateway-events#thread-members-update
type ThreadMembersUpdateReceiveEvent = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "member_count">] MemberCount: int
    [<JsonPropertyName "added_members">] AddedMembers: ThreadMember list option
    [<JsonPropertyName "removed_member_ids">] RemovedMemberIds: string list option
}

// https://discord.com/developers/docs/events/gateway-events#channel-pins-update-channel-pins-update-event-fields
type ChannelPinsUpdateReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "channel_id">] ChannelIds: string
    [<JsonPropertyName "last_pin_timestamp">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] LastPinTimestamp: DateTime option
}

// https://discord.com/developers/docs/events/gateway-events#entitlement-create
type EntitlementCreateReceiveEvent = Entitlement

// https://discord.com/developers/docs/events/gateway-events#entitlement-update
type EntitlementUpdateReceiveEvent = Entitlement

// https://discord.com/developers/docs/events/gateway-events#entitlement-delete
type EntitlementDeleteReceiveEvent = Entitlement

[<JsonConverter(typeof<GuildCreateReceiveEventAvailableGuildConverter>)>]
type GuildCreateReceiveEventAvailableGuild = {
    Guild: Guild
    ExtraFields: GuildCreateReceiveEventAvailableGuildExtraFields
}

and GuildCreateReceiveEventAvailableGuildExtraFields = {
    [<JsonPropertyName "joined_at">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] JoinedAt: DateTime
    [<JsonPropertyName "large">] Large: bool
    [<JsonPropertyName "unavailable">] Unavailable: bool
    [<JsonPropertyName "member_count">] MemberCount: int
    [<JsonPropertyName "voice_states">] VoiceStates: PartialVoiceState list
    [<JsonPropertyName "members">] Members: GuildMember list
    [<JsonPropertyName "channels">] Channels: Channel list
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "presences">] Presences: PartialUpdatePresenceSendEvent list
    [<JsonPropertyName "stage_instances">] StageInstances: StageInstance list
    [<JsonPropertyName "guild_scheduled_events">] GuildScheduledEvents: GuildScheduledEvent list
    [<JsonPropertyName "soundboard_sounds">] SoundboardSounds: SoundboardSound list
}

and GuildCreateReceiveEventAvailableGuildConverter () =
    inherit JsonConverter<GuildCreateReceiveEventAvailableGuild> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            Guild = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let guild = Json.serializeF value.Guild
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge guild extraFields)

// https://discord.com/developers/docs/events/gateway-events#guild-create
[<JsonConverter(typeof<GuildCreateConverter>)>]
type GuildCreateReceiveEvent =
    | AvailableGuild of GuildCreateReceiveEventAvailableGuild
    | UnavailableGuild of UnavailableGuild

and GuildCreateConverter () =
    inherit JsonConverter<GuildCreateReceiveEvent> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let available =
            match document.RootElement.TryGetProperty "name" with
            | exists, _ -> exists

        let json = document.RootElement.GetRawText()

        match available with
        | true -> AvailableGuild <| Json.deserializeF json
        | false -> UnavailableGuild <| Json.deserializeF json

    override _.Write (writer, value, options) =
        match value with
        | AvailableGuild a -> Json.serializeF a |> writer.WriteRawValue
        | UnavailableGuild u -> Json.serializeF u |> writer.WriteRawValue

// https://discord.com/developers/docs/events/gateway-events#guild-update
type GuildUpdateReceiveEvent = Guild

// https://discord.com/developers/docs/events/gateway-events#guild-delete
type GuildDeleteReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "unavailable">] Unavailable: bool option
}

// https://discord.com/developers/docs/events/gateway-events#guild-audit-log-entry-create
[<JsonConverter(typeof<GuildAuditLogEntryCreateReceiveEventConverter>)>]
type GuildAuditLogEntryCreateReceiveEvent = {
    AuditLogEntry: AuditLogEntry
    ExtraFields: GuildAuditLogEntryCreateReceiveEventExtraFields
}

and GuildAuditLogEntryCreateReceiveEventExtraFields = {
    [<JsonPropertyName "guild_id">] GuildId: string
}

and GuildAuditLogEntryCreateReceiveEventConverter () =
    inherit JsonConverter<GuildAuditLogEntryCreateReceiveEvent> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            AuditLogEntry = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let auditLogEntry = Json.serializeF value.AuditLogEntry
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge auditLogEntry extraFields)

// https://discord.com/developers/docs/events/gateway-events#guild-ban-add
type GuildBanAddReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "user">] user: User
}

// https://discord.com/developers/docs/events/gateway-events#guild-ban-remove
type GuildBanRemoveReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "user">] user: User
}

// https://discord.com/developers/docs/events/gateway-events#guild-emojis-update
type GuildEmojisUpdateReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "emojis">] Emojis: Emoji list
}

// https://discord.com/developers/docs/events/gateway-events#guild-stickers-update
type GuildStickersUpdateReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "stickers">] Stickers: Sticker list
}

// https://discord.com/developers/docs/events/gateway-events#guild-integrations-update
type GuildIntegrationsUpdateReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-member-add
[<JsonConverter(typeof<GuildMemberAddReceiveEventConverter>)>]
type GuildMemberAddReceiveEvent = {
    GuildMember: GuildMember
    ExtraFields: GuildMemberAddReceiveEventExtraFields
}

and GuildMemberAddReceiveEventExtraFields = {
    [<JsonPropertyName "guild_id">] GuildId: string
}

and GuildMemberAddReceiveEventConverter () =
    inherit JsonConverter<GuildMemberAddReceiveEvent> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            GuildMember = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let guildMember = Json.serializeF value.GuildMember
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge guildMember extraFields)

// https://discord.com/developers/docs/events/gateway-events#guild-member-remove
type GuildMemberRemoveReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "user">] User: User
}

// https://discord.com/developers/docs/events/gateway-events#guild-member-update
type GuildMemberUpdateReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "roles">] Roles: string list
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "nick">] Nick: string option
    [<JsonPropertyName "avatar">] Avatar: string option
    [<JsonPropertyName "banner">] Banner: string option
    [<JsonPropertyName "joined_at">] JoinedAt: DateTime
    [<JsonPropertyName "premium_since">] PremiumSince: DateTime option
    [<JsonPropertyName "deaf">] Deaf: bool option
    [<JsonPropertyName "mute">] Mute: bool option
    [<JsonPropertyName "pending">] Pending: bool option
    [<JsonPropertyName "communication_disabled_until">] CommunicationDisabledUntil: DateTime option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "avatar_decoration_data">] AvatarDecorationData: AvatarDecorationData option
}

// https://discord.com/developers/docs/events/gateway-events#guild-members-chunk
type GuildMembersChunkReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "members">] Members: GuildMember list
    [<JsonPropertyName "chunk_index">] ChunkIndex: int
    [<JsonPropertyName "chunk_count">] ChunkCount: int
    [<JsonPropertyName "not_found">] NotFound: string list option
    [<JsonPropertyName "presences">] Presences: UpdatePresenceSendEvent list option
    [<JsonPropertyName "nonce">] Nonce: string option
}

// https://discord.com/developers/docs/events/gateway-events#guild-role-create
type GuildRoleCreateReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "role">] Role: Role
}

// https://discord.com/developers/docs/events/gateway-events#guild-role-update
type GuildRoleUpdateReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "role">] Role: Role
}

// https://discord.com/developers/docs/events/gateway-events#guild-role-delete
type GuildRoleDeleteReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "role_id">] RoleId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-create
type GuildScheduledEventCreateReceiveEvent = GuildScheduledEvent

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-update
type GuildScheduledEventUpdateReceiveEvent = GuildScheduledEvent

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-delete
type GuildScheduledEventDeleteReceiveEvent = GuildScheduledEvent

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-user-add
type GuildScheduledEventUserAddReceiveEvent = {
    [<JsonPropertyName "guild_scheduled_event_id">] GuildScheduledEventId: string
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "guild_id">] GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-scheduled-event-user-remove
type GuildScheduledEventUserRemoveReceiveEvent = {
    [<JsonPropertyName "guild_scheduled_event_id">] GuildScheduledEventId: string
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "guild_id">] GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-soundboard-sound-create
type GuildSoundboardSoundCreateReceiveEvent = SoundboardSound

// https://discord.com/developers/docs/events/gateway-events#guild-soundboard-sound-update
type GuildSoundboardSoundUpdateReceiveEvent = SoundboardSound

// https://discord.com/developers/docs/events/gateway-events#guild-soundboard-sound-delete
type GuildSoundboardSoundDeleteReceiveEvent = {
    [<JsonPropertyName "sound_id">] SoundId: string
    [<JsonPropertyName "guild_id">] GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#guild-soundboard-sounds-update
type GuildSoundboardSoundsUpdateReceiveEvent = {
    [<JsonPropertyName "soundboard_sounds">] SoundboardSounds: SoundboardSound list
    [<JsonPropertyName "guild_id">] GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#soundboard-sounds
type GuildSoundboardSoundsReceiveEvent = {
    [<JsonPropertyName "soundboard_sounds">] SoundboardSounds: SoundboardSound list
    [<JsonPropertyName "guild_id">] GuildId: string
}

// https://discord.com/developers/docs/events/gateway-events#integration-create
[<JsonConverter(typeof<IntegrationCreateReceiveEventConverter>)>]
type IntegrationCreateReceiveEvent = {
    Integration: GuildIntegration
    ExtraFields: IntegrationCreateReceiveEventExtraFields
}

and IntegrationCreateReceiveEventExtraFields = {
    [<JsonPropertyName "guild_id">] GuildId: string
}

and IntegrationCreateReceiveEventConverter () =
    inherit JsonConverter<IntegrationCreateReceiveEvent> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            Integration = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let integration = Json.serializeF value.Integration
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge integration extraFields)

// https://discord.com/developers/docs/events/gateway-events#integration-update
[<JsonConverter(typeof<IntegrationUpdateReceiveEventConverter>)>]
type IntegrationUpdateReceiveEvent = {
    Integration: GuildIntegration
    ExtraFields: IntegrationUpdateReceiveEventExtraFields
}

and IntegrationUpdateReceiveEventExtraFields = {
    [<JsonPropertyName "guild_id">] GuildId: string
}

and IntegrationUpdateReceiveEventConverter () =
    inherit JsonConverter<IntegrationUpdateReceiveEvent> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            Integration = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let integration = Json.serializeF value.Integration
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge integration extraFields)

// https://discord.com/developers/docs/events/gateway-events#integration-delete
type IntegrationDeleteReceiveEvent = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "application_id">] ApplicationId: string
}

// https://discord.com/developers/docs/events/gateway-events#invite-create
type InviteCreateReceiveEvent = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "code">] Code: string
    [<JsonPropertyName "created_at">] CreatedAt: DateTime
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "inviter">] Inviter: User option
    [<JsonPropertyName "max_age">] MaxAge: int
    [<JsonPropertyName "max_uses">] MaxUses: int
    [<JsonPropertyName "target_type">] TargetType: InviteTargetType option
    [<JsonPropertyName "target_user">] TargetUser: User option
    [<JsonPropertyName "target_application">] TargetApplication: PartialApplication option
    [<JsonPropertyName "temporary">] Temporary: bool
    [<JsonPropertyName "uses">] Uses: int
}

// https://discord.com/developers/docs/events/gateway-events#invite-delete
type InviteDeleteReceiveEvent = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "code">] Code: string
}

[<JsonConverter(typeof<MessageCreateReceiveEventExtraFieldsMentionConverter>)>]
type MessageCreateReceiveEventExtraFieldsMention = {
    User: User
    ExtraFields: MessageCreateReceiveEventExtraFieldsMentionExtraFields
}

and MessageCreateReceiveEventExtraFieldsMentionExtraFields = {
    [<JsonPropertyName "member">] Member: PartialGuildMember option
}

and MessageCreateReceiveEventExtraFieldsMentionConverter () =
    inherit JsonConverter<MessageCreateReceiveEventExtraFieldsMention> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            User = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let user = Json.serializeF value.User
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge user extraFields)

// https://discord.com/developers/docs/events/gateway-events#message-create
[<JsonConverter(typeof<MessageCreateReceiveEventConverter>)>]
type MessageCreateReceiveEvent = {
    Message: Message
    ExtraFields: MessageCreateReceiveEventExtraFields
}

and MessageCreateReceiveEventExtraFields = {
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "member">] Member: PartialGuildMember option
    [<JsonPropertyName "mentions">] Mentions: MessageCreateReceiveEventExtraFieldsMention list
}

and MessageCreateReceiveEventConverter () =
    inherit JsonConverter<MessageCreateReceiveEvent> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            Message = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let message = Json.serializeF value.Message
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge message extraFields)

[<JsonConverter(typeof<MessageUpdateReceiveEventExtraFieldsMentionConverter>)>]
type MessageUpdateReceiveEventExtraFieldsMention = {
    User: User
    ExtraFields: MessageUpdateReceiveEventExtraFieldsMentionExtraFields
}

and MessageUpdateReceiveEventExtraFieldsMentionExtraFields = {
    [<JsonPropertyName "member">] Member: PartialGuildMember option
}

and MessageUpdateReceiveEventExtraFieldsMentionConverter () =
    inherit JsonConverter<MessageUpdateReceiveEventExtraFieldsMention> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            User = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let user = Json.serializeF value.User
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge user extraFields)

// https://discord.com/developers/docs/events/gateway-events#message-update
[<JsonConverter(typeof<MessageUpdateReceiveEventConverter>)>]
type MessageUpdateReceiveEvent = {
    Message: Message
    ExtraFields: MessageUpdateReceiveEventExtraFields
}

and MessageUpdateReceiveEventExtraFields = {
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "member">] Member: PartialGuildMember option
    [<JsonPropertyName "mentions">] Mentions: MessageUpdateReceiveEventExtraFieldsMention list
}

and MessageUpdateReceiveEventConverter () =
    inherit JsonConverter<MessageUpdateReceiveEvent> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            Message = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let message = Json.serializeF value.Message
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge message extraFields)

// https://discord.com/developers/docs/events/gateway-events#message-delete
type MessageDeleteReceiveEvent = {
    [<JsonPropertyName "ids">] Ids: string list
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
}

// https://discord.com/developers/docs/events/gateway-events#message-delete-bulk
type MessageDeleteBulkReceiveEvent = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
}

// https://discord.com/developers/docs/events/gateway-events#message-reaction-add
type MessageReactionAddReceiveEvent = {
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "message_id">] MessageId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "member">] Member: GuildMember option
    [<JsonPropertyName "emoji">] Emoji: PartialEmoji
    [<JsonPropertyName "message_author_id">] MessageAuthorId: string option
    [<JsonPropertyName "burst">] Burst: bool
    [<JsonPropertyName "burst_colors">] BurstColors: string list
    [<JsonPropertyName "type">] Type: ReactionType
}

// https://discord.com/developers/docs/events/gateway-events#message-reaction-remove
type MessageReactionRemoveReceiveEvent = {
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "message_id">] MessageId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "emoji">] Emoji: PartialEmoji
    [<JsonPropertyName "burst">] Burst: bool
    [<JsonPropertyName "type">] Type: ReactionType
}

// https://discord.com/developers/docs/events/gateway-events#message-reaction-remove-all
type MessageReactionRemoveAllReceiveEvent = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "message_id">] MessageId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
}

// https://discord.com/developers/docs/events/gateway-events#message-reaction-remove-emoji
type MessageReactionRemoveEmojiReceiveEvent = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "message_id">] MessageId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "emoji">] Emoji: PartialEmoji
}

// https://discord.com/developers/docs/events/gateway-events#presence-update
type PresenceUpdateReceiveEvent = {
    [<JsonPropertyName "user">] User: PartialUser
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "status">] Status: StatusType option
    [<JsonPropertyName "activities">] Activities: Activity list option
    [<JsonPropertyName "client_status">] ClientStatus: ClientStatus option
}

// https://discord.com/developers/docs/topics/gateway-events#typing-start-typing-start-event-fields
type TypingStartReceiveEvent = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "timestamp">] Timestamp: DateTime
    [<JsonPropertyName "member">] Member: GuildMember
}

// https://discord.com/developers/docs/events/gateway-events#user-update
type UserUpdateReceiveEvent = User

// https://discord.com/developers/docs/events/gateway-events#voice-channel-effect-send
type VoiceChannelEffectSendReceiveEvent = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "emoji">] Emoji: Emoji option
    [<JsonPropertyName "animation_type">] AnimationType: AnimationType option
    [<JsonPropertyName "animation_id">] AnimationId: string option
    [<JsonPropertyName "sound_id">] SoundId: string option
    [<JsonPropertyName "sound_volume">] SoundVolume: double option
}

// https://discord.com/developers/docs/events/gateway-events#voice-state-update
type VoiceStateUpdateReceiveEvent = VoiceState

// https://discord.com/developers/docs/events/gateway-events#voice-server-update
type VoiceServerUpdateReceiveEvent = {
    [<JsonPropertyName "token">] Token: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "endpoint">] Endpoint: string option
}

// https://discord.com/developers/docs/events/gateway-events#webhooks-update
type WebhooksUpdateReceiveEvent = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "channel_id">] ChannelId: string
}

// https://discord.com/developers/docs/events/gateway-events#interaction-create
type InteractionCreateReceiveEvent = Interaction

// https://discord.com/developers/docs/events/gateway-events#stage-instance-create
type StageInstanceCreateReceiveEvent = StageInstance

// https://discord.com/developers/docs/events/gateway-events#stage-instance-update
type StageInstanceUpdateReceiveEvent = StageInstance

// https://discord.com/developers/docs/events/gateway-events#stage-instance-delete
type StageInstanceDeleteReceiveEvent = StageInstance

// https://discord.com/developers/docs/events/gateway-events#subscription-create
type SubscriptionCreateReceiveEvent = Subscription

// https://discord.com/developers/docs/events/gateway-events#subscription-update
type SubscriptionUpdateReceiveEvent = Subscription

// https://discord.com/developers/docs/events/gateway-events#subscription-delete
type SubscriptionDeleteReceiveEvent = Subscription

// https://discord.com/developers/docs/events/gateway-events#message-poll-vote-add
type MessagePollVoteAddReceiveEvent = {
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "message_id">] MessageId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "answer_id">] AnswerId: string
}

// https://discord.com/developers/docs/events/gateway-events#message-poll-vote-remove
type MessagePollVoteRemoveReceiveEvent = {
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "message_id">] MessageId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "answer_id">] AnswerId: string
}
