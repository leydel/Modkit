namespace Discordfs.Rest.Types

open Discordfs.Rest.Common
open Discordfs.Types
open System
open System.Collections.Generic
open System.Net
open System.Net.Http
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

type RateLimitHeaders = {
    TODO: bool // Parse rate limit headers into this type
}

type RateLimitInfo<'a> = 'a * RateLimitHeaders

type DiscordResponse<'a> = Result<RateLimitInfo<'a>, RateLimitInfo<DiscordError>>

module DiscordResponse =
    let private withRateLimitHeaders<'a> (res: HttpResponseMessage) (obj: 'a) =
        let rateLimitHeaders = { TODO = true } // TODO: Get properly
        (obj, rateLimitHeaders)

    let asJson<'a> (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | v when v >= 200 && v < 300 -> return! (Http.toJson<'a> res) ?> withRateLimitHeaders res ?> Ok
        | v when v = 429 -> return! RateLimit <? (Http.toJson res) ?> withRateLimitHeaders res ?> Error
        | v when v >= 400 && v < 500 -> return! ClientError <? (Http.toJson res) ?> withRateLimitHeaders res ?> Error
        | _ -> return Unexpected (res.StatusCode) |> withRateLimitHeaders res |> Error
    }

    let unwrap<'a> (res: DiscordResponse<'a>) =
        match res with
        | Ok (v, _) -> v
        | Error _ -> failwith "Unsuccessful discord response was unwrapped"

    // TODO: Figure out how 204 would work with this
    // TODO: Figure out how an endpoint returning either 200 or 204 would work

    // NOTE: Ideally, 204 could be treated as Option<Empty>.None, meaning 200/204 is Option<'a> and 200 is 'a

// TODO: Move remaining payloads below into resources as refactored into modules

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
