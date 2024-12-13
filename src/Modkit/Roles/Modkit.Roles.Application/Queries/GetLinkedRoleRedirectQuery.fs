namespace Modkit.Roles.Application.Queries

open System
open System.Collections.Specialized
open System.Security.Cryptography

open MediatR
open Microsoft.Extensions.Options

open Modkit.Roles.Domain.Entities

open Modkit.Roles.Application.Options
open Modkit.Roles.Application.Repositories

type GetLinkedRoleRedirectQueryError =
    | UnknownApplication

type GetLinkedRoleRedirectQueryData = {
    State: string
    Location: string
}

type GetLinkedRoleRedirectQueryResponse = Result<GetLinkedRoleRedirectQueryData, GetLinkedRoleRedirectQueryError>

type GetLinkedRoleRedirectQuery (applicationId: string, hostAuthority: string) =
    interface IRequest<GetLinkedRoleRedirectQueryResponse>

    member val ApplicationId = applicationId with get, set
    member val HostAuthority = hostAuthority with get, set

type GetLinkedRoleRedirectQueryHandler (
    applicationRepository: IApplicationRepository,
    options: IOptions<CryptoOptions>
) =
    interface IRequestHandler<GetLinkedRoleRedirectQuery, GetLinkedRoleRedirectQueryResponse> with
        member _.Handle (req, ct) = task {
            let! application = applicationRepository.Get req.ApplicationId
            match application with
            | Error _ -> return Error UnknownApplication
            | Ok app ->
                let state = RandomNumberGenerator.GetInt32(Int32.MaxValue).ToString()
                let location =
                    let builder = new UriBuilder("https://discord.com/api/oauth2/authorize")

                    let query = NameValueCollection()
                    query.Add("client_id", app.Id)
                    query.Add("redirect_uri", req.HostAuthority + $"/applications/{app.Id}/oauth-callback")
                    query.Add("response_type", "code")
                    query.Add("state", state)
                    query.Add("scope", "identify role_connections.write")
                    query.Add("prompt", "consent")

                    builder.Query <- query.ToString()
                    builder.Uri.ToString()

                    // TODO: Move this into Discordfs along with implementing other oauth endpoints

                return Ok {
                    State = state
                    Location = location
                }
        }
