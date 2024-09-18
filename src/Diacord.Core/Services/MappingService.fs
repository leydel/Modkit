namespace Modkit.Diacord.Core.Services

open Microsoft.Extensions.Configuration
open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Common
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks

type IMappingService =
    abstract member save:
        guildId: string ->
        mappings: IDictionary<string, string> ->
        Task<Result<IDictionary<string, string>, unit>>

type MappingService (configuration: IConfiguration, httpClientFactory: IHttpClientFactory) =
    let apiBaseUrl = configuration.GetValue "apiBaseUrl"

    interface IMappingService with
        member _.save guildId mappings = task {
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

                return Ok mapping.Mappings
            with | _ ->
                return Error ()
        }
