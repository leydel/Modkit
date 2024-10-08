open Modkit.Gateway.Clients
open Modkit.Gateway.Factories
open Discordfs.Rest.Clients
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
                .AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>()
                .AddTransient<IRestClient, RestClient>()
                .AddTransient<Client>()
            |> ignore
        )
        .Build()

let client = host.Services.GetRequiredService<Client>()

client.StartAsync()
|> Async.AwaitTask
|> Async.RunSynchronously
