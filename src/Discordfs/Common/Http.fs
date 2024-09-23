namespace Modkit.Discordfs.Common

open Modkit.Discordfs.Utils
open System
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Threading.Tasks
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
        member this.Payload(_, content: Payload) =
            match content.Type with
            | Json ->
                this.HttpRequestMessage.Content <- new StringContent(content.ToString())
                this.HttpRequestMessage.Headers.Add("Content-Type", "application/json")
            
            this.HttpRequestMessage

    let req = RequestBuilder()
    
    type TypedResponseHandler<'a> = HttpResponseMessage -> Result<'a, HttpStatusCode>
    type TypedTaskResponseHandler<'a> = HttpResponseMessage -> Task<Result<'a, HttpStatusCode>>
    type ResponseHandler = HttpResponseMessage -> Result<obj, HttpStatusCode>
    type TaskResponseHandler = HttpResponseMessage -> Task<Result<obj, HttpStatusCode>>
    type ResponseHandlers = IDictionary<HttpStatusCode * HttpStatusCode, TaskResponseHandler>

    type ResponseBuilder() =
        member val Handlers: ResponseHandlers = Dictionary()

        member this.HandleRange (min: HttpStatusCode) (max: HttpStatusCode) (handler: TaskResponseHandler) =
            this.Handlers.Add((min, max), handler)

        member this.Handle (status: HttpStatusCode) (handler: TaskResponseHandler) =
            this.HandleRange status status handler

        member _.Map<'a> (handler: TypedTaskResponseHandler<'a>) (res: HttpResponseMessage) = task {
            let! value = handler res
            return Result.map (fun v -> v :> obj) value
        }

        member this.Yield(_) =
            this
            
        [<CustomOperation("success")>]
        member this.Success (_, handler: TypedTaskResponseHandler<'a>) =
            this.HandleRange (enum<HttpStatusCode> 200) (enum<HttpStatusCode> 299) (this.Map handler) |> ignore
            this.Handlers
            
        [<CustomOperation("success")>]
        member this.Success (x, handler: TypedResponseHandler<'a>) =
            this.Success(x, Task.apply handler)
            
        [<CustomOperation("ok")>]
        member this.Ok (_, handler: TypedTaskResponseHandler<'a>) =
            this.Handle HttpStatusCode.OK (this.Map handler) |> ignore
            this.Handlers

        [<CustomOperation("ok")>]
        member this.Ok (x, handler: TypedResponseHandler<'a>) =
            this.Ok(x, Task.apply handler)
            
        [<CustomOperation("created")>]
        member this.Created (_, handler: TypedTaskResponseHandler<'a>) =
            this.Handle HttpStatusCode.Created (this.Map handler) |> ignore
            this.Handlers

        [<CustomOperation("created")>]
        member this.Created (x, handler: TypedResponseHandler<'a>) =
            this.Created(x, Task.apply handler)
            
        [<CustomOperation("accepted")>]
        member this.Accepted (_, handler: TypedTaskResponseHandler<'a>) =
            this.Handle HttpStatusCode.Accepted (this.Map handler) |> ignore
            this.Handlers

        [<CustomOperation("accepted")>]
        member this.Accepted (x, handler: TypedResponseHandler<'a>) =
            this.Accepted(x, Task.apply handler)
            
        [<CustomOperation("noContent")>]
        member this.NoContent (_, handler: TypedTaskResponseHandler<'a>) =
            this.Handle HttpStatusCode.NoContent (this.Map handler) |> ignore
            this.Handlers

        [<CustomOperation("noContent")>]
        member this.NoContent (x, handler: TypedResponseHandler<'a>) =
            this.NoContent(x, Task.apply handler)

        // TODO: Add specific failure operations here

    let res = ResponseBuilder()

    let handle<'a> (handlers: ResponseHandlers) (res: HttpResponseMessage) =
        let status = res.StatusCode

        let handler =
            handlers
            |> Seq.toList
            |> List.map (fun kvp -> (kvp.Key, kvp.Value))
            |> List.tryFind (fun ((min, max), _) -> status >= min && status <= max)
            |> Option.map (fun (_, handler) -> handler)
            
        match handler with
        | Some handler -> (handler res) |> Task.map (Result.map (fun v -> v :?> 'a))
        | None -> Error res.StatusCode |> Task.FromResult

    let toOk<'a> (apply: string -> 'a) (res: HttpResponseMessage) =
        res.Content.ReadAsStringAsync()
        |> Task.map apply
        |> Task.map Ok

    let toOkJson<'a> (res: HttpResponseMessage) =
        toOk (fun body -> FsJson.deserialize<'a> body)
