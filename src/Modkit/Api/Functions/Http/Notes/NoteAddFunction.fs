namespace Modkit.Api.Functions

open Discordfs.Types.Utils
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Api.Actions
open Modkit.Api.DTOs
open System.Net
open System.Net.Http
open System.Text.Json.Serialization

type NoteAddFunctionPayload = {
    [<JsonPropertyName "message">] Message: string
}

type NoteAddFunction (noteAddAction: INoteAddAction) =
    [<Function(nameof NoteAddFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/{userId}/notes")>] req: HttpRequestMessage,
        log: ILogger,
        userId: string
    ) = task {
        let! json = req.Content.ReadAsStringAsync()
        let body = FsJson.deserialize<NoteAddFunctionPayload> json

        let! note = noteAddAction.run userId body.Message

        match note with
        | Error _ ->
            log.LogInformation($"Failed to call note add function for user {userId}")
            return new HttpResponseMessage(HttpStatusCode.Conflict)
        | Ok note ->
            let payload = NoteDto.from note

            let res = new HttpResponseMessage(HttpStatusCode.OK)
            res.Content <- new StringContent(FsJson.serialize payload)
            res.Headers.Add("Content-Type", "application/json")
            
            log.LogInformation($"Successfully called note add function for user {userId}")
            return res
    }
