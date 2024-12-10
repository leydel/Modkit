namespace Modkit.Bot.Functions

open Discordfs.Rest
open Discordfs.Rest.Modules
open Discordfs.Types
open Microsoft.Azure.Cosmos
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Roles.Common
open Modkit.Roles.Types
open System.Net.Http
open System.Threading.Tasks

type AddConditionCommand (
    logger: ILogger<AddConditionCommand>,
    httpClientFactory: IHttpClientFactory
) =
    [<Function(nameof AddConditionCommand)>]
    member _.Run (
        [<QueueTrigger(nameof AddConditionCommand)>] interaction: Interaction,
        [<CosmosDBInput(containerName = ROLE_APP_CONTAINER_NAME, databaseName = DATABASE_NAME, Id = "{applicationId}", PartitionKey = "{applicationId}")>] app: RoleApp option,
        [<CosmosDBInput(containerName = CONDITION_CONTAINER_NAME, databaseName = DATABASE_NAME, SqlQuery = "SELECT * FROM c WHERE c.applicationId = {applicationId}")>] conditions: Condition list,
        [<CosmosDBInput(containerName = CONDITION_CONTAINER_NAME, databaseName = DATABASE_NAME)>] container: Container
    ) = task {
        match app with
        | None ->
            logger.LogError("Attempted to add condition for unconfigured app {ApplicationId}", interaction.ApplicationId)

        | Some app ->
            let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient app.Token

            let respond text = task {
                let content = CreateInteractionResponsePayload({
                    Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE;
                    Data = MessageInteractionResponse.create (content = text)
                })

                do! client |> Rest.createInteractionResponse interaction.Id interaction.Token (Some false) content :> Task
            }

            let condition: Condition = {
                Id = ""
                ApplicationId = app.Id
                Type = ApplicationRoleConnectionMetadataType.BOOLEAN_EQUAL
                Name = ""
                Description = ""
                NameLocalizations = None
                DescriptionLocalizations = None
            }

            // TODO: Get above values from interaction data
            
            let content =
                conditions
                |> List.append [condition]
                |> List.distinctBy (fun c -> c.Id)
                |> List.map Condition.toApplicationRoleConnectionMetadata
                |> UpdateApplicationRoleConnectionMetadataRecordsPayload

            // TODO: Ensure there aren't more than 5 conditions and metadata keys are valid (a-z0-9_, 1-50 chars long)

            let! res = client |> Rest.updateApplicationRoleConnectionMetadataRecords app.Id content
            match res with
            | Error _ ->
                logger.LogError("Failed to update role connection metadata for application {ApplicationId}", app.Id)
                do! respond "Failed to update role connection metadata. Please try again later."

            | Ok _ ->
                try
                    do! container.UpsertItemAsync(condition, PartitionKey app.Id) :> Task

                    logger.LogInformation("Successfully updated role connection metadata for app {ApplicationId}", app.Id)
                    do! respond $"Successfully added condition **{condition.Id}**."

                with | ex ->
                    logger.LogError(ex, "Failed to upsert role connection metadata for app {ApplicationId}", app.Id)
                    do! respond "Failed to add condition. Please try again later."
    }

    // TODO: Should conditions just be stored in an array on the RoleApp model itself? Probably.
