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

[<JsonConverter(typeof<RateLimitScopeConverter>)>]
type RateLimitScope =
    | USER
    | GLOBAL
    | SHARED

and RateLimitScopeConverter () =
    inherit JsonConverter<RateLimitScope>()

    override _.Read (reader, typeToConvert, options) =
        match reader.GetString() with
        | "user" -> RateLimitScope.USER
        | "global" -> RateLimitScope.GLOBAL
        | "shared" -> RateLimitScope.SHARED
        | _ -> failwith "Unexpected RateLimitScope provided"

    override _.Write (writer, value, options) =
        match value with
        | USER -> "user"
        | GLOBAL -> "global"
        | SHARED -> "shared"
        |> writer.WriteStringValue

type RateLimitHeaders = {
    Limit: int option
    Remaining: int option
    Reset: DateTime option
    ResetAfter: double option
    Bucket: string option
    Global: bool option
    Scope: RateLimitScope option
}

type RateLimitInfo<'a> = 'a * RateLimitHeaders

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
