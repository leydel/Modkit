namespace Modkit.Roles.Domain.Entities

open Modkit.Roles.Domain.Types

type Application = {
    Id: string
    Token: string
    PublicKey: string
    ClientSecret: string
    Metadata: (string * MetadataType) seq
}
