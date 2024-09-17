namespace Modkit.Api.Functions

open Azure.Messaging.ServiceBus
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging

type GatewayServiceBusFunction () =
    [<Function(nameof GatewayServiceBusFunction)>]
    member _.Run (
        [<ServiceBusTrigger("%GatewayQueueName%", IsSessionsEnabled = true )>] message: ServiceBusReceivedMessage,
        log: ILogger
    ) = task {
        log.LogDebug <| message.Body.ToString()
        log.LogInformation $"Gateway event (id: {message.MessageId}) received"
    }
