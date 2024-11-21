namespace System.Net.Http

open System
open System.Web

[<AutoOpen>]
module RequestBuilder =
    type RequestBuilder(host: string) =
        member val Host = host with get, set // TODO: Figure out neat way to clean this up

        member _.Yield(_) = ()
        member _.Yield(req: HttpRequestMessage) = req

        [<CustomOperation>]
        member this.host (req: HttpRequestMessage, host: string) =
            this.Host <- host
            req

        [<CustomOperation>]
        member this.get (req: HttpRequestMessage, endpoint: string) =
            req.RequestUri <- new Uri(this.Host + "/" + endpoint)
            req.Method <- HttpMethod.Get
            req

        [<CustomOperation>]
        member this.post (req: HttpRequestMessage, endpoint: string) =
            req.RequestUri <- new Uri(this.Host + "/" + endpoint)
            req.Method <- HttpMethod.Post
            req

        [<CustomOperation>]
        member this.put (req: HttpRequestMessage, endpoint: string) =
            req.RequestUri <- new Uri(this.Host + "/" + endpoint)
            req.Method <- HttpMethod.Put
            req

        [<CustomOperation>]
        member this.patch (req: HttpRequestMessage, endpoint: string) =
            req.RequestUri <- new Uri(this.Host + "/" + endpoint)
            req.Method <- HttpMethod.Patch
            req

        [<CustomOperation>]
        member this.delete (req: HttpRequestMessage, endpoint: string) =
            req.RequestUri <- new Uri(this.Host + "/" + endpoint)
            req.Method <- HttpMethod.Delete
            req

        [<CustomOperation>]
        member _.header(req: HttpRequestMessage, key: string, value: string option) =
            match value with
            | Some value -> req.Headers.Add(key, value)
            | None -> ()
            req

        [<CustomOperation>]
        member this.header (req: HttpRequestMessage, key: string, value: string) =
            this.header(req, key, Some value)

        [<CustomOperation>]
        member _.query(req: HttpRequestMessage, key: string, value: string option) =
            match value with
            | Some value ->
                let uriBuilder = UriBuilder(req.RequestUri)
                let query = HttpUtility.ParseQueryString(uriBuilder.Query)
                query.Add(key, value)
                uriBuilder.Query <- query.ToString()
                req.RequestUri <- uriBuilder.Uri
                req
            | None -> req

        [<CustomOperation>]
        member this.query(req: HttpRequestMessage, key: string, value: string) =
            this.query(req, key, Some value)

        [<CustomOperation>]
        member _.payload(req: HttpRequestMessage, payload: Payload) =
            let content = payload.Content.ToContent()

            req.Headers.Add("Content-Type", content.Headers.ContentType.MediaType)
            req.Content <- content
            req
    