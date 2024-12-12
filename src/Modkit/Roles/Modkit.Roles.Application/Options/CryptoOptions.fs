namespace Modkit.Roles.Application.Options

type CryptoOptions () =
    static member Key = "Crypto"

    member val CookieKey: string
    