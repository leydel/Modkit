namespace Modkit.Gateway.Clients

open Azure.Messaging.ServiceBus
open Discordfs.Gateway.Clients
open Discordfs.Gateway.Types
open Discordfs.Rest.Clients
open Discordfs.Types
open Modkit.Gateway.Factories
open Microsoft.Extensions.Configuration
open System.Threading.Tasks

type Client (
    configuration: IConfiguration,
    serviceBusClientFactory: IServiceBusClientFactory,
    restClient: IRestClient
) =
    let serviceBusConnectionString = configuration.GetValue "AzureWebJobsServiceBus"
    let serviceBusQueueName = configuration.GetValue<string> "GatewayQueueName"
    let discordBotToken = configuration.GetValue<string> "DiscordBotToken"
    let useGateway = configuration.GetValue<bool> "UseGateway"

    member _.StartAsync () = task {
        let intents = 
            int <| (
                    GatewayIntent.GUILDS
                ||| GatewayIntent.GUILD_MEMBERS
                ||| GatewayIntent.GUILD_MODERATION
                ||| GatewayIntent.GUILD_EMOJIS_AND_STICKERS
                ||| GatewayIntent.GUILD_INTEGRATIONS
                ||| GatewayIntent.GUILD_WEBHOOKS
                ||| GatewayIntent.GUILD_INVITES
                ||| GatewayIntent.GUILD_VOICE_STATES
                ||| GatewayIntent.GUILD_PRESENCES
                ||| GatewayIntent.GUILD_MESSAGES
                ||| GatewayIntent.GUILD_MESSAGE_REACTIONS
                ||| GatewayIntent.GUILD_MESSAGE_TYPING
                ||| GatewayIntent.DIRECT_MESSAGES
                ||| GatewayIntent.DIRECT_MESSAGE_REACTIONS
                ||| GatewayIntent.DIRECT_MESSAGE_TYPING
                ||| GatewayIntent.MESSAGE_CONTENT
                ||| GatewayIntent.GUILD_SCHEDULED_EVENTS
                ||| GatewayIntent.AUTO_MODERATION_CONFIGURATION
                ||| GatewayIntent.AUTO_MODERATION_EXECUTION
                ||| GatewayIntent.GUILD_MESSAGE_POLLS
                ||| GatewayIntent.DIRECT_MESSAGE_POLLS
            )

        let identify = Identify.build(
            Token = discordBotToken,
            Intents = intents,
            Properties = ConnectionProperties.build(),
            Shard = (0, 1),
            Presence = UpdatePresence.build(
                Status = StatusType.ONLINE,
                Activities = [
                    Activity.build(
                        Name = "Modkit",
                        Type = ActivityType.CUSTOM,
                        State = "🛠️ modkit.org | 1 server" // TODO: Get actual server count
                    )
                ] // TODO: Move to READY event received by API and send presence update back to gateway
            )
        )

        let gatewayClient: IGatewayClient = GatewayClient()
        let! gateway = restClient.Gateway.GetGateway "10" GatewayEncoding.JSON None

        let handler =
            match useGateway with
            | false ->
                fun _ -> Task.FromResult ()
            | true ->
                let client = serviceBusClientFactory.CreateClient serviceBusConnectionString
                let sender = client.CreateSender serviceBusQueueName

                fun (event: string) -> task {
                    do! sender.SendMessageAsync <| ServiceBusMessage event
                }

        do! gatewayClient.Connect gateway.Url identify handler :> Task
    }
