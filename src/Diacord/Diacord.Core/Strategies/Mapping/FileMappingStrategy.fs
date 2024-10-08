namespace Modkit.Diacord.Core.Strategies

open Microsoft.Extensions.Configuration
open Modkit.Diacord.Core.Interfaces
open System.Collections.Generic
open System.IO
open System.Text.Json

type FileMappingStrategy (configuration: IConfiguration) =
    let fileMappingStoragePath = configuration.GetValue<string> "FileMappingStoragePath"

    interface IMappingStrategy with
        member _.save mappings = task {
            try
                File.Create(fileMappingStoragePath) |> ignore
                do! File.WriteAllTextAsync(fileMappingStoragePath, JsonSerializer.Serialize mappings)

                return Ok mappings
            with | _ ->
                return Error ()
        }

        member _.get () = task {
            try
                let! json = File.ReadAllTextAsync(fileMappingStoragePath)
                let mappings = JsonSerializer.Deserialize<IDictionary<string, string>> json

                return Ok mappings
            
            with | _ ->
                return Error ()
        }
