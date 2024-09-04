namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpChannelActions =
    abstract member CreateChannelInvite:
        channelId: string ->
        payload: CreateChannelInvite ->
        Task<Invite>

type DiscordHttpChannelActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpChannelActions with
        member _.CreateChannelInvite channelId payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"channels/{channelId}/invites"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body
