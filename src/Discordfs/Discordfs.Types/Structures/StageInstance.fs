namespace Discordfs.Types

open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/stage-instance#stage-instance-object-stage-instance-structure
type StageInstance = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "topic">] Topic: string
    [<JsonPropertyName "privacy_level">] PrivacyLevel: PrivacyLevelType
    [<JsonPropertyName "discoverable_enabled">] DiscoverableEnabled: bool
    [<JsonPropertyName "guild_scheduled_event_id">] GuildScheduledEventId: string option
}
