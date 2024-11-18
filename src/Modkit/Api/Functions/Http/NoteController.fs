namespace Modkit.Api.Functions.Http

open Microsoft.Azure.Cosmos
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Modkit.Api.Common
open Modkit.Api.Modules
open System
open System.Net
open System.Net.Http
open System.Text.Json
open System.Text.Json.Serialization
open System.Threading.Tasks

type AddMemberNotePayload = {
    [<JsonPropertyName "message_id">] MessageId: string option
    [<JsonPropertyName "content">] Content: string
}

type NoteController (logger: ILogger<NoteController>) =
    [<Function "ListMemberNotes">]
    member _.ListMemberNotes (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "guilds/{guildId}/members/{memberId}/notes")>] req: HttpRequestData,
        [<CosmosDBInput(containerName = Constants.noteContainerName, databaseName = Constants.noteDatabaseName, SqlQuery = "SELECT * FROM c WHERE c.guildId = {guildId} AND c.memberId = {memberId}")>] notes: Note list,
        ctx: FunctionContext,
        guildId: string,
        memberId: string
    ) =
        logger.LogInformation("Found {NoteCount} notes for member {MemberId} in guild {GuildId}", notes.Length, memberId, guildId)
        req.CreateResponse HttpStatusCode.OK |> Response.withJson (notes |> List.map Note.toDto)

    [<Function "GetMemberNote">]
    member _.GetMemberNote (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "guilds/{guildId}/members/{memberId}/notes/{noteId}")>] req: HttpRequestData,
        [<CosmosDBInput(containerName = Constants.noteContainerName, databaseName = Constants.noteDatabaseName, Id = "{noteId}", PartitionKey = "{guildId}")>] note: Note option,
        ctx: FunctionContext,
        guildId: string,
        memberId: string,
        noteId: string
    ) =
        match note with
        | None ->
            logger.LogInformation("Could not find note {NoteId} for member {MemberId} in guild {GuildId}", noteId, memberId, guildId)
            req.CreateResponse HttpStatusCode.NotFound
        | Some note ->
            logger.LogInformation("Found note {NoteId} for member {MemberId} in guild {GuildId}", noteId, memberId, guildId)
            req.CreateResponse HttpStatusCode.OK |> Response.withJson (note |> Note.toDto)

    [<Function "AddMemberNote">]
    member _.AddMemberNote (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "guilds/{guildId}/members/{memberId}/notes")>] req: HttpRequestData,
        [<FromBody>] payload: AddMemberNotePayload,
        [<CosmosDBInput(containerName = Constants.noteContainerName, databaseName = Constants.noteDatabaseName)>] container: Container,
        ctx: FunctionContext,
        guildId: string,
        memberId: string
    ) = task {
        let note: Note = {
            Id = Guid.NewGuid().ToString();
            GuildId = guildId;
            MemberId = memberId;
            MessageId = payload.MessageId;
            Content = payload.Content;
            CreatedAt = DateTime.Now;
        }

        try
            do! container.CreateItemAsync(note) :> Task

            logger.LogInformation("Created note {NoteId} for member {MemberId} in guild {GuildId}", note.Id, memberId, guildId)
            return req.CreateResponse HttpStatusCode.Created
            |> Response.withJson (note |> Note.toDto |> Json.serializeF)
            |> Response.withHeader "Location" $"/guilds/{note.GuildId}/members/{note.MemberId}/notes/{note.Id}";
        with | _ ->
            logger.LogError("Failed to create a note for member {MemberId} in guild {GuildId}", memberId, guildId)
            return req.CreateResponse HttpStatusCode.InternalServerError
    }

    [<Function "RemoveMemberNote">]
    member _.RemoveMemberNote (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "guilds/{guildId}/members/{memberId}/notes/{noteId}")>] req: HttpRequestData,
        [<CosmosDBInput(containerName = Constants.noteContainerName, databaseName = Constants.noteDatabaseName)>] container: Container,
        ctx: FunctionContext,
        guildId: string,
        memberId: string,
        noteId: string
    ) = task {
        try
            do! container.DeleteItemAsync(noteId, PartitionKey guildId) :> Task

            logger.LogInformation("Deleted note {NoteId} for member {MemberId} in guild {GuildId}", noteId, memberId, guildId)
            return req.CreateResponse HttpStatusCode.NoContent
        with | _ ->
            logger.LogInformation("Could not delete note {NoteId} for member {MemberId} in guild {GuildId}", noteId, memberId, guildId)
            return req.CreateResponse HttpStatusCode.NotFound
    }
