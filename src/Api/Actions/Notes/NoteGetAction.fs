namespace Modkit.Api.Actions

open Modkit.Api.Models
open Modkit.Api.Repositories
open System.Threading.Tasks

type INoteGetAction =
    abstract member run:
        userId: string ->
        noteId: string ->
        Task<Result<Note, unit>>
        
type NoteGetAction (noteRepository: INoteRepository) =
    interface INoteGetAction with
        member _.run userId noteId = task {
            return! noteRepository.get userId noteId
        }
