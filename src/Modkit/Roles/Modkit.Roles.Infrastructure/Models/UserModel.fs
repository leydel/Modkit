namespace Modkit.Roles.Infrastructure.Models

open System
open System.Collections.Generic
open System.Text.Json.Serialization

module UserModel =
    [<Literal>]
    let Id = "id"

    [<Literal>]
    let ApplicationId = "applicationId"

    [<Literal>]
    let AccessToken = "accessToken"

    [<Literal>]
    let AccessTokenExpiry = "accessTokenExpiry"

    [<Literal>]
    let RefreshToken = "refreshToken"

    [<Literal>]
    let Metadata = "metadata"

type UserModel = {
    [<JsonPropertyName(UserModel.Id)>] Id: string
    [<JsonPropertyName(UserModel.ApplicationId)>] ApplicationId: string
    [<JsonPropertyName(UserModel.AccessToken)>] AccessToken: string
    [<JsonPropertyName(UserModel.AccessTokenExpiry)>] AccessTokenExpiry: DateTime // TODO: Should this serialize to unix timestamp?
    [<JsonPropertyName(UserModel.RefreshToken)>] RefreshToken: string
    [<JsonPropertyName(UserModel.Metadata)>] Metadata: IDictionary<string, int>
}
