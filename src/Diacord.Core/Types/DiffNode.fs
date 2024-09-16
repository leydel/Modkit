namespace Modkit.Diacord.Core.Types

type DiffNodeType =
    | Unchanged
    | Added
    | Removed
    | Modified

type DiffNode =
    | Root of children: DiffNode list
    | Branch of name: string * children: DiffNode list
    | Leaf of changes: Diff list * ``type``: DiffNodeType
with
    static member leaf (a: 'a option) (b: 'b option) diffs =
        match a, b with
        | None, None -> DiffNode.Leaf(diffs, DiffNodeType.Unchanged)
        | Some _, None -> DiffNode.Leaf(diffs, DiffNodeType.Added)
        | None, Some _ -> DiffNode.Leaf(diffs, DiffNodeType.Removed)
        | Some _, Some _ when diffs.Length = 0 -> DiffNode.Leaf(diffs, DiffNodeType.Unchanged)
        | Some _, Some _ -> DiffNode.Leaf(diffs, DiffNodeType.Modified)
