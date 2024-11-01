namespace Modkit.Api.Functions.Http

open Microsoft.Azure.Cosmos
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Api.Common
open Modkit.Api.Modules
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Text.Json
open System.Text.Json.Serialization
open System.Threading.Tasks

type PutDiacordMappingPayload = {
    [<JsonPropertyName "mapping">] Mapping: IDictionary<string, string>
}

type PutDiacordMappingResponse = {
    [<CosmosDBOutput(containerName = Constants.diacordMappingsContainerName, databaseName = Constants.diacordMappingsDatabaseName)>] Mapping: DiacordMapping
    [<HttpResult>] Response: HttpResponseMessage
}

type DiacordMappingController () =
    [<Function "GetDiacordMapping">]
    member _.GetDiacordMapping (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "guilds/{guildId}/mapping")>] req: HttpRequestMessage,
        [<CosmosDBInput(containerName = Constants.diacordMappingsContainerName, databaseName = Constants.diacordMappingsDatabaseName, Id = "{guildId}", PartitionKey = "{guildId}")>] mapping: DiacordMapping option,
        logger: ILogger,
        guildId: string
    ) =
        match mapping with
        | None ->
            logger.LogInformation $"Could not find diacord mapping for guild {guildId}"
            Response.create HttpStatusCode.NotFound
        | Some note ->
            logger.LogInformation $"Found diacord mapping for guild {guildId}"
            Response.create HttpStatusCode.OK |> Response.withJson (note |> DiacordMapping.toDto |> Json.serializeF)

    [<Function "PutDiacordMapping">]
    member _.PutDiacordMapping (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "guilds/{guildId}/mapping")>] payload: PutDiacordMappingPayload,
        [<CosmosDBInput(containerName = Constants.diacordMappingsContainerName, databaseName = Constants.diacordMappingsDatabaseName, Id = "{guildId}", PartitionKey = "{guildId}")>] existing: DiacordMapping option,
        logger: ILogger,
        guildId: string
    ) =
        let mapping: DiacordMapping = {
            GuildId = guildId;
            Mapping = payload.Mapping;
        }

        match existing with
        | None ->
            logger.LogInformation $"Creating new diacord mapping for guild {guildId}"

            {
                Mapping = mapping;
                Response =
                    Response.create HttpStatusCode.OK
                    |> Response.withJson (mapping |> DiacordMapping.toDto |> Json.serializeF)
            }
        | Some _ ->
            logger.LogInformation $"Updating existing diacord mapping for guild {guildId}"

            {
                Mapping = mapping;
                Response =
                    Response.create HttpStatusCode.Created
                    |> Response.withJson (mapping |> DiacordMapping.toDto |> Json.serializeF)
                    |> Response.withHeader "Location" $"/guilds/{mapping.GuildId}/mapping";
            }

    [<Function "RemoveDiacordMapping">]
    member _.RemoveDiacordMapping (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "guilds/{guildId}/members/{userId}/notes/{noteId}")>] req: HttpRequestMessage,
        [<CosmosDBInput(containerName = Constants.diacordMappingsContainerName, databaseName = Constants.diacordMappingsDatabaseName)>] container: Container,
        logger: ILogger,
        guildId: string
    ) = task {
        try
            do! container.DeleteItemAsync(guildId, PartitionKey guildId) :> Task
            logger.LogInformation $"Deleted diacord mapping for guild {guildId}"
            return Response.create HttpStatusCode.NoContent
        with | _ ->
            logger.LogInformation $"Could not delete diacord mapping for guild {guildId}"
            return Response.create HttpStatusCode.NotFound
    }
