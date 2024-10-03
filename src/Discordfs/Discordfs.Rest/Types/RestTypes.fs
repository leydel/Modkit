namespace Discordfs.Rest.Types

open Discordfs.Types
open System
open System.Collections.Generic
open System.Text.Json.Serialization

#nowarn "49"

type InteractionCallbackResponse = {
    [<JsonPropertyName "interaction">] Interaction: InteractionCallback
    [<JsonPropertyName "resource">] Resource: InteractionCallbackResource
}

type BulkOverwriteApplicationCommand = {
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: IDictionary<string, string> option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "description_localizations">] DescriptionLocalizations: IDictionary<string, string> option
    [<JsonPropertyName "options">] Options: ApplicationCommandOption list option
    [<JsonPropertyName "default_member_permissions">] DefaultMemberPermissions: string option
    [<JsonPropertyName "integration_types">] IntegrationTypes: ApplicationIntegrationType list option
    [<JsonPropertyName "contexts">] Contexts: InteractionContextType list option
    [<JsonPropertyName "type">] Type: ApplicationCommandType option
    [<JsonPropertyName "nsfw">] Nsfw: bool option
}

type VoiceChannelEffect = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "emoji">] Emoji: Emoji option
    [<JsonPropertyName "animation_type">] AnimationType: AnimationType option
    [<JsonPropertyName "animation_id">] AnimationId: int option
    [<JsonPropertyName "sound_id">] SoundId: SoundboardSoundId option
    [<JsonPropertyName "sound_volume">] SoundVolume: double option
}

type ListPublicArchivedThreadsResponse = {
    [<JsonPropertyName "threads">]
    Threads: Channel list
    
    [<JsonPropertyName "members">]
    Members: ThreadMember list
    
    [<JsonPropertyName "has_more">]
    HasMore: bool
}

type ListPrivateArchivedThreadsResponse = {
    [<JsonPropertyName "threads">]
    Threads: Channel list
    
    [<JsonPropertyName "members">]
    Members: ThreadMember list
    
    [<JsonPropertyName "has_more">]
    HasMore: bool
}

type ListJoinedPrivateArchivedThreadsResponse = {
    [<JsonPropertyName "threads">]
    Threads: Channel list
    
    [<JsonPropertyName "members">]
    Members: ThreadMember list
    
    [<JsonPropertyName "has_more">]
    HasMore: bool
}

// https://discord.com/developers/docs/resources/emoji#list-application-emojis
type ListApplicationEmojisResponse = {
    [<JsonPropertyName "items">]
    Items: Emoji list
}

type GetGatewayResponse = {
    [<JsonPropertyName "url">]
    Url: string
}

type GetGatewayBotResponse = {
    [<JsonPropertyName "url">]
    Url: string

    [<JsonPropertyName "shards">]
    Shards: int

    [<JsonPropertyName "session_start_limit">]
    SessionStartLimit: SessionStartLimit
}

// https://discord.com/developers/docs/resources/guild#modify-guild-channel-positions-json-params
type ModifyGuildChannelPosition = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "lock_permissions">] LockPermissions: bool option
    [<JsonPropertyName "parent_id">] ParentId: string option
}

// https://discord.com/developers/docs/resources/guild#list-active-guild-threads-response-body
type ListActiveGuildThreadsResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: GuildMember list
}

// https://discord.com/developers/docs/resources/guild#bulk-guild-ban-bulk-ban-response
type BulkGuildBanResponse = {
    [<JsonPropertyName "banned_users">] BannedUsers: string list
    [<JsonPropertyName "failed_users">] FailedUsers: string list
}

// https://discord.com/developers/docs/resources/guild#modify-guild-role-positions-json-params
type ModifyGuildRolePosition = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "position">] Position: int option
}

// https://discord.com/developers/docs/resources/guild#get-guild-prune-count
type GetGuildPruneCountResponse = {
    [<JsonPropertyName "pruned">] Pruned: int
}

// https://discord.com/developers/docs/resources/guild#begin-guild-prune
type BeginGuildPruneResponse = {
    [<JsonPropertyName "pruned">] Pruned: int option
}

// https://discord.com/developers/docs/resources/guild#get-guild-vanity-url
type GetGuildVanityUrlResponse = {
    [<JsonPropertyName "code">] Code: string option
    [<JsonPropertyName "uses">] Uses: int
}

// https://discord.com/developers/docs/resources/poll#get-answer-voters-response-body
type GetAnswerVotersResponse = {
    [<JsonPropertyName "users">] Users: User list
}

// https://discord.com/developers/docs/resources/sticker#list-sticker-packs-response-structure
type ListStickerPacksResponse = {
    [<JsonPropertyName "sticker_packs">] StickerPacks: StickerPack list
}

// https://discord.com/developers/docs/topics/oauth2#get-current-authorization-information-response-structure
type GetCurrentAuthorizationInformationResponse = {
    [<JsonPropertyName "application">] Application: Application
    [<JsonPropertyName "scopes">] Scopes: OAuth2Scope list
    [<JsonPropertyName "expires">] Expires: DateTime
    [<JsonPropertyName "user">] User: User option
}
