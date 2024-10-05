namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
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

type GatewayResource (httpClientFactory, token) =
    interface IGatewayResource with
        member _.GetGateway version encoding compression =
            req {
                get "gateway"
                query "v" version
                query "encoding" (encoding.ToString())
                query "compress" (compression >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGatewayBot version encoding compression =
            req {
                get "gateway/bot"
                bot token
                query "v" version
                query "encoding" (encoding.ToString())
                query "compress" (compression >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
