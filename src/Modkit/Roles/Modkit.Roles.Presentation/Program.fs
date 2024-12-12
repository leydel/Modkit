open Azure.Core.Serialization
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Modkit.Roles.Application.Options
open System.IO
open System.Text.Json

HostBuilder()
    .ConfigureFunctionsWorkerDefaults(fun _ builder ->
        // Setup json serializer
        !builder.Services.Configure(fun (workerOptions: WorkerOptions) -> workerOptions.Serializer <- JsonObjectSerializer(Json.options))
    )
    .ConfigureAppConfiguration(fun builder ->
        // Add environment variables to configuration
        !builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", true)
            .AddEnvironmentVariables()
    )
    .ConfigureServices(fun ctx services ->
        // Register services
        !services.AddHttpClient()
        !services.AddLogging()
        !services.AddApplicationInsightsTelemetryWorkerService()
        !services.ConfigureFunctionsApplicationInsights()

        // Setup configuration options
        !services.AddOptions<CryptoOptions>().Configure<IConfiguration>(fun s c -> c.GetSection(CryptoOptions.Key).Bind(s))
    )
    .Build()
    .RunAsync()
|> Async.AwaitTask
|> Async.RunSynchronously
