open Modkit.Gateway.Clients
open Modkit.Gateway.Configuration
open Modkit.Gateway.Factories
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open System
open System.IO

Host
    .CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureAppConfiguration(fun builder ->
        // Add environment variables to configuration
        !builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
    )
    .ConfigureServices(fun services ->
        // Register services
        !services.AddHttpClient()
        !services.AddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>()
        !services.AddTransient<Client>()

        // Setup configuration options
        !services.AddOptions<DiscordOptions>().Configure<IConfiguration>(fun s c -> c.GetSection(DiscordOptions.Key).Bind(s))
        !services.AddOptions<ServiceBusOptions>().Configure<IConfiguration>(fun s c -> c.GetSection(ServiceBusOptions.Key).Bind(s))
    )
    .Build()
    .Services.GetRequiredService<Client>()
    .StartAsync()
|> Async.AwaitTask
|> Async.RunSynchronously
