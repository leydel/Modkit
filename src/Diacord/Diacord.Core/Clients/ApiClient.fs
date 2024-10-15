namespace Modkit.Diacord.Core.Clients

open Microsoft.Extensions.Configuration
open Modkit.Diacord.Core.Types
open System
open System.Collections.Generic
open System.Net.Http
open System.Text.Json
open System.Threading.Tasks
open System.Web

// TODO: Delete Dto, Req, and Res modules and use some better approach (moved here as only remaining usage)

type Dto () =
    member val private Properties: IDictionary<string, obj> = Dictionary()

    /// Add a property to the DTO.
    static member property (key: string) (value: 'a) (dto: Dto) =
        dto.Properties.Add(key, value)
        dto

    /// Add a property to the DTO if the option is some.
    static member propertyIf (key: string) (value: 'a option) (dto: Dto) =
        if value.IsSome then
            dto.Properties.Add(key, value)
        dto

    /// Get the underlying data of the DTO to use as a value of another DTO's property.
    static member object (dto: Dto) =
        dto.Properties

    /// Serialize the DTO into json.
    static member json (dto: Dto) =
        Json.serializeF dto.Properties

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

    let send (httpClientFactory: IHttpClientFactory) (req: HttpRequestMessage) =
        httpClientFactory.CreateClient().SendAsync req

module Res =
    let json<'a> (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        let! body = res.Content.ReadAsStringAsync()
        return Json.deserializeF<'a> body
    }

    let raw (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        return! res.Content.ReadAsStringAsync()
    }

    let ignore (resTask: Task<HttpResponseMessage>) = task {
        do! resTask :> Task
    }
    

type IApiClient =
    abstract member GetDiacordMapping:
        guildId: string ->
        Task<Result<ApiDiacordMapping, unit>>

    abstract member PutDiacordMapping:
        guildId: string ->
        mappings: IDictionary<string, string> ->
        Task<Result<ApiDiacordMapping, unit>>

type ApiClient (configuration: IConfiguration, httpClientFactory: IHttpClientFactory) =
    let apiBaseUrl = configuration.GetValue "apiBaseUrl"

    interface IApiClient with
        member _.GetDiacordMapping guildId = task {
            try
                let! mapping = 
                    Req.create
                        HttpMethod.Get
                        apiBaseUrl
                        $"diacord/mapping/{guildId}"
                    |> Req.send httpClientFactory
                    |> Res.json<ApiDiacordMapping>

                return Ok mapping
            with | _ ->
                return Error ()
        }

        member _.PutDiacordMapping guildId mappings = task {
            try
                let! mapping = 
                    Req.create
                        HttpMethod.Put
                        apiBaseUrl
                        $"diacord/mapping/{guildId}"
                    |> Req.json (
                        Dto()
                        |> Dto.property "mappings" mappings
                        |> Dto.json
                    )
                    |> Req.send httpClientFactory
                    |> Res.json<ApiDiacordMapping>

                return Ok mapping
            with | _ ->
                return Error ()
        }
