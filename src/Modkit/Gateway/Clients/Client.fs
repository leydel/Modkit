namespace Modkit.Gateway.Clients

open Azure.Messaging.ServiceBus
open Discordfs.Gateway.Clients
open Discordfs.Gateway.Payloads
open Discordfs.Rest.Resources
open Discordfs.Types
open Modkit.Gateway.Factories
open Microsoft.Extensions.Configuration
open System.Net.Http
open System.Text.Json
open System.Threading.Tasks

type Client (
    configuration: IConfiguration,
    httpClientFactory: IHttpClientFactory,
    serviceBusClientFactory: IServiceBusClientFactory
) =
    let httpClient = httpClientFactory.CreateClient()
    
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

        let identify = IdentifySendEvent.build(
            Token = discordBotToken,
            Intents = intents,
            Properties = ConnectionProperties.build(),
            Shard = (0, 1), // TODO: Causing INVALID_SHARD error, probably because this tuple isnt serializing correctly
            Presence = UpdatePresenceSendEvent.build(
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

        let handler =
            match useGateway with
            | false ->
                fun _ -> Task.FromResult ()
            | true ->
                let client = serviceBusClientFactory.CreateClient serviceBusConnectionString
                let sender = client.CreateSender serviceBusQueueName

                fun (event: GatewayReceiveEvent) -> task {
                    let json = Json.serializeF event
                    do! sender.SendMessageAsync <| ServiceBusMessage json
                }

        try
            let gatewayClient: IGatewayClient = GatewayClient()

            let! getGatewayResponse = httpClient |> Gateway.getGateway "10" GatewayEncoding.JSON None

            match getGatewayResponse with
            | GetGatewayResponse.Ok res -> do! gatewayClient.Connect res.Url identify handler :> Task
            | _ -> failwith "Failed to get gateway URL"
        with | exn ->
            System.Console.WriteLine(exn)
            System.Console.ReadKey() |> ignore

        do! _.ReceiveMessagesAsync()
    }

    member _.ReceiveMessagesAsync () = task {
        let client = serviceBusClientFactory.CreateClient serviceBusConnectionString
        let processor = client.CreateProcessor(serviceBusQueueName, ServiceBusProcessorOptions())

        processor.ProcessMessageAsync.Add(fun args -> task {
            let message = args.Message
            let body = message.Body.ToString()
            // Implement the logic to process the received messages here
            printfn "Received message: %s" body
            do! args.CompleteMessageAsync(message)
        })

        processor.ProcessErrorAsync.Add(fun args -> task {
            printfn "Error processing message: %s" args.Exception.Message
        })

        do! processor.StartProcessingAsync()
    }
