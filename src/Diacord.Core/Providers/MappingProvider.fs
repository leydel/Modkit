namespace Modkit.Diacord.Core.Providers

open Microsoft.Extensions.Configuration
open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Common
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks

type IMappingProvider =
    abstract member get:
        guildId: string ->
        Task<Result<IDictionary<string, string>, unit>>

type MappingProvider (configuration: IConfiguration, httpClientFactory: IHttpClientFactory) =
    let apiBaseUrl = configuration.GetValue "apiBaseUrl"

    interface IMappingProvider with
        member _.get guildId = task {
            try
                let! mapping = 
                    Req.create
                        HttpMethod.Get
                        apiBaseUrl
                        $"diacord/mapping/{guildId}"
                    |> Req.send httpClientFactory
                    |> Res.json<ApiDiacordMapping>

                return Ok mapping.Mappings
            with | _ ->
                return Error ()
        }
