namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type ListGuildEmojisResponse =
    | Ok of Emoji list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetGuildEmojiResponse =
    | Ok of Emoji
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateGuildEmojiPayload(
    name:  string,
    image: string,
    roles: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "image" image
            required "roles" roles
        }

type CreateGuildEmojiResponse =
    | Created of Emoji
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyGuildEmojiPayload(
    ?name:  string,
    ?roles: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "roles" roles
        }

type ModifyGuildEmojiResponse =
    | Ok of Emoji
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteGuildEmojiResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ListApplicationEmojisResponse =
    | Ok of Emoji list
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetApplicationEmojiResponse =
    | Ok of Emoji
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type CreateApplicationEmojiPayload(
    name:  string,
    image: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "image" image
        }

type CreateApplicationEmojiResponse =
    | Created of Emoji
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type ModifyApplicationEmojiPayload(
    name: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
        }

type ModifyApplicationEmojiResponse =
    | Ok of Emoji
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type DeleteApplicationEmojiResponse =
    | NoContent
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Emoji =
    let listGuildEmojis
        (guildId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/emojis"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListGuildEmojisResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ListGuildEmojisResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListGuildEmojisResponse.TooManyRequests (Http.toJson res)
                | status -> return ListGuildEmojisResponse.Other status
            })

    let getGuildEmoji
        (guildId: string)
        (emojiId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/emojis/{emojiId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGuildEmojiResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetGuildEmojiResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetGuildEmojiResponse.TooManyRequests (Http.toJson res)
                | status -> return GetGuildEmojiResponse.Other status
            })

    let createGuildEmoji
        (guildId: string)
        (auditLogReason: string option)
        (content: CreateGuildEmojiPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"guilds/{guildId}/emojis"
                bot botToken
                audit auditLogReason
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map CreateGuildEmojiResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateGuildEmojiResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map CreateGuildEmojiResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateGuildEmojiResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateGuildEmojiResponse.Other status
            })
        
    let modifyGuildEmoji
        (guildId: string)
        (emojiId: string)
        (auditLogReason: string option)
        (content: ModifyGuildEmojiPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                patch $"guilds/{guildId}/emojis/{emojiId}"
                bot botToken
                audit auditLogReason
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyGuildEmojiResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyGuildEmojiResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyGuildEmojiResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyGuildEmojiResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyGuildEmojiResponse.Other status
            })

    let deleteGuildEmoji
        (guildId: string)
        (emojiId: string)
        (auditLogReason: string option)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"guilds/{guildId}/emojis/{emojiId}"
                bot botToken
                audit auditLogReason
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteGuildEmojiResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteGuildEmojiResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteGuildEmojiResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteGuildEmojiResponse.Other status
            })
        
    let listApplicationEmojis
        (applicationId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/emojis"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ListApplicationEmojisResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ListApplicationEmojisResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ListApplicationEmojisResponse.TooManyRequests (Http.toJson res)
                | status -> return ListApplicationEmojisResponse.Other status
            })

    let getApplicationEmoji
        (applicationId: string)
        (emojiId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"applications/{applicationId}/emojis/{emojiId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetApplicationEmojiResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetApplicationEmojiResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetApplicationEmojiResponse.TooManyRequests (Http.toJson res)
                | status -> return GetApplicationEmojiResponse.Other status
            })

    let createApplicationEmoji
        (applicationId: string)
        (content: CreateApplicationEmojiPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"applications/{applicationId}/emojis"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.Created -> return! Task.map CreateApplicationEmojiResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateApplicationEmojiResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map CreateApplicationEmojiResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateApplicationEmojiResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateApplicationEmojiResponse.Other status
            })
        
    let modifyApplicationEmoji
        (applicationId: string)
        (emojiId: string)
        (content: ModifyApplicationEmojiPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                patch $"applications/{applicationId}/emojis/{emojiId}"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyApplicationEmojiResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyApplicationEmojiResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyApplicationEmojiResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyApplicationEmojiResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyApplicationEmojiResponse.Other status
            })

    let deleteApplicationEmoji
        (applicationId: string)
        (emojiId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"applications/{applicationId}/emojis/{emojiId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteApplicationEmojiResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteApplicationEmojiResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteApplicationEmojiResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteApplicationEmojiResponse.Other status
            })
