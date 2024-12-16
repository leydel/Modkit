namespace Modkit.Roles.Infrastructure.Repositories

open System.Threading.Tasks

open Microsoft.Azure.Cosmos

open Modkit.Roles.Application.Repositories

open Modkit.Roles.Infrastructure.Common
open Modkit.Roles.Infrastructure.Models
open Modkit.Roles.Infrastructure.Mappers

type ApplicationRepository (cosmosClient: CosmosClient) =
    let container = cosmosClient.GetContainer(DATABASE_NAME, APPLICATION_CONTAINER_NAME)

    interface IApplicationRepository with
        member _.Put application = task {
            let model = ApplicationMapper.fromDomain application

            try
                let! res = container.UpsertItemAsync<ApplicationModel>(model, PartitionKey model.Id)
                return ApplicationMapper.toDomain res.Resource |> Ok

            with | :? CosmosException as ex ->
                return Error ex.StatusCode
        }

        member _.Get applicationId = task {
            try
                let! res = container.ReadItemAsync<ApplicationModel>(applicationId, PartitionKey applicationId)
                return ApplicationMapper.toDomain res.Resource |> Ok

            with | :? CosmosException as ex ->
                return Error ex.StatusCode
        }

        member _.Update applicationId changes = task {
            let operations =
                changes
                |> List.map (function
                    | ApplicationChange.Token t -> PatchOperation.Set("/" + ApplicationModel.Token, t)
                    | ApplicationChange.ClientSecret cs -> PatchOperation.Set("/" + ApplicationModel.ClientSecret, cs)
                    | ApplicationChange.Metadata m -> PatchOperation.Set("/" + ApplicationModel.Metadata, m)
                )

            try
                let! res = container.PatchItemAsync<ApplicationModel>(applicationId, PartitionKey applicationId, operations)
                return ApplicationMapper.toDomain res.Resource |> Ok
            
            with | :? CosmosException as ex ->
                return Error ex.StatusCode
        }

        member _.Delete applicationId = task {
            try
                do! container.DeleteItemAsync<ApplicationModel>(applicationId, PartitionKey applicationId) :> Task
                return Ok ()
            
            with | :? CosmosException as ex ->
                return Error ex.StatusCode
        }
