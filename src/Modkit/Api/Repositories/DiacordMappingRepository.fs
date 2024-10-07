namespace Modkit.Api.Repositories

open Microsoft.Azure.Cosmos
open Modkit.Api.Models
open Microsoft.Extensions.Configuration
open System.Collections.Generic
open System.Threading.Tasks

type IDiacordMappingRepository =
    abstract member get:
        guildId: string ->
        Task<Result<DiacordMapping, unit>>

    abstract member put:
        guildId: string ->
        mappings: IDictionary<string, string> ->
        Task<Result<DiacordMapping, unit>>

    abstract member delete:
        guildId: string ->
        Task<Result<unit, unit>>

type DiacordMappingRepository (configuration: IConfiguration, cosmosClient: CosmosClient) =
    let databaseName = configuration.GetValue "CosmosDbDiacordMappingDatabaseName"
    let containerName = configuration.GetValue "CosmosDbDiacordMappingContainerName"
    let container = cosmosClient.GetContainer(databaseName, containerName)

    interface IDiacordMappingRepository with
        member _.get guildId = task {
            try
                let! res = container.ReadItemAsync<DiacordMapping>(guildId, PartitionKey guildId)
                return Ok res.Resource
            with | _ ->
                return Error ()
        }

        member _.put guildId mappings = task {
            try
                let mapping = {
                    GuildId = guildId;
                    Mappings = mappings;
                }

                let! res = container.UpsertItemAsync<DiacordMapping>(mapping)
                return Ok res.Resource
            with | _ ->
                return Error ()
        }

        member _.delete guildId = task {
            try
                do! container.DeleteItemAsync(guildId, PartitionKey guildId) :> Task
                return Ok ()
            with | _ ->
                return Error ()
        }
