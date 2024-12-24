namespace Modkit.Roles.Application.Commands

open System.Net.Http

open Discordfs
open Discordfs.Rest.Modules
open Discordfs.Types
open MediatR

open Modkit.Roles.Domain.Entities

open Modkit.Roles.Application.Common
open Modkit.Roles.Application.Repositories

type CreateApplicationCommandError =
    | InvalidToken
    | MissingRedirectUri of string
    | InvalidClientSecret
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
    let getCurrentApplication (token: string) = task {
        let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient token
        let! res = client |> Bot.getApplication

        match res with
        | None -> return Error CreateApplicationCommandError.InvalidToken
        | Some app -> return Ok app
    }

    let validateApplicationRedirectUri (redirectUris: string list) (redirectUri: string) =
        match List.exists (fun v -> v = redirectUri) redirectUris with
        | false -> Error (CreateApplicationCommandError.MissingRedirectUri redirectUri)
        | true -> Ok ()

    let validateClientSecret (applicationId: string) (clientSecret: string) = task {
        let client = httpClientFactory.CreateClient() |> HttpClient.toBasicClient applicationId clientSecret
        let! res = client |> OAuthFlow.clientCredentialsGrant [OAuth2Scope.IDENTIFY]

        match res with
        | None -> return Error InvalidClientSecret
        | Some _ -> return Ok ()
    }

    let updateDiscordApplication (app: Types.Application) (token: string) (hostAuthority: string) = task {
        let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient token
        let! res = client |> Bot.editApplication [
            Description "A custom bot built with Modkit Roles! https://modkit.org/linked-roles"
            RoleConnectionVerificationUrl $"{hostAuthority}/applications/{app.Id}/linked-role"
            InteractionsEndpointUrl $"{hostAuthority}/applications/{app.Id}/interactions"
            IntegrationTypesConfig [
                (ApplicationIntegrationType.GUILD_INSTALL, { Oauth2InstallParams = None })
                // TODO: Check if this install params is correct (maybe none means invalid and both need to be defined?)
            ]
            // TODO: Set avatar and banner? Rewrite description?
        ]

        match res with
        | None -> return Error CreateApplicationCommandError.DiscordAppUpdateFailed
        | Some _ -> return Ok ()
    }

    let uploadToDatabase (app: Types.Application) (token: string) (clientSecret: string) = task {
        let! application = applicationRepository.Put {
            Id = app.Id
            Token = token
            PublicKey = app.VerifyKey
            ClientSecret = clientSecret
            Metadata = []
        }

        return Result.mapError (fun _ -> DatabaseUpdateFailed) application
    }

    interface IRequestHandler<CreateApplicationCommand, CreateApplicationCommandResponse> with
        member _.Handle (req, ct) = task {
            return! railway {
                let! app = getCurrentApplication req.Token

                let redirectUris = (app.RedirectUris |> Option.defaultValue [])
                let linkedRoleUri = $"{req.HostAuthority}/applications/{app.Id}/linked-role"
                do! validateApplicationRedirectUri redirectUris linkedRoleUri

                do! validateClientSecret app.Id req.ClientSecret
                do! updateDiscordApplication app req.Token req.HostAuthority
                return! uploadToDatabase app req.Token req.ClientSecret
            } |> Railway.toTask
        }
