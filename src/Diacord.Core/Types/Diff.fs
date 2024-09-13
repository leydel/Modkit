namespace Modkit.Diacord.Core.Types

type Diff =
    | Added of name: string * current: obj
    | Removed of name: string * previous: obj
    | Modified of name: string * current: obj * previous: obj
with
    static member from name previous current =
        match previous, current with
        | None, None -> None
        | Some p, None -> Some (Diff.Removed(name, p))
        | None, Some c -> Some (Diff.Added(name, c))
        | Some p, Some c -> Some (Diff.Modified(name, c, p))
