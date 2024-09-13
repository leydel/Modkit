namespace Modkit.Diacord.Core.Interfaces

open Modkit.Diacord.Core.Structures

type IParserStrategy =
    abstract member Parse:
        raw: string ->
        Result<DiacordTemplate, string>
