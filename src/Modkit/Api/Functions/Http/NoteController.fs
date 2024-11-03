namespace Modkit.Api.Functions.Http

open Microsoft.Azure.Cosmos
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Api.Common
open Modkit.Api.Modules
open System
open System.Net
open System.Net.Http
open System.Text.Json
open System.Text.Json.Serialization
open System.Threading.Tasks

type CreateNotePayload = {
    [<JsonPropertyName "message_id">] MessageId: string option
    [<JsonPropertyName "content">] Content: string
}

type CreateNoteResponse = {
    [<CosmosDBOutput(containerName = Constants.noteContainerName, databaseName = Constants.noteDatabaseName)>] Note: Note
    [<HttpResult>] Response: HttpResponseMessage
}

type NoteController () =
    [<Function "ListMemberNotes">]
    member _.ListMemberNotes (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "guilds/{guildId}/members/{memberId}/notes")>] req: HttpRequestMessage,
        [<CosmosDBInput(containerName = Constants.noteContainerName, databaseName = Constants.noteDatabaseName, SqlQuery = "SELECT * FROM c WHERE c.guildId = {guildId} AND c.memberId = {memberId}")>] notes: Note list,
        logger: ILogger,
        guildId: string,
        memberId: string
    ) =
        logger.LogInformation $"Found {notes.Length} notes for member {memberId} in guild {guildId}"
        Response.create HttpStatusCode.OK |> Response.withJson (notes |> List.map Note.toDto |> Json.serializeF)

    [<Function "GetMemberNote">]
    member _.GetMemberNote (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "guilds/{guildId}/members/{memberId}/notes/{noteId}")>] req: HttpRequestMessage,
        [<CosmosDBInput(containerName = Constants.noteContainerName, databaseName = Constants.noteDatabaseName, Id = "{noteId}", PartitionKey = "{guildId}")>] note: Note option,
        logger: ILogger,
        guildId: string,
        memberId: string,
        noteId: string
    ) =
        match note with
        | None ->
            logger.LogInformation $"Could not find note {noteId} for member {memberId} in guild {guildId}"
            Response.create HttpStatusCode.NotFound
        | Some note ->
            logger.LogInformation $"Found note {noteId} for member {memberId} in guild {guildId}"
            Response.create HttpStatusCode.OK |> Response.withJson (note |> Note.toDto |> Json.serializeF)

    [<Function "AddMemberNote">]
    member _.AddMemberNote (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "guilds/{guildId}/members/{memberId}/notes")>] payload: CreateNotePayload,
        logger: ILogger,
        guildId: string,
        memberId: string
    ) =
        let note: Note = {
            Id = Guid.NewGuid().ToString();
            GuildId = guildId;
            MemberId = memberId;
            MessageId = payload.MessageId;
            Content = payload.Content;
            CreatedAt = DateTime.Now;
        }

        logger.LogInformation $"Created note {note.Id} for member {memberId} in guild {guildId}"

        {
            Note = note;
            Response =
                Response.create HttpStatusCode.Created
                |> Response.withJson (note |> Note.toDto |> Json.serializeF)
                |> Response.withHeader "Location" $"/guilds/{note.GuildId}/members/{note.MemberId}/notes/{note.Id}";
        }

    [<Function "RemoveMemberNote">]
    member _.RemoveMemberNote (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "guilds/{guildId}/members/{memberId}/notes/{noteId}")>] req: HttpRequestMessage,
        [<CosmosDBInput(containerName = Constants.noteContainerName, databaseName = Constants.noteDatabaseName)>] container: Container,
        logger: ILogger,
        guildId: string,
        memberId: string,
        noteId: string
    ) = task {
        try
            do! container.DeleteItemAsync(noteId, PartitionKey guildId) :> Task
            logger.LogInformation $"Deleted note for member {memberId} in guild {guildId}"
            return Response.create HttpStatusCode.NoContent
        with | _ ->
            logger.LogInformation $"Could not delete note for member {memberId} in guild {guildId}"
            return Response.create HttpStatusCode.NotFound
    }
