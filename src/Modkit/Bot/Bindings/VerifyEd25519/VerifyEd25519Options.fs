namespace Modkit.Bot.Bindings

type VerifyEd25519Options (?publicKey: string) =
    static member val DefaultPublicKeyConfigurationName: string = "DiscordPublicKey" with get

    member val PublicKey: string = publicKey >>? null with get, set
