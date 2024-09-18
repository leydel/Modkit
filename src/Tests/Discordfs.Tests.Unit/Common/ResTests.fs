namespace Modkit.Discordfs.Common

open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Net
open System.Net.Http
open System.Threading.Tasks

type Nonce = {
    [<JsonField("nonce")>]
    Nonce: int
}

[<TestClass>]
type ResTests () =
    // TODO: Add `json` tests
    
    [<TestMethod>]
    member _.ignore_ReturnsUnit (): Task = task {
        // Arrange
        let body = { Nonce = 1 }
        let httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        httpResponseMessage.Content <- new StringContent(Json.serializeU body)

        let task = Task.FromResult httpResponseMessage

        // Act
        let! res = Res.ignore task

        // Assert
        Assert.IsNull res
    }
