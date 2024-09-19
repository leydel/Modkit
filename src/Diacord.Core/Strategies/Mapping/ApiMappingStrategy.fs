namespace Modkit.Diacord.Core.Strategies

open Modkit.Diacord.Core.Clients
open Modkit.Diacord.Core.Interfaces

type ApiMappingStrategy (apiClient: IApiClient) =
    interface IMappingStrategy with
        member _.save guildId mappings = task {
            let! res = apiClient.PutDiacordMapping guildId mappings

            match res with
            | Error _ -> return Error ()
            | Ok mapping -> return Ok mapping.Mappings
        }

        member _.get guildId = task {
            let! res = apiClient.GetDiacordMapping guildId

            match res with
            | Error _ -> return Error ()
            | Ok mapping -> return Ok mapping.Mappings
        }
