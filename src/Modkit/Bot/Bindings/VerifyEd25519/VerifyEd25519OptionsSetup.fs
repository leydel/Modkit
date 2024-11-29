namespace Modkit.Bot.Bindings

open System
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Options

type VerifyEd25519OptionsSetup (configuration: IConfiguration) =
    interface IConfigureNamedOptions<VerifyEd25519Options> with
        member _.Configure (name: string, options: VerifyEd25519Options) =
            let key =
                match String.IsNullOrWhiteSpace name with
                | false -> name
                | true -> VerifyEd25519Options.DefaultPublicKeyConfigurationName

            options.PublicKey <- configuration.GetValue<string> key

        member this.Configure (options: VerifyEd25519Options) =
            (this :> IConfigureNamedOptions<VerifyEd25519Options>).Configure(Options.DefaultName, options)
