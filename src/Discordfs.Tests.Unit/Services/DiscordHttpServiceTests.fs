namespace Modkit.Discordfs.Services

open Microsoft.VisualStudio.TestTools.UnitTesting
open Modkit.Discordfs.Types
open NSubstitute
open System.Net.Http

[<TestClass>]
type DiscordHttpServiceTests () =
    [<DefaultValue>] val mutable _httpClientFactory: IHttpClientFactory
    [<DefaultValue>] val mutable _token: string

    [<TestInitialize>]
    member this.TestInitialize () =
        this._httpClientFactory <- Substitute.For<IHttpClientFactory>()
        this._token <- "DISCORD_BOT_TOKEN"
        
    [<TestMethod>]
    member this.send_SerializesResponse_WithoutBody () = task {
        // Arrange
        let res = new HttpResponseMessage()
        res.Content <- new StringContent """{"nonce":1}"""

        let httpClient = Substitute.For<HttpClient>()
        httpClient.SendAsync(Arg.Any<HttpRequestMessage>()).Returns(res) |> ignore

        this._httpClientFactory.CreateClient().Returns(httpClient) |> ignore

        let discordHttpService = DiscordHttpService(this._httpClientFactory, this._token)

        // Act
        let! res = discordHttpService.send HttpMethod.Post "endpoint" None

        // Assert
        Assert.AreEqual(1, res ["nonce"])
    }
    
    [<TestMethod>]
    member this.send_SerializesResponse_WithBody () = task {
        // Arrange
        let res = new HttpResponseMessage()
        res.Content <- new StringContent """{"nonce":1}"""

        let httpClient = Substitute.For<HttpClient>()
        httpClient.SendAsync(Arg.Any<HttpRequestMessage>()).Returns(res) |> ignore

        this._httpClientFactory.CreateClient().Returns(httpClient) |> ignore

        let discordHttpService = DiscordHttpService(this._httpClientFactory, this._token)

        let body = Some (new StringContent "" :> HttpContent)

        // Act
        let! res = discordHttpService.send HttpMethod.Post "endpoint" body

        // Assert
        body.Value.Received(1) |> ignore
        Assert.AreEqual(1, res ["nonce"])
    }
    
    [<TestMethod>]
    member this.content_BuildsStringContent () = task {
        // Arrange
        let payload = InteractionCallback.build InteractionCallbackType.PONG
        let expected = """{"type":1}"""

        let discordHttpService = DiscordHttpService(this._httpClientFactory, this._token)

        // Act
        let content = discordHttpService.content payload
        
        // Assert
        Assert.IsTrue(content.IsSome)

        let! actual = content.Value.ReadAsStringAsync()
        Assert.AreEqual(expected, actual)
    }
