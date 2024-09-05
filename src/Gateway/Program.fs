open Azure.Messaging.ServiceBus
open Azure.Identity
open FSharp.Json
open Modkit.Discordfs.Services
open Modkit.Discordfs.Types
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open System
open System.Net.Http
open System.Threading.Tasks

type Program (configuration: IConfiguration, discordGatewayService: IDiscordGatewayService) =
    let serviceBusConnectionString = configuration.GetValue "SERVICE_BUS_CONNECTION_STRING"
    let serviceBusQueueName = configuration.GetValue "SERVICE_BUS_QUEUE_NAME"

    member _.Handle (sender: ServiceBusSender) (event: GatewayEvent) = task {
        do! Json.serialize event |> ServiceBusMessage |> sender.SendMessageAsync
    }

    member this.Run () =
        let serviceBusClient = ServiceBusClient(
            serviceBusConnectionString,
            DefaultAzureCredential(),
            ServiceBusClientOptions(TransportType = ServiceBusTransportType.AmqpWebSockets)
        )

        let sender = serviceBusClient.CreateSender serviceBusQueueName

        discordGatewayService.Connect (this.Handle sender)
        :> Task
        |> Async.AwaitTask
        |> Async.RunSynchronously

Host
    .CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureServices(fun ctx services ->
        services
            .AddHttpClient()
            .AddTransient<IDiscordHttpService, DiscordHttpService>(fun provider ->
                let httpClientFactory = provider.GetRequiredService<IHttpClientFactory>()
                let token = ctx.Configuration.GetValue "DISCORD_BOT_TOKEN"
                DiscordHttpService(httpClientFactory, token)
            )
            .AddSingleton<IDiscordGatewayService, DiscordGatewayService>()
            .AddTransient<Program>()
        |> ignore
    )
    .Build()
    .Services.GetRequiredService<Program>()
    .Run()
