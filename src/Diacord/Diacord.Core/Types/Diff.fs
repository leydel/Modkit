namespace Modkit.Diacord.Core.Types

type Diff =
    | Unchanged of name: string * current: obj
    | Added of name: string * current: obj
    | Removed of name: string * previous: obj
    | Modified of name: string * current: obj * previous: obj
    | Object of name: string * diffs: Diff list
    | Root of diffs: Diff list
with
    static member value name (a: 'a option) (b: 'a option) =
        match a, b with
        | None, None -> Diff.Unchanged(name, None)
        | Some _, None -> Diff.Added(name, a)
        | None, Some _ -> Diff.Removed(name, b)
        | Some _, Some _ when a = b -> Diff.Unchanged(name, b)
        | Some _, Some _ -> Diff.Modified(name, a, b)

    static member object name diffs =
        Diff.Object(name, diffs)

    static member root diffs =
        Diff.Root(diffs)
