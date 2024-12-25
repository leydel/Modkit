open System.IO
open System.Text.Json

open Azure.Core.Serialization
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

open Modkit.Roles.Application.Options
open Modkit.Roles.Application.Repositories
open Modkit.Roles.Application.Commands
open Modkit.Roles.Infrastructure.Repositories

open Modkit.Roles.Presentation.Middleware

HostBuilder()
    .ConfigureFunctionsWorkerDefaults(fun _ builder ->
        // Setup json serializer
        !builder.Services.Configure(fun (workerOptions: WorkerOptions) -> workerOptions.Serializer <- JsonObjectSerializer(Json.options))
        !builder.UseWhen<Ed25519Middleware>(fun ctx -> ctx.FunctionDefinition.Name = "PostInteraction") // TODO: Use reflection with VerifyEd25519 attribute
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
        !services.AddMediatR(fun services -> !services.RegisterServicesFromAssemblyContaining<CreateApplicationCommand>()) // TODO: Test if correctly gets all command/query from Application assembly
        !services.AddHttpClient()
        !services.AddLogging()
        !services.AddApplicationInsightsTelemetryWorkerService()
        !services.ConfigureFunctionsApplicationInsights()
        // TODO: Register CosmosClient

        // Setup configuration options
        !services.AddOptions<CryptoOptions>().Configure<IConfiguration>(fun s c -> c.GetSection(CryptoOptions.Key).Bind(s))

        // Setup repositories
        !services.AddTransient<IApplicationRepository, ApplicationRepository>()
        !services.AddTransient<IUserRepository, UserRepository>()

    )
    .Build()
    .RunAsync()
|> Async.AwaitTask
|> Async.RunSynchronously
