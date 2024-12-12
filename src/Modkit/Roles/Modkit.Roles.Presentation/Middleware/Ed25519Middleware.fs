namespace Modkit.Roles.Presentation.Middleware

open System
open System.Net

open MediatR
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Azure.Functions.Worker.Middleware

open Modkit.Roles.Application.Queries

type VerifyEd25519 () =
    inherit Attribute ()

type Ed25519Middleware (
    mediator: ISender
) =
    interface IFunctionsWorkerMiddleware with
        member _.Invoke (ctx: FunctionContext, next: FunctionExecutionDelegate) = task {
            let! req = ctx.GetHttpRequestDataAsync()

            let! success = task {
                match ctx.BindingContext.BindingData.TryGetValue "applicationId" with // TODO: Test if this actually grabs correctly
                | false, _ -> return false
                | true, applicationId ->
                    let tryGetHeader name (headers: HttpHeadersCollection) =
                        match headers.Contains name with
                        | true -> headers.GetValues name |> Seq.tryHead
                        | false -> None

                    let! body = req.ReadAsStringAsync()
                    let signature = req.Headers |> tryGetHeader "X-Signature-Ed25519" >>? ""
                    let timestamp = req.Headers |> tryGetHeader "X-Signature-Timestamp" >>? ""

                    return! mediator.Send (VerifyEd25519Query(applicationId :?> string, timestamp, body, signature))
            }

            match success with
            | false -> ctx.GetInvocationResult().Value <- req.CreateResponse HttpStatusCode.Unauthorized
            | true -> do! next.Invoke(ctx)
        }
