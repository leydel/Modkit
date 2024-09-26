open Microsoft.Azure.Cosmos
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Modkit.Api.Actions
open Modkit.Api.Repositories
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
    .ConfigureServices(fun ctx services -> 
        services
            .AddApplicationInsightsTelemetryWorkerService()
            .AddSingleton<CosmosClient>(fun _ -> new CosmosClient(ctx.Configuration.GetValue "CosmosDbConnectionString"))
            // Repositories
            .AddTransient<INoteRepository, NoteRepository>()
            // Actions
            .AddTransient<INoteListAction, NoteListAction>()
            .AddTransient<INoteGetAction, NoteGetAction>()
            .AddTransient<INoteAddAction, NoteAddAction>()
            .AddTransient<INoteRemoveAction, NoteRemoveAction>()
        |> ignore
    )
    .Build()
    .RunAsync()
|> Async.AwaitTask
|> Async.RunSynchronously
