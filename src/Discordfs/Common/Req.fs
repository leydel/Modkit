namespace Modkit.Discordfs.Common

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

    let audit (reason: string option) (req: HttpRequestMessage) =
        headerOpt "X-Audit-Log-Reason" reason req

    let query (key: string) (value: string) (req: HttpRequestMessage) =
        let uriBuilder = UriBuilder(req.RequestUri)
        let query = HttpUtility.ParseQueryString(uriBuilder.Query)
        query.Add(key, value)
        uriBuilder.Query <- query.ToString()
        req.RequestUri <- uriBuilder.Uri
        req

    let queryOpt (key: string) (value: string option) (req: HttpRequestMessage) =
        Option.foldBack (query key) value req

    let json (json: string) (req: HttpRequestMessage) =
        req.Content <- new StringContent(json)
        header "Content-Type" "application/json" req

    // TODO: Create method for form content

    let send (httpClientFactory: IHttpClientFactory) (req: HttpRequestMessage) =
        httpClientFactory.CreateClient().SendAsync req
