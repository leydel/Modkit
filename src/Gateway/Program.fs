open Azure.Messaging.ServiceBus
open Modkit.Discordfs.Services
open Modkit.Discordfs.Types
open Modkit.Gateway.Factories
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open System
open System.IO
open System.Net.Http
open System.Threading.Tasks

type Program (
    configuration: IConfiguration,
    discordGatewayService: IDiscordGatewayService,
    serviceBusClientFactory: IServiceBusClientFactory
) =
    let serviceBusConnectionString = configuration.GetValue "AzureWebJobsServiceBus"
    let serviceBusQueueName = configuration.GetValue "GatewayQueueName"
    let discordBotToken = configuration.GetValue "DiscordBotToken"

    let handle (sender: ServiceBusSender) (event: string) = task {
        do! sender.SendMessageAsync <| ServiceBusMessage event
    }

    member _.Start () =
        try
            let client = serviceBusClientFactory.CreateClient serviceBusConnectionString
            let sender = client.CreateSender serviceBusQueueName

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

            let operatingSystem =
                match Environment.OSVersion.Platform with
                | PlatformID.Win32NT -> "Windows"
                | PlatformID.Unix -> "Linux"
                | _ -> "Unknown OS"

            let identify = Identify.build(
                Token = discordBotToken,
                Intents = intents,
                Properties = ConnectionProperties.build(operatingSystem)
            )

            discordGatewayService.Connect identify (handle sender)
            :> Task
            |> Async.AwaitTask
            |> Async.RunSynchronously

            Console.WriteLine("The program has finished")
            Console.ReadKey() |> ignore
        with
        | _ ->
            Console.WriteLine("The program has crashed")
            Console.ReadKey() |> ignore

Host
    .CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureAppConfiguration(fun builder ->
        builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
        |> ignore
    )
    .ConfigureServices(fun ctx services ->
        services
            .AddHttpClient()
            .AddTransient<IDiscordHttpService, DiscordHttpService>(fun provider ->
                let httpClientFactory = provider.GetRequiredService<IHttpClientFactory>()
                let token = ctx.Configuration.GetValue "DiscordBotToken"
                DiscordHttpService(httpClientFactory, token)
            )
            .AddSingleton<IDiscordGatewayService, DiscordGatewayService>()
            .AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>()
            .AddTransient<Program>()
        |> ignore
    )
    .Build()
    .Services.GetRequiredService<Program>()
    .Start()
