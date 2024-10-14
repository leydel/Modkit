namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Net
open System.Net.Http
open System.Text.Json.Serialization

type GetGatewayBotOkResponse = {
    [<JsonPropertyName "url">] Url: string
    [<JsonPropertyName "shards">] Shards: int
    [<JsonPropertyName "session_start_limit">] SessionStartLimit: SessionStartLimit
}

type GetGatewayBotResponse =
    | Ok of GetGatewayBotOkResponse
    | Other of HttpStatusCode

module Gateway =
    let getGatewayBot
        (version: string)
        (encoding: GatewayEncoding)
        (compression: GatewayCompression option)
        botToken
        (httpClient: HttpClient) =
            req {
                get "gateway/bot"
                bot botToken
                query "v" version
                query "encoding" (encoding.ToString())
                query "compress" (compression >>. _.ToString())
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGatewayBotResponse.Ok (Http.toJson res)
                | status -> return GetGatewayBotResponse.Other status
            })
        