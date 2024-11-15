namespace Modkit.Api.Functions.Http

open Microsoft.Azure.Cosmos
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Modkit.Api.Common
open Modkit.Api.Modules
open System.Collections.Generic
open System.Net
open System.Text.Json
open System.Text.Json.Serialization
open System.Threading.Tasks

type PutDiacordMappingPayload = {
    [<JsonPropertyName "mapping">] Mapping: IDictionary<string, string>
}

type DiacordMappingController () =
    [<Function "GetDiacordMapping">]
    member _.GetDiacordMapping (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "guilds/{guildId}/mapping")>] req: HttpRequestData,
        [<CosmosDBInput(containerName = Constants.diacordMappingsContainerName, databaseName = Constants.diacordMappingsDatabaseName, Id = "{guildId}", PartitionKey = "{guildId}")>] mapping: DiacordMapping option,
        ctx: FunctionContext,
        guildId: string
    ) =
        let logger = ctx.GetLogger<DiacordMappingController>()

        match mapping with
        | None ->
            logger.LogInformation $"Could not find diacord mapping for guild {guildId}"
            req.CreateResponse HttpStatusCode.NotFound
        | Some note ->
            logger.LogInformation $"Found diacord mapping for guild {guildId}"
            req.CreateResponse HttpStatusCode.OK |> Response.withJson (note |> DiacordMapping.toDto)

    [<Function "PutDiacordMapping">]
    member _.PutDiacordMapping (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "guilds/{guildId}/mapping")>] req: HttpRequestData,
        [<FromBody>] payload: PutDiacordMappingPayload,
        [<CosmosDBInput(containerName = Constants.diacordMappingsContainerName, databaseName = Constants.diacordMappingsDatabaseName)>] container: Container,
        ctx: FunctionContext,
        guildId: string
    ) = task {
        let logger = ctx.GetLogger<DiacordMappingController>()

        let mapping: DiacordMapping = {
            GuildId = guildId;
            Mapping = payload.Mapping;
        }

        try
            let! res = container.UpsertItemAsync(mapping)

            match res.StatusCode with
            | HttpStatusCode.OK ->
                logger.LogInformation $"Updated existing diacord mapping for guild {guildId}"
                return req.CreateResponse HttpStatusCode.OK |> Response.withJson (mapping |> DiacordMapping.toDto)
            | HttpStatusCode.Created ->
                logger.LogInformation $"Created new diacord mapping for guild {guildId}"
                return req.CreateResponse HttpStatusCode.Created
                |> Response.withJson (mapping |> DiacordMapping.toDto |> Json.serializeF)
                |> Response.withHeader "Location" $"/guilds/{mapping.GuildId}/mapping"
            | _ ->
                logger.LogError $"Unexpected status code when upserting diacord mapping for guild {guildId}"
                return req.CreateResponse HttpStatusCode.InternalServerError
        with | _ ->
            logger.LogError $"Failed to upsert diacord mapping for guild {guildId}"
            return req.CreateResponse HttpStatusCode.InternalServerError
    }

    [<Function "RemoveDiacordMapping">]
    member _.RemoveDiacordMapping (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "guilds/{guildId}/members/{userId}/notes/{noteId}")>] req: HttpRequestData,
        [<CosmosDBInput(containerName = Constants.diacordMappingsContainerName, databaseName = Constants.diacordMappingsDatabaseName)>] container: Container,
        ctx: FunctionContext,
        guildId: string
    ) = task {
        let logger = ctx.GetLogger<DiacordMappingController>()

        try
            do! container.DeleteItemAsync(guildId, PartitionKey guildId) :> Task

            logger.LogInformation $"Deleted diacord mapping for guild {guildId}"
            return req.CreateResponse HttpStatusCode.NoContent
        with | _ ->
            logger.LogInformation $"Could not delete diacord mapping for guild {guildId}"
            return req.CreateResponse HttpStatusCode.NotFound
    }
