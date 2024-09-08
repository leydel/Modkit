namespace Modkit.Api.Functions

open Azure.Messaging.ServiceBus
open FSharp.Json
open Microsoft.Azure.Functions.Worker
open Modkit.Discordfs.Types
open Microsoft.Extensions.Logging

type GatewayServiceBusFunction () =
    [<Function(nameof GatewayServiceBusFunction)>]
    member _.Run (
        [<ServiceBusTrigger("%GatewayQueueName%", IsSessionsEnabled = true )>] message: ServiceBusReceivedMessage,
        log: ILogger
    ) = task {
        let event = message.Body.ToString() |> Json.deserialize<GatewayEvent>

        log.LogDebug <| Json.serialize event
        log.LogInformation $"Gateway event (id: {message.MessageId}) received"
    }
