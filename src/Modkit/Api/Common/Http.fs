namespace Modkit.Api.Common

open System
open System.Net
open System.Net.Http
open System.Text.Json

[<AutoOpen>]
module Req =
    let req = Http.RequestBuilder("This should be replaced by `httpClient.BaseAddress.ToString()`")

type ApiResponse<'a> = Result<'a, HttpStatusCode>

module ApiResponse =
    let asJson<'a> (res: HttpResponseMessage) = task {
        match res.StatusCode with
        | _ when res.IsSuccessStatusCode -> return! res.Content.ReadAsStringAsync() ?> Json.deserializeF<'a> ?> Ok
        | status -> return Error status
    }

    let asEmpty (res: HttpResponseMessage) = task {
        match res.StatusCode with
        | _ when res.IsSuccessStatusCode -> return Option<Empty>.None |> Ok
        | status -> return Error status
    }

    let asOptionalJson<'a> (res: HttpResponseMessage) = task {
        match res.StatusCode with
        | _ when res.IsSuccessStatusCode ->
            let length = res.Content.Headers.ContentLength |> Nullable.toOption

            match length with
            | Some l when l = 0L -> return Option<'a>.None |> Ok
            | _ -> return! res.Content.ReadAsStringAsync() ?> Json.deserializeF<'a> ?> Some ?> Ok
        | status ->
            return Error status
    }

    let asRaw (res: HttpResponseMessage) = task {
        match res.StatusCode with
        | _ when res.IsSuccessStatusCode -> return! res.Content.ReadAsStringAsync() ?> Ok
        | status -> return Error status
    }

    let unwrap<'a> (res: ApiResponse<'a>) =
        match res with
        | Ok v -> v
        | Error _ -> failwith "Unsuccessful API response was unwrapped"
