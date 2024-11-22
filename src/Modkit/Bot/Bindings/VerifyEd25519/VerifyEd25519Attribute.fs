namespace Modkit.Bot.Bindings

open Microsoft.Azure.Functions.Worker.Converters
open System

[<AttributeUsage(AttributeTargets.Parameter)>]
type VerifyEd25519Attribute () =
    inherit InputConverterAttribute (typeof<VerifyEd25519Converter>)
    
    member val PublicKey: string = null with get, set
