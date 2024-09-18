namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpEmojiActions =
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
        name: string ->
        image: string ->
        roles: string list ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#modify-guild-emoji
    abstract member ModifyGuildEmoji:
        guildId: string ->
        emojiId: string ->
        auditLogReason: string option ->
        name: string ->
        roles: string list ->
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
        name: string ->
        image: string ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#modify-application-emoji
    abstract member ModifyApplicationEmoji:
        applicationId: string ->
        emojiId: string ->
        name: string ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#delete-application-emoji
    abstract member DeleteApplicationEmoji:
        applicationId: string ->
        emojiId: string ->
        Task<unit>

type DiscordHttpEmojiActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpEmojiActions with
        member _.ListGuildEmojis
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/emojis"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGuildEmoji
            guildId emojiId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/emojis/{emojiId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.CreateGuildEmoji
            guildId auditLogReason name image roles =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/emojis"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property "name" name
                    |> Dto.property "image" image
                    |> Dto.property "roles" roles
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json
            
        member _.ModifyGuildEmoji
            guildId emojiId auditLogReason name roles =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/emojis/{emojiId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property "name" name
                    |> Dto.property "roles" roles
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteGuildEmoji
            guildId emojiId auditLogReason =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/emojis/{emojiId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.ListApplicationEmojis
            applicationId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/emojis"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json
            
        member _.GetApplicationEmoji
            applicationId emojiId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/emojis/{emojiId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.CreateApplicationEmoji
            applicationId name image =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/emojis"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.property "name" name
                    |> Dto.property "image" image
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json
            
        member _.ModifyApplicationEmoji
            applicationId emojiId name =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/emojis/{emojiId}"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.property "name" name
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteApplicationEmoji
            applicationId emojiId =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/emojis/{emojiId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore
