namespace Discordfs.Types

open Discordfs.Types.Utils
open System
open System.Collections.Generic
open System.Text.Json
open System.Text.Json.Serialization

#nowarn "49"

type DefaultReaction = {
    [<JsonPropertyName "emoji_id">] EmojiId: string option
    [<JsonPropertyName "emoji_name">] EmojiName: string option
}

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

type CommandInteractionDataOption = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: ApplicationCommandOptionType
    [<JsonPropertyName "value">] Value: CommandInteractionDataOptionValue option
    [<JsonPropertyName "options">] Options: CommandInteractionDataOption list option
    [<JsonPropertyName "focused">] Focused: bool option
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
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "filename">] Filename: string
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "content_type">] ContentType: string option
    [<JsonPropertyName "size">] Size: int
    [<JsonPropertyName "url">] Url: string
    [<JsonPropertyName "proxy_url">] ProxyUrl: string
    [<JsonPropertyName "height">] Height: int option
    [<JsonPropertyName "width">] Width: int option
    [<JsonPropertyName "ephemeral">] Ephemeral: bool option
    [<JsonPropertyName "duration_secs">] DurationSecs: float option
    [<JsonPropertyName "waveform">] Waveform: string option
    [<JsonPropertyName "flags">] Flags: int option
}

type RoleTags = {
    [<JsonPropertyName "bot_id">] BotId: string option
    [<JsonPropertyName "integration_id">] IntegrationId: string option
    [<JsonPropertyName "premium_subscriber">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] PremiumSubscriber: unit option
    [<JsonPropertyName "subscription_listing_id">] SubscriptionListingId: string option
    [<JsonPropertyName "available_for_purchase">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] AvailableForPurchase: unit option
    [<JsonPropertyName "guild_connections">] [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] GuildConnections: unit option
}

type Role = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "color">] Color: int
    [<JsonPropertyName "hoist">] Hoist: bool
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "unicode_emoji">] UnicodeEmoji: string option
    [<JsonPropertyName "position">] Position: int
    [<JsonPropertyName "permissions">] Permissions: string
    [<JsonPropertyName "managed">] Managed: bool
    [<JsonPropertyName "mentionable">] Mentionable: bool
    [<JsonPropertyName "tags">] Tags: RoleTags option
    [<JsonPropertyName "flags">] Flags: int
}

type Entitlement = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "sku_id">] SkuId: string
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "user_id">] UserId: string option
    [<JsonPropertyName "type">] Type: EntitlementType
    [<JsonPropertyName "deleted">] Deleted: bool
    [<JsonPropertyName "starts_at">] StartsAt: DateTime option
    [<JsonPropertyName "ends_at">] EndsAt: DateTime option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "consumed">] Consumed: bool option
}

type AvatarDecorationData = {
    [<JsonPropertyName "asset">] Asset: string
    [<JsonPropertyName "sku_id">] SkuId: string
}

type User = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "username">] Username: string
    [<JsonPropertyName "discriminator">] Discriminator: string
    [<JsonPropertyName "global_name">] GlobalName: string option
    [<JsonPropertyName "avatar">] Avatar: string option
    [<JsonPropertyName "bot">] Bot: bool option
    [<JsonPropertyName "system">] System: bool option
    [<JsonPropertyName "mfa_enabled">] MfaEnabled: bool option
    [<JsonPropertyName "banner">] Banner: string option
    [<JsonPropertyName "accent_color">] AccentColor: int option
    [<JsonPropertyName "locale">] Locale: string option
    [<JsonPropertyName "verified">] Verified: bool option
    [<JsonPropertyName "email">] Email: string option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "premium_type">] PremiumType: UserPremiumType option
    [<JsonPropertyName "public_flags">] PublicFlags: int option
    [<JsonPropertyName "avatar_decoration_data">] AvatarDecorationData: AvatarDecorationData option
}

type GuildMember = {
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "nick">] Nick: string option
    [<JsonPropertyName "avatar">] Avatar: string option
    [<JsonPropertyName "roles">] Roles: string list
    [<JsonPropertyName "joined_at">] JoinedAt: DateTime option
    [<JsonPropertyName "premium_since">] PremiumSince: DateTime option
    [<JsonPropertyName "deaf">] Deaf: bool
    [<JsonPropertyName "mute">] Mute: bool
    [<JsonPropertyName "flags">] Flags: int
    [<JsonPropertyName "pending">] Pending: bool option
    [<JsonPropertyName "permissions">] Permissions: string option
    [<JsonPropertyName "communication_disabled_until">] CommunicationDisabledUntil: DateTime option
    [<JsonPropertyName "avatar_decoration_metadata">] AvatarDecorationData: AvatarDecorationData option
}

type Emoji = {
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "roles">] Roles: string list option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "require_colons">] RequireColons: bool option
    [<JsonPropertyName "managed">] Managed: bool option
    [<JsonPropertyName "animated">] Animated: bool option
    [<JsonPropertyName "available">] Available: bool option
}

// https://discord.com/developers/docs/resources/sticker#sticker-object-sticker-structure
type Sticker = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "pack_id">] PackId: string option
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "tags">] Tags: string
    [<JsonPropertyName "type">] Type: StickerType
    [<JsonPropertyName "format_type">] FormatType: StickerFormatType
    [<JsonPropertyName "available">] Available: bool option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "sort_value">] SortValue: int option
}

// https://discord.com/developers/docs/resources/sticker#sticker-item-object-sticker-item-structure
type StickerItem = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "format_type">] FormatType: StickerFormatType
}

// https://discord.com/developers/docs/resources/sticker#sticker-pack-object
type StickerPack = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "stickers">] Stickers: Sticker list
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "sku_id">] SkuId: string
    [<JsonPropertyName "cover_sticker_id">] CoverStickerId: string option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "banner_asset_id">] BannerAssetId: string option
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

type UnavailableGuild = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "unavailable">] Unavailable: bool
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

// https://discord.com/developers/docs/resources/guild#guild-widget-settings-object-guild-widget-settings-structure
type GuildWidgetSettings = {
    [<JsonPropertyName "enabled">] Enabled: bool
    [<JsonPropertyName "channel_id">] ChannelId: string option
}

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

type ChannelMention = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "type">] Type: ChannelType
    [<JsonPropertyName "name">] Name: string
}

type PermissionOverwrite = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: PermissionOverwriteType
    [<JsonPropertyName "allow">] Allow: string
    [<JsonPropertyName "deny">] Deny: string
}

type ThreadMetadata = {
    [<JsonPropertyName "archived">] Archived: bool
    [<JsonPropertyName "auto_archive_duration">] AutoArchiveDuration: int
    [<JsonPropertyName "archive_timestamp">] ArchiveTimestamp: DateTime
    [<JsonPropertyName "locked">] Locked: bool
    [<JsonPropertyName "invitable">] Invitable: bool option
    [<JsonPropertyName "create_timestamp">] CreateTimestamp: DateTime option
}

type ThreadMember = {
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "user_id">] UserId: string option
    [<JsonPropertyName "join_timestamp">] JoinTimestamp: DateTime
    [<JsonPropertyName "flags">] Flags: int
    [<JsonPropertyName "member">] Member: GuildMember option
}

type ChannelTag = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "moderated">] Moderated: bool
    [<JsonPropertyName "emoji_id">] EmojiId: string option
    [<JsonPropertyName "emoji_name">] EmojiName: string option
}

type EmbedFooter = {
    [<JsonPropertyName "text">] Text: string
    [<JsonPropertyName "icon_url">] IconUrl: string option
    [<JsonPropertyName "proxy_icon_url">] ProxyIconUrl: string option
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
    [<JsonPropertyName "url">] Url: string
    [<JsonPropertyName "proxy_url">] ProxyUrl: string option
    [<JsonPropertyName "height">] Height: int option
    [<JsonPropertyName "width">] Width: int option
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
    [<JsonPropertyName "url">] Url: string
    [<JsonPropertyName "proxy_url">] ProxyUrl: string option
    [<JsonPropertyName "height">] Height: int option
    [<JsonPropertyName "width">] Width: int option
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
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "proxy_url">] ProxyUrl: string option
    [<JsonPropertyName "height">] Height: int option
    [<JsonPropertyName "width">] Width: int option
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
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "url">] Url: string option
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
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "icon_url">] IconUrl: string option
    [<JsonPropertyName "proxy_icon_url">] ProxyIconUrl: string option
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
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "value">] Value: string
    [<JsonPropertyName "inline">] Inline: bool option
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
    [<JsonPropertyName "title">] Title: string option
    [<JsonPropertyName "type">] Type: string option
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "timestamp">] Timestamp: DateTime option
    [<JsonPropertyName "color">] Color: int option
    [<JsonPropertyName "footer">] Footer: EmbedFooter option
    [<JsonPropertyName "image">] Image: EmbedImage option
    [<JsonPropertyName "thumbnail">] Thumbnail: EmbedThumbnail option
    [<JsonPropertyName "video">] Video: EmbedVideo option
    [<JsonPropertyName "provider">] Provider: EmbedProvider option
    [<JsonPropertyName "author">] Author: EmbedAuthor option
    [<JsonPropertyName "fields">] Fields: EmbedField list option
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
    [<JsonPropertyName "burst">] Burst: int
    [<JsonPropertyName "normal">] Normal: int
}

type Reaction = {
    [<JsonPropertyName "count">] Count: int
    [<JsonPropertyName "count_details">] CountDetails: ReactionCountDetails
    [<JsonPropertyName "me">] Me: bool
    [<JsonPropertyName "me_burst">] MeBurst: bool
    [<JsonPropertyName "emoji">] Emoji: Emoji
    [<JsonPropertyName "burst_colors">] BurstColors: int list
}

type MessageActivity = {
    [<JsonPropertyName "type">] Type: MessageActivityType
    [<JsonPropertyName "party_id">] PartyId: string option
}

type OAuth2InstallParams = {
    [<JsonPropertyName "scopes">] Scopes: string list
    [<JsonPropertyName "permissions">] Permissions: string
}

type ApplicationIntegrationTypeConfiguration = {
    [<JsonPropertyName "oauth2_install_params">] Oauth2InstallParams: OAuth2InstallParams option
}

type TeamMember = {
    [<JsonPropertyName "membership_state">] MembershipState: TeamMembershipState
    [<JsonPropertyName "team_id">] TeamId: string
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "role">] Role: string
}

type Team = {
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "members">] Members: TeamMember list
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "owner_user_id">] OwnerUserId: string
}

type Application = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "rpc_origins">] RpcOrigins: string list option
    [<JsonPropertyName "bot_public">] BotPublic: bool
    [<JsonPropertyName "bot_require_code_grant">] BotRequireCodeGrant: bool
    [<JsonPropertyName "bot">] Bot: User option
    [<JsonPropertyName "terms_of_Service_url">] TermsOfServiceUrl: string option
    [<JsonPropertyName "privacy_policy_url">] PrivacyPolicyUrl: string option
    [<JsonPropertyName "owner">] Owner: User option
    [<JsonPropertyName "verify_key">] VerifyKey: string
    [<JsonPropertyName "team">] Team: Team option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "guild">] Guild: Guild option
    [<JsonPropertyName "primary_sku_id">] PrimarySkuId: string option
    [<JsonPropertyName "slug">] Slug: string option
    [<JsonPropertyName "cover_image">] CoverImage: string option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "approximate_guild_count">] ApproximateGuildCount: int option
    [<JsonPropertyName "redirect_uris">] RedirectUris: string list option
    [<JsonPropertyName "interactions_endpoint_url">] InteractionsEndpointUrl: string option
    [<JsonPropertyName "role_connections_verification_url">] RoleConnectionsVerificationUrl: string option
    [<JsonPropertyName "tags">] Tags: string list option
    [<JsonPropertyName "install_params">] InstallParams: OAuth2InstallParams option
    [<JsonPropertyName "integration_types_config">] IntegrationTypesConfig: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option
    [<JsonPropertyName "custom_install_url">] CustomInstallUrl: string option
}

type PartialApplication = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "flags">] Flags: int option
}

type MessageReference = {
    [<JsonPropertyName "message_id">] MessageId: string option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "fail_if_not_exists">] FailIfNotExists: bool option
}

type MessageInteractionMetadata = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: InteractionType
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "authorizing_integration_owners">] AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>
    [<JsonPropertyName "original_response_message_id">] OriginalResponseMessage: string option
    [<JsonPropertyName "interacted_message_id">] InteractedMessageId: string option
    [<JsonPropertyName "triggering_interaction_metadata">] TriggeringInteractionMetadata: MessageInteractionMetadata option
}

type MessageInteraction = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: InteractionType
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "member">] Member: GuildMember option
}

type RoleSubscriptionData = {
    [<JsonPropertyName "role_subscription_listing_id">] RoleSubscriptionListingId: string
    [<JsonPropertyName "tier_name">] TierName: string
    [<JsonPropertyName "total_months_subscribed">] TotalMonthsSubscribed: int
    [<JsonPropertyName "is_renewal">] IsRenewal: bool
}

type PollMedia = {
    [<JsonPropertyName "text">] Text: string option
    [<JsonPropertyName "emoji">] Emoji: Emoji option
}

type PollAnswer = {
    [<JsonPropertyName "answer_id">] AnswerId: int
    [<JsonPropertyName "poll_media">] PollMedia: PollMedia
}

type PollAnswerCount = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "count">] Count: int
    [<JsonPropertyName "me_voted">] MeVoted: bool
}

type PollResults = {
    [<JsonPropertyName "is_finalized">] IsFinalized: bool
    [<JsonPropertyName "answer_counts">] AnswerCounts: PollAnswerCount list
}

type Poll = {
    [<JsonPropertyName "question">] Question: PollMedia
    [<JsonPropertyName "answers">] Answers: PollAnswer list
    [<JsonPropertyName "expiry">] Expiry: DateTime option
    [<JsonPropertyName "allow_multiselect">] AllowMultiselect: bool
    [<JsonPropertyName "layout_type">] LayoutType: PollLayoutType
    [<JsonPropertyName "results">] Results: PollResults option
}

type MessageCall = {
    [<JsonPropertyName "participants">] Participants: string list
    [<JsonPropertyName "ended_timestamp">] EndedTimestamp: DateTime option
}

type SelectMenuOption = {
    [<JsonPropertyName "label">] Label: string
    [<JsonPropertyName "value">] Value: string
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "emoji">] Emoji: Emoji option
    [<JsonPropertyName "default">] Default: bool option
}

type SelectMenuDefaultValue = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: string
}

[<JsonConverter(typeof<ComponentConverter>)>]
type Component =
    | ActionRow of ActionRowComponent
    | Button of ButtonComponent
    | SelectMenu of SelectMenuComponent
    | TextInput of TextInputComponent

and ComponentConverter () =
    inherit JsonConverter<Component> () with
        override _.Read (reader, typeToConvert, options) =
            let success, document = JsonDocument.TryParseValue(&reader)

            if not success then
                raise (JsonException())

            let componentType = document.RootElement.GetProperty "type" |> _.GetInt32() |> enum<ComponentType>
            let json = document.RootElement.GetRawText()

            match componentType with
            | ComponentType.ACTION_ROW -> Component.ActionRow <| FsJson.deserialize<ActionRowComponent> json
            | ComponentType.BUTTON -> Component.Button <| FsJson.deserialize<ButtonComponent> json
            | ComponentType.STRING_SELECT
            | ComponentType.USER_SELECT
            | ComponentType.ROLE_SELECT
            | ComponentType.MENTIONABLE_SELECT
            | ComponentType.CHANNEL_SELECT -> Component.SelectMenu <| FsJson.deserialize<SelectMenuComponent> json
            | ComponentType.TEXT_INPUT -> Component.TextInput <| FsJson.deserialize<TextInputComponent> json
            | _ -> failwith "Unexpected ComponentType provided"

        override _.Write (writer, value, options) =
            match value with
            | Component.ActionRow a -> FsJson.serialize a |> writer.WriteRawValue
            | Component.Button b -> FsJson.serialize b |> writer.WriteRawValue
            | Component.SelectMenu s -> FsJson.serialize s |> writer.WriteRawValue
            | Component.TextInput t -> FsJson.serialize t |> writer.WriteRawValue

and ActionRowComponent = {
    [<JsonPropertyName "type">] Type: ComponentType
    [<JsonPropertyName "components">] Components: Component list
}

and ButtonComponent = {
    [<JsonPropertyName "type">] Type: ComponentType
    [<JsonPropertyName "style">] Style: ButtonStyle
    [<JsonPropertyName "label">] Label: string
    [<JsonPropertyName "emoji">] Emoji: Emoji option
    [<JsonPropertyName "custom_id">] CustomId: string option
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "disabled">] Disabled: bool option
}

and SelectMenuComponent = {
    [<JsonPropertyName "type">] Type: ComponentType
    [<JsonPropertyName "custom_id">] CustomId: string
    [<JsonPropertyName "options">] Options: SelectMenuOption list option
    [<JsonPropertyName "channel_types">] ChannelTypes: ChannelType list option
    [<JsonPropertyName "placeholder">] Placeholder: string option
    [<JsonPropertyName "default_values">] DefaultValues: SelectMenuDefaultValue option
    [<JsonPropertyName "min_values">] MinValues: int option
    [<JsonPropertyName "max_values">] MaxValues: int option
    [<JsonPropertyName "disabled">] Disabled: bool option
}

and TextInputComponent = {
    [<JsonPropertyName "type">] Type: ComponentType
    [<JsonPropertyName "custom_id">] CustomId: string
    [<JsonPropertyName "style">] Style: TextInputStyle
    [<JsonPropertyName "label">] Label: string
    [<JsonPropertyName "min_length">] MinLength: int option
    [<JsonPropertyName "max_length">] MaxLength: int option
    [<JsonPropertyName "required">] Required: bool option
    [<JsonPropertyName "value">] Value: string option
    [<JsonPropertyName "placeholder">] Placeholder: string option
}

type Channel = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: ChannelType
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "permission_overwrites">] PermissionOverwrites: PermissionOverwrite list option
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "topic">] Topic: string option
    [<JsonPropertyName "nsfw">] Nsfw: bool option
    [<JsonPropertyName "last_message_id">] LastMessageId: string option
    [<JsonPropertyName "bitrate">] Bitrate: int option
    [<JsonPropertyName "user_limit">] UserLimit: int option
    [<JsonPropertyName "rate_limit_per_user">] RateLimitPerUser: int option
    [<JsonPropertyName "recipients">] Recipients: User list option
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "owner_id">] OwnerId: string option
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "managed">] Managed: bool option
    [<JsonPropertyName "parent_id">] ParentId: string option
    [<JsonPropertyName "last_pin_timestamp">] LastPinTimestamp: DateTime option
    [<JsonPropertyName "rtc_region">] RtcRegion: string option
    [<JsonPropertyName "video_quality_mode">] VideoQualityMode: VideoQualityMode option
    [<JsonPropertyName "message_count">] MessageCount: int option
    [<JsonPropertyName "member_count">] MemberCount: int option
    [<JsonPropertyName "thread_metadata">] ThreadMetadata: ThreadMetadata option
    [<JsonPropertyName "member">] Member: ThreadMember option
    [<JsonPropertyName "default_auto_archive_duration">] DefaultAutoArchiveDuration: AutoArchiveDurationType option
    [<JsonPropertyName "permissions">] Permissions: string option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "total_messages_sent">] TotalMessagesSent: int option
    [<JsonPropertyName "available_tags">] AvailableTags: ChannelTag list option
    [<JsonPropertyName "applied_tags">] AppliedTags: int list option
    [<JsonPropertyName "default_reaction_emoji">] DefaultReactionEmoji: DefaultReaction option
    [<JsonPropertyName "default_thread_rate_limit_per_user">] DefaultThreadRateLimitPerUser: int option
    [<JsonPropertyName "default_sort_order">] DefaultSortOrder: ChannelSortOrder option
    [<JsonPropertyName "default_forum_layout">] DefaultForumLayout: ChannelForumLayout option
}

// https://discord.com/developers/docs/resources/guild#guild-widget-object-guild-widget-structure
type GuildWidget = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "instant_invite">] InstantInvite: string option
    [<JsonPropertyName "channels">] Channels: Channel list
    [<JsonPropertyName "members">] Members: User list
    [<JsonPropertyName "presence_count">] PresenceCount: int
}

type ResolvedData = {
    [<JsonPropertyName "users">] Users: IDictionary<string, User> option
    [<JsonPropertyName "members">] Members: IDictionary<string, GuildMember> option
    [<JsonPropertyName "roles">] Roles: IDictionary<string, Role> option
    [<JsonPropertyName "channels">] Channels: IDictionary<string, Channel> option
    [<JsonPropertyName "messages">] Messages: IDictionary<string, Message> option
    [<JsonPropertyName "attachments">] Attachments: IDictionary<string, Attachment> option
}

and Message = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "author">] Author: User
    [<JsonPropertyName "content">] Content: string
    [<JsonPropertyName "timestamp">] Timestamp: DateTime
    [<JsonPropertyName "edited_timestamp">] EditedTimestamp: DateTime option
    [<JsonPropertyName "tts">] Tts: bool
    [<JsonPropertyName "mention_everyone">] MentionEveryone: bool
    [<JsonPropertyName "mentions">] Mentions: User list
    [<JsonPropertyName "mention_roles">] MentionRoles: string list
    [<JsonPropertyName "mention_channels">] MentionChannels: ChannelMention list
    [<JsonPropertyName "attachments">] Attachments: Attachment list
    [<JsonPropertyName "embeds">] Embeds: Embed list
    [<JsonPropertyName "reactions">] Reactions: Reaction list
    [<JsonPropertyName "nonce">] Nonce: MessageNonce option
    [<JsonPropertyName "pinned">] Pinned: bool
    [<JsonPropertyName "webhook_id">] WebhookId: string option
    [<JsonPropertyName "type">] Type: MessageType
    [<JsonPropertyName "activity">] Activity: MessageActivity option
    [<JsonPropertyName "application">] Application: Application option
    [<JsonPropertyName "message_reference">] MessageReference: MessageReference option
    [<JsonPropertyName "flags">] Flags: int
    [<JsonPropertyName "referenced_message">] ReferencedMessage: Message option
    [<JsonPropertyName "interaction_metadata">] InteractionMetadata: MessageInteractionMetadata option
    [<JsonPropertyName "interaction">] Interaction: MessageInteraction option
    [<JsonPropertyName "thread">] Thread: Channel option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "sticker_items">] StickerItems: Sticker list option
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "role_subscription_data">] RoleSubscriptionData: RoleSubscriptionData option
    [<JsonPropertyName "resolved">] Resolved: ResolvedData option
    [<JsonPropertyName "poll">] Poll: Poll option
    [<JsonPropertyName "call">] Call: MessageCall option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-entity-metadata
type EntityMetadata = {
    [<JsonPropertyName "location">] Location: string option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-nweekday-structure
type RecurrenceRuleNWeekday = {
    [<JsonPropertyName "n">] N: int
    [<JsonPropertyName "day">] Day: RecurrenceRuleWeekdayType
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
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "id">] GuildId: string
    [<JsonPropertyName "id">] ChannelId: string option
    [<JsonPropertyName "id">] CreatorId: string option
    [<JsonPropertyName "id">] Name: string
    [<JsonPropertyName "id">] Description: string option
    [<JsonPropertyName "id">] ScheduledStartTime: DateTime option
    [<JsonPropertyName "id">] ScheduledEndTime: DateTime option
    [<JsonPropertyName "id">] PrivacyLevel: PrivacyLevelType
    [<JsonPropertyName "id">] EventStatus: EventStatusType
    [<JsonPropertyName "id">] EntityType: ScheduledEntityType
    [<JsonPropertyName "id">] EntityId: string option
    [<JsonPropertyName "id">] EntityMetadata: EntityMetadata option
    [<JsonPropertyName "id">] Creator: User option
    [<JsonPropertyName "id">] UserCount: int option
    [<JsonPropertyName "id">] Image: string option
    [<JsonPropertyName "id">] RecurrenceRule: RecurrenceRule option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-user-object-guild-scheduled-event-user-structure
type GuildScheduledEventUser = {
    [<JsonPropertyName "guild_scheduled_event_id">] GuildScheduledEventId: string
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "member">] Member: GuildMember option
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
    [<JsonPropertyName "serialized_source_guild">] SerializedSourceGuild: Guild
    [<JsonPropertyName "is_dirty">] IsDirty: bool option
}

type Invite = {
    [<JsonPropertyName "type">] Type: InviteType
    [<JsonPropertyName "code">] Code: string
    [<JsonPropertyName "guild">] Guild: Guild option
    [<JsonPropertyName "channel">] Channel: Channel option
    [<JsonPropertyName "inviter">] Inviter: User option
    [<JsonPropertyName "target_type">] TargetType: InviteTargetType option
    [<JsonPropertyName "target_user">] TargetUser: User option
    [<JsonPropertyName "target_application">] TargetApplication: Application option
    [<JsonPropertyName "approximate_presence_count">] ApproximatePresenceCount: int option
    [<JsonPropertyName "approximate_member_count">] ApproximateMemberCount: int option
    [<JsonPropertyName "expires_at">] ExpiresAt: DateTime
    [<JsonPropertyName "guild_scheduled_event">] GuildScheduledEvent: GuildScheduledEvent option
}

type InviteMetadata = {
    [<JsonPropertyName "uses">] Uses: int
    [<JsonPropertyName "max_uses">] MaxUses: int
    [<JsonPropertyName "max_age">] MaxAge: int
    [<JsonPropertyName "temporary">] Temporary: bool
    [<JsonPropertyName "created_at">] CreatedAt: DateTime
}

// GetChannelInvite returns a list Invite objects with InviteMetadata added. To make this easy to implement here, I've
// just created this type to combine the two. If changes are made to either Invite or InviteMetadata, they should be
// duplicated here. A proper fix for this is probably possible, but implementing this way means the return type is more
// accurate for now making it a lower priority fix.

// TODO: Make below type by combining `Invite` and `InviteMetadata` somehow without code duplication

type InviteWithMetadata = {
    [<JsonPropertyName "type">] Type: InviteType
    [<JsonPropertyName "code">] Code: string
    [<JsonPropertyName "guild">] Guild: Guild option
    [<JsonPropertyName "channel">] Channel: Channel option
    [<JsonPropertyName "inviter">] Inviter: User option
    [<JsonPropertyName "target_type">] TargetType: InviteTargetType option
    [<JsonPropertyName "target_user">] TargetUser: User option
    [<JsonPropertyName "target_application">] TargetApplication: Application option
    [<JsonPropertyName "approximate_presence_count">] ApproximatePresenceCount: int option
    [<JsonPropertyName "approximate_member_count">] ApproximateMemberCount: int option
    [<JsonPropertyName "expires_at">] ExpiresAt: DateTime
    [<JsonPropertyName "guild_scheduled_event">] GuildScheduledEvent: GuildScheduledEvent option
    [<JsonPropertyName "uses">] Uses: int
    [<JsonPropertyName "max_uses">] MaxUses: int
    [<JsonPropertyName "max_age">] MaxAge: int
    [<JsonPropertyName "temporary">] Temporary: bool
    [<JsonPropertyName "created_at">] CreatedAt: DateTime
}

type InteractionData = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: ApplicationCommandType
    [<JsonPropertyName "resolved">] Resolved: ResolvedData option
    [<JsonPropertyName "options">] Options: CommandInteractionDataOption list option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "target_it">] TargetId: string option
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
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "type">] Type: InteractionType
    [<JsonPropertyName "data">] Data: InteractionData option
    [<JsonPropertyName "guild">] Guild: Guild option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "channel">] Channel: Channel option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "member">] Member: GuildMember option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "token">] Token: string
    [<JsonPropertyName "version">] Version: int
    [<JsonPropertyName "message">] Message: Message option
    [<JsonPropertyName "app_permissions">] AppPermissions: string
    [<JsonPropertyName "locale">] Locale: string option
    [<JsonPropertyName "guild_locale">] GuildLocale: string option
    [<JsonPropertyName "entitlements">] Entitlements: Entitlement list
    [<JsonPropertyName "authorizing_integration_owners">] AuthorizingIntegrationOwners: IDictionary<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>
    [<JsonPropertyName "context">] Context: InteractionContextType option
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
    [<JsonPropertyName "parse">] Parse: AllowedMentionsParseType list
    [<JsonPropertyName "roles">] Roles: string list option
    [<JsonPropertyName "users">] Users: string list option
    [<JsonPropertyName "replied_user">] RepliedUser: bool option
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
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: Dictionary<string, string> option
    [<JsonPropertyName "value">] Value: ApplicationCommandOptionChoiceValue
}

type ApplicationCommandOption = {
    [<JsonPropertyName "type">] Type: ApplicationCommandOptionType
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: Dictionary<string, string> option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "description_localizations">] DescriptionLocalizations: Dictionary<string, string> option
    [<JsonPropertyName "required">] Required: bool option
    [<JsonPropertyName "choices">] Choices: ApplicationCommandOptionChoice list option
    [<JsonPropertyName "options">] Options: ApplicationCommandOption list option
    [<JsonPropertyName "channel_types">] ChannelTypes: ChannelType list option
    [<JsonPropertyName "min_value">] MinValue: ApplicationCommandMinValue option
    [<JsonPropertyName "max_value">] MaxValue: ApplicationCommandMaxValue option
    [<JsonPropertyName "min_length">] MinLength: int option
    [<JsonPropertyName "max_length">] MaxLength: int option
    [<JsonPropertyName "autocomplete">] Autocomplete: bool option
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
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: ApplicationCommandType option
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: Dictionary<string, string> option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "description_localizations">] DescriptionLocalizations: Dictionary<string, string> option
    [<JsonPropertyName "options">] Options: ApplicationCommandOption list option
    [<JsonPropertyName "default_member_permissions">] DefaultMemberPermissions: string option
    [<JsonPropertyName "dm_permission">] DmPermission: bool option
    [<JsonPropertyName "nsfw">] Nsfw: bool option
    [<JsonPropertyName "integration_types">] IntegrationTypes: ApplicationIntegrationType list option
    [<JsonPropertyName "contexts">] Contexts: InteractionContextType list option
    [<JsonPropertyName "version">] Version: string
    [<JsonPropertyName "handler">] Handler: ApplicationCommandHandlerType option

    // Only present under certain conditions: https://discord.com/developers/docs/interactions/application-commands#retrieving-localized-commands
    [<JsonPropertyName "name_localized">] NameLocalized: string option
    [<JsonPropertyName "description_localized">] DescriptionLocalized: string option

    // TODO: Create separate type with these special properties? Like invite metadata?
}

// https://discord.com/developers/docs/interactions/application-commands#application-command-permissions-object-application-command-permissions-structure
type ApplicationCommandPermission = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: ApplicationCommandPermissionType
    [<JsonPropertyName "permission">] Permission: bool
}

// https://discord.com/developers/docs/interactions/application-commands#application-command-permissions-object-guild-application-command-permissions-structure
type GuildApplicationCommandPermissions = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "permissions">] Permissions: ApplicationCommandPermission list
}

[<JsonConverter(typeof<InteractionCallbackDataConverter>)>]
type InteractionCallbackData =
    | Message of InteractionCallbackMessageData
    | Autocomplete of InteractionCallbackAutocompleteData
    | Modal of InteractionCallbackModalData

and InteractionCallbackDataConverter () =
    inherit JsonConverter<InteractionCallbackData> () with
        override _.Read (reader, typeToConvert, options) = 
            let success, document = JsonDocument.TryParseValue(&reader)

            if not success then
                raise (JsonException())

            // Using required properties only present on single types to determine which since there is no `type`
            // property for interaction callback data types.

            let isAutocomplete, _ = document.RootElement.TryGetProperty "choices"
            let isModal, _ = document.RootElement.TryGetProperty "title"

            let json = document.RootElement.GetRawText()

            if isAutocomplete then
                InteractionCallbackData.Autocomplete (FsJson.deserialize<InteractionCallbackAutocompleteData> json)
            else if isModal then
                InteractionCallbackData.Modal (FsJson.deserialize<InteractionCallbackModalData> json)
            else
                InteractionCallbackData.Message (FsJson.deserialize<InteractionCallbackMessageData> json)
            
        override _.Write (writer, value, options) =
            match value with
            | InteractionCallbackData.Message m -> FsJson.serialize m |> writer.WriteRawValue
            | InteractionCallbackData.Autocomplete a -> FsJson.serialize a |> writer.WriteRawValue
            | InteractionCallbackData.Modal m -> FsJson.serialize m |> writer.WriteRawValue

and InteractionCallbackMessageData = {
    [<JsonPropertyName "tts">] Tts: bool option
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "embeds">] Embeds: Embed list option
    [<JsonPropertyName "allowed_mentions">] AllowedMentions: AllowedMentions option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "attachments">] Attachments: Attachment list option
    [<JsonPropertyName "poll">] Poll: Poll option
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
    [<JsonPropertyName "choices">] Choices: ApplicationCommandOptionChoice list
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
    [<JsonPropertyName "custom_id">] CustomId: string
    [<JsonPropertyName "title">] Title: string
    [<JsonPropertyName "components">] Components: Component list
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

type ActivityTimestamps = {
    [<JsonPropertyName "start">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] Start: DateTime option
    [<JsonPropertyName "end">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] End: DateTime option
}

type ActivityEmoji = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "animated">] Animated: bool option
}

type ActivityParty = {
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "size">] Size: (int * int) option
}

type ActivityAssets = {
    [<JsonPropertyName "large_image">] LargeImage: string option
    [<JsonPropertyName "large_text">] LargeText: string option
    [<JsonPropertyName "small_image">] SmallImage: string option
    [<JsonPropertyName "small_text">] SmallText: string option
}

type ActivitySecrets = {
    [<JsonPropertyName "join">] Join: string option
    [<JsonPropertyName "spectate">] Spectate: string option
    [<JsonPropertyName "matcch">] Match: string option
}

type ActivityButton = {
    [<JsonPropertyName "label">] Label: string
    [<JsonPropertyName "url">] Url: string
}

// https://discord.com/developers/docs/topics/gateway-events#activity-object-activity-structure
type Activity = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: ActivityType
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "created_at">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] CreatedAt: DateTime option
    [<JsonPropertyName "timestamps">] Timestamps: ActivityTimestamps option
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "details">] Details: string option
    [<JsonPropertyName "state">] State: string option
    [<JsonPropertyName "emoji">] Emoji: ActivityEmoji option
    [<JsonPropertyName "party">] Party: ActivityParty option
    [<JsonPropertyName "assets">] Assets: ActivityAssets option
    [<JsonPropertyName "secrets">] Secrets: ActivitySecrets option
    [<JsonPropertyName "instance">] Instance: bool option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "buttons">] Buttons: ActivityButton list option
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
    [<JsonPropertyName "type">] Type: ApplicationRoleConnectionMetadataType
    [<JsonPropertyName "key">] Key: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: IDictionary<string, string> option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "description_localizations">] DescriptionLocalizations: IDictionary<string, string> option
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
    [<JsonPropertyName "platform_name">] PlatformName: string option
    [<JsonPropertyName "platform_username">] PlatformUsername: string option
    [<JsonPropertyName "metadata">] Metadata: ApplicationRoleConnectionMetadata
}

type FollowedChannel = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "webhook_id">] WebhookId: string
}

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

// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-location-object
type ActivityLocation = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "kind">] Kind: ActivityLocationKind
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
}

// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-instance-object
type ActivityInstance = {
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "instance_id">] InstanceId: string
    [<JsonPropertyName "launch_id">] LaunchId: string
    [<JsonPropertyName "location">] Location: ActivityLocation
    [<JsonPropertyName "users">] Users: string list
}

// https://discord.com/developers/docs/resources/guild#integration-account-object-integration-account-structure
type GuildIntegrationAccount = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
}

// https://discord.com/developers/docs/resources/guild#integration-application-object-integration-application-structure
type GuildIntegrationApplication = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "bot">] Bot: User option
}

// https://discord.com/developers/docs/resources/guild#integration-object-integration-structure
type GuildIntegration = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: GuildIntegrationType
    [<JsonPropertyName "enabled">] Enabled: bool
    [<JsonPropertyName "syncing">] Syncing: bool option
    [<JsonPropertyName "role_id">] RoleId: string option
    [<JsonPropertyName "enable_emoticons">] EnableEmoticons: bool option
    [<JsonPropertyName "expire_behavior">] ExpireBehavior: IntegrationExpireBehaviorType option
    [<JsonPropertyName "expire_grace_period">] ExpireGracePeriod: int option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "account">] Account: GuildIntegrationAccount
    [<JsonPropertyName "synced_at">] SyncedAt: DateTime option
    [<JsonPropertyName "subscriber_count">] SubscriberCount: int option
    [<JsonPropertyName "revoked">] Revoked: bool option
    [<JsonPropertyName "application">] Application: GuildIntegrationApplication option
    [<JsonPropertyName "scopes">] Scopes: OAuth2Scope list option
}

// https://discord.com/developers/docs/resources/user#connection-object-connection-structure
type Connection = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: ConnectionServiceType
    [<JsonPropertyName "revoked">] Revoked: bool option
    [<JsonPropertyName "integrations">] Integrations: GuildIntegration list option
    [<JsonPropertyName "verified">] Verified: bool
    [<JsonPropertyName "friend_sync">] FriendSync: bool
    [<JsonPropertyName "show_activity">] ShowActivity: bool
    [<JsonPropertyName "two_way_link">] TwoWayLink: bool
    [<JsonPropertyName "visibility">] Visibility: ConnectionVisibilityType
}

// https://discord.com/developers/docs/resources/guild#ban-object-ban-structure
type GuildBan = {
    [<JsonPropertyName "reason">] Reason: string option
    [<JsonPropertyName "user">] User: User
}

// https://discord.com/developers/docs/resources/webhook#webhook-object-webhook-structure
type Webhook = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "webhook_type">] Type: WebhookType
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "avatar">] Avatar: string option
    [<JsonPropertyName "token">] Token: string option
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "source_guild">] SourceGuild: Guild option
    [<JsonPropertyName "source_channel">] SourceChannel: Channel option
    [<JsonPropertyName "url">] Url: string option
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-change-object
type AuditLogChange = {
    [<JsonPropertyName "new_value">] NewValue: obj option
    [<JsonPropertyName "old_value">] OldValue: obj option
    [<JsonPropertyName "key">] Key: string
    // TODO: Determine what possible types the values can be and create discriminated union for them
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-optional-audit-entry-info
type AuditLogEntryOptionalInfo = {
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "auto_moderation_rule_name">] AutoModerationRuleName: string option
    [<JsonPropertyName "auto_moderation_rule_trigger_type">] AutoModerationRuleTriggerType: string option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "count">] Count: string option
    [<JsonPropertyName "delete_member_days">] DeleteMemberDays: string option
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "members_removed">] MembersRemoved: string option
    [<JsonPropertyName "message_id">] MessageId: string option
    [<JsonPropertyName "role_name">] RoleName: string option
    [<JsonPropertyName "type">] Type: string option
    [<JsonPropertyName "integration_type">] IntegrationType: string option
    // TODO: Determine if the documentation is incorrect about everything being strings
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-audit-log-entry-structure
type AuditLogEntry = {
    [<JsonPropertyName "target_id">] TargetId: string option
    [<JsonPropertyName "changes">] Changes: AuditLogChange list option
    [<JsonPropertyName "user_id">] UserId: string option
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "action_type">] ActionType: AuditLogEventType
    [<JsonPropertyName "options">] Options: AuditLogEntryOptionalInfo option
    [<JsonPropertyName "reason">] Reason: string option
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-object-audit-log-structure
type AuditLog = {
    [<JsonPropertyName "application_commands">] ApplicationCommands: ApplicationCommand list
    [<JsonPropertyName "audit_log_entries">] AuditLogEntries: AuditLogEntry list
    [<JsonPropertyName "auto_moderation_rules">] AutoModerationRules: AutoModerationRule list
    [<JsonPropertyName "guild_scheduled_events">] GuildScheduledEvents: GuildScheduledEvent list
    [<JsonPropertyName "integrations">] Integrations: GuildIntegration list
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "users">] Users: User list
    [<JsonPropertyName "webhooks">] Webhooks: Webhook list
}

// https://discord.com/developers/docs/resources/voice#voice-region-object-voice-region-structure
type VoiceRegion = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "optimal">] Optimal: bool
    [<JsonPropertyName "deprecated">] Deprecated: bool
    [<JsonPropertyName "custom">] Custom: bool
}

// https://discord.com/developers/docs/topics/gateway#session-start-limit-object-session-start-limit-structure
type SessionStartLimit = {
    [<JsonPropertyName "total">] Total: int
    [<JsonPropertyName "remaining">] Remaining: int
    [<JsonPropertyName "reset_after">] ResetAfter: int
    [<JsonPropertyName "max_concurrency">] MaxConcurrency: int
}

// https://discord.com/developers/docs/topics/gateway-events#identify-identify-connection-properties
type ConnectionProperties = {
    [<JsonPropertyName "os">] OperatingSystem: string
    [<JsonPropertyName "browser">] Browser: string
    [<JsonPropertyName "device">] Device: string
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
    [<JsonPropertyName "op">] Opcode: GatewayOpcode
    [<JsonPropertyName "t">] EventName: string option
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
    [<JsonPropertyName "s">] Sequence: int option
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
    [<JsonPropertyName "op">] Opcode: GatewayOpcode
    [<JsonPropertyName "d">] Data: 'a
    [<JsonPropertyName "s">] Sequence: int option
    [<JsonPropertyName "t">] EventName: string option
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
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: SkuType
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "slug">] Slug: string
    [<JsonPropertyName "flags">] Flags: int
}

// https://discord.com/developers/docs/resources/subscription#subscription-object
type Subscription = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "sku_id">] SkuId: string
    [<JsonPropertyName "entitlement_ids">] EntitlmentIds: string list
    [<JsonPropertyName "current_period_start">] CurrentPeriodStart: DateTime
    [<JsonPropertyName "current_period_end">] CurrentPeriodEnd: DateTime
    [<JsonPropertyName "status">] Status: SubscriptionStatusType
    [<JsonPropertyName "created_at">] CanceledAt: DateTime option
    [<JsonPropertyName "country">] Country: string option
}

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

type InteractionCallback = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: InteractionType
    [<JsonPropertyName "activity_instance_id">] ActivityInstanceId: string option
    [<JsonPropertyName "response_message_id">] ResponseMessageId: string option
    [<JsonPropertyName "response_message_loading">] ResponseMessageLoading: bool option
    [<JsonPropertyName "response_message_ephemeral">] ResponseMessageEphemeral: bool option
}

type InteractionCallbackResource = {
    [<JsonPropertyName "type">] Type: InteractionCallbackType
    [<JsonPropertyName "activity_instance">] ActivityInstance: ActivityInstance option
    [<JsonPropertyName "message">] Message: Message option
}
