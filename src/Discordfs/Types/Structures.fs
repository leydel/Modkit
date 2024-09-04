﻿namespace Modkit.Discordfs.Types

open FSharp.Json
open System
open System.Collections.Generic

#nowarn "49"

type DefaultReaction = {
    [<JsonField("emoji_id")>]
    EmojiId: string option
    
    [<JsonField("emoji_name")>]
    EmojiName: string option
}

type WelcomeScreenChannel = {
    [<JsonField("channel_id")>]
    ChannelId: string

    [<JsonField("description")>]
    Description: string
    
    [<JsonField("emoji_id")>]
    EmojiId: string option

    [<JsonField("emoji_name")>]
    EmojiName: string option
}

type WelcomeScreen = {
    [<JsonField("description")>]
    Description: string option

    [<JsonField("welcome_channels")>]
    WelcomeChannels: WelcomeScreenChannel list
}

type CommandInteractionDataOption = {
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ApplicationCommandOptionType
    
    [<JsonField("value", Transform = typeof<CommandInteractionDataOptionValueTransform>)>]
    Value: CommandInteractionDataOptionValue option
    
    [<JsonField("options")>]
    Options: CommandInteractionDataOption list option

    [<JsonField("focused")>]
    Focused: bool option
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
    [<JsonField("id")>]
    Id: string

    [<JsonField("filename")>]
    Filename: string

    [<JsonField("description")>]
    Description: string

    [<JsonField("content_type")>]
    ContentType: string option

    [<JsonField("size")>]
    Size: int

    [<JsonField("url")>]
    Url: string

    [<JsonField("proxy_url")>]
    ProxyUrl: string

    [<JsonField("height")>]
    Height: int option

    [<JsonField("width")>]
    Width: int option

    [<JsonField("ephemeral")>]
    Ephemeral: bool option

    [<JsonField("duration_secs")>]
    DurationSecs: float option

    [<JsonField("waveform")>]
    Waveform: string option

    [<JsonField("flags")>]
    Flags: int option
}

type RoleTags = {
    [<JsonField("bot_id")>]
    BotId: string option
    
    [<JsonField("integration_id")>]
    IntegrationId: string option

    [<JsonField("premium_subscriber")>]
    PremiumSubscriber: unit option

    [<JsonField("subscription_listing_id")>]
    SubscriptionListingId: string option
    
    [<JsonField("available_for_purchase")>]
    AvailableForPurchase: unit option
    
    [<JsonField("guild_connections")>]
    GuildConnections: unit option
}

// TODO: Figure out how to transform above `unit option` types to `bool` where null = true & undefined = false

// You'd need to do some testing but you can probably rely on default bool being false and fromTargetType never being called. If it is called you just return true and ignore the value
// https://discord.com/channels/196693847965696000/1279795576569069714/1279974826458484816

type Role = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("name")>]
    Name: string

    [<JsonField("color")>]
    Color: int

    [<JsonField("hoist")>]
    Hoist: bool

    [<JsonField("icon")>]
    Icon: string option

    [<JsonField("unicode_emoji")>]
    UnicodeEmoji: string option

    [<JsonField("position")>]
    Position: int

    [<JsonField("permissions")>]
    Permissions: string

    [<JsonField("managed")>]
    Managed: bool

    [<JsonField("mentionable")>]
    Mentionable: bool

    [<JsonField("tags")>]
    Tags: RoleTags option

    [<JsonField("flags")>]
    Flags: int
}

type Entitlement = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("sku_id")>]
    SkuId: string

    [<JsonField("application_id")>]
    ApplicationId: string

    [<JsonField("user_id")>]
    UserId: string option

    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: EntitlementType

    [<JsonField("deleted")>]
    Deleted: bool

    [<JsonField("starts_at")>]
    StartsAt: DateTime option

    [<JsonField("ends_at")>]
    EndsAt: DateTime option

    [<JsonField("guild_id")>]
    GuildId: string option

    [<JsonField("consumed")>]
    Consumed: bool option
}

type AvatarDecorationData = {
    [<JsonField("asset")>]
    Asset: string

    [<JsonField("sku_id")>]
    SkuId: string
}

type User = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("username")>]
    Username: string

    [<JsonField("discriminator")>]
    Discriminator: string

    [<JsonField("global_name")>]
    GlobalName: string option

    [<JsonField("avatar")>]
    Avatar: string option

    [<JsonField("bot")>]
    Bot: bool option

    [<JsonField("system")>]
    System: bool option

    [<JsonField("mfa_enabled")>]
    MfaEnabled: bool option

    [<JsonField("banner")>]
    Banner: string option

    [<JsonField("accent_color")>]
    AccentColor: int option

    [<JsonField("locale")>]
    Locale: string option

    [<JsonField("verified")>]
    Verified: bool option

    [<JsonField("email")>]
    Email: string option

    [<JsonField("flags")>]
    Flags: int option

    [<JsonField("premium_type", EnumValue = EnumMode.Value)>]
    PremiumType: UserPremiumType option

    [<JsonField("public_flags")>]
    PublicFlags: int option

    [<JsonField("avatar_decoration_data")>]
    AvatarDecorationData: AvatarDecorationData option
}

type GuildMember = {
    [<JsonField("user")>]
    User: User option
    
    [<JsonField("nick")>]
    Nick: string option

    [<JsonField("avatar")>]
    Avatar: string option

    [<JsonField("roles")>]
    Roles: string list

    [<JsonField("joined_at")>]
    JoinedAt: DateTime option

    [<JsonField("premium_since")>]
    PremiumSince: DateTime option

    [<JsonField("deaf")>]
    Deaf: bool

    [<JsonField("mute")>]
    Mute: bool

    [<JsonField("flags")>]
    Flags: int

    [<JsonField("pending")>]
    Pending: bool option

    [<JsonField("permissions")>]
    Permissions: string option

    [<JsonField("communication_disabled_until")>]
    CommunicationDisabledUntil: DateTime option

    [<JsonField("avatar_decoration_metadata")>]
    AvatarDecorationData: AvatarDecorationData option
}

type Emoji = {
    [<JsonField("id")>]
    Id: string option
    
    [<JsonField("name")>]
    Name: string option

    [<JsonField("roles")>]
    Roles: string list option

    [<JsonField("user")>]
    User: User option

    [<JsonField("require_colons")>]
    RequireColons: bool option

    [<JsonField("managed")>]
    Managed: bool option

    [<JsonField("animated")>]
    Animated: bool option

    [<JsonField("available")>]
    Available: bool option
}

type Sticker = {
    [<JsonField("id")>]
    Id: string

    [<JsonField("pack_id")>]
    PackId: string option

    [<JsonField("name")>]
    Name: string

    [<JsonField("description")>]
    Description: string option

    [<JsonField("tags")>]
    Tags: string

    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: StickerType

    [<JsonField("format_type", EnumValue = EnumMode.Value)>]
    FormatType: StickerFormatType

    [<JsonField("available")>]
    Available: bool option

    [<JsonField("guild_id")>]
    GuildId: string option

    [<JsonField("user")>]
    User: User option

    [<JsonField("sort_value")>]
    SortValue: int option
}

type Guild = {
    [<JsonField("id")>]
    Id: string

    [<JsonField("name")>]
    Name: string

    [<JsonField("icon")>]
    Icon: string option

    [<JsonField("icon_hash")>]
    IconHash: string option

    [<JsonField("splash")>]
    Splash: string option

    [<JsonField("discovery_splash")>]
    DiscoverySplash: string option

    [<JsonField("owner")>]
    Owner: bool option

    [<JsonField("owner_id")>]
    OwnerId: string

    [<JsonField("permissions")>]
    Permissions: string option

    [<JsonField("afk_channel_id")>]
    AfkChannelId: string option

    [<JsonField("afk_timeout")>]
    AfkTimeout: int

    [<JsonField("widget_enabled")>]
    WidgetEnabled: bool option

    [<JsonField("widget_channel_id")>]
    WidgetChannelId: string option

    [<JsonField("verification_level", EnumValue = EnumMode.Value)>]
    VerificationLevel: GuildVerificationLevel

    [<JsonField("default_message_notifications", EnumValue = EnumMode.Value)>]
    DefaultMessageNotifications: GuildMessageNotificationLevel

    [<JsonField("explicit_content_filter", EnumValue = EnumMode.Value)>]
    ExplicitContentFilter: GuildExplicitContentFilterLevel

    [<JsonField("roles")>]
    Roles: Role list

    [<JsonField("emojis")>]
    Emojis: Emoji list

    [<JsonField("features")>]
    Featuers: string list

    [<JsonField("mfa_level", EnumValue = EnumMode.Value)>]
    MfaLevel: GuildMfaLevel

    [<JsonField("application_id")>]
    ApplicationId: string option

    [<JsonField("system_channel_id")>]
    SystemChannelId: string option

    [<JsonField("system_channel_flags")>]
    SystemChannelFlags: int

    [<JsonField("rules_channel_id")>]
    RulesChannelId: string option

    [<JsonField("max_presences")>]
    MaxPresences: int option

    [<JsonField("max_members")>]
    MaxMembers: int option

    [<JsonField("vanity_url_code")>]
    VanityUrlCode: string option

    [<JsonField("description")>]
    Description: string option

    [<JsonField("banner")>]
    Banner: string option

    [<JsonField("premium_tier", EnumValue = EnumMode.Value)>]
    PremiumTier: GuildPremiumTier

    [<JsonField("premium_subscription_count")>]
    PremiumSubscriptionCount: int option

    [<JsonField("preferred_locale")>]
    PreferredLocale: string

    [<JsonField("public_updates_channel_id")>]
    PublicUpdatesChannelId: string option

    [<JsonField("max_video_channel_users")>]
    MaxVideoChannelUsers: int option

    [<JsonField("max_stage_video_channel_users")>]
    MaxStageVideoChannelUsers: int option

    [<JsonField("approximate_member_count")>]
    ApproximateMemberCount: int option

    [<JsonField("approximate_presence_count")>]
    ApproximatePresenceCount: int option

    [<JsonField("welcome_screen")>]
    WelcomeScreen: WelcomeScreen option

    [<JsonField("nsfw_level", EnumValue = EnumMode.Value)>]
    NsfwLevel: GuildNsfwLevel

    [<JsonField("stickers")>]
    Stickers: Sticker list option

    [<JsonField("premium_progress_bar_enabled")>]
    PremiumProgressBarEnabled: bool

    [<JsonField("safety_alerts_channel_id")>]
    SafetyAlertsChannelId: string option
}

type ChannelMention = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("guild_id")>]
    GuildId: string
    
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ChannelType
    
    [<JsonField("name")>]
    Name: string
}

type PermissionOverwrite = {
    [<JsonField("id")>]
    Id: string

    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: PermissionOverwriteType

    [<JsonField("allow")>]
    Allow: string

    [<JsonField("deny")>]
    Deny: string
}

type ThreadMetadata = {
    [<JsonField("archived")>]
    Archived: bool
    
    [<JsonField("auto_archive_duration")>]
    AutoArchiveDuration: int
    
    [<JsonField("archive_timestamp")>]
    ArchiveTimestamp: DateTime
    
    [<JsonField("locked")>]
    Locked: bool
    
    [<JsonField("invitable")>]
    Invitable: bool option
    
    [<JsonField("create_timestamp")>]
    CreateTimestamp: DateTime option
}

type ThreadMember = {
    [<JsonField("id")>]
    Id: string option
    
    [<JsonField("user_id")>]
    UserId: string option
    
    [<JsonField("join_timestamp")>]
    JoinTimestamp: DateTime
    
    [<JsonField("flags")>]
    Flags: int
    
    [<JsonField("member")>]
    Member: GuildMember option
}

type ChannelTag = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("moderated")>]
    Moderated: bool
    
    [<JsonField("emoji_id")>]
    EmojiId: string option
    
    [<JsonField("emoji_name")>]
    EmojiName: string option
}

type EmbedFooter = {
    [<JsonField("text")>]
    Text: string
    
    [<JsonField("icon_url")>]
    IconUrl: string option
    
    [<JsonField("proxy_icon_url")>]
    ProxyIconUrl: string option
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
    [<JsonField("url")>]
    Url: string
    
    [<JsonField("proxy_url")>]
    ProxyUrl: string option
    
    [<JsonField("height")>]
    Height: int option
    
    [<JsonField("width")>]
    Width: int option
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
    [<JsonField("url")>]
    Url: string
    
    [<JsonField("proxy_url")>]
    ProxyUrl: string option
    
    [<JsonField("height")>]
    Height: int option
    
    [<JsonField("width")>]
    Width: int option
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
    [<JsonField("url")>]
    Url: string option
    
    [<JsonField("proxy_url")>]
    ProxyUrl: string option
    
    [<JsonField("height")>]
    Height: int option
    
    [<JsonField("width")>]
    Width: int option
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
    [<JsonField("name")>]
    Name: string option

    [<JsonField("url")>]
    Url: string option
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
    [<JsonField("name")>]
    Name: string

    [<JsonField("url")>]
    Url: string option

    [<JsonField("icon_url")>]
    IconUrl: string option
    
    [<JsonField("proxy_icon_url")>]
    ProxyIconUrl: string option
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
    [<JsonField("name")>]
    Name: string

    [<JsonField("value")>]
    Value: string

    [<JsonField("inline")>]
    Inline: bool option
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
    [<JsonField("title")>]
    Title: string option
    
    [<JsonField("type")>]
    Type: string option
    
    [<JsonField("description")>]
    Description: string option

    [<JsonField("url")>]
    Url: string option

    [<JsonField("timestamp")>]
    Timestamp: DateTime option

    [<JsonField("color")>]
    Color: int option

    [<JsonField("footer")>]
    Footer: EmbedFooter option

    [<JsonField("image")>]
    Image: EmbedImage option

    [<JsonField("thumbnail")>]
    Thumbnail: EmbedThumbnail option

    [<JsonField("video")>]
    Video: EmbedVideo option

    [<JsonField("provider")>]
    Provider: EmbedProvider option

    [<JsonField("author")>]
    Author: EmbedAuthor option

    [<JsonField("fields")>]
    Fields: EmbedField list option
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
    [<JsonField("burst")>]
    Burst: int

    [<JsonField("normal")>]
    Normal: int
}

type Reaction = {
    [<JsonField("count")>]
    Count: int
    
    [<JsonField("count_details")>]
    CountDetails: ReactionCountDetails

    [<JsonField("me")>]
    Me: bool

    [<JsonField("me_burst")>]
    MeBurst: bool

    [<JsonField("emoji")>]
    Emoji: Emoji

    [<JsonField("burst_colors")>]
    BurstColors: int list
}

type MessageActivity = {
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: MessageActivityType

    [<JsonField("party_id")>]
    PartyId: string option
}

type OAuth2InstallParams = {
    [<JsonField("scopes")>]
    Scopes: string list

    [<JsonField("permissions")>]
    Permissions: string
}

type ApplicationIntegrationTypeConfiguration = {
    [<JsonField("oauth2_install_params")>]
    Oauth2InstallParams: OAuth2InstallParams option
}

type TeamMember = {
    [<JsonField("membership_state", EnumValue = EnumMode.Value)>]
    MembershipState: TeamMembershipState
    
    [<JsonField("team_id")>]
    TeamId: string

    [<JsonField("user")>]
    User: User

    [<JsonField("role")>]
    Role: string
}

type Team = {
    [<JsonField("icon")>]
    Icon: string option
    
    [<JsonField("id")>]
    Id: string

    [<JsonField("members")>]
    Members: TeamMember list

    [<JsonField("name")>]
    Name: string

    [<JsonField("owner_user_id")>]
    OwnerUserId: string
}

type Application = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("name")>]
    Name: string

    [<JsonField("icon")>]
    Icon: string option

    [<JsonField("description")>]
    Description: string

    [<JsonField("rpc_origins")>]
    RpcOrigins: string list option

    [<JsonField("bot_public")>]
    BotPublic: bool

    [<JsonField("bot_require_code_grant")>]
    BotRequireCodeGrant: bool

    [<JsonField("bot")>]
    Bot: User option

    [<JsonField("terms_of_Service_url")>]
    TermsOfServiceUrl: string option

    [<JsonField("privacy_policy_url")>]
    PrivacyPolicyUrl: string option

    [<JsonField("owner")>]
    Owner: User option

    [<JsonField("verify_key")>]
    VerifyKey: string

    [<JsonField("team")>]
    Team: Team option

    [<JsonField("guild_id")>]
    GuildId: string option

    [<JsonField("guild")>]
    Guild: Guild option

    [<JsonField("primary_sku_id")>]
    PrimarySkuId: string option

    [<JsonField("slug")>]
    Slug: string option

    [<JsonField("cover_image")>]
    CoverImage: string option

    [<JsonField("flags")>]
    Flags: int option

    [<JsonField("approximate_guild_count")>]
    ApproximateGuildCount: int option

    [<JsonField("redirect_uris")>]
    RedirectUris: string list option

    [<JsonField("interactions_endpoint_url")>]
    InteractionsEndpointUrl: string option

    [<JsonField("role_connections_verification_url")>]
    RoleConnectionsVerificationUrl: string option

    [<JsonField("tags")>]
    Tags: string list option

    [<JsonField("install_params")>]
    InstallParams: OAuth2InstallParams option

    [<JsonField("integration_types_config")>]
    IntegrationTypesConfig: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option

    [<JsonField("custom_install_url")>]
    CustomInstallUrl: string option
}

type MessageReference = {
    [<JsonField("message_id")>]
    MessageId: string option

    [<JsonField("channel_id")>]
    ChannelId: string option

    [<JsonField("guild_id")>]
    GuildId: string option

    [<JsonField("fail_if_not_exists")>]
    FailIfNotExists: bool option
}

type MessageInteractionMetadata = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("type")>]
    Type: InteractionType
    
    [<JsonField("user")>]
    User: User
    
    [<JsonField("authorizing_integration_owners")>]
    AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>

    [<JsonField("original_response_message_id")>]
    OriginalResponseMessage: string option

    [<JsonField("interacted_message_id")>]
    InteractedMessageId: string option

    [<JsonField("triggering_interaction_metadata")>]
    TriggeringInteractionMetadata: MessageInteractionMetadata option
}

type MessageInteraction = {
    [<JsonField("id")>]
    Id: string

    [<JsonField("type")>]
    Type: InteractionType

    [<JsonField("name")>]
    Name: string

    [<JsonField("user")>]
    User: User

    [<JsonField("member")>]
    Member: GuildMember option
}

type RoleSubscriptionData = {
    [<JsonField("role_subscription_listing_id")>]
    RoleSubscriptionListingId: string

    [<JsonField("tier_name")>]
    TierName: string

    [<JsonField("total_months_subscribed")>]
    TotalMonthsSubscribed: int

    [<JsonField("is_renewal")>]
    IsRenewal: bool
}

type PollMedia = {
    [<JsonField("text")>]
    Text: string option

    [<JsonField("emoji")>]
    Emoji: Emoji option
}

type PollAnswer = {
    [<JsonField("answer_id")>]
    AnswerId: int

    [<JsonField("poll_media")>]
    PollMedia: PollMedia
}

type PollAnswerCount = {
    [<JsonField("id")>]
    Id: string

    [<JsonField("count")>]
    Count: int

    [<JsonField("me_voted")>]
    MeVoted: bool
}

type PollResults = {
    [<JsonField("is_finalized")>]
    IsFinalized: bool

    [<JsonField("answer_counts")>]
    AnswerCounts: PollAnswerCount list
}

type Poll = {
    [<JsonField("question")>]
    Question: PollMedia

    [<JsonField("answers")>]
    Answers: PollAnswer list

    [<JsonField("expiry")>]
    Expiry: DateTime option

    [<JsonField("allow_multiselect")>]
    AllowMultiselect: bool

    [<JsonField("layout_type", EnumValue = EnumMode.Value)>]
    LayoutType: PollLayoutType

    [<JsonField("results")>]
    Results: PollResults option
}

type MessageCall = {
    [<JsonField("participants")>]
    Participants: string list

    [<JsonField("ended_timestamp")>]
    EndedTimestamp: DateTime option
}

type SelectMenuOption = {
    [<JsonField("label")>]
    Label: string

    [<JsonField("value")>]
    Value: string

    [<JsonField("description")>]
    Description: string option

    [<JsonField("emoji")>]
    Emoji: Emoji option

    [<JsonField("default")>]
    Default: bool option
}

type SelectMenuDefaultValue = {
    [<JsonField("id")>]
    Id: string

    [<JsonField("type")>]
    Type: string
}

type ActionRowComponent = {
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ComponentType
    
    [<JsonField("components", Transform = typeof<ComponentTransform>)>]
    Components: Component list
}

and ButtonComponent = {
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ComponentType

    [<JsonField("style", EnumValue = EnumMode.Value)>]
    Style: ButtonStyle

    [<JsonField("label")>]
    Label: string

    [<JsonField("emoji")>]
    Emoji: Emoji option
    
    [<JsonField("custom_id")>]
    CustomId: string option

    [<JsonField("url")>]
    Url: string option

    [<JsonField("disabled")>]
    Disabled: bool option
}

and SelectMenuComponent = {
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ComponentType

    [<JsonField("custom_id")>]
    CustomId: string

    [<JsonField("options")>]
    Options: SelectMenuOption list option

    [<JsonField("channel_types", EnumValue = EnumMode.Value)>]
    ChannelTypes: ChannelType list option

    [<JsonField("placeholder")>]
    Placeholder: string option

    [<JsonField("default_values")>]
    DefaultValues: SelectMenuDefaultValue option

    [<JsonField("min_values")>]
    MinValues: int option

    [<JsonField("max_values")>]
    MaxValues: int option

    [<JsonField("disabled")>]
    Disabled: bool option
}

and TextInputComponent = {
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ComponentType

    [<JsonField("custom_id")>]
    CustomId: string

    [<JsonField("style", EnumValue = EnumMode.Value)>]
    Style: TextInputStyle

    [<JsonField("label")>]
    Label: string

    [<JsonField("min_length")>]
    MinLength: int option

    [<JsonField("max_length")>]
    MaxLength: int option

    [<JsonField("required")>]
    Required: bool option

    [<JsonField("value")>]
    Value: string option

    [<JsonField("placeholder")>]
    Placeholder: string option
}

and Component =
    | ActionRow of ActionRowComponent
    | Button of ButtonComponent
    | SelectMenu of SelectMenuComponent
    | TextInput of TextInputComponent

and ComponentTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<obj>

        member _.toTargetType value =
            match value :?> Component with
            | Component.ActionRow v -> v
            | Component.Button v -> v
            | Component.SelectMenu v -> v
            | Component.TextInput v -> v

        member _.fromTargetType value =
            match value with
            | :? ActionRowComponent as v -> Component.ActionRow v
            | :? ButtonComponent as v -> Component.Button v
            | :? SelectMenuComponent as v -> Component.SelectMenu v
            | :? TextInputComponent as v -> Component.TextInput v
            | _ -> failwith "Unexpected Component type"

type Channel = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ChannelType
    
    [<JsonField("guild_id")>]
    GuildId: string option
    
    [<JsonField("position")>]
    Position: int option
    
    [<JsonField("permission_overwrites")>]
    PermissionOverwrites: PermissionOverwrite list option

    [<JsonField("name")>]
    Name: string option
    
    [<JsonField("topic")>]
    Topic: string option
    
    [<JsonField("nsfw")>]
    Nsfw: bool option
    
    [<JsonField("last_message_id")>]
    LastMessageId: string option
    
    [<JsonField("bitrate")>]
    Bitrate: int option
    
    [<JsonField("user_limit")>]
    UserLimit: int option
    
    [<JsonField("rate_limit_per_user")>]
    RateLimitPerUser: int option
    
    [<JsonField("recipients")>]
    Recipients: User list option
    
    [<JsonField("icon")>]
    Icon: string option
    
    [<JsonField("owner_id")>]
    OwnerId: string option
    
    [<JsonField("application_id")>]
    ApplicationId: string option
    
    [<JsonField("managed")>]
    Managed: bool option
    
    [<JsonField("parent_id")>]
    ParentId: string option
    
    [<JsonField("last_pin_timestamp")>]
    LastPinTimestamp: DateTime option
    
    [<JsonField("rtc_region")>]
    RtcRegion: string option
    
    [<JsonField("video_quality_mode", EnumValue = EnumMode.Value)>]
    VideoQualityMode: VideoQualityMode option
    
    [<JsonField("message_count")>]
    MessageCount: int option
    
    [<JsonField("member_count")>]
    MemberCount: int option
    
    [<JsonField("thread_metadata")>]
    ThreadMetadata: ThreadMetadata option
    
    [<JsonField("member")>]
    Member: ThreadMember option
    
    [<JsonField("default_auto_archive_duration")>]
    DefaultAutoArchiveDuration: int option
    
    [<JsonField("permissions")>]
    Permissions: string option
    
    [<JsonField("flags")>]
    Flags: int option
    
    [<JsonField("total_messages_sent")>]
    TotalMessagesSent: int option
    
    [<JsonField("available_tags")>]
    AvailableTags: ChannelTag list option
    
    [<JsonField("applied_tags")>]
    AppliedTags: int list option
    
    [<JsonField("default_reaction_emoji")>]
    DefaultReactionEmoji: DefaultReaction option
    
    [<JsonField("default_thread_rate_limit_per_user")>]
    DefaultThreadRateLimitPerUser: int option
    
    [<JsonField("default_sort_order", EnumValue = EnumMode.Value)>]
    DefaultSortOrder: ChannelSortOrder option
    
    [<JsonField("default_forum_layout", EnumValue = EnumMode.Value)>]
    DefaultForumLayout: ChannelForumLayout option
}

type ResolvedData = {
    [<JsonField("users")>]
    Users: Map<string, User> option
    
    [<JsonField("members")>]
    Members: Map<string, GuildMember> option
    
    [<JsonField("roles")>]
    Roles: Map<string, Role> option
    
    [<JsonField("channels")>]
    Channels: Map<string, Channel> option
    
    [<JsonField("messages")>]
    Messages: Map<string, Message> option
    
    [<JsonField("attachments")>]
    Attachments: Map<string, Attachment> option
}

and Message = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("channel_id")>]
    ChannelId: string

    [<JsonField("author")>]
    Author: User

    [<JsonField("content")>]
    Content: string
    
    [<JsonField("timestamp")>]
    Timestamp: DateTime
    
    [<JsonField("edited_timestamp")>]
    EditedTimestamp: DateTime option
    
    [<JsonField("tts")>]
    Tts: bool
    
    [<JsonField("mention_everyone")>]
    MentionEveryone: bool

    [<JsonField("mentions")>]
    Mentions: User list

    [<JsonField("mention_roles")>]
    MentionRoles: string list

    [<JsonField("mention_channels")>]
    MentionChannels: ChannelMention list

    [<JsonField("attachments")>]
    Attachments: Attachment list

    [<JsonField("embeds")>]
    Embeds: Embed list

    [<JsonField("reactions")>]
    Reactions: Reaction list

    [<JsonField("nonce", Transform = typeof<MessageNonceTransform>)>]
    Nonce: MessageNonce option

    [<JsonField("pinned")>]
    Pinned: bool

    [<JsonField("webhook_id")>]
    WebhookId: string option

    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: MessageType

    [<JsonField("activity")>]
    Activity: MessageActivity option

    [<JsonField("application")>]
    Application: Application option

    [<JsonField("message_reference")>]
    MessageReference: MessageReference option

    [<JsonField("flags")>]
    Flags: int

    [<JsonField("referenced_message")>]
    ReferencedMessage: Message option

    [<JsonField("interaction_metadata")>]
    InteractionMetadata: MessageInteractionMetadata option

    [<JsonField("interaction")>]
    Interaction: MessageInteraction option

    [<JsonField("thread")>]
    Thread: Channel option

    [<JsonField("components", Transform = typeof<ComponentTransform>)>]
    Components: Component list option

    [<JsonField("sticker_items")>]
    StickerItems: Sticker list option

    [<JsonField("position")>]
    Position: int option

    [<JsonField("role_subscription_data")>]
    RoleSubscriptionData: RoleSubscriptionData option

    [<JsonField("resolved")>]
    Resolved: ResolvedData option

    [<JsonField("poll")>]
    Poll: Poll option

    [<JsonField("call")>]
    Call: MessageCall option
}

type Invite = {
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: InviteType

    [<JsonField("code")>]
    Code: string
    
    [<JsonField("guild")>]
    Guild: Guild option
    
    [<JsonField("channel")>]
    Channel: Channel option
    
    [<JsonField("inviter")>]
    Inviter: User option
    
    [<JsonField("target_type", EnumValue = EnumMode.Value)>]
    TargetType: InviteTargetType option
    
    [<JsonField("target_user")>]
    TargetUser: User option
    
    [<JsonField("target_application")>]
    TargetApplication: Application option
    
    [<JsonField("approximate_presence_count")>]
    ApproximatePresenceCount: int option
    
    [<JsonField("approximate_member_count")>]
    ApproximateMemberCount: int option
    
    [<JsonField("expires_at")>]
    ExpiresAt: DateTime

    // TODO: Add `guild_scheduled_event` with type from: https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-structure
}

type InteractionData = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: ApplicationCommandType
    
    [<JsonField("resolved")>]
    Resolved: ResolvedData option

    [<JsonField("options")>]
    Options: CommandInteractionDataOption list option
    
    [<JsonField("guild_id")>]
    GuildId: string option
    
    [<JsonField("target_it")>]
    TargetId: string option
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
    [<JsonField("id")>]
    Id: string

    [<JsonField("application_id")>]
    ApplicationId: string

    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: InteractionType

    [<JsonField("data")>]
    Data: InteractionData option

    [<JsonField("guild")>]
    Guild: Guild option

    [<JsonField("guild_id")>]
    GuildId: string option

    [<JsonField("channel")>]
    Channel: Channel option

    [<JsonField("channel_id")>]
    ChannelId: string option

    [<JsonField("member")>]
    Member: GuildMember option

    [<JsonField("user")>]
    User: User option

    [<JsonField("token")>]
    Token: string

    [<JsonField("version")>]
    Version: int

    [<JsonField("message")>]
    Message: Message option

    [<JsonField("app_permissions")>]
    AppPermissions: string

    [<JsonField("locale")>]
    Locale: string option

    [<JsonField("guild_locale")>]
    GuildLocale: string option

    [<JsonField("entitlements")>]
    Entitlements: Entitlement list

    [<JsonField("authorizing_integration_owners")>]
    AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>

    [<JsonField("context", EnumValue = EnumMode.Value)>]
    Context: InteractionContextType option
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

    static member deserialize(json: string) =
        try Some(Json.deserialize<Interaction> json) with
        | _ -> None

type AllowedMentions = {
    [<JsonField("parse", Transform = typeof<AllowedMentionsParseTypeTransform>)>]
    Parse: AllowedMentionsParseType list
    
    [<JsonField("roles")>]
    Roles: string list option
    
    [<JsonField("users")>]
    Users: string list option
    
    [<JsonField("replied_user")>]
    RepliedUser: bool option
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
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("name_localizations")>]
    NameLocalizations: Dictionary<string, string> option
    
    [<JsonField("value", Transform = typeof<ApplicationCommandOptionChoiceValueTransform>)>]
    Value: ApplicationCommandOptionChoiceValue
}

type ApplicationCommandOption = {
    [<JsonField("type")>]
    Type: ApplicationCommandOptionType
    
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("name_localizations")>]
    NameLocalizations: Dictionary<string, string> option
    
    [<JsonField("description")>]
    Description: string
    
    [<JsonField("description_localizations")>]
    DescriptionOptions: Dictionary<string, string> option
    
    [<JsonField("required")>]
    Required: bool option
    
    [<JsonField("choices")>]
    Choices: ApplicationCommandOptionChoice list option
    
    [<JsonField("options")>]
    Options: ApplicationCommandOption list option
    
    [<JsonField("channel_types")>]
    ChannelTypes: ChannelType list option
    
    [<JsonField("min_value", Transform = typeof<ApplicationCommandMinValueTransform>)>]
    MinValue: ApplicationCommandMinValue option
    
    [<JsonField("max_value", Transform = typeof<ApplicationCommandMaxValueTransform>)>]
    MaxValue: ApplicationCommandMaxValue option
    
    [<JsonField("min_length")>]
    MinLength: int option
    
    [<JsonField("max_length")>]
    MaxLength: int option
    
    [<JsonField("autocomplete")>]
    Autocomplete: bool option
}
with
    static member build(
        Type: ApplicationCommandOptionType,
        Name: string,
        Description: string,
        ?NameLocalizations: Dictionary<string, string>,
        ?DescriptionOptions: Dictionary<string, string>,
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
        DescriptionOptions = DescriptionOptions;
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
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("type")>]
    Type: ApplicationCommandType option
    
    [<JsonField("application_id")>]
    ApplicationId: string
    
    [<JsonField("guild_id")>]
    GuildId: string option
    
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("name_localizations")>]
    NameLocalizations: Dictionary<string, string> option
    
    [<JsonField("description")>]
    Description: string
    
    [<JsonField("description_localizations")>]
    DescriptionLocalizations: Dictionary<string, string> option
    
    [<JsonField("options")>]
    Options: ApplicationCommandOption list option
    
    [<JsonField("default_member_permissions")>]
    DefaultMemberPermissions: string option
    
    [<JsonField("dm_permissions")>]
    DmPermissions: bool option
    
    [<JsonField("nsfw")>]
    Nsfw: bool option
    
    [<JsonField("integration_types")>]
    IntegrationTypes: ApplicationIntegrationType list option
    
    [<JsonField("contexts")>]
    Contexts: InteractionContextType list option
    
    [<JsonField("version")>]
    Version: string

    [<JsonField("handler")>]
    Handler: ApplicationCommandHandlerType option
}

type InteractionCallbackMessageData = {
    [<JsonField("tts")>]
    Tts: bool option
    
    [<JsonField("content")>]
    Content: string option
    
    [<JsonField("embeds")>]
    Embeds: Embed list option
    
    [<JsonField("allowed_mentions")>]
    AllowedMentions: AllowedMentions option
    
    [<JsonField("flags")>]
    Flags: int option
    
    [<JsonField("components", Transform = typeof<ComponentTransform>)>]
    Components: Component list option
    
    [<JsonField("attachments")>]
    Attachments: Attachment list option
    
    [<JsonField("poll")>]
    Poll: Poll option
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
    [<JsonField("choices")>]
    Choices: ApplicationCommandOptionChoice list
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
    [<JsonField("custom_id")>]
    CustomId: string
    
    [<JsonField("title")>]
    Title: string
    
    [<JsonField("components", Transform = typeof<ComponentTransform>)>]
    Components: Component list
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

type InteractionCallbackDataTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<obj>

        member _.toTargetType value =
            match value :?> InteractionCallbackData with
            | InteractionCallbackData.Message v -> v
            | InteractionCallbackData.Autocomplete v -> v
            | InteractionCallbackData.Modal v -> v

        member _.fromTargetType value =
            match value with
            | :? InteractionCallbackMessageData as v -> InteractionCallbackData.Message v
            | :? InteractionCallbackAutocompleteData as v -> InteractionCallbackData.Autocomplete v
            | :? InteractionCallbackModalData as v -> InteractionCallbackData.Modal v
            | _ -> failwith "Unexpected InteractionCallbackData type"

type InteractionCallback = {
    [<JsonField("type", EnumValue = EnumMode.Value)>]
    Type: InteractionCallbackType
    
    [<JsonField("data", Transform = typeof<InteractionCallbackDataTransform>)>]
    Data: InteractionCallbackData option
}
with
    static member build(
        Type: InteractionCallbackType,
        ?Data: InteractionCallbackData
    ) = {
        Type = Type;
        Data = Data;
    }

type SessionStartLimit = {
    [<JsonField("total")>]
    Total: int

    [<JsonField("remaining")>]
    Remaining: int
    
    [<JsonField("reset_after")>]
    ResetAfter: int

    [<JsonField("max_concurrency")>]
    MaxConcurrency: int
}

type GatewayEvent = {
    [<JsonField("op")>]
    Opcode: GatewayOpcode
    
    [<JsonField("d")>]
    Data: obj option
    
    [<JsonField("s")>]
    Sequence: int option
    
    [<JsonField("t")>]
    EventName: string option
}
with
    static member build(
        Opcode: GatewayOpcode,
        ?Data: obj,
        ?Sequence: int,
        ?EventName: string
    ) = {
        Opcode = Opcode;
        Data = Data;
        Sequence = Sequence;
        EventName = EventName;
    }

type ConnectionProperties = {
    [<JsonField("os")>]
    OperatingSystem: string
    
    [<JsonField("browser")>]
    Browser: string
    
    [<JsonField("device")>]
    Device: string
}
with
    static member build(
        OperatingSystem: string
    ) = {
        OperatingSystem = OperatingSystem;
        Browser = "Discordfs";
        Device = "Discordfs";
    }

    static member build(
        OperatingSystem: string,
        Browser: string,
        Device: string
    ) = {
        OperatingSystem = OperatingSystem;
        Browser = Browser;
        Device = Device;
    }

type ActivityTimestamps = {
    [<JsonField("start", Transform = typeof<Transforms.DateTimeEpoch>)>]
    Start: DateTime option
    
    [<JsonField("end", Transform = typeof<Transforms.DateTimeEpoch>)>]
    End: DateTime option
}

type ActivityEmoji = {
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("id")>]
    Id: string option
    
    [<JsonField("animated")>]
    Animated: bool option
}

type ActivityParty = {
    [<JsonField("id")>]
    Id: string option
    
    [<JsonField("size")>]
    Size: (int * int) option
}

type ActivityAssets = {
    [<JsonField("large_image")>]
    LargeImage: string option
    
    [<JsonField("large_text")>]
    LargeText: string option
    
    [<JsonField("small_image")>]
    SmallImage: string option
    
    [<JsonField("small_text")>]
    SmallText: string option
}

type ActivitySecrets = {
    [<JsonField("join")>]
    Join: string option
    
    [<JsonField("spectate")>]
    Spectate: string option
    
    [<JsonField("match")>]
    Match: string option
}

type ActivityButton = {
    [<JsonField("label")>]
    Label: string
    
    [<JsonField("url")>]
    Url: string
}

type Activity = {
    [<JsonField("name")>]
    Name: string
    
    [<JsonField("type")>]
    Type: ActivityType
    
    [<JsonField("url")>]
    Url: string option
    
    [<JsonField("created_at", Transform = typeof<Transforms.DateTimeEpoch>)>]
    CreatedAt: DateTime option
    
    [<JsonField("timestamps")>]
    Timestamps: ActivityTimestamps option
    
    [<JsonField("application_id")>]
    ApplicationId: string option
    
    [<JsonField("details")>]
    Details: string option
    
    [<JsonField("state")>]
    State: string option
    
    [<JsonField("emoji")>]
    Emoji: ActivityEmoji option
    
    [<JsonField("party")>]
    Party: ActivityParty option
    
    [<JsonField("assets")>]
    Assets: ActivityAssets option
    
    [<JsonField("secrets")>]
    Secrets: ActivitySecrets option
    
    [<JsonField("instance")>]
    Instance: bool option
    
    [<JsonField("flags")>]
    Flags: int option
    
    [<JsonField("buttons")>]
    Buttons: ActivityButton list option
}
