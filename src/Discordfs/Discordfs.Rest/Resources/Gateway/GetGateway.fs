namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Net
open System.Net.Http
open System.Text.Json.Serialization

type GetGatewayOkResponse = {
    [<JsonPropertyName "url">] Url: string
}

type GetGatewayResponse =
    | Ok of GetGatewayOkResponse
    | Other of HttpStatusCode

module Gateway =
    let getGateway
        (version: string)
        (encoding: GatewayEncoding)
        (compression: GatewayCompression option)
        (httpClient: HttpClient) =
            req {
                get "gateway"
                query "v" version
                query "encoding" (encoding.ToString())
                query "compress" (compression >>. _.ToString())
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetGatewayResponse.Ok (Http.toJson res)
                | status -> return GetGatewayResponse.Other status
            })
        