namespace Modkit.Roles.Infrastructure.Mappers

open Modkit.Roles.Domain.Types
open Modkit.Roles.Domain.Entities

open Modkit.Roles.Infrastructure.Models

module ApplicationMapper =
    let fromDomain (application: Application): ApplicationModel = {
        Id = application.Id
        Token = application.Token
        PublicKey = application.PublicKey
        ClientSecret = application.ClientSecret
        Metadata = application.Metadata |> Seq.map (fun (k, v) -> (k, MetadataTypeMapper.fromDomain v)) |> dict
    }

    let toDomain (model: ApplicationModel): Application = {
        Id = model.Id
        Token = model.Token
        PublicKey = model.PublicKey
        ClientSecret = model.ClientSecret
        Metadata = model.Metadata |> Seq.map (fun kvp -> (kvp.Key, MetadataTypeMapper.toDomain kvp.Value))
    }
