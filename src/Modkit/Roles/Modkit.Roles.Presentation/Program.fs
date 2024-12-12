open System.Reflection
open System.IO
open System.Text.Json

open Azure.Core.Serialization
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

open Modkit.Roles.Application.Options

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
        !services.AddMediatR(fun services -> !services.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
        // TODO: Check if application project same assembly for above (or maybe just resolve manually?)
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
