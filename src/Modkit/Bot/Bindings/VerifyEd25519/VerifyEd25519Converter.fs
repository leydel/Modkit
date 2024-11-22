namespace Modkit.Bot.Bindings

open Discordfs.Webhook.Modules
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Converters
open Microsoft.Azure.Functions.Worker.Core
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Options
open System.Threading.Tasks

type VerifyEd25519Converter (options: IOptionsMonitor<VerifyEd25519Options>) =
    member _.ConvertAsync (ctx: ConverterContext) =
        task {
            let success value = ConversionResult.Success value
            let fail message = ConversionResult.Failed (exn message)

            let! req = ctx.FunctionContext.GetHttpRequestDataAsync()

            match req with
            | null ->
                return fail $"The '{nameof VerifyEd25519Converter} expects an '{nameof HttpRequestData}' instance in the current context."

            | req ->
                match ctx.TargetType with
                | v when v = typeof<bool> ->
                    let! body = req.ReadAsStringAsync()
                    let signature = req.Headers.GetValueOption "X-Signature-Ed25519" >>? ""
                    let timestamp = req.Headers.GetValueOption "X-Signature-Timestamp" >>? ""
                    let publicKey =
                        match options.CurrentValue.PublicKey with
                        | null -> (ctx.Source :?> ModelBindingData).Content.ToObjectFromJson<VerifyEd25519BindingData>().PublicKey
                        | v -> v

                    return success <| Ed25519.verify timestamp body signature publicKey

                | _ ->
                    return fail $"The type '{ctx.TargetType}' is not supported by the '{nameof VerifyEd25519Converter}'."
        }
        |> ValueTask<ConversionResult>
