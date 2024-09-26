namespace Modkit.Diacord.Core.Providers

open Microsoft.Extensions.Configuration
open Modkit.Diacord.Core.Clients
open Modkit.Diacord.Core.Interfaces
open Modkit.Diacord.Core.Strategies

type IMappingStrategyProvider =
    abstract member GetStrategy:
        strategy: string ->
        IMappingStrategy

type MappingStrategyProvider (configuration: IConfiguration, apiClient: IApiClient) =
    interface IMappingStrategyProvider with
        member _.GetStrategy strategy =
            match strategy with
            | "api" -> ApiMappingStrategy(configuration, apiClient)
            | "file" -> FileMappingStrategy(configuration)
            | _ -> failwith "Unsupported mapping strategy"
