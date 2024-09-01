namespace Modkit.Discordfs.Commands

open Modkit.Discordfs.Types
open System.Threading.Tasks

[<AbstractClass>]
type Command<'a> () =
    abstract member Data: CreateGlobalApplicationCommand

    abstract member Validate: interaction: Interaction -> Result<'a, string>

    abstract member Execute: validationResult: 'a -> Task<Result<InteractionCallback, string>>

    member this.Run (interaction: Interaction): Task<Result<InteractionCallback, string>> =
        match this.Validate interaction with
        | Error err -> Task.FromResult <| Error err
        | Ok validationResult -> this.Execute validationResult
        