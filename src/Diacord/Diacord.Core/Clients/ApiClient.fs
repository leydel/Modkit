namespace Modkit.Diacord.Core.Clients

open Discordfs.Rest.Common
open Microsoft.Extensions.Configuration
open Modkit.Diacord.Core.Types
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks

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
