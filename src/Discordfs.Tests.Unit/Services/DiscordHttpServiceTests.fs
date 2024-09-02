namespace Modkit.Discordfs.Services

open FSharp.Json
open Microsoft.VisualStudio.TestTools.UnitTesting
open RichardSzalay.MockHttp
open System.Net.Http
open System.Threading.Tasks
open System.Web

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
    member this.request_BuildsRequest () =
        // Arrange
        let discordHttpService = DiscordHttpService(this._httpClientFactory, "DISCORD_BOT_TOKEN")

        let method = HttpMethod.Post
        let endpoint = "endpoint"

        // Act
        let req = discordHttpService.request method endpoint

        // Assert
        Assert.AreEqual(method, req.Method)
        Assert.AreEqual(DiscordHttpService.DISCORD_API_URL + endpoint, req.RequestUri.ToString())

    [<TestMethod>]
    member this.query_AddsQueryParameterToRequest () =
        // Arrange
        let discordHttpService = DiscordHttpService(this._httpClientFactory, "DISCORD_BOT_TOKEN")

        let req = discordHttpService.request HttpMethod.Post "endpoint"

        let key = "key"
        let value = "value"

        // Act
        let newReq = discordHttpService.query key (Some value) req

        // Assert
        let query = HttpUtility.ParseQueryString(newReq.RequestUri.Query)
        Assert.AreEqual(value, query[key])

    [<TestMethod>]
    member this.body_AddsBodyToRequest (): Task = task {
        // Arrange
        let discordHttpService = DiscordHttpService(this._httpClientFactory, "DISCORD_BOT_TOKEN")

        let req = discordHttpService.request HttpMethod.Post "endpoint"

        let payload = { Nonce = 1 }

        // Act
        let newReq = discordHttpService.body payload req

        // Assert
        let! reqBody = newReq.Content.ReadAsStringAsync()
        Assert.AreEqual(Json.serializeU payload, reqBody)
    }

    [<TestMethod>]
    member this.result_SendsRequestAndReturnsDeserializedResult (): Task = task {
        // Arrange
        let discordHttpService = DiscordHttpService(this._httpClientFactory, "DISCORD_BOT_TOKEN")

        let req = discordHttpService.request HttpMethod.Post "endpoint"

        // Act
        let! res = discordHttpService.result<Nonce> req

        // Assert
        Assert.AreEqual(1, res.Nonce)
    }

    [<TestMethod>]
    member this.unit_SendsRequestAndReturnsUnit (): Task = task {
        // Arrange
        let discordHttpService = DiscordHttpService(this._httpClientFactory, "DISCORD_BOT_TOKEN")

        let req = discordHttpService.request HttpMethod.Post "endpoint"

        // Act
        let! res = discordHttpService.unit req

        // Assert
        Assert.IsNull(res)
    }
