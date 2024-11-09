namespace Discordfs.Types

open System
open System.Text.Json
open System.Text.Json.Serialization

type TextInputStyle =
    | SHORT = 1
    | PARAGRAPH = 2

type ButtonStyle =
    | PRIMARY = 1
    | SECONDARY = 2
    | SUCCESS = 3
    | DANGER = 4
    | LINK = 5

type ComponentType =
    | ACTION_ROW = 1
    | BUTTON = 2
    | STRING_SELECT = 3
    | TEXT_INPUT = 4
    | USER_SELECT = 5
    | ROLE_SELECT = 6
    | MENTIONABLE_SELECT = 7
    | CHANNEL_SELECT = 8

type PermissionOverwriteType =
    | ROLE = 0
    | MEMBER = 1

type ChannelForumLayout =
    | NOT_SET = 0
    | LIST_VIEW = 1
    | GALLERY_VIEW = 2

type ChannelSortOrder =
    | LATEST_ACTIVITY = 0
    | CREATION_DATE = 1

type VideoQualityMode =
    | AUTO = 1
    | FULL = 2

type PollLayoutType =
    | DEFAULT = 1

type TeamMembershipState =
    | INVITED = 1
    | ACCEPTED = 2

type MessageActivityType =
    | JOIN = 1
    | SPECTATE = 2
    | LISTEN = 3
    | JOIN_REQUEST = 5

type MessageType =
    | DEFAULT = 0
    | RECIPIENT_ADD = 1
    | RECIPIENT_REMOVE = 2
    | CALL = 3
    | CHANNEL_NAME_CHANGE = 4
    | CHANNEL_ICON_CHANGE = 5
    | CHANNEL_PINNED_MESSAGE = 6
    | USER_JOIN = 7
    | GUILD_BOOST = 8
    | GUILD_BOOST_TIER_1 = 9
    | GUILD_BOOST_TIER_2 = 10
    | GUILD_BOOST_TIER_3 = 11
    | CHANNEL_FOLLOW_ADD = 12
    | GUILD_DISCOVERY_DISQUALIFIED = 14
    | GUILD_DISCOVERY_REQUALIFIED = 15
    | GUILD_DISCOVERY_GRACE_PERIOD_INITIAL_WARNING = 16
    | GUILD_DISCOVERY_GRACE_PERIOD_FINAL_WARNING = 17
    | THREAD_CREATED = 18
    | REPLY = 19
    | CHAT_INPUT_COMMAND = 20
    | THREAD_STARTER_MESSAGE = 21
    | GUILD_INVITE_REMINDER = 22
    | CONTEXT_MENU_COMMAND = 23
    | AUTO_MODERATION_ACTION = 24
    | ROLE_SUBSCRIPTION_PURCHASE = 25
    | INTERACTION_PREMIUM_UPSELL = 26
    | STAGE_START = 27
    | STAGE_END = 28
    | STAGE_SPEAKER = 29
    | STAGE_TOPIC = 31
    | GUILD_APPLICATION_PREMIUM_SUBSCRIPTION = 32
    | GUILD_INCIDENT_ALERT_MODE_ENABLED = 36
    | GUILD_INCIDENT_ALERT_MODE_DISABLED = 37
    | GUILD_INCIDENT_REPORT_RAID = 38
    | GUILD_INCIDENT_REPORT_FALSE_ALARM = 39
    | PURCHASE_NOTIFICATION = 44

type ChannelType =
    | GUILD_TEXT = 0
    | DM = 1
    | GUILD_VOICE = 2
    | GROUP_DM = 3
    | GUILD_CATEGORY = 4
    | GUILD_ANNOUNCEMENT = 5
    | ANNOUNCEMENT_THREAD = 10
    | PUBLIC_THREAD = 11
    | PRIVATE_THREAD = 12
    | GUILD_STAGE_VOICE = 13
    | GUILD_DIRECTORY = 14
    | GUILD_FORUM = 15
    | GUILD_MEDIA = 16

type ThreadType =
    | ANNOUNCEMENT_THREAD = 10
    | PUBLIC_THREAD = 11
    | PRIVATE_THREAD = 12

type EntitlementType =
    | PURCHASE = 1
    | PREMIUM_SUBSCRIPTION = 2
    | DEVELOPER_GIFT = 3
    | TEST_MODE_PURCHASE = 4
    | FREE_PURCHASE = 5
    | USER_GIFT = 6
    | PREMIUM_PURCHASE = 7
    | APPLICATION_SUBSCRIPTION = 8

type UserPremiumType =
    | NONE = 0
    | NITRO_CLASSIC = 1
    | NITRO = 2
    | NITRO_BASIC = 3

type StickerFormatType = 
    | PNG = 1
    | APNG = 2
    | LOTTIE = 3
    | GIF = 4

type StickerType = 
    | STANDARD = 1
    | GUILD = 2

// https://discord.com/developers/docs/resources/guild#guild-object-guild-nsfw-level
type GuildNsfwLevel =
    | DEFAULT = 0
    | EXPLICIT = 1
    | SAFE = 2
    | AGE_RESTRICTED = 3

// https://discord.com/developers/docs/resources/guild#guild-object-premium-tier
type GuildPremiumTier =
    | NONE = 0
    | LEVEL_1 = 1
    | LEVEL_2 = 2
    | LEVEL_3 = 3

// https://discord.com/developers/docs/resources/guild#guild-object-mfa-level
type GuildMfaLevel =
    | NONE = 0
    | ELEVATED = 1

// https://discord.com/developers/docs/resources/guild#guild-object-explicit-content-filter-level
type GuildExplicitContentFilterLevel =
    | DISABLED = 0
    | MEMBERS_WITHOUT_ROLES = 1
    | ALL_MEMBERS = 2

// https://discord.com/developers/docs/resources/guild#guild-object-default-message-notification-level
type GuildMessageNotificationLevel =
    | ALL_MESSAGES = 0
    | ONLY_MENTIONS = 1

// https://discord.com/developers/docs/resources/guild#guild-object-verification-level
type GuildVerificationLevel =
    | NONE = 0
    | LOW = 1
    | MEDIUM = 2
    | HIGH = 3
    | VERY_HIGH = 4

// https://discord.com/developers/docs/resources/guild#guild-object-system-channel-flags
type SystemChannelFlag =
    | SUPPRESS_JOIN_NOTIFICATIONS                               = 0b00000001
    | SUPPRESS_PREMIUM_SUBSCRIPTIONS                            = 0b00000010
    | SUPPRESS_GUILD_REMINDER_NOTIFICATIONS                     = 0b00000100
    | SUPPRESS_JOIN_NOTIFICATION_REPLIES                        = 0b00001000
    | SUPPRESS_ROLE_SUBSCRIPTION_PURCHASE_NOTIFICATIONS         = 0b00010000
    | SUPPRESS_ROLE_SUBSCRIPTION_PURCHASE_NOTIFICATION_REPLIES  = 0b00100000

// https://discord.com/developers/docs/resources/guild#guild-object-guild-features
[<JsonConverter(typeof<GuildFeatureConverter>)>]
type GuildFeature =
    | ANIMATED_BANNER
    | ANIMATED_ICON
    | APPLICATION_COMMAND_PERMISSIONS_V2
    | AUTO_MODERATION
    | BANNER
    | COMMUNITY // mutable
    | CREATOR_MONETIZABLE_PROVISIONAL
    | CREATOR_STORE_PAGE
    | DEVELOPER_SUPPORT_SERVER
    | DISCOVERABLE // mutable
    | FEATURABLE
    | INVITES_DISABLED // mutable
    | INVITE_SPLASH
    | MEMBER_VERIFICATION_GATE_ENABLED
    | MORE_STICKERS
    | NEWS
    | PARTNERED
    | PREVIEW_ENABLED
    | RAID_ALERTS_DISABLED // mutable
    | ROLE_ICONS
    | ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE
    | ROLE_SUBSCRIPTIONS_ENABLED
    | TICKETED_EVENTS_ENABLED
    | VANITY_URL
    | VERIFIED
    | VIP_REGIONS
    | WELCOME_SCREEN_ENABLED
with
    override this.ToString () =
        match this with
        | GuildFeature.ANIMATED_BANNER -> "ANIMATED_BANNER"
        | GuildFeature.ANIMATED_ICON -> "ANIMATED_ICON"
        | GuildFeature.APPLICATION_COMMAND_PERMISSIONS_V2 -> "APPLICATION_COMMAND_PERMISSIONS_V2"
        | GuildFeature.AUTO_MODERATION -> "AUTO_MODERATION"
        | GuildFeature.BANNER -> "BANNER"
        | GuildFeature.COMMUNITY -> "COMMUNITY"
        | GuildFeature.CREATOR_MONETIZABLE_PROVISIONAL -> "CREATOR_MONETIZABLE_PROVISIONAL"
        | GuildFeature.CREATOR_STORE_PAGE -> "CREATOR_STORE_PAGE"
        | GuildFeature.DEVELOPER_SUPPORT_SERVER -> "DEVELOPER_SUPPORT_SERVER"
        | GuildFeature.DISCOVERABLE -> "DISCOVERABLE"
        | GuildFeature.FEATURABLE -> "FEATURABLE"
        | GuildFeature.INVITES_DISABLED -> "INVITES_DISABLED"
        | GuildFeature.INVITE_SPLASH -> "INVITE_SPLASH"
        | GuildFeature.MEMBER_VERIFICATION_GATE_ENABLED -> "MEMBER_VERIFICATION_GATE_ENABLED"
        | GuildFeature.MORE_STICKERS -> "MORE_STICKERS"
        | GuildFeature.NEWS -> "NEWS"
        | GuildFeature.PARTNERED -> "PARTNERED"
        | GuildFeature.PREVIEW_ENABLED -> "PREVIEW_ENABLED"
        | GuildFeature.RAID_ALERTS_DISABLED -> "RAID_ALERTS_DISABLED"
        | GuildFeature.ROLE_ICONS -> "ROLE_ICONS"
        | GuildFeature.ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE -> "ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE"
        | GuildFeature.ROLE_SUBSCRIPTIONS_ENABLED -> "ROLE_SUBSCRIPTIONS_ENABLED"
        | GuildFeature.TICKETED_EVENTS_ENABLED -> "TICKETED_EVENTS_ENABLED"
        | GuildFeature.VANITY_URL -> "VANITY_URL"
        | GuildFeature.VERIFIED -> "VERIFIED"
        | GuildFeature.VIP_REGIONS -> "VIP_REGIONS"
        | GuildFeature.WELCOME_SCREEN_ENABLED -> "WELCOME_SCREEN_ENABLED"

    static member FromString (str: string) =
        match str with
        | "ANIMATED_BANNER" -> Some GuildFeature.ANIMATED_BANNER
        | "ANIMATED_ICON" -> Some GuildFeature.ANIMATED_ICON
        | "APPLICATION_COMMAND_PERMISSIONS_V2" -> Some GuildFeature.APPLICATION_COMMAND_PERMISSIONS_V2
        | "AUTO_MODERATION" -> Some GuildFeature.AUTO_MODERATION
        | "BANNER" -> Some GuildFeature.BANNER
        | "COMMUNITY" -> Some GuildFeature.COMMUNITY
        | "CREATOR_MONETIZABLE_PROVISIONAL" -> Some GuildFeature.CREATOR_MONETIZABLE_PROVISIONAL
        | "CREATOR_STORE_PAGE" -> Some GuildFeature.CREATOR_STORE_PAGE
        | "DEVELOPER_SUPPORT_SERVER" -> Some GuildFeature.DEVELOPER_SUPPORT_SERVER
        | "DISCOVERABLE" -> Some GuildFeature.DISCOVERABLE
        | "FEATURABLE" -> Some GuildFeature.FEATURABLE
        | "INVITES_DISABLED" -> Some GuildFeature.INVITES_DISABLED
        | "INVITE_SPLASH" -> Some GuildFeature.INVITE_SPLASH
        | "MEMBER_VERIFICATION_GATE_ENABLED" -> Some GuildFeature.MEMBER_VERIFICATION_GATE_ENABLED
        | "MORE_STICKERS" -> Some GuildFeature.MORE_STICKERS
        | "NEWS" -> Some GuildFeature.NEWS
        | "PARTNERED" -> Some GuildFeature.PARTNERED
        | "PREVIEW_ENABLED" -> Some GuildFeature.PREVIEW_ENABLED
        | "RAID_ALERTS_DISABLED" -> Some GuildFeature.RAID_ALERTS_DISABLED
        | "ROLE_ICONS" -> Some GuildFeature.ROLE_ICONS
        | "ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE" -> Some GuildFeature.ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE
        | "ROLE_SUBSCRIPTIONS_ENABLED" -> Some GuildFeature.ROLE_SUBSCRIPTIONS_ENABLED
        | "TICKETED_EVENTS_ENABLED" -> Some GuildFeature.TICKETED_EVENTS_ENABLED
        | "VANITY_URL" -> Some GuildFeature.VANITY_URL
        | "VERIFIED" -> Some GuildFeature.VERIFIED
        | "VIP_REGIONS" -> Some GuildFeature.VIP_REGIONS
        | "WELCOME_SCREEN_ENABLED" -> Some GuildFeature.WELCOME_SCREEN_ENABLED
        | _ -> None

and GuildFeatureConverter () =
    inherit JsonConverter<GuildFeature> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            let value = reader.GetString() |> GuildFeature.FromString

            match value with
            | Some gf -> gf
            | None -> failwith "Unexpected GuildFeature type"

        override _.Write (writer: Utf8JsonWriter, value: GuildFeature, options: JsonSerializerOptions) = 
            writer.WriteStringValue (value.ToString())

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-onboarding-mode
type OnboardingMode =
    | ONBOARDING_DEFAULT = 0
    | ONBOARDING_ADVANCED = 1

// https://discord.com/developers/docs/resources/guild#guild-onboarding-object-prompt-types
type OnboardingPromptType =
    | MULTIPLE_CHOICE = 0
    | DROPDOWN = 1

[<JsonConverter(typeof<CommandInteractionDataOptionValueConverter>)>]
type CommandInteractionDataOptionValue =
    | String of string
    | Int of int
    | Double of double
    | Bool of bool

and CommandInteractionDataOptionValueConverter () =
    inherit JsonConverter<CommandInteractionDataOptionValue> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.String -> CommandInteractionDataOptionValue.String (reader.GetString())
            | JsonTokenType.Number -> CommandInteractionDataOptionValue.Int (reader.GetInt32())
            | JsonTokenType.True -> CommandInteractionDataOptionValue.Bool true
            | JsonTokenType.False -> CommandInteractionDataOptionValue.Bool false
            | _ -> failwith "Unexpected CommandInteractionDataOptionValue value"

        override _.Write (writer: Utf8JsonWriter, value: CommandInteractionDataOptionValue, options: JsonSerializerOptions) =
            match value with
            | CommandInteractionDataOptionValue.String v -> writer.WriteStringValue v
            | CommandInteractionDataOptionValue.Int v -> writer.WriteNumberValue v
            | CommandInteractionDataOptionValue.Bool v -> writer.WriteBooleanValue v
            | CommandInteractionDataOptionValue.Double v -> writer.WriteNumberValue v

type ApplicationCommandType = 
    | CHAT_INPUT = 1
    | USER = 2
    | MESSAGE = 3
    | PRIMARY_ENTRY_POINT = 4

type ApplicationCommandOptionType =
    | SUB_COMMAND = 1
    | SUB_COMMAND_GROUP = 2
    | STRING = 3
    | INTEGER = 4
    | BOOLEAN = 5
    | USER = 6
    | CHANNEL = 7
    | ROLE = 8
    | MENTIONABLE = 9
    | NUMBER = 10
    | ATTACHMENT = 11

type ApplicationCommandPermissionType =
    | ROLE = 1
    | USER = 2
    | CHANNEL = 3

type InteractionContextType =
    | GUILD = 0
    | BOT_DM = 1
    | PRIVATE_CHANNEL = 2

type ApplicationIntegrationType =
    | GUILD_INSTALL = 0
    | USER_INSTALL = 1

type InteractionType = 
    | PING = 1
    | APPLICATION_COMMAND = 2
    | MESSAGE_COMPONENT = 3
    | APPLICATION_COMMAND_AUTOCOMPLETE = 4
    | MODAL_SUBMIT = 5

type InteractionCallbackType = 
    | PONG = 1
    | CHANNEL_MESSAGE_WITH_SOURCE = 4
    | DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE = 5
    | DEFERRED_UPDATE_MESSAGE = 6
    | UPDATE_MESSAGE = 7
    | APPLICATION_COMMAND_AUTOCOMPLETE_RESULT = 8
    | MODAL = 9
    | LAUNCH_ACTIVITY = 12

type InviteType =
    | GUILD = 0
    | GROUP_DM = 1
    | FRIEND = 2

type InviteTargetType =
    | STREAM = 1
    | EMBEDDED_APPLICATION = 2

[<JsonConverter(typeof<MessageNonceConverter>)>]
type MessageNonce =
    | Number of int
    | String of string

and MessageNonceConverter () =
    inherit JsonConverter<MessageNonce> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.Number -> MessageNonce.Number (reader.GetInt32())
            | JsonTokenType.String -> MessageNonce.String (reader.GetString())
            | _ -> failwith "Unexpected MessageNonce value"

        override _.Write (writer: Utf8JsonWriter, value: MessageNonce, options: JsonSerializerOptions) =
            match value with
            | MessageNonce.Number v -> writer.WriteNumberValue v
            | MessageNonce.String v -> writer.WriteStringValue v

[<JsonConverter(typeof<ApplicationCommandOptionChoiceValueConverter>)>]
type ApplicationCommandOptionChoiceValue =
    | String of string
    | Int of int
    | Double of double

and ApplicationCommandOptionChoiceValueConverter () =
    inherit JsonConverter<ApplicationCommandOptionChoiceValue> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.String -> ApplicationCommandOptionChoiceValue.String (reader.GetString())
            | JsonTokenType.Number ->
                let double: double = 0
                let int: int = 0
                if reader.TryGetInt32(ref int) then
                    ApplicationCommandOptionChoiceValue.Int int
                else if reader.TryGetDouble(ref double) then
                    ApplicationCommandOptionChoiceValue.Double double
                else
                    failwith "Unexpected ApplicationCommandOptionChoiceValue value"
                // TODO: Test if this correctly handles int and double
            | _ -> failwith "Unexpected ApplicationCommandOptionChoiceValue value"

        override _.Write (writer: Utf8JsonWriter, value: ApplicationCommandOptionChoiceValue, options: JsonSerializerOptions) =
            match value with
            | ApplicationCommandOptionChoiceValue.String v -> writer.WriteStringValue v
            | ApplicationCommandOptionChoiceValue.Int v -> writer.WriteNumberValue v
            | ApplicationCommandOptionChoiceValue.Double v -> writer.WriteNumberValue v
    
[<JsonConverter(typeof<ApplicationCommandMinValueConverter>)>]
type ApplicationCommandMinValue =
    | Int of int
    | Double of double

and ApplicationCommandMinValueConverter () =
    inherit JsonConverter<ApplicationCommandMinValue> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.Number ->
                let double: double = 0
                let int: int = 0
                if reader.TryGetInt32(ref int) then
                    ApplicationCommandMinValue.Int int
                else if reader.TryGetDouble(ref double) then
                    ApplicationCommandMinValue.Double double
                else
                    failwith "Unexpected ApplicationCommandMinValue value"
                // TODO: Test if this correctly handles int and double
            | _ -> failwith "Unexpected ApplicationCommandMinValue value"

        override _.Write (writer: Utf8JsonWriter, value: ApplicationCommandMinValue, options: JsonSerializerOptions) = 
            match value with
            | ApplicationCommandMinValue.Int v -> writer.WriteNumberValue v
            | ApplicationCommandMinValue.Double v -> writer.WriteNumberValue v
    
[<JsonConverter(typeof<ApplicationCommandMaxValueConverter>)>]
type ApplicationCommandMaxValue =
    | Int of int
    | Double of double

and ApplicationCommandMaxValueConverter () =
    inherit JsonConverter<ApplicationCommandMaxValue> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with
            | JsonTokenType.Number ->
                let double: double = 0
                let int: int = 0
                if reader.TryGetInt32(ref int) then
                    ApplicationCommandMaxValue.Int int
                else if reader.TryGetDouble(ref double) then
                    ApplicationCommandMaxValue.Double double
                else
                    failwith "Unexpected ApplicationCommandMaxValue value"
                // TODO: Test if this correctly handles int and double
            | _ -> failwith "Unexpected ApplicationCommandMaxValue value"

        override _.Write (writer: Utf8JsonWriter, value: ApplicationCommandMaxValue, options: JsonSerializerOptions) = 
            match value with
            | ApplicationCommandMaxValue.Int v -> writer.WriteNumberValue v
            | ApplicationCommandMaxValue.Double v -> writer.WriteNumberValue v

[<JsonConverter(typeof<AllowedMentionsParseTypeConverter>)>]
type AllowedMentionsParseType =
    | Roles
    | Users
    | Everyone

and AllowedMentionsParseTypeConverter () =
    inherit JsonConverter<AllowedMentionsParseType> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.GetString() with
            | "roles" -> AllowedMentionsParseType.Roles
            | "users" -> AllowedMentionsParseType.Users
            | "everyone" -> AllowedMentionsParseType.Everyone
            | _ -> failwith "Unexpected AllowedMentionsParseType value"

        override _.Write (writer: Utf8JsonWriter, value: AllowedMentionsParseType, options: JsonSerializerOptions) =
            let string =
                match value with
                | AllowedMentionsParseType.Roles -> "roles"
                | AllowedMentionsParseType.Users -> "users"
                | AllowedMentionsParseType.Everyone -> "everyone"

            writer.WriteStringValue string

type ApplicationCommandHandlerType =
    | APP_HANDER = 1
    | DISCORD_LAUNCH_ACTIVITY = 2

type GatewayEncoding =
    | JSON
    | ETF
with
    override this.ToString () =
        match this with
        | GatewayEncoding.JSON -> "json"
        | GatewayEncoding.ETF -> "etf"

type GatewayEncodingConverter () =
    inherit JsonConverter<GatewayEncoding> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.GetString() with
            | "json" -> GatewayEncoding.JSON
            | "etf" -> GatewayEncoding.ETF
            | _ -> failwith "Unexpected GatewayEncoding value"

        override _.Write (writer: Utf8JsonWriter, value: GatewayEncoding, options: JsonSerializerOptions) =
            let string =
                match value with
                | GatewayEncoding.JSON -> "json"
                | GatewayEncoding.ETF -> "etf"

            writer.WriteStringValue string

type GatewayCompression =
    | ZLIBSTREAM
    | ZSTDSTREAM
with
    override this.ToString () =
        match this with
        | GatewayCompression.ZLIBSTREAM -> "zlib-stream"
        | GatewayCompression.ZSTDSTREAM -> "zstd-stream"

type GatewayCompressionConverter () =
    inherit JsonConverter<GatewayCompression> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.GetString() with
            | "zlib-stream" -> GatewayCompression.ZLIBSTREAM
            | "zstd-stream" -> GatewayCompression.ZSTDSTREAM
            | _ -> failwith "Unexpected GatewayCompression value"

        override _.Write (writer: Utf8JsonWriter, value: GatewayCompression, options: JsonSerializerOptions) =
            let string =
                match value with
                | GatewayCompression.ZLIBSTREAM -> "zlib-stream"
                | GatewayCompression.ZSTDSTREAM -> "zstd-stream"

            writer.WriteStringValue string

type GatewayOpcode =
    | DISPATCH = 0
    | HEARTBEAT = 1
    | IDENTIFY = 2
    | PRESENCE_UPDATE = 3
    | VOICE_STATE_UPDATE = 4
    | RESUME = 6
    | RECONNECT = 7
    | REQUEST_GUILD_MEMBERS = 8
    | INVALID_SESSION = 9
    | HELLO = 10
    | HEARTBEAT_ACK = 11
    | REQUEST_SOUNDBOARD_SOUNDS = 31
    
// https://discord.com/developers/docs/topics/opcodes-and-status-codes#gateway-gateway-close-event-codes
type GatewayCloseEventCode =
    | UNKNOWN_ERROR = 4000
    | UNKNOWN_OPCODE = 4001
    | DECODE_ERROR = 4002
    | NOT_AUTHENTICATED = 4003
    | AUTHENTICATION_FAILED = 4004
    | ALREADY_AUTHENTICATED = 4005
    | INVALID_SEQ = 4007
    | RATE_LIMITED = 4008
    | SESSION_TIMED_OUT = 4009
    | INVALID_SHARD = 4010
    | SHARDING_REQUIRED = 4011
    | INVALID_API_VERSION = 4012
    | INVALID_INTENTS = 4013
    | DISALLOWED_INTENTS = 4014

type GatewayIntent =
    | GUILDS =                          0b00000000_00000000_00000000_00000001
    | GUILD_MEMBERS =                   0b00000000_00000000_00000000_00000010
    | GUILD_MODERATION =                0b00000000_00000000_00000000_00000100
    | GUILD_EMOJIS_AND_STICKERS =       0b00000000_00000000_00000000_00001000
    | GUILD_INTEGRATIONS =              0b00000000_00000000_00000000_00010000
    | GUILD_WEBHOOKS =                  0b00000000_00000000_00000000_00100000
    | GUILD_INVITES =                   0b00000000_00000000_00000000_01000000
    | GUILD_VOICE_STATES =              0b00000000_00000000_00000000_10000000
    | GUILD_PRESENCES =                 0b00000000_00000000_00000001_00000000
    | GUILD_MESSAGES =                  0b00000000_00000000_00000010_00000000
    | GUILD_MESSAGE_REACTIONS =         0b00000000_00000000_00000100_00000000
    | GUILD_MESSAGE_TYPING =            0b00000000_00000000_00001000_00000000
    | DIRECT_MESSAGES =                 0b00000000_00000000_00010000_00000000
    | DIRECT_MESSAGE_REACTIONS =        0b00000000_00000000_00100000_00000000
    | DIRECT_MESSAGE_TYPING =           0b00000000_00000000_01000000_00000000
    | MESSAGE_CONTENT =                 0b00000000_00000000_10000000_00000000
    | GUILD_SCHEDULED_EVENTS =          0b00000000_00000001_00000000_00000000
    | AUTO_MODERATION_CONFIGURATION =   0b00000000_00010000_00000000_00000000
    | AUTO_MODERATION_EXECUTION =       0b00000000_00100000_00000000_00000000
    | GUILD_MESSAGE_POLLS =             0b00000001_00000000_00000000_00000000
    | DIRECT_MESSAGE_POLLS =            0b00000010_00000000_00000000_00000000

[<JsonConverter(typeof<StatusTypeConverter>)>]
type StatusType =
    | ONLINE
    | DND
    | IDLE
    | INVISIBLE
    | OFFLINE
    
and StatusTypeConverter () =
    inherit JsonConverter<StatusType> () with
        override __.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.GetString() with
            | "online" -> StatusType.ONLINE
            | "dnd" -> StatusType.DND
            | "idle" -> StatusType.IDLE
            | "invisible" -> StatusType.INVISIBLE
            | "offline" -> StatusType.OFFLINE
            | _ -> failwith "Unexpected StatusType value"

        override __.Write (writer: Utf8JsonWriter, value: StatusType, options: JsonSerializerOptions) =
            let string =
                match value with
                | StatusType.ONLINE -> "online"
                | StatusType.DND -> "dnd"
                | StatusType.IDLE -> "idle"
                | StatusType.INVISIBLE -> "invisible"
                | StatusType.OFFLINE -> "offline"

            writer.WriteStringValue string

type ActivityType =
    | PLAYING = 0
    | STREAMING = 1
    | LISTENING = 2
    | WATCHING = 3
    | CUSTOM = 4
    | COMPETING = 5

type ActivityFlag =
    | INSTANCE =                    0b00000000_00000001
    | JOIN =                        0b00000000_00000010
    | SPECTATE =                    0b00000000_00000100
    | JOIN_REQUEST =                0b00000000_00001000
    | SYNC =                        0b00000000_00010000
    | PLAY =                        0b00000000_00100000
    | PARTY_PRIVACY_FRIENDS =       0b00000000_01000000
    | PARTY_PRIVACY_VOICE_CHANNEL = 0b00000000_10000000
    | EMBEDDED =                    0b00000001_00000000

type AnimationType =
    | PREMIUM = 0
    | BAISC = 1

[<JsonConverter(typeof<SoundboardSoundIdConverter>)>]
type SoundboardSoundId =
    | String of string
    | Int of int

and SoundboardSoundIdConverter () =
    inherit JsonConverter<SoundboardSoundId> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.TokenType with // TODO: Test this, sounds wrong
            | JsonTokenType.String -> SoundboardSoundId.String (reader.GetString())
            | JsonTokenType.Number -> SoundboardSoundId.Int (reader.GetInt32())
            | _ -> failwith "Unexpected SoundboardSoundId value"

        override _.Write (writer: Utf8JsonWriter, value: SoundboardSoundId, options: JsonSerializerOptions) =
            match value with
            | SoundboardSoundId.String v -> writer.WriteStringValue v
            | SoundboardSoundId.Int v -> writer.WriteNumberValue v

type ApplicationRoleConnectionMetadataType =
    | INTEGER_LESS_THAN_OR_EQUAL = 1
    | INTEGER_GREATER_THAN_OR_EQUAL = 2
    | INTEGER_EQUAL = 3
    | INTEGER_NOT_EQUAL = 4
    | DATETIME_LESS_THAN_OR_EQUAL = 5
    | DATETIME_GREATER_THAN_OR_EQUAL = 6
    | BOOLEAN_EQUAL = 7
    | BOOLEAN_NOT_EQUAL = 8

type EditChannelPermissionsType =
    | ROLE = 0
    | MEMBER = 1

type AutoArchiveDurationType =
    | HOUR = 60
    | DAY = 1440
    | THREE_DAYS = 4320
    | WEEK = 10080

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-event-types
type AutoModerationEventType =
    | MESSAGE_SEND = 1
    | MEMBER_UDPATE = 2

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-trigger-types
type AutoModerationTriggerType =
    | KEYWORD = 1
    | SPAM = 3
    | KEYWORD_PRESET = 4
    | MENTION_SPAM = 5
    | MEMBER_PROFILE = 6

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-rule-object-keyword-preset-types
type AutoModerationKeywordPresetType =
    | PROFANITY = 1
    | SEXUAL_CONTENT = 2
    | SLURS = 3

// https://discord.com/developers/docs/resources/auto-moderation#auto-moderation-action-object-action-types
type AutoModerationActionType =
    | BLOCK_MESSAGE = 1
    | SEND_ALERT_MESSAGE = 2
    | TIMEOUT = 3
    | BLOCK_MEMBER_INTERACTION = 4

// https://discord.com/developers/docs/resources/application#application-object-application-flags
type ApplicationFlag =
    | APPLICATION_AUTO_MODERATION_RULE_CREATE_BADGE = 0b00000000_00000000_01000000
    | GATEWAY_PRESENCE                              = 0b00000000_00010000_00000000
    | GATEWAY_PRESENCE_LIMITED                      = 0b00000000_00100000_00000000
    | GATEWAY_GUILD_MEMBERS                         = 0b00000000_01000000_00000000
    | GATEWAY_GUILD_MEMBERS_LIMITED                 = 0b00000000_10000000_00000000
    | VERIFICATION_PENDING_GUILD_LIMIT              = 0b00000001_00000000_00000000
    | EMBEDDED                                      = 0b00000010_00000000_00000000
    | GATEWAY_MESSAGE_CONTENT                       = 0b00000100_00000000_00000000
    | GATEWAY_MESSAGE_CONTENT_LIMITED               = 0b00001000_00000000_00000000
    | APPLICATION_COMMAND_BADGE                     = 0b10000000_00000000_00000000

// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-location-kind-enum
[<JsonConverter(typeof<ActivityLocationKindConverter>)>]
type ActivityLocationKind =
    | GUILD_CHANNEL
    | PRIVATE_CHANNEL

and ActivityLocationKindConverter () =
    inherit JsonConverter<ActivityLocationKind> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.GetString() with
            | "gc" -> ActivityLocationKind.GUILD_CHANNEL
            | "pc" -> ActivityLocationKind.PRIVATE_CHANNEL
            | _ -> failwith "Unexpected ActivityLocationKind value"

        override _.Write (writer: Utf8JsonWriter, value: ActivityLocationKind, options: JsonSerializerOptions) =
            let string =
                match value with
                | ActivityLocationKind.GUILD_CHANNEL -> "gc"
                | ActivityLocationKind.PRIVATE_CHANNEL -> "pc"

            writer.WriteStringValue string

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-audit-log-events
type AuditLogEventType =
    | GUILD_UPDATE = 1
    | CHANNEL_CREATE = 10
    | CHANNEL_UDPATE = 11
    | CHANNEL_DELETE = 12
    | CHANNEL_OVERWRITE_CREATE = 13
    | CHANNEL_OVERWRITE_UPDATE = 14
    | CHANNEL_OVERWRITE_DELETE = 15
    | MEMBER_KICK = 20
    | MEMBER_PRUNE = 21
    | MEMBER_BAN_ADD = 22
    | MEMBER_BAN_REMOVE = 23
    | MEMBER_UPDATE = 24
    | MEMBER_ROLE_UPDATE = 25
    | MEMBER_MOVE = 26
    | MEMBER_DISCONNECT = 27
    | BOT_ADD = 28
    | ROLE_CREATE = 30
    | ROLE_UPDATE = 31
    | ROLE_DELETE = 32
    | INVITE_CREATE = 40
    | INVITE_UPDATE = 41
    | INVITE_DELETE = 42
    | WEBHOOK_CREATE = 50
    | WEBHOOK_UPDATE = 51
    | WEBHOOK_DELETE = 52
    | EMOJI_CREATE = 60
    | EMOJI_UPDATE = 61
    | EMOJI_DELETE = 62
    | MESSAGE_DELETE = 72
    | MESSAGE_BULK_DELETE = 73
    | MESSAGE_PIN = 74
    | MESSAGE_UNPIN = 75
    | INTEGRATION_CREATE = 80
    | INTEGRATION_UPDATE = 81
    | INTEGRATION_DELETE = 82
    | STAGE_INSTANCE_CREATE = 83
    | STAGE_INSTANCE_UPDATE = 84
    | STAGE_INSTANCE_DELETE = 85
    | STICKER_CREATE = 90
    | STICKER_UPDATE = 91
    | STICKER_DELETE = 92
    | GUILD_SCHEDULED_EVENT_CREATE = 100
    | GUILD_SCHEDULED_EVENT_UPDATE = 101
    | GUILD_SCHEDULED_EVENT_DELETE = 102
    | THREAD_CREATE = 110
    | THREAD_UPDATE = 111
    | THREAD_DELETE = 112
    | APPLICATION_COMMAND_PERMISSION_UPDATE = 121
    | SOUNDBOARD_SOUND_CREATE = 130
    | SOUNDBOARD_SOUND_UPDATE = 131
    | SOUNDBOARD_SOUND_DELETE = 132
    | AUTO_MODERATION_RULE_CREATE = 140
    | AUTO_MODERATION_RULE_UPDATE = 141
    | AUTO_MODERATION_RULE_DELETE = 142
    | AUTO_MODERATION_BLOCK_MESSAGE = 143
    | AUTO_MODERATION_FLAG_TO_CHANNEL = 144
    | AUTO_MODERATION_USER_COMMUNICATION_DISABLED = 145
    | CREATOR_MONETIZATION_REQUEST_CREATED = 150
    | CREATOR_MONETIZATION_TERMS_ACCEPTED = 151
    | ONBOARDING_PROMPT_CREATE = 163
    | ONBOARDING_PROMPT_UPDATE = 164
    | ONBOARDING_PROMPT_DELETE = 165
    | ONBOARDING_CREATE = 166
    | ONBOARDING_UPDATE = 167
    | HOME_SETTINGS_CREATE = 190
    | HOME_SETTINGS_UPDATE = 191

// https://discord.com/developers/docs/resources/guild#integration-object-integration-structure
type GuildIntegrationType =
    | TWITCH
    | YOUTUBE
    | DISCORD
    | GUILD_SUBSCRIPTION

type GuildIntegrationTypeConverter () =
    inherit JsonConverter<GuildIntegrationType> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
            match reader.GetString() with
            | "twitch" -> GuildIntegrationType.TWITCH
            | "youtube" -> GuildIntegrationType.YOUTUBE
            | "discord" -> GuildIntegrationType.DISCORD
            | "guild_subscription" -> GuildIntegrationType.GUILD_SUBSCRIPTION
            | _ -> failwith "Unexpected GuildIntegrationType value"

        override _.Write (writer: Utf8JsonWriter, value: GuildIntegrationType, options: JsonSerializerOptions) =
            let string =
                match value with
                | GuildIntegrationType.TWITCH -> "twitch"
                | GuildIntegrationType.YOUTUBE -> "youtube"
                | GuildIntegrationType.DISCORD -> "discord"
                | GuildIntegrationType.GUILD_SUBSCRIPTION -> "guild_subscription"

            writer.WriteStringValue string

type IntegrationExpireBehaviorType =
    | REMOVE_ROLE = 0
    | KICK = 1

[<JsonConverter(typeof<OAuth2ScopeConverter>)>]
type OAuth2Scope =
    | ACTIVITIES_READ
    | ACTIVITIES_WRITE
    | APPLICATIONS_BUILDS_READ
    | APPLICATIONS_BUILDS_UPLOAD
    | APPLICATIONS_COMMANDS
    | APPLICATIONS_COMMANDS_UPDATE
    | APPLICATIONS_COMMANDS_PERMISSIONS_UPDATE
    | APPLICATIONS_ENTITLEMENTS
    | APPLICATIONS_STORE_UPDATE
    | BOT
    | CONNECTIONS
    | DM_CHANNELS_READ
    | EMAIL
    | GDM_JOIN
    | GUILDS
    | GUILDS_JOIN
    | GUILDS_MEMBERS_READ
    | IDENTIFY
    | MESSAGES_READ
    | RELATIONSHIPS_READ
    | ROLE_CONNECTIONS_WRITE
    | RPC
    | RPC_ACTIVITIES_WRITE
    | RPC_NOTIFICATIONS_READ
    | RPC_VOICE_READ
    | RPC_VOICE_WRITE
    | VOICE
    | WEBHOOK_INCOMING

and OAuth2ScopeConverter () =
    inherit JsonConverter<OAuth2Scope> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) = 
            match reader.GetString() with
            | "activities.read" -> OAuth2Scope.ACTIVITIES_READ
            | "activities.write" -> OAuth2Scope.ACTIVITIES_WRITE
            | "applications.builds.read" -> OAuth2Scope.APPLICATIONS_BUILDS_READ
            | "applications.builds.upload" -> OAuth2Scope.APPLICATIONS_BUILDS_UPLOAD
            | "applications.commands" -> OAuth2Scope.APPLICATIONS_COMMANDS
            | "applications.commands.update" -> OAuth2Scope.APPLICATIONS_COMMANDS_UPDATE
            | "applications.commands.permissions.update" -> OAuth2Scope.APPLICATIONS_COMMANDS_PERMISSIONS_UPDATE
            | "applications.entitlements" -> OAuth2Scope.APPLICATIONS_ENTITLEMENTS
            | "applications.store.update" -> OAuth2Scope.APPLICATIONS_STORE_UPDATE
            | "bot" -> OAuth2Scope.BOT
            | "connections" -> OAuth2Scope.CONNECTIONS
            | "dm_channels.read" -> OAuth2Scope.DM_CHANNELS_READ
            | "email" -> OAuth2Scope.EMAIL
            | "gdm.join" -> OAuth2Scope.GDM_JOIN
            | "guilds" -> OAuth2Scope.GUILDS
            | "guilds.join" -> OAuth2Scope.GUILDS_JOIN
            | "guilds.members.read" -> OAuth2Scope.GUILDS_MEMBERS_READ
            | "identify" -> OAuth2Scope.IDENTIFY
            | "messages.read" -> OAuth2Scope.MESSAGES_READ
            | "relationships.read" -> OAuth2Scope.RELATIONSHIPS_READ
            | "role_connections.write" -> OAuth2Scope.ROLE_CONNECTIONS_WRITE
            | "rpc" -> OAuth2Scope.RPC
            | "rpc.activities.write" -> OAuth2Scope.RPC_ACTIVITIES_WRITE
            | "rpc.notifications.read" -> OAuth2Scope.RPC_NOTIFICATIONS_READ
            | "rpc.voice.read" -> OAuth2Scope.RPC_VOICE_READ
            | "rpc.voice.write" -> OAuth2Scope.RPC_VOICE_WRITE
            | "voice" -> OAuth2Scope.VOICE
            | "webhook.incoming" -> OAuth2Scope.WEBHOOK_INCOMING
            | _ -> failwith "Unexpected OAuth2Scope type"

        override _.Write (writer: Utf8JsonWriter, value: OAuth2Scope, options: JsonSerializerOptions) = 
            let string =
                match value with
                | OAuth2Scope.ACTIVITIES_READ -> "activities.read"
                | OAuth2Scope.ACTIVITIES_WRITE -> "activities.write"
                | OAuth2Scope.APPLICATIONS_BUILDS_READ -> "applications.builds.read"
                | OAuth2Scope.APPLICATIONS_BUILDS_UPLOAD -> "applications.builds.upload"
                | OAuth2Scope.APPLICATIONS_COMMANDS -> "applications.commands"
                | OAuth2Scope.APPLICATIONS_COMMANDS_UPDATE -> "applications.commands.update"
                | OAuth2Scope.APPLICATIONS_COMMANDS_PERMISSIONS_UPDATE -> "applications.commands.permissions.update"
                | OAuth2Scope.APPLICATIONS_ENTITLEMENTS -> "applications.entitlements"
                | OAuth2Scope.APPLICATIONS_STORE_UPDATE -> "applications.store.update"
                | OAuth2Scope.BOT -> "bot"
                | OAuth2Scope.CONNECTIONS -> "connections"
                | OAuth2Scope.DM_CHANNELS_READ -> "dm_channels.read"
                | OAuth2Scope.EMAIL -> "email"
                | OAuth2Scope.GDM_JOIN -> "gdm.join"
                | OAuth2Scope.GUILDS -> "guilds"
                | OAuth2Scope.GUILDS_JOIN -> "guilds.join"
                | OAuth2Scope.GUILDS_MEMBERS_READ -> "guilds.members.read"
                | OAuth2Scope.IDENTIFY -> "identify"
                | OAuth2Scope.MESSAGES_READ -> "messages.read"
                | OAuth2Scope.RELATIONSHIPS_READ -> "relationships.read"
                | OAuth2Scope.ROLE_CONNECTIONS_WRITE -> "role_connections.write"
                | OAuth2Scope.RPC -> "rpc"
                | OAuth2Scope.RPC_ACTIVITIES_WRITE -> "rpc.activities.write"
                | OAuth2Scope.RPC_NOTIFICATIONS_READ -> "rpc.notifications.read"
                | OAuth2Scope.RPC_VOICE_READ -> "rpc.voice.read"
                | OAuth2Scope.RPC_VOICE_WRITE -> "rpc.voice.write"
                | OAuth2Scope.VOICE -> "voice"
                | OAuth2Scope.WEBHOOK_INCOMING -> "webhook.incoming"

            writer.WriteStringValue string

// https://discord.com/developers/docs/resources/webhook#webhook-object-webhook-types
type WebhookType =
    | INCOMING = 1
    | CHANNEL_FOLLOWER = 2
    | APPLICATION = 3

// https://discord.com/developers/docs/resources/entitlement#create-test-entitlement-json-params
type EntitlementOwnerType =
    | GUILD_SUBSCRIPTION = 1
    | USER_SUBSCRIPTION = 2

// https://discord.com/developers/docs/resources/guild#get-guild-widget-image-widget-style-options
type GuildWidgetStyle =
    | SHIELD
    | BANNER_1
    | BANNER_2
    | BANNER_3
    | BANNER_4
with
    override this.ToString () =
        match this with
        | GuildWidgetStyle.SHIELD -> "shield"
        | GuildWidgetStyle.BANNER_1 -> "banner_1"
        | GuildWidgetStyle.BANNER_2 -> "banner_2"
        | GuildWidgetStyle.BANNER_3 -> "banner_3"
        | GuildWidgetStyle.BANNER_4 -> "banner_4"

type GuildWidgetStyleConverter () =
    inherit JsonConverter<GuildWidgetStyle> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) = 
            match reader.GetString() with
            | "shield" -> GuildWidgetStyle.SHIELD
            | "banner_1" -> GuildWidgetStyle.BANNER_1
            | "banner_2" -> GuildWidgetStyle.BANNER_2
            | "banner_3" -> GuildWidgetStyle.BANNER_3
            | "banner_4" -> GuildWidgetStyle.BANNER_4
            | _ -> failwith "Unexpected GuildWidgetStyle type"

        override _.Write (writer: Utf8JsonWriter, value: GuildWidgetStyle, options: JsonSerializerOptions) =
            let string =
                match value with
                | GuildWidgetStyle.SHIELD -> "shield"
                | GuildWidgetStyle.BANNER_1 -> "banner_1"
                | GuildWidgetStyle.BANNER_2 -> "banner_2"
                | GuildWidgetStyle.BANNER_3 -> "banner_3"
                | GuildWidgetStyle.BANNER_4 -> "banner_4"

            writer.WriteStringValue string
       
// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-privacy-level
type PrivacyLevelType =
    | GUILD_ONLY = 2

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-status
type EventStatusType =
    | SCHEDULED = 1
    | ACTIVE = 2
    | COMPLETED = 3
    | CANCELED = 4

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-entity-types
type ScheduledEntityType =
    | STANCE_INSTANCE = 1
    | VOICE = 2
    | EXTERNAL = 3

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-frequency
type RecurrenceRuleFrequencyType =
    | YEARLY = 0
    | MONTHLY = 1
    | WEEKLY = 2
    | DAILY = 3

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-weekday
type RecurrenceRuleWeekdayType =
    | MONDAY = 1
    | TUESDAY = 2
    | WEDNESDAY = 3
    | THURSDAY = 4
    | FRIDAY = 5
    | SATURDAY = 6
    | SUNDAY = 7

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-month
type RecurrenceRuleMonthType =
    | JANUARY = 1
    | FEBRUARY = 2
    | MARCH = 3
    | APRIL = 4
    | MAY = 5
    | JUNE = 6
    | JULY = 7
    | AUGUST = 8
    | SEPTEMBER = 9
    | OCTOBER = 10
    | NOVEMBER = 11
    | DECEMBER = 12

// https://discord.com/developers/docs/resources/sku#sku-object-sku-types
type SkuType =
    | DURABLE = 2
    | CONSUMABLE = 3
    | SUBSCRIPTION = 5
    | SUBSCRIPTION_GROUP = 6

// https://discord.com/developers/docs/resources/sku#sku-object-sku-flags
type SkuFlag =
    | AVAILABLE          = 0b00000000_00000100
    | GUILD_SUBSCRIPTION = 0b00000000_10000000
    | USER_SUBSCRIPTION  = 0b00000001_00000000

// https://discord.com/developers/docs/resources/subscription#subscription-statuses
type SubscriptionStatusType =
    | ACTIVE = 0
    | ENDING = 1
    | INACTIVE = 2

// https://discord.com/developers/docs/resources/message#get-reactions-reaction-types
type ReactionType =
    | NORMAL = 0
    | BURST = 1

// https://discord.com/developers/docs/resources/user#connection-object-services
[<JsonConverter(typeof<ConnectionServiceTypeConverter>)>]
type ConnectionServiceType =
    | AMAZON_MUSIC
    | BATTLE_NET
    | BUNGIE
    | DOMAIN
    | EBAY
    | EPIC_GAMES
    | FACEBOOK
    | GITHUB
    | INSTAGRAM
    | LEAGUE_OF_LEGENDS
    | PAYPAL
    | PLAYSTATION
    | REDDIT
    | RIOT_GAMES
    | ROBLOX
    | SPOTIFY
    | SKYPE
    | STEAM
    | TIKTOK
    | TWITCH
    | TWITTER
    | XBOX
    | YOUTUBE

and ConnectionServiceTypeConverter () =
    inherit JsonConverter<ConnectionServiceType> () with
        override _.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) = 
            match reader.GetString() with
            | "amazon-music" -> ConnectionServiceType.AMAZON_MUSIC
            | "battlenet" -> ConnectionServiceType.BATTLE_NET
            | "bungie" -> ConnectionServiceType.BUNGIE
            | "domain" -> ConnectionServiceType.DOMAIN
            | "ebay" -> ConnectionServiceType.EBAY
            | "epicgames" -> ConnectionServiceType.EPIC_GAMES
            | "facebook" -> ConnectionServiceType.FACEBOOK
            | "github" -> ConnectionServiceType.GITHUB
            | "instagram" -> ConnectionServiceType.INSTAGRAM
            | "leagueoflegends" -> ConnectionServiceType.LEAGUE_OF_LEGENDS
            | "paypal" -> ConnectionServiceType.PAYPAL
            | "playstation" -> ConnectionServiceType.PLAYSTATION
            | "reddit" -> ConnectionServiceType.REDDIT
            | "riotgames" -> ConnectionServiceType.RIOT_GAMES
            | "roblox" -> ConnectionServiceType.ROBLOX
            | "spotify" -> ConnectionServiceType.SPOTIFY
            | "skype" -> ConnectionServiceType.SKYPE
            | "steam" -> ConnectionServiceType.STEAM
            | "tiktok" -> ConnectionServiceType.TIKTOK
            | "twitch" -> ConnectionServiceType.TWITCH
            | "twitter" -> ConnectionServiceType.TWITTER
            | "xbox" -> ConnectionServiceType.XBOX
            | "youtube" -> ConnectionServiceType.YOUTUBE
            | _ -> failwith "Unexpected ConnectionServiceType type"

        override _.Write (writer: Utf8JsonWriter, value: ConnectionServiceType, options: JsonSerializerOptions) = 
            let string =
                match value with
                | ConnectionServiceType.AMAZON_MUSIC -> "amazon-music"
                | ConnectionServiceType.BATTLE_NET -> "battlenet"
                | ConnectionServiceType.BUNGIE -> "bungie"
                | ConnectionServiceType.DOMAIN -> "domain"
                | ConnectionServiceType.EBAY -> "ebay"
                | ConnectionServiceType.EPIC_GAMES -> "epicgames"
                | ConnectionServiceType.FACEBOOK -> "facebook"
                | ConnectionServiceType.GITHUB -> "github"
                | ConnectionServiceType.INSTAGRAM -> "instagram"
                | ConnectionServiceType.LEAGUE_OF_LEGENDS -> "leagueoflegends"
                | ConnectionServiceType.PAYPAL -> "paypal"
                | ConnectionServiceType.PLAYSTATION -> "playstation"
                | ConnectionServiceType.REDDIT -> "reddit"
                | ConnectionServiceType.RIOT_GAMES -> "riotgames"
                | ConnectionServiceType.ROBLOX -> "roblox"
                | ConnectionServiceType.SPOTIFY -> "spotify"
                | ConnectionServiceType.SKYPE -> "skype"
                | ConnectionServiceType.STEAM -> "steam"
                | ConnectionServiceType.TIKTOK -> "tiktok"
                | ConnectionServiceType.TWITCH -> "twitch"
                | ConnectionServiceType.TWITTER -> "twitter"
                | ConnectionServiceType.XBOX -> "xbox"
                | ConnectionServiceType.YOUTUBE -> "youtube"

            writer.WriteStringValue string

// https://discord.com/developers/docs/resources/user#connection-object-visibility-types
type ConnectionVisibilityType =
    | NONE = 0
    | EVERYONE = 1

// https://discord.com/developers/docs/events/webhook-events#webhook-types
type WebhookPayloadType =
    | PING = 0
    | EVENT = 1

// https://discord.com/developers/docs/events/webhook-events#event-types
type WebhookEventType =
    | APPLICATION_AUTHORIZED
    | ENTITLEMENT_CREATE
with
    override this.ToString () =
        match this with
        | WebhookEventType.APPLICATION_AUTHORIZED -> "APPLICATION_AUTHORIZED"
        | WebhookEventType.ENTITLEMENT_CREATE -> "ENTITLEMENT_CREATE"

    static member FromString (str: string) =
        match str with
        | "APPLICATION_AUTHORIZED" -> Some WebhookEventType.APPLICATION_AUTHORIZED
        | "ENTITLEMENT_CREATE" -> Some WebhookEventType.ENTITLEMENT_CREATE
        | _ -> None

// https://discord.com/developers/docs/topics/opcodes-and-status-codes#json-json-error-codes
type JsonErrorCode =
    | TODO = 0
