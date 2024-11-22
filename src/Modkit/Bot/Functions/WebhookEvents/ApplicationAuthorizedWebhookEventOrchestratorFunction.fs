namespace Modkit.Bot.Functions

open Discordfs.Webhook.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.DurableTask
open Microsoft.Extensions.Logging

type ApplicationAuthorizedWebhookEventOrchestratorFunction () =
    let Run (
        [<OrchestrationTrigger>] ctx: TaskOrchestrationContext,
        event: ApplicationAuthorizedEvent
    ) = task {
        let logger = ctx.CreateReplaySafeLogger<ApplicationAuthorizedWebhookEventOrchestratorFunction>()
        logger.LogInformation("Handling application authorized webhook event orchestrator for user {UserId}", event.User.Id)

        // TODO: Handle event here
    }
