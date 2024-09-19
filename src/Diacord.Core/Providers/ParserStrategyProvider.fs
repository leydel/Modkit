namespace Modkit.Diacord.Core.Providers

open Modkit.Diacord.Core.Interfaces
open Modkit.Diacord.Core.Strategies

type IParserStrategyProvider =
    abstract member GetStrategy:
        strategy: string ->
        IParserStrategy

type ParserStrategyProvider () =
    interface IParserStrategyProvider with
        member _.GetStrategy strategy =
            match strategy with
            | "json" -> JsonParserStrategy()
            | _ -> failwith "Unsupported parser strategy"
