namespace Modkit.Bot.Functions

open Azure.Messaging.ServiceBus
open Discordfs.Gateway.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.WebJobs
open Microsoft.Extensions.Logging
open System.Text.Json

type GatewayServiceBusFunction () =
    [<Function(nameof GatewayServiceBusFunction)>]
    member _.Run (
        [<ServiceBusTrigger("%GatewayQueueName%", IsSessionsEnabled = true)>] message: ServiceBusReceivedMessage,
        ctx: FunctionContext
    ) = task {
        let json = message.Body.ToString()
        ctx.GetLogger().LogDebug json

        match Json.deserializeF<GatewayReceiveEvent> json with
        | GatewayReceiveEvent.MESSAGE_CREATE ({ Data = data }) ->
            match data.Message.Content with
            | Some content -> ctx.GetLogger().LogDebug $"{data.Message.Author.Username} said: {content}"
            | None -> ctx.GetLogger().LogDebug "Message content intent is not enabled"
        | _ -> ()
    }
