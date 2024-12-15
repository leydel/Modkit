namespace Modkit.Roles.Infrastructure.Mappers

open Modkit.Roles.Domain.Entities

open Modkit.Roles.Infrastructure.Models

module ApplicationMapper =
    let fromDomain (application: Application): ApplicationModel = {
        Id = application.Id
        Token = application.Token
        PublicKey = application.PublicKey
        ClientSecret = application.ClientSecret
    }

    let toDomain (model: ApplicationModel): Application = {
        Id = model.Id
        Token = model.Token
        PublicKey = model.PublicKey
        ClientSecret = model.ClientSecret
    }
