namespace Modkit.Roles.Application.Queries

open MediatR

open Modkit.Roles.Domain.Entities

open Modkit.Roles.Application.Repositories

type GetApplicationFromIdQueryResponse = Result<Application, unit>

type GetApplicationFromIdQuery (applicationId: string) =
    interface IRequest<GetApplicationFromIdQueryResponse>

    member val ApplicationId = applicationId with get, set

type GetApplicationFromIdQueryHandler (
    applicationRepository: IApplicationRepository
) =
    interface IRequestHandler<GetApplicationFromIdQuery, GetApplicationFromIdQueryResponse> with
        member _.Handle (req, ct) = task {
            return! applicationRepository.Get req.ApplicationId ?> Result.mapError (fun _ -> ())
        }
