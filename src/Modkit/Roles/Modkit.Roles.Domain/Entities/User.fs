namespace Modkit.Roles.Domain.Entities

open System

type User = {
    Id: string
    ApplicationId: string
    AccessToken: string
    AccessTokenExpiry: DateTime
    RefreshToken: string
    Metadata: (string * int) seq
}
