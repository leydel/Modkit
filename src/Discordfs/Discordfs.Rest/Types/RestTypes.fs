namespace Discordfs.Rest.Types

open Discordfs.Types
open System
open System.Collections.Generic
open System.Net
open System.Text.Json.Serialization

type RateLimitResponse = {
    [<JsonPropertyName "message">] Message: string
    [<JsonPropertyName "retry_after">] RetryAfter: float
    [<JsonPropertyName "global">] Global: bool
    [<JsonPropertyName "code">] Code: JsonErrorCode option
} 

type ErrorResponse = {
    [<JsonPropertyName "code">] Code: JsonErrorCode
    [<JsonPropertyName "message">] Message: string
    [<JsonPropertyName "errors">] Errors: IDictionary<string, string>
}

type RateLimitScope =
    | USER
    | GLOBAL
    | SHARED
with
    override this.ToString () =
        match this with
        | USER -> "user"
        | GLOBAL -> "global"
        | SHARED -> "shared"

    static member FromString (str: string) =
        match str with
        | "user" -> RateLimitScope.USER
        | "global" -> RateLimitScope.GLOBAL
        | "shared" -> RateLimitScope.SHARED
        | _ -> failwith "Unexpected RateLimitScope provided"

type RateLimitHeaders = {
    Limit: int option
    Remaining: int option
    Reset: DateTime option
    ResetAfter: double option
    Bucket: string option
    Global: bool option
    Scope: RateLimitScope option
}

type ResponseWithMetadata<'a> = {
    Data: 'a
    RateLimitHeaders: RateLimitHeaders
    Status: HttpStatusCode
}

// Used in StartThreadWithoutMessagePayload (should probably be reworked)
type ThreadType =
    | ANNOUNCEMENT_THREAD = 10
    | PUBLIC_THREAD = 11
    | PRIVATE_THREAD = 12

[<RequireQualifiedAccess>]
type DiscordAccessToken =
    | OAUTH of string
    | BOT of string
