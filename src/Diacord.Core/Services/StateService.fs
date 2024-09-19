namespace Modkit.Diacord.Core.Services

open Modkit.Diacord.Core.Types

// TODO: Create `save` method to apply changes to Discord

type IStateService =
    abstract member save:
        diff: DiffNode ->
        unit

type StateService () =
    interface IStateService with
        member this.save diff =
            ()
