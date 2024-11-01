module Modkit.Api.Common.Response

open System.Net
open System.Net.Http
open System.Net.Http.Headers

let create (status: HttpStatusCode) =
    new HttpResponseMessage(status)

let withBody (body: HttpContent) (res: HttpResponseMessage) =
    res.Content <- body
    res

let withJson (json: string) (res: HttpResponseMessage) =
    let body = new StringContent(json, MediaTypeHeaderValue("application/json", "utf-8"))
    withBody body res

let withHeader (key: string) (value: string) (res: HttpResponseMessage) =
    res.Headers.Add(key, value)
    res
