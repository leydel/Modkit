namespace Modkit.Bot.Functions

open Azure.Messaging.ServiceBus
open Discordfs.Gateway.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.DurableTask.Client
open Microsoft.Extensions.Logging
open System.Text.Json
open System.Threading.Tasks

type GatewayServiceBusFunction (logger: ILogger<GatewayServiceBusFunction>) =
    [<Function(nameof GatewayServiceBusFunction)>]
    member _.Run (
        [<ServiceBusTrigger("%Gateway:QueueName%")>] message: ServiceBusReceivedMessage,
        [<DurableClient>] orchestrationClient: DurableTaskClient,
        ctx: FunctionContext
    ) = task {
        let json = message.Body.ToString()
        logger.LogDebug("Received raw json for gateway event: {Json}", json)

        let run (name: string) (event: 'a) =
            logger.LogInformation("Processing gateway event with {OrchestratorName}", name)
            orchestrationClient.ScheduleNewOrchestrationInstanceAsync(name, input = event) :> Task

        match Json.deserializeF<GatewayReceiveEvent> json with
        | GatewayReceiveEvent.MESSAGE_CREATE ({ Data = event }) -> do! run (nameof MessageCreateOrchestratorFunction) event
        | _ -> logger.LogInformation "Ignoring gateway event because no orchestrator matched"
    }
