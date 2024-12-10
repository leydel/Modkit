namespace Modkit.Bot.Functions

open Discordfs.Rest
open Discordfs.Rest.Modules
open Discordfs.Types
open Microsoft.Azure.Cosmos
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Roles.Common
open Modkit.Roles.Types
open System.Text.RegularExpressions
open System.Net.Http
open System.Threading.Tasks

type AddConditionCommandResult =
    | Success of key: string
    | InvalidMetadataKey
    | TooManyConditions
    | UpdateFailed

type AddConditionCommand (
    logger: ILogger<AddConditionCommand>,
    httpClientFactory: IHttpClientFactory
) =
    [<Function(nameof AddConditionCommand)>]
    member _.Run (
        [<QueueTrigger(nameof AddConditionCommand)>] interaction: Interaction,
        [<CosmosDBInput(containerName = ROLE_APP_CONTAINER_NAME, databaseName = DATABASE_NAME)>] container: Container
    ) = task {
        let! app = container.ReadItemAsync<RoleApp>(interaction.ApplicationId, PartitionKey interaction.ApplicationId) ?> _.Resource
        let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient app.Token            

        let! res = task {
            let condition = { // TODO: Get values from interaction data
                Type = ApplicationRoleConnectionMetadataType.BOOLEAN_EQUAL
                Key = ""
                Name = ""
                Description = ""
                NameLocalizations = None
                DescriptionLocalizations = None
            }

            let metadata =
                app.Metadata
                |> List.append [condition]
                |> List.distinctBy (fun c -> c.Key)

            if metadata |> List.length > 5 then
                return TooManyConditions

            else if metadata |> List.exists (fun c -> Regex.IsMatch(c.Key, "[a-z0-9_]{1,50}")) then
                return InvalidMetadataKey

            else
                let! res = client |> Rest.updateApplicationRoleConnectionMetadataRecords app.Id (UpdateApplicationRoleConnectionMetadataRecordsPayload metadata)
                match res with
                | Error _ ->
                    logger.LogError("Failed to update role connection metadata for application {ApplicationId}", app.Id)
                    return UpdateFailed

                | Ok _ ->
                    try
                        do! container.UpsertItemAsync(condition, PartitionKey app.Id) :> Task

                        logger.LogInformation("Successfully updated key {MetadataKey} for role connection metadata for app {ApplicationId}", condition.Key, app.Id)
                        return Success condition.Key

                    with | ex ->
                        logger.LogError(ex, "Failed to upsert role connection metadata for app {ApplicationId}", app.Id)
                        return UpdateFailed // TODO: Rollback Discord update?
        }

        let text =
            match res with
            | Success key -> $"Successfully added condition **{key}**."
            | InvalidMetadataKey -> "Invalid key. Keys must only contain letters, numbers, underscores, and be between 1 and 50 characters long."
            | TooManyConditions -> "Too many conditions. You can only have up to 5 conditions."
            | UpdateFailed -> "Unexpectedly failed. Please try again later."

        let content = CreateInteractionResponsePayload({
            Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE;
            Data = MessageInteractionResponse.create (content = text)
        })

        do! client |> Rest.createInteractionResponse interaction.Id interaction.Token (Some false) content :> Task
    }
