open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open System.IO

HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(fun builder ->
        builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", true)
            .AddEnvironmentVariables()
        |> ignore
    )
    .ConfigureServices(fun services -> 
        services
            .AddApplicationInsightsTelemetryWorkerService()
        |> ignore
    )
    .Build()
    .RunAsync()
|> Async.AwaitTask
|> Async.RunSynchronously
