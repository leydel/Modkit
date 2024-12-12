namespace Modkit.Roles.Infrastructure.Repositories

open System.Threading.Tasks

open Microsoft.Azure.Cosmos

open Modkit.Roles.Application.Repositories

open Modkit.Roles.Infrastructure.Common
open Modkit.Roles.Infrastructure.Models
open Modkit.Roles.Infrastructure.Mappers

type UserRepository (cosmosClient: CosmosClient) =
    let container = cosmosClient.GetContainer(DATABASE_NAME, USER_CONTAINER_NAME)

    interface IUserRepository with
        member _.Put userId applicationId accessToken accessTokenExpiry refreshToken metadata = task {
            let model = {
                Id = userId
                ApplicationId = applicationId
                AccessToken = accessToken
                AccessTokenExpiry = accessTokenExpiry
                RefreshToken = refreshToken
                Metadata = metadata
            }

            try
                let! res = container.UpsertItemAsync<UserModel>(model, PartitionKey model.ApplicationId)
                return UserMapper.toDomain res.Resource |> Ok

            with | :? CosmosException as ex ->
                return Error ex.StatusCode
        }

        member _.Get userId applicationId = task {
            try
                let! res = container.ReadItemAsync<UserModel>(userId, PartitionKey applicationId)
                return UserMapper.toDomain res.Resource |> Ok

            with | :? CosmosException as ex ->
                return Error ex.StatusCode
        }

        member _.Update userId applicationId changes = task {
            let operations =
                changes
                |> List.collect (function
                    | UserChange.Token (a, e, r) ->
                        [
                            PatchOperation.Set("/" + UserModel.AccessToken, a)
                            PatchOperation.Set("/" + UserModel.AccessTokenExpiry, e)
                            PatchOperation.Set("/" + UserModel.RefreshToken, r)
                        ]
                    | UserChange.Metadata m -> [PatchOperation.Set("/" + UserModel.Metadata, m)]
                )

            try
                let! res = container.PatchItemAsync<UserModel>(userId, PartitionKey applicationId, operations)
                return UserMapper.toDomain res.Resource |> Ok
            
            with | :? CosmosException as ex ->
                return Error ex.StatusCode
        }

        member _.Delete userId applicationId = task {
            try
                do! container.DeleteItemAsync<UserModel>(userId, PartitionKey applicationId) :> Task
                return Ok ()
            
            with | :? CosmosException as ex ->
                return Error ex.StatusCode
        }
