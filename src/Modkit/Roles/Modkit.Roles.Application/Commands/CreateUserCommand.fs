namespace Modkit.Roles.Application.Commands

open System
open System.Collections.Generic
open System.Net.Http

open Discordfs
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

                let! tokenResponse = basicClient |> OAuthFlow.authorizationCodeGrant req.Code $"{req.HostAuthority}/applications/{app.Id}/linked-role"
                match tokenResponse with
                | None -> return Error OAuthTokenGenerationFailed
                | Some token ->
                    let client = httpClientFactory.CreateClient() |> HttpClient.toOAuthClient token.AccessToken

                    let! res = client |> OAuth.getUser
                    match res with
                    | None -> return Error DiscordUserFetchFailed
                    | Some discordUser ->
                        // TODO: Figure out correct approach for handling existing users who reauthorize

                        let accessTokenExpiry = DateTime.UtcNow.AddSeconds token.ExpiresIn
                        let! user = userRepository.Put {
                            Id = discordUser.Id
                            ApplicationId = app.Id
                            AccessToken = token.AccessToken
                            AccessTokenExpiry = accessTokenExpiry
                            RefreshToken = token.RefreshToken
                            Metadata = []
                        }

                        return user |> Result.mapError (fun _ -> DatabaseUpdateFailed)

                        // TODO: Should metadata be pushed to discord? I think it does
        }
