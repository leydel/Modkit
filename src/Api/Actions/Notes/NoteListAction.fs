namespace Modkit.Api.Actions

open Modkit.Api.Models
open Modkit.Api.Repositories
open System.Threading.Tasks

type INoteListAction =
    abstract member run:
        userId: string ->
        Task<Note list>
        
type NoteListAction (noteRepository: INoteRepository) =
    interface INoteListAction with
        member _.run userId = task {
            return! noteRepository.list userId
        }
