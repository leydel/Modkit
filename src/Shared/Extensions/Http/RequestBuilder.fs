namespace System.Net.Http

open System
open System.Web

[<AutoOpen>]
module RequestBuilder =
    let private setPath host endpoint method (req: HttpRequestMessage) =
        req.RequestUri <- new Uri(host + "/" + endpoint)
        req.Method <- method
        req

    let private addHeader key (value: string option) (req: HttpRequestMessage) =
        match value with
        | Some value -> req.Headers.Add(key, value); req
        | None -> req

    let private addQueryString key value (req: HttpRequestMessage) =
        match value with
        | Some value ->
            let uriBuilder = UriBuilder(req.RequestUri)
            let query = HttpUtility.ParseQueryString(uriBuilder.Query)
            query.Add(key, value)
            uriBuilder.Query <- query.ToString()
            req.RequestUri <- uriBuilder.Uri
            req
        | None -> req

    let private addPayload (payload: Payload) (req: HttpRequestMessage) =
        let content = payload.Content.ToContent()
        addHeader "Content-Type" (Some content.Headers.ContentType.MediaType) req |> ignore
        req.Content <- content
        req

    type RequestBuilder (host) =
        member val Host: string = host with get, set

        member _.Yield(_: unit) = new HttpRequestMessage()
        member _.Yield(req: HttpRequestMessage) = req

        /// Overwrite the host used to create the req computation expression.
        [<CustomOperation>]
        member this.host (req, host) =
            this.Host <- host
            req

        [<CustomOperation>]
        member this.get (req, endpoint) =
            setPath this.Host endpoint HttpMethod.Get req

        [<CustomOperation>]
        member this.post (req, endpoint) =
            setPath this.Host endpoint HttpMethod.Post req

        [<CustomOperation>]
        member this.put (req, endpoint) =
            setPath this.Host endpoint HttpMethod.Put req

        [<CustomOperation>]
        member this.patch (req, endpoint) =
            setPath this.Host endpoint HttpMethod.Patch req

        [<CustomOperation>]
        member this.delete (req, endpoint) =
            setPath this.Host endpoint HttpMethod.Delete req

        [<CustomOperation>]
        member _.header(req, key, value) =
            addHeader key value req

        [<CustomOperation>]
        member _.header(req, key, value) =
            addHeader key (Some value) req

        [<CustomOperation>]
        member _.query(req, key, value) =
            addQueryString key value req

        [<CustomOperation>]
        member _.query(req, key, value) =
            addQueryString key (Some value) req

        [<CustomOperation>]
        member _.payload(req: HttpRequestMessage, payload: Payload) =
            addPayload payload req
