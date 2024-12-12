namespace Modkit.Roles.Infrastructure.Models

open System.Text.Json.Serialization

module ApplicationModel =
    [<Literal>]
    let Id = "id"

    [<Literal>]
    let Token = "token"

    [<Literal>]
    let PublicKey = "publicKey"

type ApplicationModel = {
    [<JsonPropertyName(ApplicationModel.Id)>] Id: string
    [<JsonPropertyName(ApplicationModel.Token)>] Token: string
    [<JsonPropertyName(ApplicationModel.PublicKey)>] PublicKey: string
}
