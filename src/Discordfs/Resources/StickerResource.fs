namespace Modkit.Discordfs.Resources

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IStickerResource =
    // https://discord.com/developers/docs/resources/sticker#list-guild-stickers
    abstract member ListGuildStickers:
        guildId: string ->
        Task<Sticker list>

    // TODO: Add other endpoints

type StickerResource (httpClientFactory: IHttpClientFactory, token: string) =
    interface IStickerResource with
        member _.ListGuildStickers
            guildId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/stickers"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json
