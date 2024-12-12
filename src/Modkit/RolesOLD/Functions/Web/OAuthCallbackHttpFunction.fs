namespace Modkit.Roles.Functions

open Discordfs.Rest
open Discordfs.Rest.Modules
open Microsoft.Azure.Cosmos
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Roles.Common
open Modkit.Roles.Configuration
open Modkit.Roles.Modules
open Modkit.Roles.Types
open System
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Threading.Tasks

type OAuthCallbackHttpFunction (
    logger: ILogger<OAuthCallbackHttpFunction>,
    httpClientFactory: IHttpClientFactory,
    options: IOptions<CryptoOptions>
) =
    [<Function(nameof OAuthCallbackHttpFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", "applications/{applicationId}/oauth-callback")>] req: HttpRequestData,
        [<CosmosDBInput(containerName = ROLE_APP_CONTAINER_NAME, databaseName = DATABASE_NAME, Id = "{applicationId}", PartitionKey = "{applicationId}")>] app: RoleApp option,
        [<CosmosDBInput(containerName = APP_USER_CONTAINER_NAME, databaseName = DATABASE_NAME)>] container: Container,
        applicationId: string
    ) = task {
        match app with
        | None ->
            logger.LogInformation("Attempted oauth callback on non-configured application {ApplicationId}", applicationId)

            let res = req.CreateResponse(HttpStatusCode.NotFound)
            do! res.WriteAsJsonAsync({| message = "Application does not exist" |})
            return res

        | Some app ->
            let code = req.Query.GetValues("code") |> Seq.tryHead
            let state = req.Query.GetValues("state") |> Seq.tryHead
            let cookie =
                req.Cookies
                |> Seq.cast<IHttpCookie>
                |> Seq.tryFind (fun c -> c.Name = CLIENT_STATE_COOKIE_NAME)
                |> Option.map (fun c -> c :?> HttpCookie)

            let hash = state >>. Crypto.hash options.Value.CookieKey

            match cookie, hash with
            | None, _
            | _, None ->
                logger.LogInformation("Failed oauth callback because cookie or hash were missing from request")

                let res = req.CreateResponse(HttpStatusCode.BadRequest)
                do! res.WriteAsJsonAsync({| message = "Missing state" |})
                return res

            | Some cookie, Some hash when cookie.Value <> hash ->
                logger.LogInformation("Failed oauth callback because hashed state and cookie from request did not match")

                let res = req.CreateResponse(HttpStatusCode.BadRequest)
                do! res.WriteAsJsonAsync({| message = "Invalid state" |})
                return res

            | _ ->
                // TODO: Implement oauth2 endpoints into Discordfs.Rest (/oauth2/token for here, uses `code`)
                let accessToken = "TEMPORARY UNTIL ABOVE TODO IS COMPLETED"
                let accessTokenExpiry = DateTime.UtcNow
                let refreshToken = "TEMPORARY UNTIL ABOVE TODO IS COMPLETED"

                let client = httpClientFactory.CreateClient() |> HttpClient.toOAuthClient accessToken

                let! res = DiscordClient.OAuth client |> Rest.getCurrentUser
                match res with
                | Error _ ->
                    logger.LogError("Failed oauth callback because fetching user with access token unexpectedly failed")

                    let res = req.CreateResponse(HttpStatusCode.BadRequest)
                    do! res.WriteAsJsonAsync({| message = "Failed to get user" |})
                    return res

                | Ok { Data = user } ->
                    try
                        let appUser: AppUser = {
                            Id = user.Id
                            ApplicationId = app.Id
                            AccessToken = accessToken
                            AccessTokenExpiry = accessTokenExpiry
                            RefreshToken = refreshToken
                            Metadata = Dictionary<string, int>() // TODO: Figure out how metadata should be handled (change feed trigger too?)
                        }

                        do! container.UpsertItemAsync(appUser, PartitionKey app.Id) :> Task

                        // TODO: Figure out correct approach for handling existing users who reauthorize
                
                        logger.LogInformation("Successfully saved oauth token for user {UserId} on application {ApplicationId}", user.Id, app.Id)
                
                        let res = req.CreateResponse(HttpStatusCode.OK)
                        do! res.WriteStringAsync("Successfully linked, you can now return to Discord.")
                        return res

                        // TODO: Redirect to some success page or respond with a nice page directly

                    with | ex ->
                        logger.LogError(ex, "Failed to upsert user {UserId} to application {ApplicationId}", user.Id, app.Id)

                        let res = req.CreateResponse(HttpStatusCode.InternalServerError)
                        do! res.WriteAsJsonAsync({| message = "Unexpectedly failed to save user" |})
                        return res
    }
