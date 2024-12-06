namespace Modkit.Roles.Functions

open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Roles.Common
open Modkit.Roles.Configuration
open Modkit.Roles.Modules
open Modkit.Roles.Types
open System
open System.Net

type LinkedRoleHttpFunction (
    logger: ILogger<LinkedRoleHttpFunction>,
    options: IOptions<CryptoOptions>
) =
    [<Function(nameof LinkedRoleHttpFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", "applications/{applicationId}/linked-role")>] req: HttpRequestData,
        [<CosmosDBInput(containerName = ROLE_APP_CONTAINER_NAME, databaseName = ROLE_APP_DATABASE_NAME, Id = "{applicationId}", PartitionKey = "{applicationId}")>] app: RoleApp option,
        applicationId: string
    ) = task {
        match app with
        | None ->
            logger.LogInformation("Attempted to redirect to oauth for app {ApplicationId} which ahsn't been configured", applicationId)

            let res = req.CreateResponse(HttpStatusCode.NotFound)
            do! res.WriteAsJsonAsync({| message = "Application does not exist" |})
            return res

        | Some app ->
            let state = Crypto.generateRandomString()
            let location = Discord.buildOAuthUrl app.Id state

            let hash = Crypto.hash options.Value.CookieKey state

            let cookie = new HttpCookie(CLIENT_STATE_COOKIE_NAME, hash)
            cookie.Expires <- DateTime.UtcNow.AddMinutes(5)
            cookie.Secure <- true

            logger.LogInformation("Redirecting to oauth2 to request access")

            let res = req.CreateResponse(HttpStatusCode.Redirect)
            res.Headers.Add("Location", location)
            res.Headers.Add("Set-Cookie", cookie.ToString())
            return res
    }
