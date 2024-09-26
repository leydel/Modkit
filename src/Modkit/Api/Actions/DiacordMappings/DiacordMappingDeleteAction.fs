namespace Modkit.Api.Actions

open Modkit.Api.Repositories
open System.Threading.Tasks

type IDiacordMappingDeleteAction =
    abstract member run:
        guildId: string ->
        Task<Result<unit, unit>>
        
type DiacordMappingDeleteAction (diacordMappingRepository: IDiacordMappingRepository) =
    interface IDiacordMappingDeleteAction with
        member _.run guildId = task {
            return! diacordMappingRepository.delete guildId
        }
