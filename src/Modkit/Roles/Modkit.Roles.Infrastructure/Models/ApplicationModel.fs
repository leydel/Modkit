namespace Modkit.Roles.Infrastructure.Models

open System.Collections.Generic
open System.Text.Json.Serialization

module ApplicationModel =
    [<Literal>]
    let Id = "id"

    [<Literal>]
    let Token = "token"

    [<Literal>]
    let PublicKey = "publicKey"

    [<Literal>]
    let ClientSecret = "clientSecret"

    [<Literal>]
    let Metadata = "metadata"

type ApplicationModel = {
    [<JsonPropertyName(ApplicationModel.Id)>] Id: string
    [<JsonPropertyName(ApplicationModel.Token)>] Token: string
    [<JsonPropertyName(ApplicationModel.PublicKey)>] PublicKey: string
    [<JsonPropertyName(ApplicationModel.ClientSecret)>] ClientSecret: string
    [<JsonPropertyName(ApplicationModel.Metadata)>] Metadata: IDictionary<string, string>
}
