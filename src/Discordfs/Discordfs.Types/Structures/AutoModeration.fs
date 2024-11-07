namespace Discordfs.Types

open System
open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-trigger-metadata
type AutoModerationTriggerMetadata = {
    [<JsonPropertyName "keyword_filter">] KeywordFilter: string list option
    [<JsonPropertyName "regex_patterns">] RegexPatterns: string list option
    [<JsonPropertyName "presets">] Presets: AutoModerationKeywordPresetType option
    [<JsonPropertyName "allow_list">] AllowList: string list option
    [<JsonPropertyName "mention_total_limit">] MentionTotalLimit: int option
    [<JsonPropertyName "mention_raid_protection_enabled">] MentionRaidProtectionEnabled: bool option
}

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object-action-metadata
type AutoModerationActionMetadata = {
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "duration_seconds">] DurationSeconds: int option
    [<JsonPropertyName "custom_message">] CustomMessage: string option
}

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object
type AutoModerationAction = {
    [<JsonPropertyName "type">] Type: AutoModerationActionType
    [<JsonPropertyName "metadata">] Metadata: AutoModerationActionMetadata option
}

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-auto-moderation-rule-structure
type AutoModerationRule = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "creator_id">] CreatorId: string
    [<JsonPropertyName "event_type">] EventType: AutoModerationEventType
    [<JsonPropertyName "trigger_type">] TriggerType: AutoModerationTriggerType
    [<JsonPropertyName "trigger_metadata">] TriggerMetadata: AutoModerationTriggerMetadata
    [<JsonPropertyName "actions">] Actions: AutoModerationAction list
    [<JsonPropertyName "enabled">] nabled: bool
    [<JsonPropertyName "exempt_roles">] ExemptRoles: string list
    [<JsonPropertyName "exempt_channels">] ExemptChannels: string list
}
