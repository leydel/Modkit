namespace Modkit.Diacord.Core.Strategies

open Modkit.Diacord.Core.Interfaces
open Modkit.Diacord.Core.Structures
open Modkit.Discordfs.Utils

type JsonParserStrategy () =
    interface IParserStrategy with
        member _.Parse raw =
            try
                Ok <| FsJson.deserialize<DiacordTemplate> raw
            with | _ ->
                Error "Failed to parse JSON"
