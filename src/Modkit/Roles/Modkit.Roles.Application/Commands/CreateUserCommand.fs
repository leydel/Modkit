namespace Modkit.Roles.Application.Commands

open System
open System.Collections.Generic
open System.Net.Http

open Discordfs.Rest
open Discordfs.Rest.Modules
open MediatR

open Modkit.Roles.Domain.Entities

open Modkit.Roles.Application.Repositories

type CreateUserCommandError =
    | UnknownApplication
    | OAuthTokenGenerationFailed
    | DiscordUserFetchFailed
    | DatabaseUpdateFailed

type CreateUserCommandResponse = Result<User, CreateUserCommandError>

type CreateUserCommand (
    code: string,
    applicationId: string,
    hostAuthority: string
) =
    interface IRequest<CreateUserCommandResponse>

    member val Code = code with get, set
    member val ApplicationId = applicationId with get, set
    member val HostAuthority = hostAuthority with get, set

type CreateUserCommandHandler (
    applicationRepository: IApplicationRepository,
    userRepository: IUserRepository,
    httpClientFactory: IHttpClientFactory
) =
    interface IRequestHandler<CreateUserCommand, CreateUserCommandResponse> with
        member _.Handle (req, ct) = task {
            let! application = applicationRepository.Get req.ApplicationId
            match application with
            | Error _ -> return Error UnknownApplication
            | Ok app ->
                let basicClient = httpClientFactory.CreateClient() |> HttpClient.toBasicClient app.Id app.ClientSecret
                let authorizationCodeGrantPayload = AuthorizationCodeGrantPayload(req.Code, req.HostAuthority + $"/applications/{app.Id}/linked-role")
                let! tokenResponse = basicClient |> Rest.authorizationCodeGrant authorizationCodeGrantPayload

                match tokenResponse with
                | Error _ -> return Error OAuthTokenGenerationFailed
                | Ok { Data = token } ->
                    let client = httpClientFactory.CreateClient() |> HttpClient.toOAuthClient token.AccessToken
                    let! res = DiscordClient.OAuth client |> Rest.getCurrentUser

                    match res with
                    | Error _ -> return Error DiscordUserFetchFailed
                    | Ok { Data = discordUser } ->
                        let metadata = Dictionary<string, int>() // TODO: Figure out how metadata should be handled (change feed trigger too?)

                        // TODO: Figure out correct approach for handling existing users who reauthorize

                        let accessTokenExpiry = DateTime.UtcNow.AddSeconds token.ExpiresIn
                        let! user = userRepository.Put discordUser.Id app.Id token.AccessToken accessTokenExpiry token.RefreshToken metadata
                        return user |> Result.mapError (fun _ -> DatabaseUpdateFailed)
        }
