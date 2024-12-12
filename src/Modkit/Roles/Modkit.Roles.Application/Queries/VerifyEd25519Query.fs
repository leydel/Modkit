namespace Modkit.Roles.Application.Queries

open Discordfs.Webhook.Modules
open MediatR

open Modkit.Roles.Application.Repositories

type VerifyEd25519Query (
    applicationId: string,
    timestamp: string,
    body: string,
    signature: string
) =
    interface IRequest<bool>

    member val ApplicationId = applicationId with get, set
    member val Timestamp = timestamp with get, set
    member val Body = body with get, set
    member val Signature = signature with get, set

type VerifyEd25519QueryHandler (
    applicationRepository: IApplicationRepository
) =
    interface IRequestHandler<VerifyEd25519Query, bool> with
        member _.Handle (req, ct) = task {
            let! app = applicationRepository.Get req.ApplicationId

            return
                match app with
                | Error _ -> false
                | Ok app -> Ed25519.verify req.Timestamp req.Body req.Signature app.PublicKey
        }
