namespace Modkit.Diacord.Core.Strategies

open Microsoft.Extensions.Configuration
open Modkit.Diacord.Core.Clients
open Modkit.Diacord.Core.Interfaces

type ApiMappingStrategy (configuration: IConfiguration, apiClient: IApiClient) =
    let apiMappingGuildId = configuration.GetValue<string> "ApiMappingGuildId"

    interface IMappingStrategy with
        member _.save mappings = task {
            let! res = apiClient.PutDiacordMapping apiMappingGuildId mappings

            match res with
            | Error _ -> return Error ()
            | Ok mapping -> return Ok mapping.Mappings
        }

        member _.get () = task {
            let! res = apiClient.GetDiacordMapping apiMappingGuildId

            match res with
            | Error _ -> return Error ()
            | Ok mapping -> return Ok mapping.Mappings
        }
