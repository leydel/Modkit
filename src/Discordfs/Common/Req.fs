namespace Modkit.Discordfs.Common

open FSharp.Json
open System
open System.Net.Http
open System.Web

module Req =
    let create (method: HttpMethod) (host: string) (endpoint: string) =
        new HttpRequestMessage(method, host + "/" + endpoint)

    let header (key: string) (value: string) (req: HttpRequestMessage) =
        req.Headers.Add(key, value)
        req

    let headerOpt (key: string) (value: string option) (req: HttpRequestMessage) =
        Option.foldBack (header key) value req

    let bot (token: string) (req: HttpRequestMessage) =
        header "Authorization" $"Bot {token}" req

    let oauth (token: string) (req: HttpRequestMessage) =
        header "Authorization" $"Bearer {token}" req

    let query (key: string) (value: string) (req: HttpRequestMessage) =
        let uriBuilder = UriBuilder(req.RequestUri)
        let query = HttpUtility.ParseQueryString(uriBuilder.Query)
        query.Add(key, value)
        uriBuilder.Query <- query.ToString()
        req.RequestUri <- uriBuilder.Uri
        req

    let queryOpt (key: string) (value: string option) (req: HttpRequestMessage) =
        Option.foldBack (query key) value req

    let body (payload: 'a) (req: HttpRequestMessage) =
        req.Content <- new StringContent (Json.serializeU payload)
        req

    let send (httpClientFactory: IHttpClientFactory) (req: HttpRequestMessage) =
        httpClientFactory.CreateClient().SendAsync req
