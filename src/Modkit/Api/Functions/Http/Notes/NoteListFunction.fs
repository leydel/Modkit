namespace Modkit.Api.Functions

open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Api.Actions
open Modkit.Api.DTOs
open System.Net
open System.Net.Http
open System.Text.Json

type NoteListFunction (noteListAction: INoteListAction) =
    [<Function(nameof NoteListFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId}/notes")>] req: HttpRequestMessage,
        log: ILogger,
        userId: string
    ) = task {
        let! notes = noteListAction.run userId
        let payload = List.map NoteDto.from notes

        let res = new HttpResponseMessage(HttpStatusCode.OK)
        res.Content <- new StringContent(JsonSerializer.Serialize payload)
        res.Headers.Add("Content-Type", "application/json")

        log.LogInformation($"Successfully called note list function for user {userId}")
        return res
    }
