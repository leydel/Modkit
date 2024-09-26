namespace Modkit.Diacord.Core.Types

open Microsoft.VisualStudio.TestTools.UnitTesting

type DeepObject = {
    Key: int option
}

[<TestClass>]
type DiffTests () =
    [<TestMethod>]
    member _.from_CorrectlyDeterminesNoChangesOnNone () =
        // Arrange
        let previous = None
        let current = None

        // Act
        let diff = Diff.from "value" previous current

        // Assert
        match diff with
        | Diff.Unchanged _ -> ()
        | _ -> failwith "Incorrect diff result"

    [<TestMethod>]
    member _.from_CorrectlyDeterminesNoChangesOnSome () =
        // Arrange
        let previous = Some 0
        let current = Some 0

        // Act
        let diff = Diff.from "value" previous current

        // Assert
        match diff with
        | Diff.Unchanged _ -> ()
        | _ -> failwith "Incorrect diff result"

    [<TestMethod>]
    member _.from_CorrectlyDeterminesAdded () =
        // Arrange
        let previous = None
        let current = Some 0

        // Act
        let diff = Diff.from "value" previous current

        // Assert
        match diff with
        | Diff.Added _ -> ()
        | _ -> failwith "Incorrect diff result"

    [<TestMethod>]
    member _.from_CorrectlyDeterminesModified () =
        // Arrange
        let previous = Some 0
        let current = Some 1

        // Act
        let diff = Diff.from "value" previous current

        // Assert
        match diff with
        | Diff.Modified _ -> ()
        | _ -> failwith "Incorrect diff result"

    [<TestMethod>]
    member _.from_CorrectlyDeterminesRemoved () =
        // Arrange
        let previous = Some 0
        let current = None

        // Act
        let diff = Diff.from "value" previous current

        // Assert
        match diff with
        | Diff.Removed _ -> ()
        | _ -> failwith "Incorrect diff result"

    [<TestMethod>]
    member _.from_CorrectlyDeterminesDeepUnchanged () =
        // Arrange
        let previous = Some { Key = Some 1 }
        let current = Some { Key = Some 1 }

        // Act
        let diff = Diff.from "value" previous current

        // Assert
        match diff with
        | Diff.Unchanged _ -> ()
        | _ -> failwith "Incorrect diff result"

    [<TestMethod>]
    member _.from_CorrectlyDeterminesDeepModified () =
        // Arrange
        let previous = Some { Key = Some 1 }
        let current = Some { Key = Some 2 }

        // Act
        let diff = Diff.from "value" previous current

        // Assert
        match diff with
        | Diff.Modified _ -> ()
        | _ -> failwith "Incorrect diff result"
