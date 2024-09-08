namespace Modkit.Gateway.Factories

open Azure.Messaging.ServiceBus
open Azure.Identity

type IServiceBusClientFactory =
    abstract member CreateClient:
        connectionString: string ->
        ServiceBusClient

type ServiceBusClientFactory () =
    interface IServiceBusClientFactory with
        member _.CreateClient (connectionString: string) =
            ServiceBusClient(
                connectionString,
                DefaultAzureCredential(),
                ServiceBusClientOptions(TransportType = ServiceBusTransportType.AmqpWebSockets)
            )
