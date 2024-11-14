namespace Discordfs.Rest.Types

open Discordfs.Types
open System
open System.Collections.Generic
open System.Net
open System.Text.Json.Serialization

type RateLimitResponse = {
    [<JsonPropertyName "message">] Message: string
    [<JsonPropertyName "retry_after">] RetryAfter: float
    [<JsonPropertyName "global">] Global: bool
    [<JsonPropertyName "code">] Code: JsonErrorCode option
}

type ErrorResponse = {
    [<JsonPropertyName "code">] Code: JsonErrorCode
    [<JsonPropertyName "message">] Message: string
    [<JsonPropertyName "errors">] Errors: IDictionary<string, string>
}

type DiscordError =
    | RateLimit of RateLimitResponse
    | ClientError of ErrorResponse
    | Unexpected of HttpStatusCode

type RateLimitScope =
    | USER
    | GLOBAL
    | SHARED
with
    override this.ToString () =
        match this with
        | USER -> "user"
        | GLOBAL -> "global"
        | SHARED -> "shared"

    static member FromString (str: string) =
        match str with
        | "user" -> RateLimitScope.USER
        | "global" -> RateLimitScope.GLOBAL
        | "shared" -> RateLimitScope.SHARED
        | _ -> failwith "Unexpected RateLimitScope provided"

type RateLimitHeaders = {
    Limit: int option
    Remaining: int option
    Reset: DateTime option
    ResetAfter: double option
    Bucket: string option
    Global: bool option
    Scope: RateLimitScope option
}

type ResponseWithMetadata<'a> = {
    Data: 'a
    RateLimitHeaders: RateLimitHeaders
    Status: HttpStatusCode
}

// TODO: Move remaining payloads below into resources as refactored into modules

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

// https://discord.com/developers/docs/resources/emoji#list-application-emojis
type ListApplicationEmojisResponse = {
    [<JsonPropertyName "items">] Items: Emoji list
}

// https://discord.com/developers/docs/resources/sticker#list-sticker-packs-response-structure
type ListStickerPacksResponse = {
    [<JsonPropertyName "sticker_packs">] StickerPacks: StickerPack list
}

// https://discord.com/developers/docs/topics/oauth2#get-current-authorization-information-response-structure
type GetCurrentAuthorizationInformationResponse = {
    [<JsonPropertyName "application">] Application: PartialApplication
    [<JsonPropertyName "scopes">] Scopes: OAuth2Scope list
    [<JsonPropertyName "expires">] Expires: DateTime
    [<JsonPropertyName "user">] User: User option
}
