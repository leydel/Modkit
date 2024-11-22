module Modkit.Bot.Bindings.VerifyEd25519Builder

open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.DependencyInjection.Extensions
open Microsoft.Extensions.Options

let configure (builder: IFunctionsWorkerApplicationBuilder) =
    !builder.Services.AddOptions<VerifyEd25519Options>()
    !builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<VerifyEd25519Options>, VerifyEd25519OptionsSetup>())
