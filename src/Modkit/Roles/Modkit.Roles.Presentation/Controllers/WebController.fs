namespace Modkit.Roles.Presentation.Controllers

open System
open System.Collections.Specialized
open System.IO
open System.Net
open System.Security.Cryptography
open System.Text

open MediatR
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Options

open Modkit.Roles.Domain.Entities
open Modkit.Roles.Application.Options
open Modkit.Roles.Application.Commands
open Modkit.Roles.Application.Queries

open Modkit.Roles.Presentation.Common

type WebController (
    mediator: IMediator,
    options: IOptions<CryptoOptions>
) =
    let hashCookie (state: string) =
        use stream = new MemoryStream(Encoding.UTF8.GetBytes state)
        use hmac = new HMACSHA256(Encoding.UTF8.GetBytes options.Value.CookieKey)
        Convert.ToHexString(hmac.ComputeHash stream)

    [<Function "LinkedRole">]
    member _.LinkedRole (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", "applications/{applicationId}/linked-role")>] req: HttpRequestData,
        applicationId: string
    ) = task {
        let! application = mediator.Send(GetApplicationFromIdQuery applicationId)
        match application with
        | Error _ ->
            let res = req.CreateResponse(HttpStatusCode.NotFound)
            do! res.WriteAsJsonAsync({| message = "Application does not exist" |})
            return res

        | Ok app ->
            let state = RandomNumberGenerator.GetInt32(Int32.MaxValue).ToString()
            let location =
                let builder = new UriBuilder("https://discord.com/api/oauth2/authorize")

                let query = NameValueCollection()
                query.Add("client_id", app.Id)
                query.Add("redirect_uri", "https://modkit.io/oauth-callback")
                query.Add("response_type", "code")
                query.Add("state", state)
                query.Add("scope", "identify role_connections.write")
                query.Add("prompt", "consent")

                builder.Query <- query.ToString()
                builder.Uri.ToString()

                // TODO: Move this into Discordfs along with implementing other oauth endpoints

            let cookie = new HttpCookie(CLIENT_STATE_COOKIE_NAME, hashCookie state)
            cookie.Expires <- DateTime.UtcNow.AddMinutes(5)
            cookie.Secure <- true

            let res = req.CreateResponse(HttpStatusCode.Redirect)
            res.Headers.Add("Location", location)
            res.Headers.Add("Set-Cookie", cookie.ToString())
            return res

        // TODO: Rewrite with ROP
    }

    [<Function "OAuthCallback">]
    member _.OAuthCallback (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", "applications/{applicationId}/oauth-callback")>] req: HttpRequestData,
        applicationId: string
    ) = task {
        let! application = mediator.Send(GetApplicationFromIdQuery applicationId)
        match application with
        | Error _ ->
            let res = req.CreateResponse HttpStatusCode.NotFound
            do! res.WriteAsJsonAsync {| message = "Application does not exist" |}
            return res

        | Ok app ->
            let code = req.Query.GetValues("code") |> Seq.tryHead
            let state = req.Query.GetValues("state") |> Seq.tryHead
            let cookie =
                req.Cookies
                |> Seq.cast<IHttpCookie>
                |> Seq.tryFind (fun c -> c.Name = CLIENT_STATE_COOKIE_NAME)
                |> Option.map (fun c -> c :?> HttpCookie)

            let hash = Option.map hashCookie state

            match cookie, hash, state with
            | None, _, _
            | _, None, _ ->
                let res = req.CreateResponse HttpStatusCode.BadRequest
                do! res.WriteAsJsonAsync {| message = "Missing state" |}
                return res

            | Some cookie, Some hash, _ when cookie.Value <> hash ->
                let res = req.CreateResponse HttpStatusCode.BadRequest
                do! res.WriteAsJsonAsync {| message = "Invalid state" |}
                return res

            | _, _, None ->
                let res = req.CreateResponse HttpStatusCode.BadRequest
                do! res.WriteAsJsonAsync {| message = "Missing code" |}
                return res

            | _, _, Some code ->
                let! user = mediator.Send(CreateUserCommand(code, app.Id))
                match user with
                | Error CreateUserCommandError.OAuthTokenGenerationFailed ->
                    let res = req.CreateResponse HttpStatusCode.BadRequest
                    do! res.WriteAsJsonAsync {| message = "Failed to get token from provided 'code'" |}
                    return res

                | Error CreateUserCommandError.DiscordUserFetchFailed ->
                    let res = req.CreateResponse HttpStatusCode.InternalServerError
                    do! res.WriteAsJsonAsync {| message = "Unexpectedly failed to fetch user from Discord" |}
                    return res

                | Error CreateUserCommandError.DatabaseUpdateFailed ->
                    let res = req.CreateResponse HttpStatusCode.InternalServerError
                    do! res.WriteAsJsonAsync {| message = "Unexpectedly failed to save user" |}
                    return res

                | Ok _ ->
                    let res = req.CreateResponse HttpStatusCode.OK
                    do! res.WriteStringAsync "Successfully linked, you can now return to Discord."
                    return res

                    // TODO: Redirect to some success page or respond with a nice page directly

        // TODO: Rewrite with ROP
    }
