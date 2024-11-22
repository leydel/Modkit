namespace Modkit.Bot.Functions

open Discordfs.Webhook.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.DurableTask
open Microsoft.Extensions.Logging

type EntitlementCreateWebhookEventOrchestratorFunction () =
    let Run (
        [<OrchestrationTrigger>] ctx: TaskOrchestrationContext,
        event: EntitlementCreateEvent
    ) = task {
        let logger = ctx.CreateReplaySafeLogger<EntitlementCreateWebhookEventOrchestratorFunction>()
        logger.LogInformation("Handling entitlement create webhook event orchestrator for user {UserId}", event.UserId)

        // TODO: Handle event here
    }
