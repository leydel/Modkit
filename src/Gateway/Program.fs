open Modkit.Discordfs.Services
open Modkit.Gateway.Factories
open Modkit.Gateway.Services
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open System
open System.IO

let host = 
    Host
        .CreateDefaultBuilder(Environment.GetCommandLineArgs())
        .ConfigureAppConfiguration(fun builder ->
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
            |> ignore
        )
        .ConfigureServices(fun services ->
            services
                .AddHttpClient()
                .AddTransient<IHttpService, HttpService>()
                .AddSingleton<IGatewayService, GatewayService>()
                .AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>()
                .AddTransient<Client>()
            |> ignore
        )
        .Build()

let client = host.Services.GetRequiredService<Client>()

client.Start()
