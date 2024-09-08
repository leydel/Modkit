open Azure.Messaging.ServiceBus
open FSharp.Json
open Modkit.Discordfs.Services
open Modkit.Discordfs.Types
open Modkit.Gateway.Factories
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open System
open System.Net.Http
open System.Threading.Tasks

type Program (
    configuration: IConfiguration,
    discordGatewayService: IDiscordGatewayService,
    serviceBusClientFactory: IServiceBusClientFactory
) =
    let serviceBusConnectionString = configuration.GetValue "SERVICE_BUS_CONNECTION_STRING"
    let serviceBusQueueName = configuration.GetValue "SERVICE_BUS_QUEUE_NAME"

    let handle (sender: ServiceBusSender) (event: GatewayEvent) = task {
        do! Json.serialize event |> ServiceBusMessage |> sender.SendMessageAsync
    }

    member _.Start () =
        let client = serviceBusClientFactory.CreateClient serviceBusConnectionString
        let sender = client.CreateSender serviceBusQueueName

        discordGatewayService.Connect <| handle sender
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
            .AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>()
            .AddTransient<Program>()
        |> ignore
    )
    .Build()
    .Services.GetRequiredService<Program>()
    .Start()
