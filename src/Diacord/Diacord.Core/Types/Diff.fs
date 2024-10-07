namespace Modkit.Diacord.Core.Types

type Diff =
    | Unchanged of name: string * current: obj
    | Added of name: string * current: obj
    | Removed of name: string * previous: obj
    | Modified of name: string * current: obj * previous: obj
    | Nested of name: string * node: DiffNode
with
    static member from name previous current =
        match previous, current with
        | None, None -> Diff.Unchanged(name, None)
        | Some p, Some c when p = c -> Diff.Unchanged(name, c)
        | Some p, None -> Diff.Removed(name, p)
        | None, Some c -> Diff.Added(name, c)
        | Some p, Some c -> Diff.Modified(name, c, p)

    static member nest name node =
        Diff.Nested(name, node)

and DiffNodeType =
    | Unchanged
    | Added
    | Removed
    | Modified

and DiffNode =
    | Root of children: DiffNode list
    | Branch of name: string * children: DiffNode list
    | Leaf of changes: Diff list * ``type``: DiffNodeType
with
    static member leaf (a: 'a option) (b: 'b option) diffs =
        let changed (diff: Diff) =
            match diff with
            | Diff.Unchanged _ -> false
            | _ -> true

        match a, b with
        | None, None -> DiffNode.Leaf(diffs, DiffNodeType.Unchanged)
        | Some _, None -> DiffNode.Leaf(diffs, DiffNodeType.Added)
        | None, Some _ -> DiffNode.Leaf(diffs, DiffNodeType.Removed)
        | Some _, Some _ when List.exists changed diffs -> DiffNode.Leaf(diffs, DiffNodeType.Modified)
        | Some _, Some _ -> DiffNode.Leaf(diffs, DiffNodeType.Unchanged)

// TODO: Rewrite diffs to just make a single tree rather than DiffNode and Diff
