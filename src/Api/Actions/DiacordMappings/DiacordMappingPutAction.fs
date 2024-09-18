namespace Modkit.Api.Actions

open Modkit.Api.Models
open Modkit.Api.Repositories
open System.Collections.Generic
open System.Threading.Tasks

type IDiacordMappingPutAction =
    abstract member run:
        guildId: string ->
        mappings: IDictionary<string, string> ->
        Task<Result<DiacordMapping, unit>>
        
type DiacordMappingPutAction (diacordMappingRepository: IDiacordMappingRepository) =
    interface IDiacordMappingPutAction with
        member _.run guildId mappings = task {
            return! diacordMappingRepository.put guildId mappings
        }
