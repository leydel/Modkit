namespace Modkit.Api.Actions

open Modkit.Api.Models
open Modkit.Api.Repositories
open System
open System.Threading.Tasks

type INoteAddAction =
    abstract member run:
        userId: string ->
        message: string ->
        Task<Result<Note, unit>>
        
type NoteAddAction (noteRepository: INoteRepository) =
    interface INoteAddAction with
        member _.run userId message = task {
            return! noteRepository.create userId message DateTime.UtcNow
        }
