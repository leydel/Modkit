namespace Modkit.Diacord.Core.Types

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type DiffNodeTests () =
    [<TestMethod>]
    member _.leaf_CorrectlyDeterminesUnchangedOnNone () =
        // Arrange
        let a = None
        let b = None

        // Act
        let node = DiffNode.leaf a b []

        // Assert
        match node with
        | DiffNode.Leaf(_, t) when t = DiffNodeType.Unchanged -> ()
        | _ -> failwith "Unexpected DiffNodeType found"

    [<TestMethod>]
    member _.leaf_CorrectlyDeterminesUnchangedOnSomeEmpty () =
        // Arrange
        let a = Some ()
        let b = Some ()

        // Act
        let node = DiffNode.leaf a b []

        // Assert
        match node with
        | DiffNode.Leaf(_, t) when t = DiffNodeType.Unchanged -> ()
        | _ -> failwith "Unexpected DiffNodeType found"

    [<TestMethod>]
    member _.leaf_CorrectlyDeterminesUnchangedOnSomeUnchanged () =
        // Arrange
        let a = Some ()
        let b = Some ()
        let diffs = [Diff.Unchanged("", 0)]

        // Act
        let node = DiffNode.leaf a b diffs

        // Assert
        match node with
        | DiffNode.Leaf(_, t) when t = DiffNodeType.Unchanged -> ()
        | _ -> failwith "Unexpected DiffNodeType found"
        
    [<TestMethod>]
    member _.leaf_CorrectlyDeterminesAdded () =
        // Arrange
        let a = Some ()
        let b = None

        // Act
        let node = DiffNode.leaf a b []

        // Assert
        match node with
        | DiffNode.Leaf(_, t) when t = DiffNodeType.Added -> ()
        | _ -> failwith "Unexpected DiffNodeType found"
        
    [<TestMethod>]
    member _.leaf_CorrectlyDeterminesRemoved () =
        // Arrange
        let a = None
        let b = Some ()

        // Act
        let node = DiffNode.leaf a b []

        // Assert
        match node with
        | DiffNode.Leaf(_, t) when t = DiffNodeType.Removed -> ()
        | _ -> failwith "Unexpected DiffNodeType found"
        
    [<TestMethod>]
    member _.leaf_CorrectlyDeterminesModified () =
        // Arrange
        let a = Some ()
        let b = Some ()
        let diffs = [Diff.Added("", 0)]

        // Act
        let node = DiffNode.leaf a b diffs

        // Assert
        match node with
        | DiffNode.Leaf(_, t) when t = DiffNodeType.Modified -> ()
        | _ -> failwith "Unexpected DiffNodeType found"
