namespace Modkit.Gateway.Configuration

type ServiceBusOptions () =
    static member Key = "ServiceBus"

    member val ConnectionString: string

    member val GatewayEventQueueName: string

    member val Enabled: bool
    