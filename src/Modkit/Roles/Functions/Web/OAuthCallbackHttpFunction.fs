namespace Modkit.Roles.Functions

open Discordfs.Rest
open Discordfs.Rest.Modules
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Roles.Common
open Modkit.Roles.Configuration
open Modkit.Roles.Modules
open System.Net
open System.Net.Http

type OAuthCallbackHttpFunction (
    logger: ILogger<OAuthCallbackHttpFunction>,
    httpClientFactory: IHttpClientFactory,
    options: IOptions<CryptoOptions>
) =
    [<Function(nameof OAuthCallbackHttpFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", "applications/{applicationId}/oauth-callback")>] req: HttpRequestData,
        applicationId: string
    ) = task {
        // TODO: Check if applicationId is configured with platform

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
            res.WriteAsJsonAsync({| message = "Missing state" |}) |> ignore
            return res

        | Some cookie, Some hash when cookie.Value <> hash ->
            logger.LogInformation("Failed oauth callback because hashed state and cookie from request did not match")

            let res = req.CreateResponse(HttpStatusCode.BadRequest)
            res.WriteAsJsonAsync({| message = "Invalid state" |}) |> ignore
            return res

        | _ ->
            // TODO: Implement oauth2 endpoints into Discordfs.Rest (/oauth2/token for here, uses `code`)
            let accessToken = "TEMPORARY UNTIL ABOVE TODO IS COMPLETED"

            let client = httpClientFactory.CreateClient() |> HttpClient.toOAuthClient accessToken

            let! res = DiscordClient.OAuth client |> Rest.getCurrentUser
            match res with
            | Error _ ->
                logger.LogError("Failed oauth callback because fetching user with access token unexpectedly failed")

                let res = req.CreateResponse(HttpStatusCode.BadRequest)
                res.WriteAsJsonAsync({| message = "Failed to get user" |}) |> ignore
                return res

            | Ok { Data = user } ->
                // TODO: Save user to database (and store access/refresh tokens for updating connection metadata)
                
                logger.LogInformation("Successfully saved oauth token for user {UserId}", user.Id)
                
                let res = req.CreateResponse(HttpStatusCode.OK)
                res.WriteString("Successfully linked, you can now return to Discord.") |> ignore
                return res

                // TODO: Redirect to some success page or respond with a nice page directly
    }
