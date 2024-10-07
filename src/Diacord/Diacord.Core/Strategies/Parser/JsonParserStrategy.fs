namespace Modkit.Diacord.Core.Strategies

open Discordfs.Types.Utils
open Modkit.Diacord.Core.Interfaces
open Modkit.Diacord.Core.Structures

type JsonParserStrategy () =
    interface IParserStrategy with
        member _.Parse raw =
            try
                Ok <| FsJson.deserialize<DiacordTemplate> raw
            with | _ ->
                Error "Failed to parse JSON"
