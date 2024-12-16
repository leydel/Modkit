namespace Modkit.Roles.Infrastructure.Mappers

open Modkit.Roles.Domain.Entities

open Modkit.Roles.Infrastructure.Models

module UserMapper =
    let fromDomain (user: User): UserModel = {
        Id = user.Id
        ApplicationId = user.ApplicationId
        AccessToken = user.AccessToken
        AccessTokenExpiry = user.AccessTokenExpiry
        RefreshToken = user.RefreshToken
        Metadata = user.Metadata |> dict
    }

    let toDomain (model: UserModel): User = {
        Id = model.Id
        ApplicationId = model.ApplicationId
        AccessToken = model.AccessToken
        AccessTokenExpiry = model.AccessTokenExpiry
        RefreshToken = model.RefreshToken
        Metadata = model.Metadata |> Seq.map (fun kvp -> (kvp.Key, kvp.Value))
    }
