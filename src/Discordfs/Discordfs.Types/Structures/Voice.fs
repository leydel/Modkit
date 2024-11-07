namespace Discordfs.Types

open System
open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/voice#voice-state-object-voice-state-structure
type VoiceState = {
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "user_id">] UserId: string option
    [<JsonPropertyName "member">] Member: GuildMember option
    [<JsonPropertyName "session_id">] SessionId: string
    [<JsonPropertyName "deaf">] Deaf: bool
    [<JsonPropertyName "mute">] Mute: bool
    [<JsonPropertyName "self_deaf">] SelfDeaf: bool
    [<JsonPropertyName "self_mute">] SelfMute: bool
    [<JsonPropertyName "self_stream">] SelfStream: bool option
    [<JsonPropertyName "self_video">] SelfVideo: bool
    [<JsonPropertyName "suppress">] Suppress: bool
    [<JsonPropertyName "request_to_speak_timestamp">] RequestToSpeakTimestamp: DateTime option
}

type PartialVoiceState = {
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "user_id">] UserId: string option
    [<JsonPropertyName "member">] Member: GuildMember option
    [<JsonPropertyName "session_id">] SessionId: string option
    [<JsonPropertyName "deaf">] Deaf: bool option
    [<JsonPropertyName "mute">] Mute: bool option
    [<JsonPropertyName "self_deaf">] SelfDeaf: bool option
    [<JsonPropertyName "self_mute">] SelfMute: bool option
    [<JsonPropertyName "self_stream">] SelfStream: bool option
    [<JsonPropertyName "self_video">] SelfVideo: bool option
    [<JsonPropertyName "suppress">] Suppress: bool option
    [<JsonPropertyName "request_to_speak_timestamp">] RequestToSpeakTimestamp: DateTime option
}

// https://discord.com/developers/docs/resources/voice#voice-region-object-voice-region-structure
type VoiceRegion = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "optimal">] Optimal: bool
    [<JsonPropertyName "deprecated">] Deprecated: bool
    [<JsonPropertyName "custom">] Custom: bool
}
