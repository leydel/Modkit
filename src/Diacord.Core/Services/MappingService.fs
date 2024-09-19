namespace Modkit.Diacord.Core.Services

open Modkit.Diacord.Core.Clients
open System.Collections.Generic
open System.Threading.Tasks

type IMappingService =
    abstract member save:
        guildId: string ->
        mappings: IDictionary<string, string> ->
        Task<Result<IDictionary<string, string>, unit>>

type MappingService (apiClient: IApiClient) =
    interface IMappingService with
        member _.save guildId mappings = task {
            let! res = apiClient.PutDiacordMapping guildId mappings

            match res with
            | Error _ -> return Error ()
            | Ok mapping -> return Ok mapping.Mappings
        }
