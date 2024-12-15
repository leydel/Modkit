namespace System.Net.Http

open System.Collections.Generic
open System.Net.Http.Headers
open System.Text.Json

// TODO: Create alternative approach for creating payloads (operation to add record directly?)

[<AutoOpen>]
module Payload =
    type IPayloadBuilder =
        abstract member ToContent: unit -> HttpContent
    
    type JsonPayloadBuilder() =
        member val Properties: IDictionary<string, obj> = Dictionary()

        member this.Yield(_) =
            this

        [<CustomOperation>]
        member this.required (_, name: string, value: 'a) =
            this.Properties.Add(name, value)
            this

        [<CustomOperation>]
        member this.optional (_, name: string, value: 'a option) =
            if value.IsSome then
                this.Properties.Add(name, value)
            this

        interface IPayloadBuilder with
            member this.ToContent () =
                new StringContent(Json.serializeF this.Properties, MediaTypeHeaderValue("application/json"))

    let json = JsonPayloadBuilder()

    type UrlEncodedPayloadBuilder() =
        member val Properties: IDictionary<string, obj> = Dictionary()

        member this.Yield(_) =
            this

        [<CustomOperation>]
        member this.required (_, name: string, value: 'a) =
            this.Properties.Add(name, value)
            this

        [<CustomOperation>]
        member this.optional (_, name: string, value: 'a option) =
            if value.IsSome then
                this.Properties.Add(name, value)
            this

        interface IPayloadBuilder with
            member this.ToContent () =
                new FormUrlEncodedContent(
                    this.Properties
                    |> Seq.map (|KeyValue|)
                    |> Seq.map (fun (k, v) -> KeyValuePair(k, v.ToString()))
                )

    let urlencoded = UrlEncodedPayloadBuilder()

    type JsonPayload<'a>(payload: 'a) =
        interface IPayloadBuilder with
            member _.ToContent () =
                new StringContent(Json.serializeF payload, MediaTypeHeaderValue("application/json"))

    type JsonListPayload<'a>(list: 'a list) =
        interface IPayloadBuilder with
            member _.ToContent () =
                new StringContent(Json.serializeF list, MediaTypeHeaderValue("application/json"))

    type StringPayload(str: string) =
        interface IPayloadBuilder with
            member _.ToContent () =
                new StringContent(str, MediaTypeHeaderValue("plain/text"))

    type FilePayload(fileContent: string, mimeType: string) =
            member _.ToContent () =
                new StringContent(fileContent, MediaTypeHeaderValue(mimeType))

    type MultipartPayloadBuilder() =
        let mutable fileCount = 0

        member val Form = new MultipartFormDataContent()

        member this.Yield(_) =
            this

        [<CustomOperation>]
        member this.part (_, name: string, content: IPayloadBuilder) =
            this.Form.Add(content.ToContent(), name)
            this
            
        [<CustomOperation>]
        member this.file (_, fileName: string, fileContent: IPayloadBuilder) =
            this.Form.Add(fileContent.ToContent(), $"files[{fileCount}]", fileName)
            fileCount <- fileCount + 1
            this
            
        [<CustomOperation>]
        member this.files (_, files: IDictionary<string, IPayloadBuilder>) =
            for fileName, fileContent in Seq.map (|KeyValue|) files do
                this.Form.Add(fileContent.ToContent(), $"files[{fileCount}]", fileName)
                fileCount <- fileCount + 1
            this

        interface IPayloadBuilder with
            member this.ToContent () =
                this.Form

    let multipart = MultipartPayloadBuilder()

    [<AbstractClass>]
    type Payload() =
        abstract member Content: IPayloadBuilder

    type JsonPayloadImpl<'a> (content: 'a) =
        inherit Payload()

        override _.Content = JsonPayload content

    let fromObj<'a> (value: 'a) =
        JsonPayloadImpl value :> Payload

    // TODO: Refactor/rewrite these payloads to clean up usage
