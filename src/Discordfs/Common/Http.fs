namespace Modkit.Discordfs.Common

open Modkit.Discordfs.Utils
open System
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks
open System.Web

[<AutoOpen>]
module Http =
    let send (httpClientFactory: IHttpClientFactory) (req: HttpRequestMessage) =
        httpClientFactory.CreateClient().SendAsync req

    let toJson<'a> (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        let! body = res.Content.ReadAsStringAsync()
        return FsJson.deserialize<'a> body
    }
    let toRaw (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        return! res.Content.ReadAsStringAsync()
    }

    let ignore (resTask: Task<HttpResponseMessage>) = task {
        do! resTask :> Task
    }

    [<AutoOpen>]
    type PayloadType =
        | Json

    [<AbstractClass>]
    type Payload(``type``: PayloadType) =
        member val Type = ``type`` with get

        abstract member Serialize: unit -> string
    
    [<AbstractClass>]
    type PayloadBuilder() =
        member val Properties: IDictionary<string, obj> = Dictionary()

        abstract member Serialize: unit -> string

        member this.Yield(_) =
            this

        [<CustomOperation("required")>]
        member this.Required (_, name: string, value: 'a) =
            this.Properties.Add(name, value)

            this.Serialize()

        [<CustomOperation("optional")>]
        member this.Optional (_, name: string, value: 'a option) =
            if value.IsSome then
                this.Properties.Add(name, value)

            this.Serialize()

        [<CustomOperation("property")>]
        member this.Property (_, name: string, value: 'a) =
            0

        [<CustomOperation("property")>]
        member this.Property (_, name: string, value: 'a option) =
            if value.IsSome then
                this.Properties.Add(name, value)

            this.Serialize()

    type JsonBuilder() =
        inherit PayloadBuilder() with
            [<CustomOperation("json")>]
            override this.Serialize() =
                FsJson.serialize this.Properties

    let json = JsonBuilder()

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
        member this.Payload(_, payloadType: PayloadType, content: Payload) =
            match payloadType with
            | Json ->
                this.HttpRequestMessage.Content <- new StringContent(content.ToString())
                this.HttpRequestMessage.Headers.Add("Content-Type", "application/json")
            
            this.HttpRequestMessage

    let req = RequestBuilder()
