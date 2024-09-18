namespace Modkit.Api.Actions

open Modkit.Api.Models
open Modkit.Api.Repositories
open System.Threading.Tasks

type IDiacordMappingGetAction =
    abstract member run:
        guildId: string ->
        Task<Result<DiacordMapping, unit>>
        
type DiacordMappingGetAction (diacordMappingRepository: IDiacordMappingRepository) =
    interface IDiacordMappingGetAction with
        member _.run guildId = task {
            return! diacordMappingRepository.get guildId
        }
