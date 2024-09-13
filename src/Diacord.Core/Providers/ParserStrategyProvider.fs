namespace Modkit.Diacord.Core.Providers

open Modkit.Diacord.Core.Interfaces
open Modkit.Diacord.Core.Strategies

type IParserStrategyProvider =
    abstract member GetStrategy:
        strategy: string ->
        Result<IParserStrategy, string>

type ParserStrategyProvider () =
    interface IParserStrategyProvider with
        member _.GetStrategy strategy =
            match strategy with
            | "json" -> Ok (JsonParserStrategy())
            | _ -> Error "Unsupported parser strategy"
