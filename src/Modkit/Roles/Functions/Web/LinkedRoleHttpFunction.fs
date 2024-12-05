namespace Modkit.Roles.Functions

open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Roles.Common
open Modkit.Roles.Configuration
open Modkit.Roles.Modules
open System
open System.Net

type LinkedRoleHttpFunction (
    logger: ILogger<LinkedRoleHttpFunction>,
    options: IOptions<CryptoOptions>
) =
    [<Function(nameof LinkedRoleHttpFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", "applications/{applicationId}/linked-role")>] req: HttpRequestData,
        applicationId: string
    ) = task {
        // TODO: Check if applicationId is configured with platform

        let state = Crypto.generateRandomString()
        let location = Discord.buildOAuthUrl applicationId state

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
