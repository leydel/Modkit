namespace Modkit.Roles.Domain.Entities

open System
open System.Collections.Generic

type User = {
    Id: string
    ApplicationId: string
    AccessToken: string
    AccessTokenExpiry: DateTime
    RefreshToken: string
    Metadata: IDictionary<string, int>
}
