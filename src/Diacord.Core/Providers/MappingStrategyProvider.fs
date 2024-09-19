namespace Modkit.Diacord.Core.Providers

open Modkit.Diacord.Core.Clients
open Modkit.Diacord.Core.Interfaces
open Modkit.Diacord.Core.Strategies

type IMappingStrategyProvider =
    abstract member GetStrategy:
        strategy: string ->
        IMappingStrategy

type MappingStrategyProvider (apiClient: IApiClient) =
    interface IMappingStrategyProvider with
        member _.GetStrategy strategy =
            match strategy with
            | "api" -> ApiMappingStrategy(apiClient)
            // TODO: Add local file strategy
            | _ -> failwith "Unsupported mapping strategy"
