namespace Modkit.Discordfs.Common

open FSharp.Json
open Microsoft.VisualStudio.TestTools.UnitTesting
open RichardSzalay.MockHttp
open System.Net
open System.Net.Http
open System.Threading.Tasks
open System.Web

[<TestClass>]
type ReqTests () =
    [<DefaultValue>] val mutable _req: HttpRequestMessage

    [<DefaultValue>] val mutable _httpClientFactory: IHttpClientFactory

    [<TestInitialize>]
    member this.TestInitialize () =
        let requestUri = "https://example.com/endpoint"

        this._req <- new HttpRequestMessage(HttpMethod.Get, requestUri)

        this._httpClientFactory <- {
            new IHttpClientFactory with
                member _.CreateClient (_: string) =
                    let mock =new MockHttpMessageHandler()
                    mock.When(requestUri).Respond(HttpStatusCode.NoContent) |> ignore
                    mock.ToHttpClient()
        }
        
    [<TestMethod>]
    member _.hreate_CreatesMatchingHttpRequestMessage () =
        // Arrange
        let method = HttpMethod.Get
        let host = "https://example.com"
        let endpoint = "endpoint"

        // Act
        let req = Req.create method host endpoint

        // Assert
        Assert.AreEqual(method, req.Method)
        Assert.AreEqual(host + "/" + endpoint, req.RequestUri.ToString())

    [<TestMethod>]
    member this.header_AddsNewHeaderToHttpRequestMessage () =
        // Arrange
        let key = "key"
        let value = "value"

        // Act
        let req = Req.header key value this._req

        // Assert
        Assert.AreEqual(value, req.Headers.GetValues key |> Seq.exactlyOne)

    [<TestMethod>]
    member this.headerOpt_AddsNewHeaderToHttpRequestMessage () =
        // Arrange
        let key = "key"
        let value = Some "value"

        // Act
        let req = Req.headerOpt key value this._req

        // Assert
        Assert.AreEqual(value.Value, req.Headers.GetValues key |> Seq.exactlyOne)

    [<TestMethod>]
    member this.headerOpt_DoesNotAddHeaderIfValueIsNone () =
        // Arrange
        let key = "key"
        let value = None

        // Act
        let req = Req.headerOpt key value this._req

        // Assert
        Assert.IsFalse(req.Headers.Contains key)

    [<TestMethod>]
    member this.bot_AddsBotAuthorizationHeader () =
        // Arrange
        let token = "token"

        // Act
        let req = Req.bot token this._req

        // Assert
        Assert.AreEqual($"Bot {token}", req.Headers.GetValues "Authorization" |> Seq.exactlyOne)

    [<TestMethod>]
    member this.oauth_AddsOauthAuthorizationHeader () =
        // Arrange
        let token = "token"

        // Act
        let req = Req.oauth token this._req

        // Assert
        Assert.AreEqual($"Bearer {token}", req.Headers.GetValues "Authorization" |> Seq.exactlyOne)

    [<TestMethod>]
    member this.query_AddsNewQueryParameterToHttpRequestMessage () =
        // Arrange
        let key = "key"
        let value = "value"

        // Act
        let req = Req.query key value this._req

        // Assert
        let query = HttpUtility.ParseQueryString(req.RequestUri.Query)
        Assert.AreEqual(value, query.Get key)

    [<TestMethod>]
    member this.queryOpt_AddsNewQueryParameterToHttpRequestMessage () =
        // Arrange
        let key = "key"
        let value = Some "value"

        // Act
        let req = Req.queryOpt key value this._req

        // Assert
        let query = HttpUtility.ParseQueryString(req.RequestUri.Query)
        Assert.AreEqual(value.Value, query.GetValues key |> Seq.exactlyOne)
        
    [<TestMethod>]
    member this.queryOpt_DoesNotAddQueryParameterIfValueIsNone () =
        // Arrange
        let key = "key"
        let value = None

        // Act
        let req = Req.queryOpt key value this._req

        // Assert
        let query = HttpUtility.ParseQueryString(req.RequestUri.Query)
        Assert.IsFalse(query.HasKeys())

    [<TestMethod>]
    member this.body_AddsJsonSerializedStringContentBodyToHttpRequestMessage (): Task = task {
        // Arrange
        let payload = {| test = 1 |}

        // Act
        let req = Req.body payload this._req

        // Assert
        let! body = req.Content.ReadAsStringAsync()
        Assert.AreEqual(Json.serializeU payload, body)
    }

    [<TestMethod>]
    member this.send_SendsRequestUsingHttpClient (): Task = task {
        // Arrange

        // Act
        let! res = Req.send this._httpClientFactory this._req

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, res.StatusCode)
    }
