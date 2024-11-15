namespace Modkit.Bot.Functions

open Discordfs.Gateway.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.DurableTask
open Microsoft.Extensions.Logging

type MessageCreateOrchestratorFunction () =
    let Run (
        [<OrchestrationTrigger>] ctx: TaskOrchestrationContext,
        { Message = message }: MessageCreateReceiveEvent
    ) = task {
        let logger = ctx.CreateReplaySafeLogger<MessageCreateOrchestratorFunction>()
        logger.LogInformation("Handling message create orchestrator for message {MessageId}", message.Id)

        match message.Content with
        | Some content -> logger.LogDebug("{AuthorUsername} said: {MessageContent}", message.Author.Username, content)
        | None -> logger.LogDebug "Message content intent is not enabled"

        // Actual message create event handling logic can be orchestrated here
    }
