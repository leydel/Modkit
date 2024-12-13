namespace Modkit.Roles.Application.Commands

open System.Net.Http

open Discordfs.Rest
open Discordfs.Rest.Modules
open MediatR

open Modkit.Roles.Domain.Entities

open Modkit.Roles.Application.Repositories

type CreateApplicationCommandError =
    | InvalidToken
    | DiscordAppUpdateFailed
    | DatabaseUpdateFailed

type CreateApplicationCommandResponse = Result<Application, CreateApplicationCommandError>

type CreateApplicationCommand (
    token: string,
    hostAuthority: string
) =
    interface IRequest<CreateApplicationCommandResponse>

    member val Token = token with get, set
    member val HostAuthority = hostAuthority with get, set

type CreateApplicationCommandHandler (
    applicationRepository: IApplicationRepository,
    httpClientFactory: IHttpClientFactory
) =
    interface IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResponse> with
        member _.Handle (req, ct) = task {        
            let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient req.Token

            let! currentApplication = client |> Rest.getCurrentApplication
            match currentApplication with
            | Error _ -> return Error InvalidToken
            | Ok { Data = app } ->
                let! appResult = applicationRepository.Put app.Id req.Token app.VerifyKey
                match appResult with
                | Error _ -> return Error DatabaseUpdateFailed
                | Ok application ->
                    let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient req.Token

                    let editCurrentApplicationPayload = EditCurrentApplicationPayload(
                        description = "A custom bot built with Modkit Roles! https://modkit.org/linked-roles",
                        role_connection_verification_url = req.HostAuthority + $"/applications/{app.Id}/linked-role",
                        interactions_endpoint_url = req.HostAuthority + $"/applications/{app.Id}/interactions"
                    )

                    let! res = client |> Rest.editCurrentApplication editCurrentApplicationPayload
                    return res |> function | Error _ -> Error DiscordAppUpdateFailed | Ok _ -> Ok application

            // TODO: Rewrite into ROP
        }
