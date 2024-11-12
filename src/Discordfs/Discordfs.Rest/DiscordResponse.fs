namespace Discordfs.Rest

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open System.Net.Http
open System.Text.Json

type DiscordResponse<'a> = Result<RateLimitInfo<'a>, RateLimitInfo<DiscordError>>

module DiscordResponse =
    let private withRateLimitHeaders<'a> (res: HttpResponseMessage) (obj: 'a) =
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

        (obj, rateLimitHeaders)

    // Used in requests that return content in a success result
    let asJson<'a> (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | v when v >= 200 && v < 300 -> return! (Http.toJson<'a> res) ?> withRateLimitHeaders res ?> Ok
        | v when v = 429 -> return! RateLimit <? (Http.toJson res) ?> withRateLimitHeaders res ?> Error
        | v when v >= 400 && v < 500 -> return! ClientError <? (Http.toJson res) ?> withRateLimitHeaders res ?> Error
        | _ -> return Unexpected (res.StatusCode) |> withRateLimitHeaders res |> Error
    }

    // Used in requests that do not return content in a success result
    let asEmpty (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | v when v >= 200 && v < 300 -> return Option<Empty>.None |> withRateLimitHeaders res |> Ok
        | v when v = 429 -> return! RateLimit <? (Http.toJson res) ?> withRateLimitHeaders res ?> Error
        | v when v >= 400 && v < 500 -> return! ClientError <? (Http.toJson res) ?> withRateLimitHeaders res ?> Error
        | _ -> return Unexpected (res.StatusCode) |> withRateLimitHeaders res |> Error
    }

    /// Used in requests that may return a content or no content success result
    let asOptionalJson<'a> (res: HttpResponseMessage) = task {
        match int res.StatusCode with
        | v when res.IsSuccessStatusCode ->
            let length = res.Content.Headers.ContentLength |> Nullable.toOption

            match length with
            | Some l when l = 0L -> return Option<'a>.None |> withRateLimitHeaders res |> Ok
            | _ -> return! (Http.toJson res) ?> withRateLimitHeaders res ?> Ok

        | v when v = 429 ->
            return! RateLimit <? (Http.toJson res) ?> withRateLimitHeaders res ?> Error

        | v when v >= 400 && v < 500 ->
            return! ClientError <? (Http.toJson res) ?> withRateLimitHeaders res ?> Error
        | _ ->
            return Unexpected (res.StatusCode) |> withRateLimitHeaders res |> Error
    }

    let unwrap<'a> (res: DiscordResponse<'a>) =
        match res with
        | Ok (v, _) -> v
        | Error _ -> failwith "Unsuccessful discord response was unwrapped"
