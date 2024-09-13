namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpGuildActions =
    // https://discord.com/developers/docs/resources/guild#get-guild-roles
    abstract member GetGuildRoles:
        guildId: string ->
        Task<Role list>

    // TODO: Add other endpoints

type DiscordHttpGuildActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpGuildActions with
        member _.GetGuildRoles guildId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/roles"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body
