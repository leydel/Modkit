namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Threading.Tasks

type CreateGuildEmoji(
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

type ModifyGuildEmoji(
    ?name:  string,
    ?roles: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "roles" roles
        }

type CreateApplicationEmoji(
    name:  string,
    image: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "image" image
        }

type ModifyApplicationEmoji(
    name: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
        }

type IEmojiResource =
    // https://discord.com/developers/docs/resources/emoji#list-guild-emojis
    abstract member ListGuildEmojis:
        guildId: string ->
        Task<Emoji list>

    // https://discord.com/developers/docs/resources/emoji#get-guild-emoji
    abstract member GetGuildEmoji:
        guildId: string ->
        emojiId: string ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#create-guild-emoji
    abstract member CreateGuildEmoji:
        guildId: string ->
        auditLogReason: string option ->
        content: CreateGuildEmoji ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#modify-guild-emoji
    abstract member ModifyGuildEmoji:
        guildId: string ->
        emojiId: string ->
        auditLogReason: string option ->
        content: ModifyGuildEmoji ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#delete-guild-emoji
    abstract member DeleteGuildEmoji:
        guildId: string ->
        emojiId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/emoji#list-application-emojis
    abstract member ListApplicationEmojis:
        applicationId: string ->
        Task<ListApplicationEmojisResponse>

    // https://discord.com/developers/docs/resources/emoji#get-application-emoji
    abstract member GetApplicationEmoji:
        applicationId: string ->
        emojiId: string ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#create-application-emoji
    abstract member CreateApplicationEmoji:
        applicationId: string ->
        content: CreateApplicationEmoji ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#modify-application-emoji
    abstract member ModifyApplicationEmoji:
        applicationId: string ->
        emojiId: string ->
        content: ModifyApplicationEmoji ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#delete-application-emoji
    abstract member DeleteApplicationEmoji:
        applicationId: string ->
        emojiId: string ->
        Task<unit>

type EmojiResource (httpClientFactory, token) =
    interface IEmojiResource with
        member _.ListGuildEmojis guildId =
            req {
                get $"guilds/{guildId}/emojis"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildEmoji guildId emojiId =
            req {
                get $"guilds/{guildId}/emojis/{emojiId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateGuildEmoji guildId auditLogReason content =
            req {
                post $"guilds/{guildId}/emojis"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildEmoji guildId emojiId auditLogReason content =
            req {
                patch $"guilds/{guildId}/emojis/{emojiId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteGuildEmoji guildId emojiId auditLogReason =
            req {
                delete $"guilds/{guildId}/emojis/{emojiId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.ListApplicationEmojis applicationId =
            req {
                get $"applications/{applicationId}/emojis"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetApplicationEmoji applicationId emojiId =
            req {
                get $"applications/{applicationId}/emojis/{emojiId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateApplicationEmoji applicationId content =
            req {
                post $"applications/{applicationId}/emojis"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyApplicationEmoji applicationId emojiId content =
            req {
                patch $"applications/{applicationId}/emojis/{emojiId}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteApplicationEmoji applicationId emojiId =
            req {
                delete $"applications/{applicationId}/emojis/{emojiId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait
