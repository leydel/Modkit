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

        match message.Content with
        | Some content -> logger.LogDebug $"{message.Author.Username} said: {content}"
        | None -> logger.LogDebug "Message content intent is not enabled"

        // Actual message create event handling logic can be orchestrated here
    }
