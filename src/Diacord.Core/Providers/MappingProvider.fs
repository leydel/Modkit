namespace Modkit.Diacord.Core.Providers

open Modkit.Diacord.Core.Clients
open System.Collections.Generic
open System.Threading.Tasks

type IMappingProvider =
    abstract member get:
        guildId: string ->
        Task<Result<IDictionary<string, string>, unit>>

type MappingProvider (apiClient: IApiClient) =
    interface IMappingProvider with
        member _.get guildId = task {
            let! res = apiClient.GetDiacordMapping guildId

            match res with
            | Error _ -> return Error ()
            | Ok mapping -> return Ok mapping.Mappings
        }
