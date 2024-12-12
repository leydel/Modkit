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
    | OAuthTokenGenerationFailed
    | DiscordUserFetchFailed
    | DatabaseUpdateFailed

type CreateUserCommandResponse = Result<User, CreateUserCommandError>

type CreateUserCommand (
    code: string,
    applicationId: string
) =
    interface IRequest<CreateUserCommandResponse>

    member val Code = code with get, set
    member val ApplicationId = applicationId with get, set

type CreateUserCommandHandler (
    userRepository: IUserRepository,
    httpClientFactory: IHttpClientFactory
) =
    interface IRequestHandler<CreateUserCommand, CreateUserCommandResponse> with
        member _.Handle (req, ct) = task {
            // TODO: Implement oauth2 endpoints into Discordfs.Rest (/oauth2/token for here, uses `code`)
            let accessToken = "TEMPORARY UNTIL ABOVE TODO IS COMPLETED"
            let accessTokenExpiry = DateTime.UtcNow
            let refreshToken = "TEMPORARY UNTIL ABOVE TODO IS COMPLETED"

            // TODO: Should above be done in a separate command?

            let client = httpClientFactory.CreateClient() |> HttpClient.toOAuthClient accessToken

            let! res = DiscordClient.OAuth client |> Rest.getCurrentUser
            match res with
            | Error _ -> return Error DiscordUserFetchFailed
            | Ok { Data = discordUser } ->
                let metadata = Dictionary<string, int>() // TODO: Figure out how metadata should be handled (change feed trigger too?)

                // TODO: Figure out correct approach for handling existing users who reauthorize

                let! user = userRepository.Put discordUser.Id req.ApplicationId accessToken accessTokenExpiry refreshToken metadata
                match user with
                | Error _ -> return Error DatabaseUpdateFailed
                | Ok user -> return Ok user
        }
