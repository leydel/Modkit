namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpStickerActions =
    // https://discord.com/developers/docs/resources/sticker#list-guild-stickers
    abstract member ListGuildStickers:
        guildId: string ->
        Task<Sticker list>

    // TODO: Add other endpoints

type DiscordHttpStickerActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpStickerActions with
        member _.ListGuildStickers guildId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/stickers"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body
