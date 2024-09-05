open Modkit.Discordfs.Services
open Modkit.Discordfs.Types
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open System
open System.Net.Http
open System.Threading.Tasks

type Program (discordGatewayService: IDiscordGatewayService) =
    member _.Handle (event: GatewayEvent) = task {
        // TODO: Send event to service bus
        ()
    }

    member this.Run () =
        discordGatewayService.Connect this.Handle
        :> Task
        |> Async.AwaitTask
        |> Async.RunSynchronously

Host
    .CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureServices(fun ctx services ->
        services
            .AddHttpClient()
            .AddTransient<IDiscordHttpService, DiscordHttpService>(fun provider ->
                DiscordHttpService(
                    provider.GetRequiredService<IHttpClientFactory>(),
                    ctx.Configuration.GetValue "DISCORD_BOT_TOKEN"
                )
            )
            .AddSingleton<IDiscordGatewayService, DiscordGatewayService>()
            .AddTransient<Program>()
        |> ignore
    )
    .Build()
    .Services.GetRequiredService<Program>()
    .Run()
