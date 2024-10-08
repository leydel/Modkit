namespace Discordfs.Types

open Microsoft.VisualStudio.TestTools.UnitTesting
open System
open System.Text.Json
open System.Text.Json.Serialization

type UnixEpochRecord = {
    [<JsonConverter(typeof<Converters.UnixEpoch>)>] Timestamp: DateTime
}

type NullUndefinedAsBoolRecord = {
    [<JsonConverter(typeof<Converters.NullUndefinedAsBool>)>] State: bool
}

[<TestClass>]
type ConvertersTests () =
    [<TestMethod>]
    member _.UnixEpoch_CorrectlySerializesTimestamp () =
        // Arrange
        let time = DateTime(2024, 9, 27, 0, 0, 0, DateTimeKind.Utc)
        let expected = 1727395200000L
        let object = { Timestamp = time; }

        // Act
        let json = JsonSerializer.Serialize object

        // Assert
        Assert.IsTrue(json.Contains(expected.ToString()))
        
    [<TestMethod>]
    member _.UnixEpoch_CorrectlyDeserializesDateTime () =
        // Arrange
        let expected = DateTime(2024, 9, 27, 0, 0, 0, DateTimeKind.Utc)
        let timestamp = 1727395200000L
        let json = $"""{{"Timestamp":{timestamp}}}"""

        // Act
        let actual = JsonSerializer.Deserialize<UnixEpochRecord> json

        // Assert
        Assert.AreEqual<DateTime>(expected, actual.Timestamp)

    [<TestMethod>]
    member _.NullUndefinedAsBool_ThrowsErrorIfSerializesUndefined () =
        // Arrange
        let original = { State = false }

        // Act
        let res () = JsonSerializer.Serialize original |> ignore

        // Assert
        Assert.ThrowsException<NotImplementedException> res |> ignore

    [<TestMethod>]
    member _.NullUndefinedAsBool_ThrowsErrorIfSSerializesNull () =
        // Arrange
        let original = { State = true }

        // Act
        let res () = JsonSerializer.Serialize original |> ignore

        // Assert
        Assert.ThrowsException<NotImplementedException> res |> ignore

    [<TestMethod>]
    member _.NullUndefinedAsBool_CorrectlyDeserializesFalse () =
        // Arrange
        let original = "{}"

        // Act
        let actual = JsonSerializer.Deserialize<NullUndefinedAsBoolRecord> original

        // Assert
        Assert.IsFalse(actual.State)

    [<TestMethod>]
    member _.NullUndefinedAsBool_CorrectlyDeserializesTrue () =
        // Arrange
        let original = """{"State":null}"""

        // Act
        let actual = JsonSerializer.Deserialize<NullUndefinedAsBoolRecord> original

        // Assert
        Assert.IsTrue(actual.State)
