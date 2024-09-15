namespace Modkit.Diacord.Core.Types

type Diff =
    | Unchanged of name: string * current: obj
    | Added of name: string * current: obj
    | Removed of name: string * previous: obj
    | Modified of name: string * current: obj * previous: obj
with
    static member from name previous current =
        match previous, current with
        | None, None -> Diff.Unchanged(name, None)
        | Some p, Some c when p = c -> Diff.Unchanged(name, c)
        | Some p, None -> Diff.Removed(name, p)
        | None, Some c -> Diff.Added(name, c)
        | Some p, Some c -> Diff.Modified(name, c, p)

    static member isUnchanged (diff: Diff) =
        match diff with
        | Diff.Unchanged _ -> false
        | _ -> true
