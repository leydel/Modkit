namespace Modkit.Discordfs.Commands

open Modkit.Discordfs.Types
open System.Threading.Tasks

[<AbstractClass>]
type Command () =
    abstract member Data: CreateGlobalApplicationCommand

    abstract member Execute:
        interaction: Interaction ->
        Task<Result<InteractionCallback, string>>
