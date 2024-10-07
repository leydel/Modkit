namespace Modkit.Api.Functions

open Discordfs.Types.Utils
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Api.Actions
open Modkit.Api.DTOs
open System.Net
open System.Net.Http

type NoteGetFunction (noteGetAction: INoteGetAction) =
    [<Function(nameof NoteGetFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId}/notes/{noteId}")>] req: HttpRequestMessage,
        log: ILogger,
        userId: string,
        noteId: string
    ) = task {
        let! note = noteGetAction.run userId noteId

        match note with
        | Error _ ->
            log.LogInformation($"Failed to call note get function for user {userId} and note {noteId}")
            return new HttpResponseMessage(HttpStatusCode.NotFound)
        | Ok note ->
            let payload = NoteDto.from note

            let res = new HttpResponseMessage(HttpStatusCode.OK)
            res.Content <- new StringContent(FsJson.serialize payload)
            res.Headers.Add("Content-Type", "application/json")

            log.LogInformation($"Successfully called note get function for user {userId} and note {noteId}")
            return res
    }
