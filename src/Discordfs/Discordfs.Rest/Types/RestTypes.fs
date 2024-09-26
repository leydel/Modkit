namespace Discordfs.Rest.Types

open Discordfs.Types
open System
open System.Text.Json.Serialization

#nowarn "49"

type VoiceChannelEffect = {
    [<JsonName "channel_id">] ChannelId: string
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "user_id">] UserId: string
    [<JsonName "emoji">] Emoji: Emoji option
    [<JsonName "animation_type">] AnimationType: AnimationType option
    [<JsonName "animation_id">] AnimationId: int option
    [<JsonName "sound_id">] [<JsonConverter(typeof<SoundboardSoundIdConverter>)>] SoundId: SoundboardSoundId option
    [<JsonName "sound_volume">] SoundVolume: double option
}

type ListPublicArchivedThreadsResponse = {
    [<JsonName "threads">]
    Threads: Channel list
    
    [<JsonName "members">]
    Members: ThreadMember list
    
    [<JsonName "has_more">]
    HasMore: bool
}

type ListPrivateArchivedThreadsResponse = {
    [<JsonName "threads">]
    Threads: Channel list
    
    [<JsonName "members">]
    Members: ThreadMember list
    
    [<JsonName "has_more">]
    HasMore: bool
}

type ListJoinedPrivateArchivedThreadsResponse = {
    [<JsonName "threads">]
    Threads: Channel list
    
    [<JsonName "members">]
    Members: ThreadMember list
    
    [<JsonName "has_more">]
    HasMore: bool
}

// https://discord.com/developers/docs/resources/emoji#list-application-emojis
type ListApplicationEmojisResponse = {
    [<JsonName "items">]
    Items: Emoji list
}

type GetGatewayResponse = {
    [<JsonName "url">]
    Url: string
}

type GetGatewayBotResponse = {
    [<JsonName "url">]
    Url: string

    [<JsonName "shards">]
    Shards: int

    [<JsonName "session_start_limit">]
    SessionStartLimit: SessionStartLimit
}

// https://discord.com/developers/docs/resources/guild#modify-guild-channel-positions-json-params
type ModifyGuildChannelPosition = {
    [<JsonName "id">] Id: string
    [<JsonName "position">] Position: int option
    [<JsonName "lock_permissions">] LockPermissions: bool option
    [<JsonName "parent_id">] ParentId: string option
}

// https://discord.com/developers/docs/resources/guild#list-active-guild-threads-response-body
type ListActiveGuildThreadsResponse = {
    [<JsonName "threads">] Threads: Channel list
    [<JsonName "members">] Members: GuildMember list
}

// https://discord.com/developers/docs/resources/guild#bulk-guild-ban-bulk-ban-response
type BulkGuildBanResponse = {
    [<JsonName "banned_users">] BannedUsers: string list
    [<JsonName "failed_users">] FailedUsers: string list
}

// https://discord.com/developers/docs/resources/guild#modify-guild-role-positions-json-params
type ModifyGuildRolePosition = {
    [<JsonName "id">] Id: string
    [<JsonName "position">] Position: int option
}

// https://discord.com/developers/docs/resources/guild#get-guild-prune-count
type GetGuildPruneCountResponse = {
    [<JsonName "pruned">] Pruned: int
}

// https://discord.com/developers/docs/resources/guild#begin-guild-prune
type BeginGuildPruneResponse = {
    [<JsonName "pruned">] Pruned: int option
}

// https://discord.com/developers/docs/resources/guild#get-guild-vanity-url
type GetGuildVanityUrlResponse = {
    [<JsonName "code">] Code: string option
    [<JsonName "uses">] Uses: int
}

// https://discord.com/developers/docs/resources/poll#get-answer-voters-response-body
type GetAnswerVotersResponse = {
    [<JsonName "users">] Users: User list
}

// https://discord.com/developers/docs/resources/sticker#list-sticker-packs-response-structure
type ListStickerPacksResponse = {
    [<JsonName "sticker_packs">] StickerPacks: StickerPack list
}

// https://discord.com/developers/docs/topics/oauth2#get-current-authorization-information-response-structure
type GetCurrentAuthorizationInformationResponse = {
    [<JsonName "application">] Application: Application
    [<JsonName "scopes">] [<JsonConverter(typeof<OAuth2ScopeConverter>)>] Scopes: OAuth2Scope list // TODO: Test if converter works on list
    [<JsonName "expires">] Expires: DateTime
    [<JsonName "user">] User: User option
}
