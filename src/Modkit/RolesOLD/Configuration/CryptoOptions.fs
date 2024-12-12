namespace Modkit.Roles.Configuration

type CryptoOptions () =
    static member Key = "Crypto"

    member val CookieKey: string
    