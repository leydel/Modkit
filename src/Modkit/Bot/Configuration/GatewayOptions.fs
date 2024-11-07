namespace Modkit.Bot.Configuration

type GatewayOptions () =
    static member Key = "Gateway"

    member val QueueName: string
