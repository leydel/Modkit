namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type SendSoundboardSoundPayload (
    sound_id:         string,
    ?source_guild_id: string
) =
    inherit Payload() with
        override _.Content = json {
            required "sound_id" sound_id
            optional "source_guild_id" source_guild_id
        }

type SendSoundboardSoundResponse =
    | NoContent
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ListDefaultSoundboardSoundsResponse =
    | Ok of SoundboardSound list
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ListGuildSoundboardSoundsResponse =
    | Ok of SoundboardSound list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode
    
type GetGuildSoundboardSoundResponse =
    | Ok of SoundboardSound
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateGuildSoundboardSoundPayload (
    name: string,
    sound: string,
    ?volume: double option,
    ?emoji_id: string option,
    ?emoji_name: string option
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "sound" sound
            optional "volume" volume
            optional "emoji_id" emoji_id
            optional "emoji_name" emoji_name
        }
    
type CreateGuildSoundboardSoundResponse =
    | Created of SoundboardSound
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildSoundboardSoundPayload (
    ?name: string,
    ?volume: double option,
    ?emoji_id: string option,
    ?emoji_name: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "volume" volume
            optional "emoji_id" emoji_id
            optional "emoji_name" emoji_name
        }
    
type ModifyGuildSoundboardSoundResponse =
    | Ok of SoundboardSound
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteGuildSoundboardSoundResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Soundboard =
    let sendSoundboardSound
        (channelId: string)
        (content: SendSoundboardSoundPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"channels/{channelId}/send-soundboard-sound"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return SendSoundboardSoundResponse.NoContent
                | HttpStatusCode.BadRequest -> return! Task.map SendSoundboardSoundResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map SendSoundboardSoundResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map SendSoundboardSoundResponse.TooManyRequests (Http.toJson res)
                | status -> return SendSoundboardSoundResponse.Other status
            })

    let listDefaultSoundboardSounds
        botToken
        (httpClient: HttpClient) =
            req {
                get "soundboard-default-sounds"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListDefaultSoundboardSoundsResponse.Ok (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListDefaultSoundboardSoundsResponse.TooManyRequests (Http.toJson res)
                | status -> return ListDefaultSoundboardSoundsResponse.Other status
            })

    let listGuildSoundboardSounds
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/soundboard-sounds"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListGuildSoundboardSoundsResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ListGuildSoundboardSoundsResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListGuildSoundboardSoundsResponse.TooManyRequests (Http.toJson res)
                | status -> return ListGuildSoundboardSoundsResponse.Other status
            })

    let getGuildSoundboardSound
        (guildId: string)
        (soundId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/soundboard-sounds/{soundId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildSoundboardSoundResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildSoundboardSoundResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildSoundboardSoundResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildSoundboardSoundResponse.Other status
            })

    let createGuildSoundboardSound
        (guildId: string)
        (auditLogReason: string option)
        (content: CreateGuildSoundboardSoundPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"guilds/{guildId}/soundboard-sounds"
                bot botToken
                audit auditLogReason
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map CreateGuildSoundboardSoundResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateGuildSoundboardSoundResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map CreateGuildSoundboardSoundResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGuildSoundboardSoundResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGuildSoundboardSoundResponse.Other status
            })

    let modifyGuildSoundboardSound
        (guildId: string)
        (soundId: string)
        (auditLogReason: string option)
        (content: ModifyGuildSoundboardSoundPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                patch $"guilds/{guildId}/soundboard-sounds/{soundId}"
                bot botToken
                audit auditLogReason
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyGuildSoundboardSoundResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildSoundboardSoundResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildSoundboardSoundResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildSoundboardSoundResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildSoundboardSoundResponse.Other status
            })

    let deleteGuildSoundboardSound
        (guildId: string)
        (soundId: string)
        (auditLogReason: string option)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"guilds/{guildId}/soundboard-sounds/{soundId}"
                bot botToken
                audit auditLogReason
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteGuildSoundboardSoundResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteGuildSoundboardSoundResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteGuildSoundboardSoundResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteGuildSoundboardSoundResponse.Other status
            })
