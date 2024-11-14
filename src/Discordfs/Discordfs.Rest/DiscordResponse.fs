namespace Discordfs.Rest

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open System
open System.Net.Http
open System.Text.Json

type DiscordResponse<'a> = Result<ResponseWithMetadata<'a>, ResponseWithMetadata<DiscordError>>

module DiscordResponse =
    let private withMetadata<'a> (res: HttpResponseMessage) (obj: 'a) =
        let getOptionalHeader (key: string) (res: HttpResponseMessage) =
            match res.Headers.TryGetValues key with
            | true, v -> v |> Seq.tryHead
            | false, _ -> None

        {
            Data = obj
            RateLimitHeaders = {
                Limit = res |> getOptionalHeader "X-RateLimit-Limit" >>. int
                Remaining = res |> getOptionalHeader "X-RateLimit-Remaining" >>. int
                Reset = res |> getOptionalHeader "X-RateLimit-Reset" >>. DateTime.Parse
                ResetAfter = res |> getOptionalHeader "X-RateLimit-ResetAfter" >>. double
                Bucket = res |> getOptionalHeader "X-RateLimit-Bucket"
                Global = res |> getOptionalHeader "X-RateLimit-Global" >>. bool.Parse
                Scope = res |> getOptionalHeader "X-RateLimit-Scope" >>. RateLimitScope.FromString
            }
            Status = res.StatusCode
        }

    // Used in requests that return content in a success result
    let asJson<'a> (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | v when v >= 200 && v < 300 -> return! (Http.toJson<'a> res) ?> withMetadata res ?> Ok
        | v when v = 429 -> return! RateLimit <? (Http.toJson res) ?> withMetadata res ?> Error
        | v when v >= 400 && v < 500 -> return! ClientError <? (Http.toJson res) ?> withMetadata res ?> Error
        | _ -> return Unexpected (res.StatusCode) |> withMetadata res |> Error
    }

    // Used in requests that do not return content in a success result
    let asEmpty (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | v when v >= 200 && v < 300 -> return Option<Empty>.None |> withMetadata res |> Ok
        | v when v = 429 -> return! RateLimit <? (Http.toJson res) ?> withMetadata res ?> Error
        | v when v >= 400 && v < 500 -> return! ClientError <? (Http.toJson res) ?> withMetadata res ?> Error
        | _ -> return Unexpected (res.StatusCode) |> withMetadata res |> Error
    }

    /// Used in requests that may return a content or no content success result
    let asOptionalJson<'a> (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | v when res.IsSuccessStatusCode ->
            let length = res.Content.Headers.ContentLength |> Nullable.toOption

            match length with
            | Some l when l = 0L -> return Option<'a>.None |> withMetadata res |> Ok
            | _ -> return! (Http.toJson res) ?> withMetadata res ?> Ok

        | v when v = 429 ->
            return! RateLimit <? (Http.toJson res) ?> withMetadata res ?> Error

        | v when v >= 400 && v < 500 ->
            return! ClientError <? (Http.toJson res) ?> withMetadata res ?> Error
        | _ ->
            return Unexpected (res.StatusCode) |> withMetadata res |> Error
    }

    let unwrap<'a> (res: DiscordResponse<'a>) =
        match res with
        | Ok { Data = v } -> v
        | Error _ -> failwith "Unsuccessful discord response was unwrapped"
