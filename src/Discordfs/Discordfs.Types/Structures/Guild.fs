namespace Discordfs.Types

open System
open System.Text.Json.Serialization

type WelcomeScreenChannel = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "emoji_id">] EmojiId: string option
    [<JsonPropertyName "emoji_name">] EmojiName: string option
}

type WelcomeScreen = {
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "welcome_channels">] WelcomeChannels: WelcomeScreenChannel list
}

// https://discord.com/developers/docs/resources/guild#guild-object-guild-structure
type Guild = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "icon_hash">] IconHash: string option
    [<JsonPropertyName "splash">] Splash: string option
    [<JsonPropertyName "discovery_splash">] DiscoverySplash: string option
    [<JsonPropertyName "owner">] Owner: bool option
    [<JsonPropertyName "owner_id">] OwnerId: string
    [<JsonPropertyName "permissions">] Permissions: string option
    [<JsonPropertyName "afk_channel_id">] AfkChannelId: string option
    [<JsonPropertyName "afk_timeout">] AfkTimeout: int
    [<JsonPropertyName "widget_enabled">] WidgetEnabled: bool option
    [<JsonPropertyName "widget_channel_id">] WidgetChannelId: string option
    [<JsonPropertyName "verification_level">] VerificationLevel: GuildVerificationLevel
    [<JsonPropertyName "default_message_notifications">] DefaultMessageNotifications: GuildMessageNotificationLevel
    [<JsonPropertyName "explicit_content_filter">] ExplicitContentFilter: GuildExplicitContentFilterLevel
    [<JsonPropertyName "roles">] Roles: Role list
    [<JsonPropertyName "emojis">] Emojis: Emoji list
    [<JsonPropertyName "features">] Features: GuildFeature list
    [<JsonPropertyName "mfa_level">] MfaLevel: GuildMfaLevel
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "system_channel_id">] SystemChannelId: string option
    [<JsonPropertyName "system_channel_flags">] SystemChannelFlags: int
    [<JsonPropertyName "rules_channel_id">] RulesChannelId: string option
    [<JsonPropertyName "max_presences">] MaxPresences: int option
    [<JsonPropertyName "max_members">] MaxMembers: int option
    [<JsonPropertyName "vanity_url_code">] VanityUrlCode: string option
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "banner">] Banner: string option
    [<JsonPropertyName "premium_tier">] PremiumTier: GuildPremiumTier
    [<JsonPropertyName "premium_subscription_count">] PremiumSubscriptionCount: int option
    [<JsonPropertyName "preferred_locale">] PreferredLocale: string
    [<JsonPropertyName "public_updates_channel_id">] PublicUpdatesChannelId: string option
    [<JsonPropertyName "max_video_channel_users">] MaxVideoChannelUsers: int option
    [<JsonPropertyName "max_stage_video_channel_users">] MaxStageVideoChannelUsers: int option
    [<JsonPropertyName "approximate_member_count">] ApproximateMemberCount: int option
    [<JsonPropertyName "approximate_presence_count">] ApproximatePresenceCount: int option
    [<JsonPropertyName "welcome_screen">] WelcomeScreen: WelcomeScreen option
    [<JsonPropertyName "nsfw_level">] NsfwLevel: GuildNsfwLevel
    [<JsonPropertyName "stickers">] Stickers: Sticker list option
    [<JsonPropertyName "premium_progress_bar_enabled">] PremiumProgressBarEnabled: bool
    [<JsonPropertyName "safety_alerts_channel_id">] SafetyAlertsChannelId: string option
}

type PartialGuild = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "icon_hash">] IconHash: string option
    [<JsonPropertyName "splash">] Splash: string option
    [<JsonPropertyName "discovery_splash">] DiscoverySplash: string option
    [<JsonPropertyName "owner">] Owner: bool option
    [<JsonPropertyName "owner_id">] OwnerId: string option
    [<JsonPropertyName "permissions">] Permissions: string option
    [<JsonPropertyName "afk_channel_id">] AfkChannelId: string option
    [<JsonPropertyName "afk_timeout">] AfkTimeout: int option
    [<JsonPropertyName "widget_enabled">] WidgetEnabled: bool option
    [<JsonPropertyName "widget_channel_id">] WidgetChannelId: string option
    [<JsonPropertyName "verification_level">] VerificationLevel: GuildVerificationLevel option
    [<JsonPropertyName "default_message_notifications">] DefaultMessageNotifications: GuildMessageNotificationLevel option
    [<JsonPropertyName "explicit_content_filter">] ExplicitContentFilter: GuildExplicitContentFilterLevel option
    [<JsonPropertyName "roles">] Roles: Role list option
    [<JsonPropertyName "emojis">] Emojis: Emoji list option
    [<JsonPropertyName "features">] Features: GuildFeature list option
    [<JsonPropertyName "mfa_level">] MfaLevel: GuildMfaLevel option
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "system_channel_id">] SystemChannelId: string option
    [<JsonPropertyName "system_channel_flags">] SystemChannelFlags: int option
    [<JsonPropertyName "rules_channel_id">] RulesChannelId: string option
    [<JsonPropertyName "max_presences">] MaxPresences: int option
    [<JsonPropertyName "max_members">] MaxMembers: int option
    [<JsonPropertyName "vanity_url_code">] VanityUrlCode: string option
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "banner">] Banner: string option
    [<JsonPropertyName "premium_tier">] PremiumTier: GuildPremiumTier option
    [<JsonPropertyName "premium_subscription_count">] PremiumSubscriptionCount: int option
    [<JsonPropertyName "preferred_locale">] PreferredLocale: string option
    [<JsonPropertyName "public_updates_channel_id">] PublicUpdatesChannelId: string option
    [<JsonPropertyName "max_video_channel_users">] MaxVideoChannelUsers: int option
    [<JsonPropertyName "max_stage_video_channel_users">] MaxStageVideoChannelUsers: int option
    [<JsonPropertyName "approximate_member_count">] ApproximateMemberCount: int option
    [<JsonPropertyName "approximate_presence_count">] ApproximatePresenceCount: int option
    [<JsonPropertyName "welcome_screen">] WelcomeScreen: WelcomeScreen option
    [<JsonPropertyName "nsfw_level">] NsfwLevel: GuildNsfwLevel option
    [<JsonPropertyName "stickers">] Stickers: Sticker list option
    [<JsonPropertyName "premium_progress_bar_enabled">] PremiumProgressBarEnabled: bool option
    [<JsonPropertyName "safety_alerts_channel_id">] SafetyAlertsChannelId: string option
}

type UnavailableGuild = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "unavailable">] Unavailable: bool
}

// https://discord.com/developers/docs/resources/guild-template#guild-template-object-guild-template-structure
type GuildTemplate = {
    [<JsonPropertyName "code">] Code: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "usage_count">] UsageCount: int
    [<JsonPropertyName "creator_id">] CreatorId: string
    [<JsonPropertyName "creator">] Creator: User
    [<JsonPropertyName "created_at">] CreatedAt: DateTime
    [<JsonPropertyName "updated_at">] UpdatedAt: DateTime
    [<JsonPropertyName "source_guild_id">] SourceGuildId: string
    [<JsonPropertyName "serialized_source_guild">] SerializedSourceGuild: PartialGuild
    [<JsonPropertyName "is_dirty">] IsDirty: bool option
}

// https://discord.com/developers/docs/resources/guild#guild-widget-object-guild-widget-structure
type GuildWidget = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "instant_invite">] InstantInvite: string option
    [<JsonPropertyName "channels">] Channels: PartialChannel list
    [<JsonPropertyName "members">] Members: PartialUser list
    [<JsonPropertyName "presence_count">] PresenceCount: int
}

// https://discord.com/developers/docs/resources/guild#guild-widget-settings-object-guild-widget-settings-structure
type GuildWidgetSettings = {
    [<JsonPropertyName "enabled">] Enabled: bool
    [<JsonPropertyName "channel_id">] ChannelId: string option
}

// https://discord.com/developers/docs/resources/guild#guild-preview-object-guild-preview-structure
type GuildPreview = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "splash">] Splash: string option
    [<JsonPropertyName "discovery_splash">] DiscoverySplash: string option
    [<JsonPropertyName "emojis">] Emojis: Emoji list
    [<JsonPropertyName "features">] Features: GuildFeature list
    [<JsonPropertyName "approximate_member_count">] ApproximateMemberCount: int
    [<JsonPropertyName "approximate_presence_count">] ApproximatePresenceCount: int
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "stickers">] Stickers: Sticker list
}

// https://discord.com/developers/docs/resources/guild#ban-object-ban-structure
type GuildBan = {
    [<JsonPropertyName "reason">] Reason: string option
    [<JsonPropertyName "user">] User: User
}
