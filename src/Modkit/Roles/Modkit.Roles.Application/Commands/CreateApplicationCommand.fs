namespace Modkit.Roles.Application.Commands

open System.Net.Http

open Discordfs.Rest
open Discordfs.Rest.Modules
open MediatR

open Modkit.Roles.Domain.Entities

open Modkit.Roles.Application.Repositories

type CreateApplicationCommandError =
    | DiscordAppUpdateFailed
    | DatabaseUpdateFailed

type CreateApplicationCommandResponse = Result<Application, CreateApplicationCommandError>

type CreateApplicationCommand (
    applicationId: string,
    token: string,
    publicKey: string,
    hostAuthority: string
) =
    interface IRequest<CreateApplicationCommandResponse>

    member val ApplicationId = applicationId with get, set
    member val Token = token with get, set
    member val PublicKey = publicKey with get, set
    member val HostAuthority = hostAuthority with get, set

type CreateApplicationCommandHandler (
    applicationRepository: IApplicationRepository,
    httpClientFactory: IHttpClientFactory
) =
    interface IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResponse> with
        member _.Handle (req, ct) = task {
            let! appResult = applicationRepository.Put req.ApplicationId req.Token req.PublicKey
            match appResult with
            | Error _ -> return Error DatabaseUpdateFailed
            | Ok application ->
                let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient req.Token

                let editCurrentApplicationPayload = EditCurrentApplicationPayload(
                    description = "A custom bot built with Modkit Roles! https://modkit.org/linked-roles",
                    role_connection_verification_url = req.HostAuthority + $"/applications/{req.ApplicationId}/linked-role",
                    interactions_endpoint_url = req.HostAuthority + $"/applications/{req.ApplicationId}/interactions"
                )

                let! updatedApplication = client |> Rest.editCurrentApplication editCurrentApplicationPayload
                match updatedApplication with
                | Error _ -> return Error DiscordAppUpdateFailed
                | Ok _ -> return Ok application

            // TODO: Rewrite into ROP
        }
