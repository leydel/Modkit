namespace Modkit.Diacord.Core.Interfaces

open System.Collections.Generic
open System.Threading.Tasks

type IMappingStrategy =
    abstract member save:
        mappings: IDictionary<string, string> ->
        Task<Result<IDictionary<string, string>, unit>>

    abstract member get:
        unit ->
        Task<Result<IDictionary<string, string>, unit>>
