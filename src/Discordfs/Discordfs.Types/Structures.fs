namespace Discordfs.Types

open Discordfs.Types.Utils
open System
open System.Collections.Generic
open System.Text.Json
open System.Text.Json.Serialization

#nowarn "49"

type DefaultReaction = {
    [<JsonName "emoji_id">] EmojiId: string option
    [<JsonName "emoji_name">] EmojiName: string option
}

type WelcomeScreenChannel = {
    [<JsonName "channel_id">] ChannelId: string
    [<JsonName "description">] Description: string
    [<JsonName "emoji_id">] EmojiId: string option
    [<JsonName "emoji_name">] EmojiName: string option
}

type WelcomeScreen = {
    [<JsonName "description">] Description: string option
    [<JsonName "welcome_channels">] WelcomeChannels: WelcomeScreenChannel list
}

type CommandInteractionDataOption = {
    [<JsonName "name">] Name: string
    [<JsonName "type">] Type: ApplicationCommandOptionType
    [<JsonName "value">] [<JsonConverter(typeof<CommandInteractionDataOptionValueConverter>)>] Value: CommandInteractionDataOptionValue option
    [<JsonName "options">] Options: CommandInteractionDataOption list option
    [<JsonName "focused">] Focused: bool option
}
with
    static member build(
        Name: string,
        Type: ApplicationCommandOptionType,
        ?Value: CommandInteractionDataOptionValue,
        ?Options: CommandInteractionDataOption list,
        ?Focused: bool
    ) = {
        Name = Name;
        Type = Type;
        Value = Value;
        Options = Options;
        Focused = Focused;
    }

type Attachment = {
    [<JsonName "id">] Id: string
    [<JsonName "filename">] Filename: string
    [<JsonName "description">] Description: string
    [<JsonName "content_type">] ContentType: string option
    [<JsonName "size">] Size: int
    [<JsonName "url">] Url: string
    [<JsonName "proxy_url">] ProxyUrl: string
    [<JsonName "height">] Height: int option
    [<JsonName "width">] Width: int option
    [<JsonName "ephemeral">] Ephemeral: bool option
    [<JsonName "duration_secs">] DurationSecs: float option
    [<JsonName "waveform">] Waveform: string option
    [<JsonName "flags">] Flags: int option
}

type RoleTags = {
    [<JsonName "bot_id">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] BotId: string option
    [<JsonName "integration_id">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] IntegrationId: string option
    [<JsonName "premium_subscriber">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] PremiumSubscriber: unit option
    [<JsonName "subscription_listing_id">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] SubscriptionListingId: string option
    [<JsonName "available_for_purchase">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] AvailableForPurchase: unit option
    [<JsonName "guild_connections">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] GuildConnections: unit option
}

type Role = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "color">] Color: int
    [<JsonName "hoist">] Hoist: bool
    [<JsonName "icon">] Icon: string option
    [<JsonName "unicode_emoji">] UnicodeEmoji: string option
    [<JsonName "position">] Position: int
    [<JsonName "permissions">] Permissions: string
    [<JsonName "managed">] Managed: bool
    [<JsonName "mentionable">] Mentionable: bool
    [<JsonName "tags">] Tags: RoleTags option
    [<JsonName "flags">] Flags: int
}

type Entitlement = {
    [<JsonName "id">] Id: string
    [<JsonName "sku_id">] SkuId: string
    [<JsonName "application_id">] ApplicationId: string
    [<JsonName "user_id">] UserId: string option
    [<JsonName "type">] Type: EntitlementType
    [<JsonName "deleted">] Deleted: bool
    [<JsonName "starts_at">] StartsAt: DateTime option
    [<JsonName "ends_at">] EndsAt: DateTime option
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "consumed">] Consumed: bool option
}

type AvatarDecorationData = {
    [<JsonName "asset">] Asset: string
    [<JsonName "sku_id">] SkuId: string
}

type User = {
    [<JsonName "id">] Id: string
    [<JsonName "username">] Username: string
    [<JsonName "discriminator">] Discriminator: string
    [<JsonName "global_name">] GlobalName: string option
    [<JsonName "avatar">] Avatar: string option
    [<JsonName "bot">] Bot: bool option
    [<JsonName "system">] System: bool option
    [<JsonName "mfa_enabled">] MfaEnabled: bool option
    [<JsonName "banner">] Banner: string option
    [<JsonName "accent_color">] AccentColor: int option
    [<JsonName "locale">] Locale: string option
    [<JsonName "verified">] Verified: bool option
    [<JsonName "email">] Email: string option
    [<JsonName "flags">] Flags: int option
    [<JsonName "premium_type">] PremiumType: UserPremiumType option
    [<JsonName "public_flags">] PublicFlags: int option
    [<JsonName "avatar_decoration_data">] AvatarDecorationData: AvatarDecorationData option
}

type GuildMember = {
    [<JsonName "user">] User: User option
    [<JsonName "nick">] Nick: string option
    [<JsonName "avatar">] Avatar: string option
    [<JsonName "roles">] Roles: string list
    [<JsonName "joined_at">] JoinedAt: DateTime option
    [<JsonName "premium_since">] PremiumSince: DateTime option
    [<JsonName "deaf">] Deaf: bool
    [<JsonName "mute">] Mute: bool
    [<JsonName "flags">] Flags: int
    [<JsonName "pending">] Pending: bool option
    [<JsonName "permissions">] Permissions: string option
    [<JsonName "communication_disabled_until">] CommunicationDisabledUntil: DateTime option
    [<JsonName "avatar_decoration_metadata">] AvatarDecorationData: AvatarDecorationData option
}

type Emoji = {
    [<JsonName "id">] Id: string option
    [<JsonName "name">] Name: string option
    [<JsonName "roles">] Roles: string list option
    [<JsonName "user">] User: User option
    [<JsonName "require_colons">] RequireColons: bool option
    [<JsonName "managed">] Managed: bool option
    [<JsonName "animated">] Animated: bool option
    [<JsonName "available">] Available: bool option
}

// https://discord.com/developers/docs/resources/sticker#sticker-object-sticker-structure
type Sticker = {
    [<JsonName "id">] Id: string
    [<JsonName "pack_id">] PackId: string option
    [<JsonName "name">] Name: string
    [<JsonName "description">] Description: string option
    [<JsonName "tags">] Tags: string
    [<JsonName "type">] Type: StickerType
    [<JsonName "format_type">] FormatType: StickerFormatType
    [<JsonName "available">] Available: bool option
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "user">] User: User option
    [<JsonName "sort_value">] SortValue: int option
}

// https://discord.com/developers/docs/resources/sticker#sticker-item-object-sticker-item-structure
type StickerItem = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "format_type">] FormatType: StickerFormatType
}

// https://discord.com/developers/docs/resources/sticker#sticker-pack-object
type StickerPack = {
    [<JsonName "id">] Id: string
    [<JsonName "stickers">] Stickers: Sticker list
    [<JsonName "name">] Name: string
    [<JsonName "sku_id">] SkuId: string
    [<JsonName "cover_sticker_id">] CoverStickerId: string option
    [<JsonName "description">] Description: string
    [<JsonName "banner_asset_id">] BannerAssetId: string option
}

// https://discord.com/developers/docs/resources/guild#guild-object-guild-structure
type Guild = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "icon">] Icon: string option
    [<JsonName "icon_hash">] IconHash: string option
    [<JsonName "splash">] Splash: string option
    [<JsonName "discovery_splash">] DiscoverySplash: string option
    [<JsonName "owner">] Owner: bool option
    [<JsonName "owner_id">] OwnerId: string
    [<JsonName "permissions">] Permissions: string option
    [<JsonName "afk_channel_id">] AfkChannelId: string option
    [<JsonName "afk_timeout">] AfkTimeout: int
    [<JsonName "widget_enabled">] WidgetEnabled: bool option
    [<JsonName "widget_channel_id">] WidgetChannelId: string option
    [<JsonName "verification_level">] VerificationLevel: GuildVerificationLevel
    [<JsonName "default_message_notifications">] DefaultMessageNotifications: GuildMessageNotificationLevel
    [<JsonName "explicit_content_filter">] ExplicitContentFilter: GuildExplicitContentFilterLevel
    [<JsonName "roles">] Roles: Role list
    [<JsonName "emojis">] Emojis: Emoji list
    [<JsonName "features">] [<JsonConverter(typeof<GuildFeatureConverter>)>] Features: GuildFeature list // TODO: Test if this transform works on list
    [<JsonName "mfa_level">] MfaLevel: GuildMfaLevel
    [<JsonName "application_id">] ApplicationId: string option
    [<JsonName "system_channel_id">] SystemChannelId: string option
    [<JsonName "system_channel_flags">] SystemChannelFlags: int
    [<JsonName "rules_channel_id">] RulesChannelId: string option
    [<JsonName "max_presences">] MaxPresences: int option
    [<JsonName "max_members">] MaxMembers: int option
    [<JsonName "vanity_url_code">] VanityUrlCode: string option
    [<JsonName "description">] Description: string option
    [<JsonName "banner">] Banner: string option
    [<JsonName "premium_tier">] PremiumTier: GuildPremiumTier
    [<JsonName "premium_subscription_count">] PremiumSubscriptionCount: int option
    [<JsonName "preferred_locale">] PreferredLocale: string
    [<JsonName "public_updates_channel_id">] PublicUpdatesChannelId: string option
    [<JsonName "max_video_channel_users">] MaxVideoChannelUsers: int option
    [<JsonName "max_stage_video_channel_users">] MaxStageVideoChannelUsers: int option
    [<JsonName "approximate_member_count">] ApproximateMemberCount: int option
    [<JsonName "approximate_presence_count">] ApproximatePresenceCount: int option
    [<JsonName "welcome_screen">] WelcomeScreen: WelcomeScreen option
    [<JsonName "nsfw_level">] NsfwLevel: GuildNsfwLevel
    [<JsonName "stickers">] Stickers: Sticker list option
    [<JsonName "premium_progress_bar_enabled">] PremiumProgressBarEnabled: bool
    [<JsonName "safety_alerts_channel_id">] SafetyAlertsChannelId: string option
}

type UnavailableGuild = {
    [<JsonName "id">] Id: string
    [<JsonName "unavailable">] Unavailable: bool
}

// https://discord.com/developers/docs/resources/guild#guild-preview-object-guild-preview-structure
type GuildPreview = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "icon">] Icon: string option
    [<JsonName "splash">] Splash: string option
    [<JsonName "discovery_splash">] DiscoverySplash: string option
    [<JsonName "emojis">] Emojis: Emoji list
    [<JsonName "features">] [<JsonConverter(typeof<GuildFeatureConverter>)>] Features: GuildFeature list // TODO: Test if this transform works on list
    [<JsonName "approximate_member_count">] ApproximateMemberCount: int
    [<JsonName "approximate_presence_count">] ApproximatePresenceCount: int
    [<JsonName "description">] Description: string option
    [<JsonName "stickers">] Stickers: Sticker list
}

// https://discord.com/developers/docs/resources/guild#guild-widget-settings-object-guild-widget-settings-structure
type GuildWidgetSettings = {
    [<JsonName "enabled">] Enabled: bool
    [<JsonName "channel_id">] ChannelId: string option
}

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-prompt-option-structure
type GuildOnboardingPromptOption = {
    [<JsonName "id">] Id: string
    [<JsonName "channel_ids">] ChannelIds: string list
    [<JsonName "role_ids">] RoleIds: string list
    [<JsonName "emoji">] Emoji: Emoji option
    [<JsonName "emoji_id">] EmojiId: string option
    [<JsonName "emoji_name">] EmojiName: string option
    [<JsonName "emoji_animated">] EmojiAnimated: bool option
    [<JsonName "title">] Title: string
    [<JsonName "description">] Description: string
}

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-onboarding-prompt-structure
type GuildOnboardingPrompt = {
    [<JsonName "id">] Id: string
    [<JsonName "type">] Type: OnboardingPromptType
    [<JsonName "options">] Options: GuildOnboardingPromptOption list
    [<JsonName "title">] Title: string
    [<JsonName "single_select">] SingleSelect: bool
    [<JsonName "required">] Required: bool
    [<JsonName "in_onboarding">] InOnboarding: bool
}

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-guild-onboarding-structure
type GuildOnboarding = {
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "prompts">] Prompts: GuildOnboardingPrompt list
    [<JsonName "default_channel_ids">] DefaultChannelIds: string list
    [<JsonName "enabled">] Enabled: bool
    [<JsonName "mode">] Mode: OnboardingMode
}

type ChannelMention = {
    [<JsonName "id">] Id: string
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "type">] Type: ChannelType
    [<JsonName "name">] Name: string
}

type PermissionOverwrite = {
    [<JsonName "id">] Id: string
    [<JsonName "type">] Type: PermissionOverwriteType
    [<JsonName "allow">] Allow: string
    [<JsonName "deny">] Deny: string
}

type ThreadMetadata = {
    [<JsonName "archived">] Archived: bool
    [<JsonName "auto_archive_duration">] AutoArchiveDuration: int
    [<JsonName "archive_timestamp">] ArchiveTimestamp: DateTime
    [<JsonName "locked">] Locked: bool
    [<JsonName "invitable">] Invitable: bool option
    [<JsonName "create_timestamp">] CreateTimestamp: DateTime option
}

type ThreadMember = {
    [<JsonName "id">] Id: string option
    [<JsonName "user_id">] UserId: string option
    [<JsonName "join_timestamp">] JoinTimestamp: DateTime
    [<JsonName "flags">] Flags: int
    [<JsonName "member">] Member: GuildMember option
}

type ChannelTag = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "moderated">] Moderated: bool
    [<JsonName "emoji_id">] EmojiId: string option
    [<JsonName "emoji_name">] EmojiName: string option
}

type EmbedFooter = {
    [<JsonName "text">] Text: string
    [<JsonName "icon_url">] IconUrl: string option
    [<JsonName "proxy_icon_url">] ProxyIconUrl: string option
}
with
    static member build(
        Text: string,
        ?IconUrl: string,
        ?ProxyIconUrl: string
    ) = {
        Text = Text;
        IconUrl = IconUrl;
        ProxyIconUrl = ProxyIconUrl;
    }

type EmbedImage = {
    [<JsonName "url">] Url: string
    [<JsonName "proxy_url">] ProxyUrl: string option
    [<JsonName "height">] Height: int option
    [<JsonName "width">] Width: int option
}
with
    static member build(
        Url: string,
        ?ProxyUrl: string,
        ?Height: int,
        ?Width: int
    ) = {
        Url = Url;
        ProxyUrl = ProxyUrl;
        Height = Height;
        Width = Width;
    }

type EmbedThumbnail = {
    [<JsonName "url">] Url: string
    [<JsonName "proxy_url">] ProxyUrl: string option
    [<JsonName "height">] Height: int option
    [<JsonName "width">] Width: int option
}
with
    static member build(
        Url: string,
        ?ProxyUrl: string,
        ?Height: int,
        ?Width: int
    ) = {
        Url = Url;
        ProxyUrl = ProxyUrl;
        Height = Height;
        Width = Width;
    }

type EmbedVideo = {
    [<JsonName "url">] Url: string option
    [<JsonName "proxy_url">] ProxyUrl: string option
    [<JsonName "height">] Height: int option
    [<JsonName "width">] Width: int option
}
with
    static member build(
        ?Url: string,
        ?ProxyUrl: string,
        ?Height: int,
        ?Width: int
    ) = {
        Url = Url;
        ProxyUrl = ProxyUrl;
        Height = Height;
        Width = Width;
    }

type EmbedProvider = {
    [<JsonName "name">] Name: string option
    [<JsonName "url">] Url: string option
}
with
    static member build(
        ?Name: string,
        ?Url: string
    ) = {
        Name = Name;
        Url = Url;
    }

type EmbedAuthor = {
    [<JsonName "name">] Name: string
    [<JsonName "url">] Url: string option
    [<JsonName "icon_url">] IconUrl: string option
    [<JsonName "proxy_icon_url">] ProxyIconUrl: string option
}
with
    static member build(
        Name: string,
        ?Url: string,
        ?IconUrl: string,
        ?ProxyIconUrl: string
    ) = {
        Name = Name;
        Url = Url;
        IconUrl = IconUrl;
        ProxyIconUrl = ProxyIconUrl;
    }

type EmbedField = {
    [<JsonName "name">] Name: string
    [<JsonName "value">] Value: string
    [<JsonName "inline">] Inline: bool option
}
with
    static member build(
        Name: string,
        Value: string,
        ?Inline: bool
    ) = {
        Name = Name;
        Value = Value;
        Inline = Inline;
    }

type Embed = {
    [<JsonName "title">] Title: string option
    [<JsonName "type">] Type: string option
    [<JsonName "description">] Description: string option
    [<JsonName "url">] Url: string option
    [<JsonName "timestamp">] Timestamp: DateTime option
    [<JsonName "color">] Color: int option
    [<JsonName "footer">] Footer: EmbedFooter option
    [<JsonName "image">] Image: EmbedImage option
    [<JsonName "thumbnail">] Thumbnail: EmbedThumbnail option
    [<JsonName "video">] Video: EmbedVideo option
    [<JsonName "provider">] Provider: EmbedProvider option
    [<JsonName "author">] Author: EmbedAuthor option
    [<JsonName "fields">] Fields: EmbedField list option
}
with
    static member build(
        ?Title: string,
        ?Type: string,
        ?Description: string,
        ?Url: string,
        ?Timestamp: DateTime,
        ?Color: int,
        ?Footer: EmbedFooter,
        ?Image: EmbedImage,
        ?Thumbnail: EmbedThumbnail,
        ?Video: EmbedVideo,
        ?Provider: EmbedProvider,
        ?Author: EmbedAuthor,
        ?Fields: EmbedField list
    ) = {
        Title = Title;
        Type = Type;
        Description = Description;
        Url = Url;
        Timestamp = Timestamp;
        Color = Color;
        Footer = Footer;
        Image = Image;
        Thumbnail = Thumbnail;
        Video = Video;
        Provider = Provider;
        Author = Author;
        Fields = Fields;
    }

type ReactionCountDetails = {
    [<JsonName "burst">] Burst: int
    [<JsonName "normal">] Normal: int
}

type Reaction = {
    [<JsonName "count">] Count: int
    [<JsonName "count_details">] CountDetails: ReactionCountDetails
    [<JsonName "me">] Me: bool
    [<JsonName "me_burst">] MeBurst: bool
    [<JsonName "emoji">] Emoji: Emoji
    [<JsonName "burst_colors">] BurstColors: int list
}

type MessageActivity = {
    [<JsonName "type">] Type: MessageActivityType
    [<JsonName "party_id">] PartyId: string option
}

type OAuth2InstallParams = {
    [<JsonName "scopes">] Scopes: string list
    [<JsonName "permissions">] Permissions: string
}

type ApplicationIntegrationTypeConfiguration = {
    [<JsonName "oauth2_install_params">] Oauth2InstallParams: OAuth2InstallParams option
}

type TeamMember = {
    [<JsonName "membership_state">] MembershipState: TeamMembershipState
    [<JsonName "team_id">] TeamId: string
    [<JsonName "user">] User: User
    [<JsonName "role">] Role: string
}

type Team = {
    [<JsonName "icon">] Icon: string option
    [<JsonName "id">] Id: string
    [<JsonName "members">] Members: TeamMember list
    [<JsonName "name">] Name: string
    [<JsonName "owner_user_id">] OwnerUserId: string
}

type Application = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "icon">] Icon: string option
    [<JsonName "description">] Description: string
    [<JsonName "rpc_origins">] RpcOrigins: string list option
    [<JsonName "bot_public">] BotPublic: bool
    [<JsonName "bot_require_code_grant">] BotRequireCodeGrant: bool
    [<JsonName "bot">] Bot: User option
    [<JsonName "terms_of_Service_url">] TermsOfServiceUrl: string option
    [<JsonName "privacy_policy_url">] PrivacyPolicyUrl: string option
    [<JsonName "owner">] Owner: User option
    [<JsonName "verify_key">] VerifyKey: string
    [<JsonName "team">] Team: Team option
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "guild">] Guild: Guild option
    [<JsonName "primary_sku_id">] PrimarySkuId: string option
    [<JsonName "slug">] Slug: string option
    [<JsonName "cover_image">] CoverImage: string option
    [<JsonName "flags">] Flags: int option
    [<JsonName "approximate_guild_count">] ApproximateGuildCount: int option
    [<JsonName "redirect_uris">] RedirectUris: string list option
    [<JsonName "interactions_endpoint_url">] InteractionsEndpointUrl: string option
    [<JsonName "role_connections_verification_url">] RoleConnectionsVerificationUrl: string option
    [<JsonName "tags">] Tags: string list option
    [<JsonName "install_params">] InstallParams: OAuth2InstallParams option
    [<JsonName "integration_types_config">] IntegrationTypesConfig: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option
    [<JsonName "custom_install_url">] CustomInstallUrl: string option
}

type PartialApplication = {
    [<JsonName "id">] Id: string
    [<JsonName "flags">] Flags: int option
}

type MessageReference = {
    [<JsonName "message_id">] MessageId: string option
    [<JsonName "channel_id">] ChannelId: string option
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "fail_if_not_exists">] FailIfNotExists: bool option
}

type MessageInteractionMetadata = {
    [<JsonName "id">] Id: string
    [<JsonName "type">] Type: InteractionType
    [<JsonName "user">] User: User
    [<JsonName "authorizing_integration_owners">] AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>
    [<JsonName "original_response_message_id">] OriginalResponseMessage: string option
    [<JsonName "interacted_message_id">] InteractedMessageId: string option
    [<JsonName "triggering_interaction_metadata">] TriggeringInteractionMetadata: MessageInteractionMetadata option
}

type MessageInteraction = {
    [<JsonName "id">] Id: string
    [<JsonName "type">] Type: InteractionType
    [<JsonName "name">] Name: string
    [<JsonName "user">] User: User
    [<JsonName "member">] Member: GuildMember option
}

type RoleSubscriptionData = {
    [<JsonName "role_subscription_listing_id">] RoleSubscriptionListingId: string
    [<JsonName "tier_name">] TierName: string
    [<JsonName "total_months_subscribed">] TotalMonthsSubscribed: int
    [<JsonName "is_renewal">] IsRenewal: bool
}

type PollMedia = {
    [<JsonName "text">] Text: string option
    [<JsonName "emoji">] Emoji: Emoji option
}

type PollAnswer = {
    [<JsonName "answer_id">] AnswerId: int
    [<JsonName "poll_media">] PollMedia: PollMedia
}

type PollAnswerCount = {
    [<JsonName "id">] Id: string
    [<JsonName "count">] Count: int
    [<JsonName "me_voted">] MeVoted: bool
}

type PollResults = {
    [<JsonName "is_finalized">] IsFinalized: bool
    [<JsonName "answer_counts">] AnswerCounts: PollAnswerCount list
}

type Poll = {
    [<JsonName "question">] Question: PollMedia
    [<JsonName "answers">] Answers: PollAnswer list
    [<JsonName "expiry">] Expiry: DateTime option
    [<JsonName "allow_multiselect">] AllowMultiselect: bool
    [<JsonName "layout_type">] LayoutType: PollLayoutType
    [<JsonName "results">] Results: PollResults option
}

type MessageCall = {
    [<JsonName "participants">] Participants: string list
    [<JsonName "ended_timestamp">] EndedTimestamp: DateTime option
}

type SelectMenuOption = {
    [<JsonName "label">] Label: string
    [<JsonName "value">] Value: string
    [<JsonName "description">] Description: string option
    [<JsonName "emoji">] Emoji: Emoji option
    [<JsonName "default">] Default: bool option
}

type SelectMenuDefaultValue = {
    [<JsonName "id">] Id: string
    [<JsonName "type">] Type: string
}

type ActionRowComponent = {
    [<JsonName "type">] Type: ComponentType
    [<JsonName "components">] [<JsonConverter(typeof<ComponentConverter>)>] Components: Component list
}

and ButtonComponent = {
    [<JsonName "type">] Type: ComponentType
    [<JsonName "style">] Style: ButtonStyle
    [<JsonName "label">] Label: string
    [<JsonName "emoji">] Emoji: Emoji option
    [<JsonName "custom_id">] CustomId: string option
    [<JsonName "url">] Url: string option
    [<JsonName "disabled">] Disabled: bool option
}

and SelectMenuComponent = {
    [<JsonName "type">] Type: ComponentType
    [<JsonName "custom_id">] CustomId: string
    [<JsonName "options">] Options: SelectMenuOption list option
    [<JsonName "channel_types">] ChannelTypes: ChannelType list option
    [<JsonName "placeholder">] Placeholder: string option
    [<JsonName "default_values">] DefaultValues: SelectMenuDefaultValue option
    [<JsonName "min_values">] MinValues: int option
    [<JsonName "max_values">] MaxValues: int option
    [<JsonName "disabled">] Disabled: bool option
}

and TextInputComponent = {
    [<JsonName "type">] Type: ComponentType
    [<JsonName "custom_id">] CustomId: string
    [<JsonName "style">] Style: TextInputStyle
    [<JsonName "label">] Label: string
    [<JsonName "min_length">] MinLength: int option
    [<JsonName "max_length">] MaxLength: int option
    [<JsonName "required">] Required: bool option
    [<JsonName "value">] Value: string option
    [<JsonName "placeholder">] Placeholder: string option
}

and Component =
    | ActionRow of ActionRowComponent
    | Button of ButtonComponent
    | SelectMenu of SelectMenuComponent
    | TextInput of TextInputComponent

and ComponentConverter () =
    inherit JsonConverter<Component> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) = 
            raise <| NotImplementedException()
            
        override _.Write (writer: Utf8JsonWriter, value: Component, options: JsonSerializerOptions) = 
            raise <| NotImplementedException()

    // TODO: Implement (Consider making a single `Component` with all properties and try to convert to specific in code elsewhere?)

type Channel = {
    [<JsonName "id">] Id: string
    [<JsonName "type">] Type: ChannelType
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "position">] Position: int option
    [<JsonName "permission_overwrites">] PermissionOverwrites: PermissionOverwrite list option
    [<JsonName "name">] Name: string option
    [<JsonName "topic">] Topic: string option
    [<JsonName "nsfw">] Nsfw: bool option
    [<JsonName "last_message_id">] LastMessageId: string option
    [<JsonName "bitrate">] Bitrate: int option
    [<JsonName "user_limit">] UserLimit: int option
    [<JsonName "rate_limit_per_user">] RateLimitPerUser: int option
    [<JsonName "recipients">] Recipients: User list option
    [<JsonName "icon">] Icon: string option
    [<JsonName "owner_id">] OwnerId: string option
    [<JsonName "application_id">] ApplicationId: string option
    [<JsonName "managed">] Managed: bool option
    [<JsonName "parent_id">] ParentId: string option
    [<JsonName "last_pin_timestamp">] LastPinTimestamp: DateTime option
    [<JsonName "rtc_region">] RtcRegion: string option
    [<JsonName "video_quality_mode">] VideoQualityMode: VideoQualityMode option
    [<JsonName "message_count">] MessageCount: int option
    [<JsonName "member_count">] MemberCount: int option
    [<JsonName "thread_metadata">] ThreadMetadata: ThreadMetadata option
    [<JsonName "member">] Member: ThreadMember option
    [<JsonName "default_auto_archive_duration">] DefaultAutoArchiveDuration: AutoArchiveDurationType option
    [<JsonName "permissions">] Permissions: string option
    [<JsonName "flags">] Flags: int option
    [<JsonName "total_messages_sent">] TotalMessagesSent: int option
    [<JsonName "available_tags">] AvailableTags: ChannelTag list option
    [<JsonName "applied_tags">] AppliedTags: int list option
    [<JsonName "default_reaction_emoji">] DefaultReactionEmoji: DefaultReaction option
    [<JsonName "default_thread_rate_limit_per_user">] DefaultThreadRateLimitPerUser: int option
    [<JsonName "default_sort_order">] DefaultSortOrder: ChannelSortOrder option
    [<JsonName "default_forum_layout">] DefaultForumLayout: ChannelForumLayout option
}

// https://discord.com/developers/docs/resources/guild#guild-widget-object-guild-widget-structure
type GuildWidget = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "instant_invite">] InstantInvite: string option
    [<JsonName "channels">] Channels: Channel list
    [<JsonName "members">] Members: User list
    [<JsonName "presence_count">] PresenceCount: int
}

type ResolvedData = {
    [<JsonName "users">] Users: IDictionary<string, User> option
    [<JsonName "members">] Members: IDictionary<string, GuildMember> option
    [<JsonName "roles">] Roles: IDictionary<string, Role> option
    [<JsonName "channels">] Channels: IDictionary<string, Channel> option
    [<JsonName "messages">] Messages: IDictionary<string, Message> option
    [<JsonName "attachments">] Attachments: IDictionary<string, Attachment> option
}

and Message = {
    [<JsonName "id">] Id: string
    [<JsonName "channel_id">] ChannelId: string
    [<JsonName "author">] Author: User
    [<JsonName "content">] Content: string
    [<JsonName "timestamp">] Timestamp: DateTime
    [<JsonName "edited_timestamp">] EditedTimestamp: DateTime option
    [<JsonName "tts">] Tts: bool
    [<JsonName "mention_everyone">] MentionEveryone: bool
    [<JsonName "mentions">] Mentions: User list
    [<JsonName "mention_roles">] MentionRoles: string list
    [<JsonName "mention_channels">] MentionChannels: ChannelMention list
    [<JsonName "attachments">] Attachments: Attachment list
    [<JsonName "embeds">] Embeds: Embed list
    [<JsonName "reactions">] Reactions: Reaction list
    [<JsonName "nonce">] [<JsonConverter(typeof<MessageNonceConverter>)>] Nonce: MessageNonce option
    [<JsonName "pinned">] Pinned: bool
    [<JsonName "webhook_id">] WebhookId: string option
    [<JsonName "type">] Type: MessageType
    [<JsonName "activity">] Activity: MessageActivity option
    [<JsonName "application">] Application: Application option
    [<JsonName "message_reference">] MessageReference: MessageReference option
    [<JsonName "flags">] Flags: int
    [<JsonName "referenced_message">] ReferencedMessage: Message option
    [<JsonName "interaction_metadata">] InteractionMetadata: MessageInteractionMetadata option
    [<JsonName "interaction">] Interaction: MessageInteraction option
    [<JsonName "thread">] Thread: Channel option
    [<JsonName "components">] Components: Component list option
    [<JsonName "sticker_items">] StickerItems: Sticker list option
    [<JsonName "position">] Position: int option
    [<JsonName "role_subscription_data">] RoleSubscriptionData: RoleSubscriptionData option
    [<JsonName "resolved">] Resolved: ResolvedData option
    [<JsonName "poll">] Poll: Poll option
    [<JsonName "call">] Call: MessageCall option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-entity-metadata
type EntityMetadata = {
    [<JsonName "location">] Location: string option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-nweekday-structure
type RecurrenceRuleNWeekday = {
    [<JsonName "n">] N: int
    [<JsonName "day">] Day: RecurrenceRuleWeekdayType
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object
type RecurrenceRule = {
    Start: string
    End: string option
    Frequency: RecurrenceRuleFrequencyType
    Interval: int
    ByWeekday: RecurrenceRuleWeekdayType list option
    ByWeekend: RecurrenceRuleNWeekday list option
    ByMonth: RecurrenceRuleMonthType list option
    ByMonthDay: int list option
    ByYearDay: int list option
    Count: int option

}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-structure
type GuildScheduledEvent = {
    [<JsonName "id">] Id: string
    [<JsonName "id">] GuildId: string
    [<JsonName "id">] ChannelId: string option
    [<JsonName "id">] CreatorId: string option
    [<JsonName "id">] Name: string
    [<JsonName "id">] Description: string option
    [<JsonName "id">] ScheduledStartTime: DateTime option
    [<JsonName "id">] ScheduledEndTime: DateTime option
    [<JsonName "id">] PrivacyLevel: PrivacyLevelType
    [<JsonName "id">] EventStatus: EventStatusType
    [<JsonName "id">] EntityType: ScheduledEntityType
    [<JsonName "id">] EntityId: string option
    [<JsonName "id">] EntityMetadata: EntityMetadata option
    [<JsonName "id">] Creator: User option
    [<JsonName "id">] UserCount: int option
    [<JsonName "id">] Image: string option
    [<JsonName "id">] RecurrenceRule: RecurrenceRule option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-user-object-guild-scheduled-event-user-structure
type GuildScheduledEventUser = {
    [<JsonName "guild_scheduled_event_id">] GuildScheduledEventId: string
    [<JsonName "user">] User: User
    [<JsonName "member">] Member: GuildMember option
}

// https://discord.com/developers/docs/resources/guild-template#guild-template-object-guild-template-structure
type GuildTemplate = {
    [<JsonName "code">] Code: string
    [<JsonName "name">] Name: string
    [<JsonName "description">] Description: string option
    [<JsonName "usage_count">] UsageCount: int
    [<JsonName "creator_id">] CreatorId: string
    [<JsonName "creator">] Creator: User
    [<JsonName "created_at">] CreatedAt: DateTime
    [<JsonName "updated_at">] UpdatedAt: DateTime
    [<JsonName "source_guild_id">] SourceGuildId: string
    [<JsonName "serialized_source_guild">] SerializedSourceGuild: Guild
    [<JsonName "is_dirty">] IsDirty: bool option
}

type Invite = {
    [<JsonName "type">] Type: InviteType
    [<JsonName "code">] Code: string
    [<JsonName "guild">] Guild: Guild option
    [<JsonName "channel">] Channel: Channel option
    [<JsonName "inviter">] Inviter: User option
    [<JsonName "target_type">] TargetType: InviteTargetType option
    [<JsonName "target_user">] TargetUser: User option
    [<JsonName "target_application">] TargetApplication: Application option
    [<JsonName "approximate_presence_count">] ApproximatePresenceCount: int option
    [<JsonName "approximate_member_count">] ApproximateMemberCount: int option
    [<JsonName "expires_at">] ExpiresAt: DateTime
    [<JsonName "guild_scheduled_event">] GuildScheduledEvent: GuildScheduledEvent option
}

type InviteMetadata = {
    [<JsonName "uses">] Uses: int
    [<JsonName "max_uses">] MaxUses: int
    [<JsonName "max_age">] MaxAge: int
    [<JsonName "temporary">] Temporary: bool
    [<JsonName "created_at">] CreatedAt: DateTime
}

// GetChannelInvite returns a list Invite objects with InviteMetadata added. To make this easy to implement here, I've
// just created this type to combine the two. If changes are made to either Invite or InviteMetadata, they should be
// duplicated here. A proper fix for this is probably possible, but implementing this way means the return type is more
// accurate for now making it a lower priority fix.

// TODO: Make below type by combining `Invite` and `InviteMetadata` somehow without code duplication

type InviteWithMetadata = {
    [<JsonName "type">] Type: InviteType
    [<JsonName "code">] Code: string
    [<JsonName "guild">] Guild: Guild option
    [<JsonName "channel">] Channel: Channel option
    [<JsonName "inviter">] Inviter: User option
    [<JsonName "target_type">] TargetType: InviteTargetType option
    [<JsonName "target_user">] TargetUser: User option
    [<JsonName "target_application">] TargetApplication: Application option
    [<JsonName "approximate_presence_count">] ApproximatePresenceCount: int option
    [<JsonName "approximate_member_count">] ApproximateMemberCount: int option
    [<JsonName "expires_at">] ExpiresAt: DateTime
    [<JsonName "guild_scheduled_event">] GuildScheduledEvent: GuildScheduledEvent option
    [<JsonName "uses">] Uses: int
    [<JsonName "max_uses">] MaxUses: int
    [<JsonName "max_age">] MaxAge: int
    [<JsonName "temporary">] Temporary: bool
    [<JsonName "created_at">] CreatedAt: DateTime
}

type InteractionData = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "type">] Type: ApplicationCommandType
    [<JsonName "resolved">] Resolved: ResolvedData option
    [<JsonName "options">] Options: CommandInteractionDataOption list option
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "target_it">] TargetId: string option
}
with
    static member build(
        Id: string,
        Name: string,
        Type: ApplicationCommandType,
        ?Resolved: ResolvedData,
        ?Options: CommandInteractionDataOption list,
        ?GuildId: string,
        ?TargetId: string
    ) = {
        Id = Id;
        Name = Name;
        Type = Type;
        Resolved = Resolved;
        Options = Options;
        GuildId = GuildId;
        TargetId = TargetId;
    }

type Interaction = {
    [<JsonName "id">] Id: string
    [<JsonName "application_id">] ApplicationId: string
    [<JsonName "type">] Type: InteractionType
    [<JsonName "data">] Data: InteractionData option
    [<JsonName "guild">] Guild: Guild option
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "channel">] Channel: Channel option
    [<JsonName "channel_id">] ChannelId: string option
    [<JsonName "member">] Member: GuildMember option
    [<JsonName "user">] User: User option
    [<JsonName "token">] Token: string
    [<JsonName "version">] Version: int
    [<JsonName "message">] Message: Message option
    [<JsonName "app_permissions">] AppPermissions: string
    [<JsonName "locale">] Locale: string option
    [<JsonName "guild_locale">] GuildLocale: string option
    [<JsonName "entitlements">] Entitlements: Entitlement list
    [<JsonName "authorizing_integration_owners">] AuthorizingIntegrationOwners: IDictionary<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>
    [<JsonName "context">] Context: InteractionContextType option
}
with
    static member build(
        Id: string,
        ApplicationId: string,
        Type: InteractionType,
        Token: string,
        Version: int,
        AppPermissions: string,
        Entitlements: Entitlement list,
        AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>,
        ?Data: InteractionData,
        ?Guild: Guild,
        ?GuildId: string,
        ?Channel: Channel,
        ?ChannelId: string,
        ?Member: GuildMember,
        ?User: User,
        ?Message: Message,
        ?Locale: string,
        ?GuildLocale: string,
        ?Context: InteractionContextType
    ) = {
        Id = Id;
        ApplicationId = ApplicationId;
        Type = Type;
        Data = Data;
        Guild = Guild;
        GuildId = GuildId;
        Channel = Channel;
        ChannelId = ChannelId;
        Member = Member;
        User = User;
        Token = Token;
        Version = Version;
        Message = Message;
        AppPermissions = AppPermissions;
        Locale = Locale;
        GuildLocale = GuildLocale;
        Entitlements = Entitlements;
        AuthorizingIntegrationOwners = AuthorizingIntegrationOwners;
        Context = Context;
    }

type AllowedMentions = {
    [<JsonName "parse">] [<JsonConverter(typeof<AllowedMentionsParseTypeConverter>)>] Parse: AllowedMentionsParseType list
    [<JsonName "roles">] Roles: string list option
    [<JsonName "users">] Users: string list option
    [<JsonName "replied_user">] RepliedUser: bool option
}
with
    static member build(
        Parse: AllowedMentionsParseType list,
        ?Roles: string list,
        ?Users: string list,
        ?RepliedUser: bool
    ) = {
        Parse = Parse;
        Roles = Roles;
        Users = Users;
        RepliedUser = RepliedUser;
    }

type ApplicationCommandOptionChoice = {
    [<JsonName "name">] Name: string
    [<JsonName "name_localizations">] NameLocalizations: Dictionary<string, string> option
    [<JsonName "value">] [<JsonConverter(typeof<ApplicationCommandOptionChoiceValueConverter>)>] Value: ApplicationCommandOptionChoiceValue
}

type ApplicationCommandOption = {
    [<JsonName "type">] Type: ApplicationCommandOptionType
    [<JsonName "name">] Name: string
    [<JsonName "name_localizations">] NameLocalizations: Dictionary<string, string> option
    [<JsonName "description">] Description: string
    [<JsonName "description_localizations">] DescriptionLocalizations: Dictionary<string, string> option
    [<JsonName "required">] Required: bool option
    [<JsonName "choices">] Choices: ApplicationCommandOptionChoice list option
    [<JsonName "options">] Options: ApplicationCommandOption list option
    [<JsonName "channel_types">] ChannelTypes: ChannelType list option
    [<JsonName "min_value">] [<JsonConverter(typeof<ApplicationCommandMinValueConverter>)>] MinValue: ApplicationCommandMinValue option
    [<JsonName "max_value">] [<JsonConverter(typeof<ApplicationCommandMaxValueConverter>)>] MaxValue: ApplicationCommandMaxValue option
    [<JsonName "min_length">] MinLength: int option
    [<JsonName "max_length">] MaxLength: int option
    [<JsonName "autocomplete">] Autocomplete: bool option
}
with
    static member build(
        Type: ApplicationCommandOptionType,
        Name: string,
        Description: string,
        ?NameLocalizations: Dictionary<string, string>,
        ?DescriptionLocalizations: Dictionary<string, string>,
        ?Required: bool,
        ?Choices: ApplicationCommandOptionChoice list,
        ?Options: ApplicationCommandOption list,
        ?ChannelTypes: ChannelType list,
        ?MinValue: ApplicationCommandMinValue,
        ?MaxValue: ApplicationCommandMaxValue,
        ?MinLength: int,
        ?MaxLength: int,
        ?Autocomplete: bool
    ) = {
        Type = Type;
        Name = Name;
        NameLocalizations = NameLocalizations;
        Description = Description;
        DescriptionLocalizations = DescriptionLocalizations;
        Required = Required;
        Choices = Choices;
        Options = Options;
        ChannelTypes = ChannelTypes;
        MinValue = MinValue;
        MaxValue = MaxValue;
        MinLength = MinLength;
        MaxLength = MaxLength;
        Autocomplete = Autocomplete;
    }

type ApplicationCommand = {
    [<JsonName "id">] Id: string
    [<JsonName "type">] Type: ApplicationCommandType option
    [<JsonName "application_id">] ApplicationId: string
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "name">] Name: string
    [<JsonName "name_localizations">] NameLocalizations: Dictionary<string, string> option
    [<JsonName "description">] Description: string
    [<JsonName "description_localizations">] DescriptionLocalizations: Dictionary<string, string> option
    [<JsonName "options">] Options: ApplicationCommandOption list option
    [<JsonName "default_member_permissions">] DefaultMemberPermissions: string option
    [<JsonName "dm_permission">] DmPermission: bool option
    [<JsonName "nsfw">] Nsfw: bool option
    [<JsonName "integration_types">] IntegrationTypes: ApplicationIntegrationType list option
    [<JsonName "contexts">] Contexts: InteractionContextType list option
    [<JsonName "version">] Version: string
    [<JsonName "handler">] Handler: ApplicationCommandHandlerType option

    // Only present under certain conditions: https://discord.com/developers/docs/interactions/application-commands#retrieving-localized-commands
    [<JsonName "name_localized">] NameLocalized: string option
    [<JsonName "description_localized">] DescriptionLocalized: string option

    // TODO: Create separate type with these special properties? Like invite metadata?
}

// https://discord.com/developers/docs/interactions/application-commands#application-command-permissions-object-application-command-permissions-structure
type ApplicationCommandPermission = {
    [<JsonName "id">] Id: string
    [<JsonName "type">] Type: ApplicationCommandPermissionType
    [<JsonName "permission">] Permission: bool
}

// https://discord.com/developers/docs/interactions/application-commands#application-command-permissions-object-guild-application-command-permissions-structure
type GuildApplicationCommandPermissions = {
    [<JsonName "id">] Id: string
    [<JsonName "application_id">] ApplicationId: string
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "permissions">] Permissions: ApplicationCommandPermission list
}

type InteractionCallbackMessageData = {
    [<JsonName "tts">] Tts: bool option
    [<JsonName "content">] Content: string option
    [<JsonName "embeds">] Embeds: Embed list option
    [<JsonName "allowed_mentions">] AllowedMentions: AllowedMentions option
    [<JsonName "flags">] Flags: int option
    [<JsonName "components">] [<JsonConverter(typeof<ComponentConverter>)>] Components: Component list option
    [<JsonName "attachments">] Attachments: Attachment list option
    [<JsonName "poll">] Poll: Poll option
}
with
    static member build(
        ?Tts: bool,
        ?Content: string,
        ?Embeds: Embed list,
        ?AllowedMentions: AllowedMentions,
        ?Flags: int,
        ?Components: Component list,
        ?Attachments: Attachment list,
        ?Poll: Poll
    ) = {
        Tts = Tts;
        Content = Content;
        Embeds = Embeds;
        AllowedMentions = AllowedMentions;
        Flags = Flags;
        Components = Components;
        Attachments = Attachments;
        Poll = Poll;
    }

    static member buildBase(
        ?Tts: bool,
        ?Content: string,
        ?Embeds: Embed list,
        ?AllowedMentions: AllowedMentions,
        ?Flags: int,
        ?Components: Component list,
        ?Attachments: Attachment list,
        ?Poll: Poll
    ) = InteractionCallbackData.Message (
        InteractionCallbackMessageData.build (
            ?Tts = Tts,
            ?Content = Content,
            ?Embeds = Embeds,
            ?AllowedMentions = AllowedMentions,
            ?Flags = Flags,
            ?Components = Components,
            ?Attachments = Attachments,
            ?Poll = Poll
        )
        
    )

and InteractionCallbackAutocompleteData = {
    [<JsonName "choices">] Choices: ApplicationCommandOptionChoice list
}
with
    static member build(
        Choices: ApplicationCommandOptionChoice list
    ) = {
        Choices = Choices;
    }

    static member buildBase(
        Choices: ApplicationCommandOptionChoice list
    ) = InteractionCallbackData.Autocomplete (
        InteractionCallbackAutocompleteData.build Choices
    )

and InteractionCallbackModalData = {
    [<JsonName "custom_id">] CustomId: string
    [<JsonName "title">] Title: string
    [<JsonName "components">] [<JsonConverter(typeof<ComponentConverter>)>] Components: Component list
}
with
    static member build(
        CustomId: string,
        Title: string,
        Components: Component list
    ) = {
        CustomId = CustomId;
        Title = Title;
        Components = Components;
    }

    static member buildBase(
        CustomId: string,
        Title: string,
        Components: Component list
    ) = InteractionCallbackData.Modal (
        InteractionCallbackModalData.build (CustomId, Title, Components)
    )

and InteractionCallbackData =
    | Message of InteractionCallbackMessageData
    | Autocomplete of InteractionCallbackAutocompleteData
    | Modal of InteractionCallbackModalData

and InteractionCallbackDataConverter () =
    inherit JsonConverter<InteractionCallbackData> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) = 
            raise <| NotImplementedException()
            
        override _.Write (writer: Utf8JsonWriter, value: InteractionCallbackData, options: JsonSerializerOptions) = 
            raise <| NotImplementedException()

    // TODO: Implement (Consider making a single `InteractionCallbackData` with all properties and try to convert to specific in code elsewhere?)

type ActivityTimestamps = {
    [<JsonName "start">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] Start: DateTime option
    [<JsonName "end">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] End: DateTime option
}

type ActivityEmoji = {
    [<JsonName "name">] Name: string
    [<JsonName "id">] Id: string option
    [<JsonName "animated">] Animated: bool option
}

type ActivityParty = {
    [<JsonName "id">] Id: string option
    [<JsonName "size">] Size: (int * int) option
}

type ActivityAssets = {
    [<JsonName "large_image">] LargeImage: string option
    [<JsonName "large_text">] LargeText: string option
    [<JsonName "small_image">] SmallImage: string option
    [<JsonName "small_text">] SmallText: string option
}

type ActivitySecrets = {
    [<JsonName "join">] Join: string option
    [<JsonName "spectate">] Spectate: string option
    [<JsonName "matcch">] Match: string option
}

type ActivityButton = {
    [<JsonName "label">] Label: string
    [<JsonName "url">] Url: string
}

// https://discord.com/developers/docs/topics/gateway-events#activity-object-activity-structure
type Activity = {
    [<JsonName "name">] Name: string
    [<JsonName "type">] Type: ActivityType
    [<JsonName "url">] Url: string option
    [<JsonName "created_at">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] CreatedAt: DateTime option
    [<JsonName "timestamps">] Timestamps: ActivityTimestamps option
    [<JsonName "application_id">] ApplicationId: string option
    [<JsonName "details">] Details: string option
    [<JsonName "state">] State: string option
    [<JsonName "emoji">] Emoji: ActivityEmoji option
    [<JsonName "party">] Party: ActivityParty option
    [<JsonName "assets">] Assets: ActivityAssets option
    [<JsonName "secrets">] Secrets: ActivitySecrets option
    [<JsonName "instance">] Instance: bool option
    [<JsonName "flags">] Flags: int option
    [<JsonName "buttons">] Buttons: ActivityButton list option
}
with
    static member build (
        Type: ActivityType,
        Name: string,
        ?Url: string,
        ?CreatedAt: DateTime,
        ?Timestamps: ActivityTimestamps,
        ?ApplicationId: string,
        ?Details: string,
        ?State: string,
        ?Emoji: ActivityEmoji,
        ?Party: ActivityParty,
        ?Assets: ActivityAssets,
        ?Secrets: ActivitySecrets,
        ?Instance: bool,
        ?Flags: int,
        ?Buttons: ActivityButton list
    ) = {
        Name = Name;
        Type = Type;
        Url = Url;
        CreatedAt = CreatedAt;
        Timestamps = Timestamps;
        ApplicationId = ApplicationId;
        Details = Details;
        State = State;
        Emoji = Emoji;
        Party = Party;
        Assets = Assets;
        Secrets = Secrets;
        Instance = Instance;
        Flags = Flags;
        Buttons = Buttons;
    }

type ApplicationRoleConnectionMetadata = {
    [<JsonName "type">] Type: ApplicationRoleConnectionMetadataType
    [<JsonName "key">] Key: string
    [<JsonName "name">] Name: string
    [<JsonName "name_localizations">] NameLocalizations: IDictionary<string, string> option
    [<JsonName "description">] Description: string
    [<JsonName "description_localizations">] DescriptionLocalizations: IDictionary<string, string> option
}
with
    static member build(
        Type: ApplicationRoleConnectionMetadataType,
        Key: string,
        Name: string,
        Description: string,
        ?NameLocalizations: IDictionary<string, string>,
        ?DescriptionLocalizations: IDictionary<string, string>
    ) = {
        Type = Type;
        Key = Key;
        Name = Name;
        NameLocalizations = NameLocalizations;
        Description = Description;
        DescriptionLocalizations = DescriptionLocalizations;
    }

type ApplicationRoleConnection = {
    [<JsonName "platform_name">] PlatformName: string option
    [<JsonName "platform_username">] PlatformUsername: string option
    [<JsonName "metadata">] Metadata: ApplicationRoleConnectionMetadata
}

type FollowedChannel = {
    [<JsonName "channel_id">] ChannelId: string
    [<JsonName "webhook_id">] WebhookId: string
}

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-trigger-metadata
type AutoModerationTriggerMetadata = {
    [<JsonName "keyword_filter">] KeywordFilter: string list option
    [<JsonName "regex_patterns">] RegexPatterns: string list option
    [<JsonName "presets">] Presets: AutoModerationKeywordPresetType option
    [<JsonName "allow_list">] AllowList: string list option
    [<JsonName "mention_total_limit">] MentionTotalLimit: int option
    [<JsonName "mention_raid_protection_enabled">] MentionRaidProtectionEnabled: bool option
}

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object-action-metadata
type AutoModerationActionMetadata = {
    [<JsonName "channel_id">] ChannelId: string option
    [<JsonName "duration_seconds">] DurationSeconds: int option
    [<JsonName "custom_message">] CustomMessage: string option
}

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object
type AutoModerationAction = {
    [<JsonName "type">] Type: AutoModerationActionType
    [<JsonName "metadata">] Metadata: AutoModerationActionMetadata option
}

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-auto-moderation-rule-structure
type AutoModerationRule = {
    [<JsonName "id">] Id: string
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "name">] Name: string
    [<JsonName "creator_id">] CreatorId: string
    [<JsonName "event_type">] EventType: AutoModerationEventType
    [<JsonName "trigger_type">] TriggerType: AutoModerationTriggerType
    [<JsonName "trigger_metadata">] TriggerMetadata: AutoModerationTriggerMetadata
    [<JsonName "actions">] Actions: AutoModerationAction list
    [<JsonName "enabled">] nabled: bool
    [<JsonName "exempt_roles">] ExemptRoles: string list
    [<JsonName "exempt_channels">] ExemptChannels: string list
}

// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-location-object
type ActivityLocation = {
    [<JsonName "id">] Id: string
    [<JsonName "kind">] [<JsonConverter(typeof<ActivityLocationKindConverter>)>] Kind: ActivityLocationKind
    [<JsonName "channel_id">] ChannelId: string
    [<JsonName "guild_id">] GuildId: string option
}

// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-instance-object
type ActivityInstance = {
    [<JsonName "application_id">] ApplicationId: string
    [<JsonName "instance_id">] InstanceId: string
    [<JsonName "launch_id">] LaunchId: string
    [<JsonName "location">] Location: ActivityLocation
    [<JsonName "users">] Users: string list
}

// https://discord.com/developers/docs/resources/guild#integration-account-object-integration-account-structure
type GuildIntegrationAccount = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
}

// https://discord.com/developers/docs/resources/guild#integration-application-object-integration-application-structure
type GuildIntegrationApplication = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "icon">] Icon: string option
    [<JsonName "description">] Description: string
    [<JsonName "bot">] Bot: User option
}

// https://discord.com/developers/docs/resources/guild#integration-object-integration-structure
type GuildIntegration = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "type">] Type: GuildIntegrationType
    [<JsonName "enabled">] Enabled: bool
    [<JsonName "syncing">] Syncing: bool option
    [<JsonName "role_id">] RoleId: string option
    [<JsonName "enable_emoticons">] EnableEmoticons: bool option
    [<JsonName "expire_behavior">] ExpireBehavior: IntegrationExpireBehaviorType option
    [<JsonName "expire_grace_period">] ExpireGracePeriod: int option
    [<JsonName "user">] User: User option
    [<JsonName "account">] Account: GuildIntegrationAccount
    [<JsonName "synced_at">] SyncedAt: DateTime option
    [<JsonName "subscriber_count">] SubscriberCount: int option
    [<JsonName "revoked">] Revoked: bool option
    [<JsonName "application">] Application: GuildIntegrationApplication option
    [<JsonName "scopes">] [<JsonConverter(typeof<OAuth2ScopeConverter>)>] Scopes: OAuth2Scope list option // TODO: Test if converter works on list
}

// https://discord.com/developers/docs/resources/user#connection-object-connection-structure
type Connection = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "type">] [<JsonConverter(typeof<ConnectionServiceTypeConverter>)>] Type: ConnectionServiceType
    [<JsonName "revoked">] Revoked: bool option
    [<JsonName "integrations">] Integrations: GuildIntegration list option
    [<JsonName "verified">] Verified: bool
    [<JsonName "friend_sync">] FriendSync: bool
    [<JsonName "show_activity">] ShowActivity: bool
    [<JsonName "two_way_link">] TwoWayLink: bool
    [<JsonName "visibility">] Visibility: ConnectionVisibilityType
}

// https://discord.com/developers/docs/resources/guild#ban-object-ban-structure
type GuildBan = {
    [<JsonName "reason">] Reason: string option
    [<JsonName "user">] User: User
}

// https://discord.com/developers/docs/resources/webhook#webhook-object-webhook-structure
type Webhook = {
    [<JsonName "id">] Id: string
    [<JsonName "webhook_type">] Type: WebhookType
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "channel_id">] ChannelId: string option
    [<JsonName "user">] User: User option
    [<JsonName "name">] Name: string option
    [<JsonName "avatar">] Avatar: string option
    [<JsonName "token">] Token: string option
    [<JsonName "application_id">] ApplicationId: string option
    [<JsonName "source_guild">] SourceGuild: Guild option
    [<JsonName "source_channel">] SourceChannel: Channel option
    [<JsonName "url">] Url: string option
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-change-object
type AuditLogChange = {
    [<JsonName "new_value">] NewValue: obj option
    [<JsonName "old_value">] OldValue: obj option
    [<JsonName "key">] Key: string
    // TODO: Determine what possible types the values can be and create discriminated union for them
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-optional-audit-entry-info
type AuditLogEntryOptionalInfo = {
    [<JsonName "application_id">] ApplicationId: string option
    [<JsonName "auto_moderation_rule_name">] AutoModerationRuleName: string option
    [<JsonName "auto_moderation_rule_trigger_type">] AutoModerationRuleTriggerType: string option
    [<JsonName "channel_id">] ChannelId: string option
    [<JsonName "count">] Count: string option
    [<JsonName "delete_member_days">] DeleteMemberDays: string option
    [<JsonName "id">] Id: string option
    [<JsonName "members_removed">] MembersRemoved: string option
    [<JsonName "message_id">] MessageId: string option
    [<JsonName "role_name">] RoleName: string option
    [<JsonName "type">] Type: string option
    [<JsonName "integration_type">] IntegrationType: string option
    // TODO: Determine if the documentation is incorrect about everything being strings
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-audit-log-entry-structure
type AuditLogEntry = {
    [<JsonName "target_id">] TargetId: string option
    [<JsonName "changes">] Changes: AuditLogChange list option
    [<JsonName "user_id">] UserId: string option
    [<JsonName "id">] Id: string
    [<JsonName "action_type">] ActionType: AuditLogEventType
    [<JsonName "options">] Options: AuditLogEntryOptionalInfo option
    [<JsonName "reason">] Reason: string option
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-object-audit-log-structure
type AuditLog = {
    [<JsonName "application_commands">] ApplicationCommands: ApplicationCommand list
    [<JsonName "audit_log_entries">] AuditLogEntries: AuditLogEntry list
    [<JsonName "auto_moderation_rules">] AutoModerationRules: AutoModerationRule list
    [<JsonName "guild_scheduled_events">] GuildScheduledEvents: GuildScheduledEvent list
    [<JsonName "integrations">] Integrations: GuildIntegration list
    [<JsonName "threads">] Threads: Channel list
    [<JsonName "users">] Users: User list
    [<JsonName "webhooks">] Webhooks: Webhook list
}

// https://discord.com/developers/docs/resources/voice#voice-region-object-voice-region-structure
type VoiceRegion = {
    [<JsonName "id">] Id: string
    [<JsonName "name">] Name: string
    [<JsonName "optimal">] Optimal: bool
    [<JsonName "deprecated">] Deprecated: bool
    [<JsonName "custom">] Custom: bool
}

// https://discord.com/developers/docs/topics/gateway#session-start-limit-object-session-start-limit-structure
type SessionStartLimit = {
    [<JsonName "total">] Total: int
    [<JsonName "remaining">] Remaining: int
    [<JsonName "reset_after">] ResetAfter: int
    [<JsonName "max_concurrency">] MaxConcurrency: int
}

// https://discord.com/developers/docs/topics/gateway-events#identify-identify-connection-properties
type ConnectionProperties = {
    [<JsonName "os">] OperatingSystem: string
    [<JsonName "browser">] Browser: string
    [<JsonName "device">] Device: string
}
with
    static member build(
        OperatingSystem: string,
        Browser: string,
        Device: string
    ) = {
        OperatingSystem = OperatingSystem;
        Browser = Browser;
        Device = Device;
    }

    static member build(OperatingSystem: string) =
        ConnectionProperties.build(OperatingSystem, "Discordfs", "Discordfs")

    static member build() =
        let operatingSystem =
            match Environment.OSVersion.Platform with
            | PlatformID.Win32NT -> "Windows"
            | PlatformID.Unix -> "Linux"
            | _ -> "Unknown OS"

        ConnectionProperties.build(operatingSystem)

// https://discord.com/developers/docs/topics/gateway-events#payload-structure
type GatewayEventIdentifier = {
    [<JsonName "op">] Opcode: GatewayOpcode
    [<JsonName "t">] EventName: string option
}
with
    static member getType (json: string) =
        try
            Some <| FsJson.deserialize<GatewayEventIdentifier> json
        with
        | _ ->
            None

// https://discord.com/developers/docs/topics/gateway-events#payload-structure
type GatewaySequencer = {
    [<JsonName "s">] Sequence: int option
}
with
    static member getSequenceNumber (json: string) =
        try
            let seq = FsJson.deserialize<GatewaySequencer> json
            seq.Sequence
        with
        | _ ->
            None

// https://discord.com/developers/docs/topics/gateway-events#payload-structure
type GatewayEvent<'a> = {
    [<JsonName "op">] Opcode: GatewayOpcode
    [<JsonName "d">] Data: 'a
    [<JsonName "s">] Sequence: int option
    [<JsonName "t">] EventName: string option
}
with
    static member build(
        Opcode: GatewayOpcode,
        Data: 'a,
        ?Sequence: int,
        ?EventName: string
    ) = {
        Opcode = Opcode;
        Data = Data;
        Sequence = Sequence;
        EventName = EventName;
    }

    static member deserializeF (json: string) =
        FsJson.deserialize<GatewayEvent<'a>> json

    static member deserialize (json: string) =
        try
            Some <| GatewayEvent<'a>.deserializeF json
        with
        | _ ->
            None

// https://discord.com/developers/docs/resources/sku#sku-object-sku-structure
type Sku = {
    [<JsonName "id">] Id: string
    [<JsonName "type">] Type: SkuType
    [<JsonName "application_id">] ApplicationId: string
    [<JsonName "name">] Name: string
    [<JsonName "slug">] Slug: string
    [<JsonName "flags">] Flags: int
}

// https://discord.com/developers/docs/resources/subscription#subscription-object
type Subscription = {
    [<JsonName "id">] Id: string
    [<JsonName "user_id">] UserId: string
    [<JsonName "sku_id">] SkuId: string
    [<JsonName "entitlement_ids">] EntitlmentIds: string list
    [<JsonName "current_period_start">] CurrentPeriodStart: DateTime
    [<JsonName "current_period_end">] CurrentPeriodEnd: DateTime
    [<JsonName "status">] Status: SubscriptionStatusType
    [<JsonName "created_at">] CanceledAt: DateTime option
    [<JsonName "country">] Country: string option
}

// https://discord.com/developers/docs/resources/stage-instance#stage-instance-object-stage-instance-structure
type StageInstance = {
    [<JsonName "id">] Id: string
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "channel_id">] ChannelId: string
    [<JsonName "topic">] Topic: string
    [<JsonName "privacy_level">] PrivacyLevel: PrivacyLevelType
    [<JsonName "discoverable_enabled">] DiscoverableEnabled: bool
    [<JsonName "guild_scheduled_event_id">] GuildScheduledEventId: string option
}

// https://discord.com/developers/docs/resources/voice#voice-state-object-voice-state-structure
type VoiceState = {
    [<JsonName "guild_id">] GuildId: string option
    [<JsonName "channel_id">] ChannelId: string option
    [<JsonName "user_id">] UserId: string option
    [<JsonName "member">] Member: GuildMember option
    [<JsonName "session_id">] SessionId: string
    [<JsonName "deaf">] Deaf: bool
    [<JsonName "mute">] Mute: bool
    [<JsonName "self_deaf">] SelfDeaf: bool
    [<JsonName "self_mute">] SelfMute: bool
    [<JsonName "self_stream">] SelfStream: bool option
    [<JsonName "self_video">] SelfVideo: bool
    [<JsonName "suppress">] Suppress: bool
    [<JsonName "request_to_speak_timestamp">] RequestToSpeakTimestamp: DateTime option
}
