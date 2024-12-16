namespace Modkit.Roles.Application.Commands

open System.Net.Http

open Discordfs
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
    clientSecret: string,
    hostAuthority: string
) =
    interface IRequest<CreateApplicationCommandResponse>

    member val Token = token with get, set
    member val ClientSecret = clientSecret with get, set
    member val HostAuthority = hostAuthority with get, set

type CreateApplicationCommandHandler (
    applicationRepository: IApplicationRepository,
    httpClientFactory: IHttpClientFactory
) =
    interface IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResponse> with
        member _.Handle (req, ct) = task {        
            let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient req.Token

            // TODO: Add test to ensure client secret is valid (how? client_credentials grant maybe?)

            let! currentApplication = client |> Bot.getApplication
            match currentApplication with
            | None -> return Error InvalidToken
            | Some app ->
                let! appResult = applicationRepository.Put app.Id req.Token app.VerifyKey req.ClientSecret
                match appResult with
                | Error _ -> return Error DatabaseUpdateFailed
                | Ok application ->
                    let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient req.Token

                    let! res = client |> Bot.editApplication [
                        Description "A custom bot built with Modkit Roles! https://modkit.org/linked-roles"
                        RoleConnectionVerificationUrl $"{req.HostAuthority}/applications/{app.Id}/linked-role"
                        InteractionsEndpointUrl $"{req.HostAuthority}/applications/{app.Id}/interactions"
                    ]

                    return res |> function | None -> Error DiscordAppUpdateFailed | Some _ -> Ok application

            // TODO: Rewrite into ROP
        }
