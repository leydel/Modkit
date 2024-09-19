namespace Modkit.Diacord.Core.Interfaces

open System.Collections.Generic
open System.Threading.Tasks

type IMappingStrategy =
    abstract member save:
        guildId: string ->
        mappings: IDictionary<string, string> ->
        Task<Result<IDictionary<string, string>, unit>>

    abstract member get:
        guildId: string ->
        Task<Result<IDictionary<string, string>, unit>>
