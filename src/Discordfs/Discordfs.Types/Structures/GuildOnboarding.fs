namespace Discordfs.Types

open System
open System.Text.Json
open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-prompt-option-structure
type GuildOnboardingPromptOption = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "channel_ids">] ChannelIds: string list
    [<JsonPropertyName "role_ids">] RoleIds: string list
    [<JsonPropertyName "emoji">] Emoji: Emoji option
    [<JsonPropertyName "emoji_id">] EmojiId: string option
    [<JsonPropertyName "emoji_name">] EmojiName: string option
    [<JsonPropertyName "emoji_animated">] EmojiAnimated: bool option
    [<JsonPropertyName "title">] Title: string
    [<JsonPropertyName "description">] Description: string
}

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-onboarding-prompt-structure
type GuildOnboardingPrompt = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: OnboardingPromptType
    [<JsonPropertyName "options">] Options: GuildOnboardingPromptOption list
    [<JsonPropertyName "title">] Title: string
    [<JsonPropertyName "single_select">] SingleSelect: bool
    [<JsonPropertyName "required">] Required: bool
    [<JsonPropertyName "in_onboarding">] InOnboarding: bool
}

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-guild-onboarding-structure
type GuildOnboarding = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "prompts">] Prompts: GuildOnboardingPrompt list
    [<JsonPropertyName "default_channel_ids">] DefaultChannelIds: string list
    [<JsonPropertyName "enabled">] Enabled: bool
    [<JsonPropertyName "mode">] Mode: OnboardingMode
}
