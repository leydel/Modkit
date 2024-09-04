namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpGatewayActions =
    abstract member GetGateway:
        version: string ->
        encoding: GatewayEncoding ->
        compression: GatewayCompression option ->
        Task<GetGateway>

    abstract member GetGatewayBot:
        version: string ->
        encoding: GatewayEncoding ->
        compression: GatewayCompression option ->
        Task<GetGatewayBot>

type DiscordHttpGatewayActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpGatewayActions with
        member _.GetGateway version encoding compression =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"gateway"
            |> Req.query "v" version
            |> Req.query "encoding" (encoding.ToString())
            |> Req.queryOpt "compress" (Option.map _.ToString() compression)
            |> Req.send httpClientFactory
            |> Res.body

        member _.GetGatewayBot version encoding compression =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"gateway/bot"
            |> Req.bot token
            |> Req.query "v" version
            |> Req.query "encoding" (encoding.ToString())
            |> Req.queryOpt "compress" (Option.map _.ToString() compression)
            |> Req.send httpClientFactory
            |> Res.body
