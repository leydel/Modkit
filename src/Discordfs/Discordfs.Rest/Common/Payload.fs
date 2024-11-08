namespace Discordfs.Rest.Common

open System.Collections.Generic
open System.Net.Http
open System.Net.Http.Headers
open System.Text.Json

[<AutoOpen>]
module Payload =
    type IPayloadBuilder =
        abstract member ToContent: unit -> HttpContent
    
    type JsonPayloadBuilder() =
        member val Properties: IDictionary<string, obj> = Dictionary()

        member this.Yield(_) =
            this

        [<CustomOperation "required">]
        member this.Required (_, name: string, value: 'a) =
            this.Properties.Add(name, value)
            this

        [<CustomOperation "optional">]
        member this.Optional (_, name: string, value: 'a option) =
            if value.IsSome then
                this.Properties.Add(name, value)
            this

        interface IPayloadBuilder with
            member this.ToContent () =
                new StringContent(Json.serializeF this.Properties, MediaTypeHeaderValue("application/json"))

    let json = JsonPayloadBuilder()

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

        [<CustomOperation "part">]
        member this.Part (_, name: string, content: IPayloadBuilder) =
            this.Form.Add(content.ToContent(), name)
            this
            
        [<CustomOperation "file">]
        member this.File (_, fileName: string, fileContent: IPayloadBuilder) =
            this.Form.Add(fileContent.ToContent(), $"files[{fileCount}]", fileName)
            fileCount <- fileCount + 1
            this
            
        [<CustomOperation "files">]
        member this.Files (_, files: IDictionary<string, IPayloadBuilder>) =
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
