namespace Modkit.Api.Repositories

open Microsoft.Azure.Cosmos
open Microsoft.Azure.Cosmos.Linq
open Modkit.Api.Models
open System
open System.Linq
open System.Threading.Tasks
open Microsoft.Extensions.Configuration

type INoteRepository =
    abstract member get:
        userId: string ->
        noteId: string ->
        Task<Result<Note, unit>>

    abstract member list:
        userId: string ->
        Task<Note list>

    abstract member create:
        userId: string ->
        message: string ->
        time: DateTime ->
        Task<Result<Note, unit>>

    abstract member delete:
        userId: string ->
        noteId: string ->
        Task<Result<unit, unit>>

type NoteRepository (configuration: IConfiguration, cosmosClient: CosmosClient) =
    let databaseName = configuration.GetValue "CosmosDbNotesDatabaseName"
    let containerName = configuration.GetValue "CosmosDbNotesContainerName"
    let container = cosmosClient.GetContainer(databaseName, containerName)

    interface INoteRepository with
        member _.get userId noteId = task {
            try
                let! res = container.ReadItemAsync<Note>(noteId, PartitionKey userId)
                return Ok res.Resource
            with
            | _ ->
                return Error ()
        }

        member _.list userId = task {
            let feed =
                container
                    .GetItemLinqQueryable<Note>()
                    .Where(fun n -> n.UserId = userId)
                    .ToFeedIterator()

            let mutable items: Note list = []

            while feed.HasMoreResults do
                let! res = feed.ReadNextAsync()
                items <- List.append items (List.ofSeq res.Resource)

            return items
        }

        member _.create userId message time = task {
            try
                let note = {
                    Id = Guid.NewGuid().ToString();
                    UserId = userId;
                    Message = message;
                    CreatedAt = time;
                }

                let! res = container.CreateItemAsync note
                return Ok res.Resource
            with
            | _ ->
                return Error ()
        }

        member _.delete userId noteId = task {
            try
                do! container.DeleteItemAsync(noteId, PartitionKey userId) :> Task
                return Ok ()
            with
            | _ ->
                return Error ()
        }
