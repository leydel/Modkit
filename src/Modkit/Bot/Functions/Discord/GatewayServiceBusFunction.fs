namespace Modkit.Bot.Functions

open Azure.Messaging.ServiceBus
open Discordfs.Gateway.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.DurableTask.Client
open Microsoft.Extensions.Logging
open System.Text.Json
open System.Threading.Tasks

type GatewayServiceBusFunction () =
    [<Function(nameof GatewayServiceBusFunction)>]
    member _.Run (
        [<ServiceBusTrigger("%Gateway:QueueName%")>] message: ServiceBusReceivedMessage,
        [<DurableClient>] orchestrationClient: DurableTaskClient,
        ctx: FunctionContext
    ) = task {
        let json = message.Body.ToString()
        ctx.GetLogger().LogDebug json

        let run (name: string) (event: 'a) =
            orchestrationClient.ScheduleNewOrchestrationInstanceAsync(name, input = event) :> Task

        match Json.deserializeF<GatewayReceiveEvent> json with
        | GatewayReceiveEvent.MESSAGE_CREATE ({ Data = event }) -> do! run (nameof MessageCreateOrchestratorFunction) event
        | _ -> ()
    }
