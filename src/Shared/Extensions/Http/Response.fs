module System.Net.Http.Response

open Microsoft.Azure.Functions.Worker.Http
open System.Text.Json

let withBody (body: string) (res: HttpResponseData) =
    res.WriteString body
    res

let withHeader (key: string) (value: string) (res: HttpResponseData) =
    res.Headers.Add(key, value)
    res

let withJson (obj: 'a) (res: HttpResponseData) =
    res
    |> withBody (Json.serializeF obj)
    |> withHeader "Content-Type" "application/json"
