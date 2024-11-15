namespace Discordfs.Rest

open Discordfs.Rest.Types
open System.Net
open System.Text.Json

type DiscordError =
    | RateLimit of RateLimitResponse
    | ClientError of ErrorResponse
    | Unexpected of HttpStatusCode

module DiscordError =
    let toExn (error: DiscordError) =
        match error with
        | RateLimit err ->
            let ex = exn err.Message
            ex.Data.Add(nameof err.Code, err.Code)
            ex.Data.Add(nameof err.Global, err.Global)
            ex.Data.Add(nameof err.RetryAfter, err.RetryAfter)
            ex

        | ClientError err ->
            let ex = exn err.Message
            ex.Data.Add(nameof err.Code, err.Code)
            ex.Data.Add(nameof err.Errors, Json.serializeF err.Errors)
            ex

        | Unexpected status ->
            let ex = exn $"The request unexpectedly resulted in a {status} result"
            ex.Data.Add("status", status)
            ex
