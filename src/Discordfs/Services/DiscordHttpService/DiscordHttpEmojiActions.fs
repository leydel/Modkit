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
        payload: CreateGuildEmoji ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#modify-guild-emoji
    abstract member ModifyGuildEmoji:
        guildId: string ->
        emojiId: string ->
        auditLogReason: string option ->
        payload: ModifyGuildEmoji ->
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
        payload: CreateApplicationEmoji ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#modify-application-emoji
    abstract member ModifyApplicationEmoji:
        applicationId: string ->
        emojiId: string ->
        payload: ModifyApplicationEmoji ->
        Task<Emoji>

    // https://discord.com/developers/docs/resources/emoji#delete-application-emoji
    abstract member DeleteApplicationEmoji:
        applicationId: string ->
        emojiId: string ->
        Task<unit>

type DiscordHttpEmojiActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpEmojiActions with
        member _.ListGuildEmojis guildId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/emojis"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.GetGuildEmoji guildId emojiId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/emojis/{emojiId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.CreateGuildEmoji guildId auditLogReason payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/emojis"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body
            
        member _.ModifyGuildEmoji guildId emojiId auditLogReason payload =
            Req.create
                HttpMethod.Patch
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/emojis/{emojiId}"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.DeleteGuildEmoji guildId emojiId auditLogReason =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/emojis/{emojiId}"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.ListApplicationEmojis applicationId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"applications/{applicationId}/emojis"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body
            
        member _.GetApplicationEmoji applicationId emojiId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"applications/{applicationId}/emojis/{emojiId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.CreateApplicationEmoji applicationId payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"applications/{applicationId}/emojis"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body
            
        member _.ModifyApplicationEmoji applicationId emojiId payload =
            Req.create
                HttpMethod.Patch
                Constants.DISCORD_API_URL
                $"applications/{applicationId}/emojis/{emojiId}"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.DeleteApplicationEmoji applicationId emojiId =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"applications/{applicationId}/emojis/{emojiId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore
