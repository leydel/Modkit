namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IGatewayResource =
    abstract member GetGateway:
        version: string ->
        encoding: GatewayEncoding ->
        compression: GatewayCompression option ->
        Task<GetGatewayResponse>

    abstract member GetGatewayBot:
        version: string ->
        encoding: GatewayEncoding ->
        compression: GatewayCompression option ->
        Task<GetGatewayBotResponse>

type GatewayResource (httpClientFactory: IHttpClientFactory, token: string) =
    interface IGatewayResource with
        member _.GetGateway
            version encoding compression =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"gateway"
                |> Req.query "v" version
                |> Req.query "encoding" (encoding.ToString())
                |> Req.queryOpt "compress" (Option.map _.ToString() compression)
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetGatewayBot
            version encoding compression =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"gateway/bot"
                |> Req.bot token
                |> Req.query "v" version
                |> Req.query "encoding" (encoding.ToString())
                |> Req.queryOpt "compress" (Option.map _.ToString() compression)
                |> Req.send httpClientFactory
                |> Res.json
