namespace Modkit.Api.Actions

open Modkit.Api.Repositories
open System.Threading.Tasks

type INoteRemoveAction =
    abstract member run:
        userId: string ->
        noteId: string ->
        Task<Result<unit, unit>>
        
type NoteRemoveAction (noteRepository: INoteRepository) =
    interface INoteRemoveAction with
        member _.run userId noteId = task {
            return! noteRepository.delete userId noteId
        }
