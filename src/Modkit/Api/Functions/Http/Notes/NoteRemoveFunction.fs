namespace Modkit.Api.Functions

open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Api.Actions
open System.Net
open System.Net.Http

type NoteRemoveFunction (noteRemoveAction: INoteRemoveAction) =
    [<Function(nameof NoteRemoveFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/{userId}/notes/{noteId}")>] req: HttpRequestMessage,
        log: ILogger,
        userId: string,
        noteId: string
    ) = task {
        let! action = noteRemoveAction.run userId noteId

        match action with
        | Error _ ->
            log.LogInformation($"Failed to call note remove function for user {userId} and note {noteId}")
            return new HttpResponseMessage(HttpStatusCode.NotFound)
        | Ok _ ->
            log.LogInformation($"Successfully called note remove function for user {userId} and note {noteId}")
            return new HttpResponseMessage(HttpStatusCode.NoContent)
    }
