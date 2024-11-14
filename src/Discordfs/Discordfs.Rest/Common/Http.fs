namespace Discordfs.Rest.Common

open System
open System.Net.Http
open System.Text.Json
open System.Threading.Tasks
open System.Web

[<AutoOpen>]
module Http =
    //let send (httpClientFactory: IHttpClientFactory) (req: HttpRequestMessage) =
    //    httpClientFactory.CreateClient().SendAsync req
    let send (httpClientFactory) (req: HttpRequestMessage) =
        Task.FromResult <| new HttpResponseMessage()

    // TODO: Remove all remaining uses of Http.send then delete this (only keeping to prevent build failures)

    let toJson<'a> (res: HttpResponseMessage) =
        res.Content.ReadAsStringAsync()
        |> Task.map (fun body -> Json.deserializeF<'a> body) 

    let toRaw (res: HttpResponseMessage) =
        res.Content.ReadAsStringAsync()

    type RequestBuilder() =
        member val HttpRequestMessage = new HttpRequestMessage()

        member this.Yield(_) =
            this

        [<CustomOperation>]
        member this.get (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Get

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.post (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Post

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.put (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Put

        [<CustomOperation>]
        member this.patch (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Patch

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.delete (_, endpoint: string) =
            this.HttpRequestMessage.RequestUri <- new Uri(Constants.DISCORD_API_URL + "/" + endpoint)
            this.HttpRequestMessage.Method <- HttpMethod.Delete

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.header (_, key: string, value: string) =
            this.HttpRequestMessage.Headers.Add(key, value)

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.header(_, key: string, value: string option) =
            match value with
            | Some value -> this.HttpRequestMessage.Headers.Add(key, value)
            | None -> ()

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.bot (_, token: string) =
            this.HttpRequestMessage.Headers.Add("Authorization", $"Bot {token}")

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.oauth (_, token: string) =
            this.HttpRequestMessage.Headers.Add("Authorization", $"Bearer {token}")

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.audit(_, reason: string option) =
            match reason with
            | Some reason -> this.HttpRequestMessage.Headers.Add("X-Audit-Log-Reason", reason)
            | None -> ()

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.query(_, key: string, value: string) =
            let uriBuilder = UriBuilder(this.HttpRequestMessage.RequestUri)
            let query = HttpUtility.ParseQueryString(uriBuilder.Query)
            query.Add(key, value)
            uriBuilder.Query <- query.ToString()
            this.HttpRequestMessage.RequestUri <- uriBuilder.Uri

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.query(_, key: string, value: string option) =
            match value with
            | Some value ->
                let uriBuilder = UriBuilder(this.HttpRequestMessage.RequestUri)
                let query = HttpUtility.ParseQueryString(uriBuilder.Query)
                query.Add(key, value)
                uriBuilder.Query <- query.ToString()
                this.HttpRequestMessage.RequestUri <- uriBuilder.Uri
            | None -> ()

            this.HttpRequestMessage

        [<CustomOperation>]
        member this.payload(_, payload: Payload) =
            let content = payload.Content.ToContent()

            this.HttpRequestMessage.Headers.Add("Content-Type", content.Headers.ContentType.MediaType)
            this.HttpRequestMessage.Content <- content
            
            this.HttpRequestMessage

    let req = RequestBuilder()
    