namespace Modkit.Discordfs.Services

open FSharp.Json
open Microsoft.VisualStudio.TestTools.UnitTesting
open Modkit.Discordfs.Types
open RichardSzalay.MockHttp
open System.Net.Http
open System.Threading.Tasks

type Nonce = {
    [<JsonField("nonce")>]
    Nonce: int
}

[<TestClass>]
type DiscordHttpServiceTests () =
    [<DefaultValue>] val mutable _httpClientFactory: IHttpClientFactory

    [<TestInitialize>]
    member this.TestInitialize () =
        this._httpClientFactory <- {
            new IHttpClientFactory with
                member _.CreateClient (_: string) =
                    let mockHttp = new MockHttpMessageHandler()

                    mockHttp
                        .When(DiscordHttpService.DISCORD_API_URL + "endpoint")
                        .Respond(new StringContent """{"nonce":1}""")
                        |> ignore

                    mockHttp.ToHttpClient()
        }
        
    [<TestMethod>]
    member this.send_SerializesResponse (): Task = task {
        // Arrange
        let discordHttpService = DiscordHttpService(this._httpClientFactory, "DISCORD_BOT_TOKEN")

        // Act
        let! res = discordHttpService.send<Nonce> HttpMethod.Post "endpoint" None

        // Assert
        Assert.AreEqual(1, res.Nonce)
    }
    
    [<TestMethod>]
    member this.content_BuildsStringContent (): Task = task {
        // Arrange
        let payload = InteractionCallback.build InteractionCallbackType.PONG
        let expected = """{"type":1,"data":null}"""

        let discordHttpService = DiscordHttpService(this._httpClientFactory, "DISCORD_BOT_TOKEN")

        // Act
        let content = discordHttpService.content payload
        
        // Assert
        Assert.IsTrue(content.IsSome)

        let! actual = content.Value.ReadAsStringAsync()
        Assert.AreEqual(expected, actual)
    }
