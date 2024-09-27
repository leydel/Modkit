namespace Discordfs.Rest.Common

open Discordfs.Types.Utils
open System
open System.Net.Http
open System.Web

[<AutoOpen>]
module Http =
    let send (httpClientFactory: IHttpClientFactory) (req: HttpRequestMessage) =
        httpClientFactory.CreateClient().SendAsync req

    let toJson<'a> (res: HttpResponseMessage) =
        res.Content.ReadAsStringAsync()
        |> Task.map (fun body -> FsJson.deserialize<'a> body) 

    let toRaw (res: HttpResponseMessage) =
        res.Content.ReadAsStringAsync()

    type RequestBuilder() =
        member val HttpRequestMessage = new HttpRequestMessage()

        member this.Yield(_) =
            this

        [<CustomOperation("get")>]
        member this.Get (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Get

            this.HttpRequestMessage

        [<CustomOperation("post")>]
        member this.Post (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Post

            this.HttpRequestMessage

        [<CustomOperation("put")>]
        member this.Put (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Put

        [<CustomOperation("patch")>]
        member this.Patch (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Patch

            this.HttpRequestMessage

        [<CustomOperation("delete")>]
        member this.Delete (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Delete

            this.HttpRequestMessage

        [<CustomOperation("header")>]
        member this.Header (_, key: string, value: string) =
            this.HttpRequestMessage.Headers.Add(key, value)

            this.HttpRequestMessage

        [<CustomOperation("header")>]
        member this.Header(_, key: string, value: string option) =
            match value with
            | Some value -> this.HttpRequestMessage.Headers.Add(key, value)
            | None -> ()

            this.HttpRequestMessage

        [<CustomOperation("bot")>]
        member this.Bot (_, token: string) =
            this.HttpRequestMessage.Headers.Add("Authorization", $"Bot {token}")

            this.HttpRequestMessage

        [<CustomOperation("oauth")>]
        member this.Oauth (_, token: string) =
            this.HttpRequestMessage.Headers.Add("Authorization", $"Bearer {token}")

            this.HttpRequestMessage

        [<CustomOperation("audit")>]
        member this.Audit(_, reason: string option) =
            match reason with
            | Some reason -> this.HttpRequestMessage.Headers.Add("X-Audit-Log-Reason", reason)
            | None -> ()

            this.HttpRequestMessage

        [<CustomOperation("query")>]
        member this.Query(_, key: string, value: string) =
            let uriBuilder = UriBuilder(this.HttpRequestMessage.RequestUri)
            let query = HttpUtility.ParseQueryString(uriBuilder.Query)
            query.Add(key, value)
            uriBuilder.Query <- query.ToString()
            this.HttpRequestMessage.RequestUri <- uriBuilder.Uri

            this.HttpRequestMessage

        [<CustomOperation("query")>]
        member this.Query(_, key: string, value: string option) =
            match value with
            | Some value ->
                let uriBuilder = UriBuilder(this.HttpRequestMessage.RequestUri)
                let query = HttpUtility.ParseQueryString(uriBuilder.Query)
                query.Add(key, value)
                uriBuilder.Query <- query.ToString()
                this.HttpRequestMessage.RequestUri <- uriBuilder.Uri
            | None -> ()

            this.HttpRequestMessage

        [<CustomOperation("payload")>]
        member this.Payload(_, payload: Payload) =
            let content = payload.Content.ToContent()

            this.HttpRequestMessage.Headers.Add("Content-Type", content.Headers.ContentType.MediaType)
            this.HttpRequestMessage.Content <- content
            
            this.HttpRequestMessage

    let req = RequestBuilder()
