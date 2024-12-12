namespace Modkit.Roles.Types

open System
open System.Collections.Generic
open System.Text.Json
open System.Text.Json.Serialization

type AppUser = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "applicationId">] ApplicationId: string
    [<JsonPropertyName "accessToken">] AccessToken: string
    [<JsonPropertyName "accessTokenExpiry">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] AccessTokenExpiry: DateTime
    [<JsonPropertyName "refreshToken">] RefreshToken: string
    [<JsonPropertyName "metadata">] Metadata: IDictionary<string, int>
}
