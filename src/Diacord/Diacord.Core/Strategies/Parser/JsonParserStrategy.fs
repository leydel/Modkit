namespace Modkit.Diacord.Core.Strategies

open Modkit.Diacord.Core.Interfaces
open Modkit.Diacord.Core.Structures
open System.Text.Json

type JsonParserStrategy () =
    interface IParserStrategy with
        member _.Parse raw =
            try
                Ok <| Json.deserializeF<DiacordTemplate> raw
            with | _ ->
                Error "Failed to parse JSON"
