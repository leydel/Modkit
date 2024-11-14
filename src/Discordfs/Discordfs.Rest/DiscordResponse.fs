namespace Discordfs.Rest

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open System.Net.Http
open System.Text.Json

type DiscordResponse<'a> = Result<ResponseWithMetadata<'a>, ResponseWithMetadata<DiscordError>>

module DiscordResponse =
    let private withMetadata<'a> (res: HttpResponseMessage) (obj: 'a) =
        let rateLimitHeaders = {
            Limit = None
            Remaining = None
            Reset = None
            ResetAfter = None
            Bucket = None
            Global = None
            Scope = None
        }
        
        // TODO: Get actual headers from HttpResponseMessage

        {
            Data = obj
            RateLimitHeaders = rateLimitHeaders
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
