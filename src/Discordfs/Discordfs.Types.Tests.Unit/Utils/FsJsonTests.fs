namespace Discordfs.Types.Utils

open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Text.Json.Serialization

type ExampleRecord = {
    [<JsonName "nonce">] Nonce: int
}

[<TestClass>]
type FsJsonTests () =
    [<TestMethod>]
    member _.Serialize_CorrectlySerializesRecord () =
        // Arrange
        let value = { Nonce = 1234 }
        let expected = """{"nonce":1234}"""

        // Act
        let actual = FsJson.serialize value

        // Assert
        Assert.AreEqual<string>(expected, actual)
        
    [<TestMethod>]
    member _.Deserialize_CorrectlyDeserializesRecord () =
        // Arrange
        let nonce = 1234
        let value = $"""{{"nonce":{nonce}}}"""

        // Act
        let actual = FsJson.deserialize<ExampleRecord> value

        // Assert
        Assert.AreEqual<int>(nonce, actual.Nonce)
