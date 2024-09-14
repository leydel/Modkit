namespace Modkit.Discordfs.Types

open FSharp.Json

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

type GuildNsfwLevel =
    | DEFAULT = 0
    | EXPLICIT = 1
    | SAFE = 2
    | AGE_RESTRICTED = 3

type GuildPremiumTier =
    | NONE = 0
    | LEVEL_1 = 1
    | LEVEL_2 = 2
    | LEVEL_3 = 3

type GuildMfaLevel =
    | NONE = 0
    | ELEVATED = 1

type GuildExplicitContentFilterLevel =
    | DISABLED = 0
    | MEMBERS_WITHOUT_ROLES = 1
    | ALL_MEMBERS = 2

type GuildMessageNotificationLevel =
    | ALL_MESSAGES = 0
    | ONLY_MENTIONS = 1

type GuildVerificationLevel =
    | NONE = 0
    | LOW = 1
    | MEDIUM = 2
    | HIGH = 3
    | VERY_HIGH = 4

type CommandInteractionDataOptionValue =
    | String of string
    | Int of int
    | Double of double
    | Bool of bool

type CommandInteractionDataOptionValueTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<obj>

        member _.toTargetType value =
            match value :?> CommandInteractionDataOptionValue with
            | CommandInteractionDataOptionValue.String v -> v
            | CommandInteractionDataOptionValue.Int v -> v
            | CommandInteractionDataOptionValue.Double v -> v
            | CommandInteractionDataOptionValue.Bool v -> v

        member _.fromTargetType value =
            match value with
            | :? string as v -> CommandInteractionDataOptionValue.String v
            | :? int as v -> CommandInteractionDataOptionValue.Int v
            | :? double as v -> CommandInteractionDataOptionValue.Double v
            | :? bool as v -> CommandInteractionDataOptionValue.Bool v
            | _ -> failwith "Unexpected CommandInteractionDataOptionValue type"

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

type MessageNonce =
    | Number of int
    | String of string

type MessageNonceTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<obj>

        member _.toTargetType value =
            match value :?> MessageNonce with
            | MessageNonce.Number v -> v
            | MessageNonce.String v -> v

        member _.fromTargetType value =
            match value with
            | :? int as v -> MessageNonce.Number v
            | :? string as v -> MessageNonce.String v
            | _ -> failwith "Unexpected MessageNonce type"

type ApplicationCommandOptionChoiceValue =
    | String of string
    | Integer of int
    | Double of double

type ApplicationCommandOptionChoiceValueTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<obj>

        member _.toTargetType value =
            match value :?> ApplicationCommandOptionChoiceValue with
            | ApplicationCommandOptionChoiceValue.String v -> v
            | ApplicationCommandOptionChoiceValue.Integer v -> v
            | ApplicationCommandOptionChoiceValue.Double v -> v

        member _.fromTargetType value =
            match value with
            | :? string as v -> ApplicationCommandOptionChoiceValue.String v
            | :? int as v -> ApplicationCommandOptionChoiceValue.Integer v
            | :? double as v -> ApplicationCommandOptionChoiceValue.Double v
            | _ -> failwith "Unexpected ApplicationCommandOptionChoiceValue type"
    
type ApplicationCommandMinValue =
    | Integer of int
    | Double of double

type ApplicationCommandMinValueTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<obj>

        member _.toTargetType value =
            match value :?> ApplicationCommandMinValue with
            | ApplicationCommandMinValue.Integer v -> v
            | ApplicationCommandMinValue.Double v -> v

        member _.fromTargetType value =
            match value with
            | :? int as v -> ApplicationCommandMinValue.Integer v
            | :? double as v -> ApplicationCommandMinValue.Double v
            | _ -> failwith "Unexpected ApplicationCommandMinValue type"
    
type ApplicationCommandMaxValue =
    | Integer of int
    | Double of double

type ApplicationCommandMaxValueTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<obj>

        member _.toTargetType value =
            match value :?> ApplicationCommandMaxValue with
            | ApplicationCommandMaxValue.Integer v -> v
            | ApplicationCommandMaxValue.Double v -> v

        member _.fromTargetType value =
            match value with
            | :? int as v -> ApplicationCommandMaxValue.Integer v
            | :? double as v -> ApplicationCommandMaxValue.Double v
            | _ -> failwith "Unexpected ApplicationCommandMaxValue type"

type AllowedMentionsParseType =
    | Roles
    | Users
    | Everyone

type AllowedMentionsParseTypeTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<string>

        member _.toTargetType value =
            match value :?> AllowedMentionsParseType with
            | AllowedMentionsParseType.Roles -> "roles"
            | AllowedMentionsParseType.Users -> "users"
            | AllowedMentionsParseType.Everyone -> "everyone"

        member _.fromTargetType value =
            match value with
            | v when v = "roles" -> AllowedMentionsParseType.Roles
            | v when v = "users" -> AllowedMentionsParseType.Users
            | v when v = "everyone" -> AllowedMentionsParseType.Everyone
            | _ -> failwith "Unexpected AllowedMentionsParseType type"

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

type GatewayEncodingTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<obj>

        member _.toTargetType value =
            match value :?> GatewayEncoding with
            | GatewayEncoding.JSON -> "json"
            | GatewayEncoding.ETF -> "etf"

        member _.fromTargetType value =
            match value with
            | v when v = "json" -> GatewayEncoding.JSON
            | v when v = "etf" -> GatewayEncoding.ETF
            | _ -> failwith "Unexpected GatewayEncoding type"

type GatewayCompression =
    | ZLIBSTREAM
    | ZSTDSTREAM
with
    override this.ToString () =
        match this with
        | GatewayCompression.ZLIBSTREAM -> "zlib-stream"
        | GatewayCompression.ZSTDSTREAM -> "zstd-stream"

type GatewayCompressionTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<string>

        member _.toTargetType value =
            match value :?> GatewayCompression with
            | GatewayCompression.ZLIBSTREAM -> "zlib-stream"
            | GatewayCompression.ZSTDSTREAM -> "zstd-stream"

        member _.fromTargetType value =
            match value with
            | v when v = "zlib-stream" -> GatewayCompression.ZLIBSTREAM
            | v when v = "zstd-stream" -> GatewayCompression.ZSTDSTREAM
            | _ -> failwith "Unexpected GatewayCompression type"

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

type GatewayCloseEventCode =
    | UNKNOWN_ERROR = 4000
    | UNKNOWN_OPCODE = 4001
    | DECODE_ERROR = 4002
    | NOT_AUTHORIZED = 4003
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

type StatusType =
    | ONLINE
    | DND
    | IDLE
    | INVISIBLE
    | OFFLINE
    
type StatusTypeTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<string>

        member _.toTargetType value =
            match value :?> StatusType with
            | StatusType.ONLINE -> "online"
            | StatusType.DND -> "dnd"
            | StatusType.IDLE -> "idle"
            | StatusType.INVISIBLE -> "invisible"
            | StatusType.OFFLINE -> "offline"

        member _.fromTargetType value =
            match value with
            | v when v = "online" -> StatusType.ONLINE
            | v when v = "dnd" -> StatusType.DND
            | v when v = "idle" -> StatusType.IDLE
            | v when v = "invisible" -> StatusType.INVISIBLE
            | v when v = "offline" -> StatusType.OFFLINE
            | _ -> failwith "Unexpected StatusType type"

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

type SoundboardSoundId =
    | String of string
    | Int of int

type SoundboardSoundIdTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<string>

        member _.toTargetType value =
            match value :?> SoundboardSoundId with
            | SoundboardSoundId.String v -> v
            | SoundboardSoundId.Int v -> v

        member _.fromTargetType value =
            match value with
            | :? string as v -> SoundboardSoundId.String v
            | :? int as v -> SoundboardSoundId.Int v
            | _ -> failwith "Unexpected SoundboardSoundId type"

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
type ActivityLocationKind =
    | GUILD_CHANNEL
    | PRIVATE_CHANNEL

type ActivityLocationKindTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<string>

        member _.toTargetType value =
            match value :?> ActivityLocationKind with
            | ActivityLocationKind.GUILD_CHANNEL -> "gc"
            | ActivityLocationKind.PRIVATE_CHANNEL -> "pc"

        member _.fromTargetType value =
            match value with
            | v when v = "gc" -> ActivityLocationKind.GUILD_CHANNEL
            | v when v = "pc" -> ActivityLocationKind.PRIVATE_CHANNEL
            | _ -> failwith "Unexpected ActivityLocationKind type"

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

type GuildIntegrationTypeTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<string>

        member _.toTargetType value =
            match value :?> GuildIntegrationType with
            | GuildIntegrationType.TWITCH -> "twitch"
            | GuildIntegrationType.YOUTUBE -> "youtube"
            | GuildIntegrationType.DISCORD -> "discord"
            | GuildIntegrationType.GUILD_SUBSCRIPTION -> "guild_subscription"

        member _.fromTargetType value =
            match value with
            | v when v = "twitch" -> GuildIntegrationType.TWITCH
            | v when v = "youtube" -> GuildIntegrationType.YOUTUBE
            | v when v = "discord" -> GuildIntegrationType.DISCORD
            | v when v = "guild_subscription" -> GuildIntegrationType.GUILD_SUBSCRIPTION
            | _ -> failwith "Unexpected GuildIntegrationType type"

type IntegrationExpireBehaviorType =
    | REMOVE_ROLE = 0
    | KICK = 1

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

type OAuth2ScopeTransform () =
    interface ITypeTransform with
        member _.targetType () =
            typeof<string>

        member _.toTargetType value =
            match value :?> OAuth2Scope with
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

        member _.fromTargetType value =
            match value with
            | v when v = "activities.read" -> OAuth2Scope.ACTIVITIES_READ
            | v when v = "activities.write" -> OAuth2Scope.ACTIVITIES_WRITE
            | v when v = "applications.builds.read" -> OAuth2Scope.APPLICATIONS_BUILDS_READ
            | v when v = "applications.builds.upload" -> OAuth2Scope.APPLICATIONS_BUILDS_UPLOAD
            | v when v = "applications.commands" -> OAuth2Scope.APPLICATIONS_COMMANDS
            | v when v = "applications.commands.update" -> OAuth2Scope.APPLICATIONS_COMMANDS_UPDATE
            | v when v = "applications.commands.permissions.update" -> OAuth2Scope.APPLICATIONS_COMMANDS_PERMISSIONS_UPDATE
            | v when v = "applications.entitlements" -> OAuth2Scope.APPLICATIONS_ENTITLEMENTS
            | v when v = "applications.store.update" -> OAuth2Scope.APPLICATIONS_STORE_UPDATE
            | v when v = "bot" -> OAuth2Scope.BOT
            | v when v = "connections" -> OAuth2Scope.CONNECTIONS
            | v when v = "dm_channels.read" -> OAuth2Scope.DM_CHANNELS_READ
            | v when v = "email" -> OAuth2Scope.EMAIL
            | v when v = "gdm.join" -> OAuth2Scope.GDM_JOIN
            | v when v = "guilds" -> OAuth2Scope.GUILDS
            | v when v = "guilds.join" -> OAuth2Scope.GUILDS_JOIN
            | v when v = "guilds.members.read" -> OAuth2Scope.GUILDS_MEMBERS_READ
            | v when v = "identify" -> OAuth2Scope.IDENTIFY
            | v when v = "messages.read" -> OAuth2Scope.MESSAGES_READ
            | v when v = "relationships.read" -> OAuth2Scope.RELATIONSHIPS_READ
            | v when v = "role_connections.write" -> OAuth2Scope.ROLE_CONNECTIONS_WRITE
            | v when v = "rpc" -> OAuth2Scope.RPC
            | v when v = "rpc.activities.write" -> OAuth2Scope.RPC_ACTIVITIES_WRITE
            | v when v = "rpc.notifications.read" -> OAuth2Scope.RPC_NOTIFICATIONS_READ
            | v when v = "rpc.voice.read" -> OAuth2Scope.RPC_VOICE_READ
            | v when v = "rpc.voice.write" -> OAuth2Scope.RPC_VOICE_WRITE
            | v when v = "voice" -> OAuth2Scope.VOICE
            | v when v = "webhook.incoming" -> OAuth2Scope.WEBHOOK_INCOMING
            | _ -> failwith "Unexpected OAuth2Scope type"

// https://discord.com/developers/docs/resources/webhook#webhook-object-webhook-types
type WebhookType =
    | INCOMING = 1
    | CHANNEL_FOLLOWER = 2
    | APPLICATION = 3

// https://discord.com/developers/docs/resources/entitlement#create-test-entitlement-json-params
type EntitlementOwnerType =
    | GUILD_SUBSCRIPTION = 1
    | USER_SUBSCRIPTION = 2
