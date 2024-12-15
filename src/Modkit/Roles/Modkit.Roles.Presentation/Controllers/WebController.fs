namespace Modkit.Roles.Presentation.Controllers

open System
open System.IO
open System.Net
open System.Security.Cryptography
open System.Text

open MediatR
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Options

open Modkit.Roles.Application.Options
open Modkit.Roles.Application.Commands
open Modkit.Roles.Application.Queries

open Modkit.Roles.Presentation.Common
open Modkit.Roles.Presentation.Modules

type WebController (
    mediator: ISender,
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
        let hostAuthority = req.Url.GetLeftPart UriPartial.Authority
        let! data = mediator.Send(GetLinkedRoleRedirectQuery(applicationId, hostAuthority))

        match data with
        | Error GetLinkedRoleRedirectQueryError.UnknownApplication ->
            return! req.CreateResponse HttpStatusCode.NotFound
            |> withJson {| message = "Application does not exist" |}

        | Ok data ->
            let cookie = new HttpCookie(CLIENT_STATE_COOKIE_NAME, hashCookie data.State)
            cookie.Expires <- DateTime.UtcNow.AddMinutes(5)
            cookie.Secure <- true

            let res = req.CreateResponse(HttpStatusCode.Redirect)
            res.Headers.Add("Location", data.Location)
            res.Headers.Add("Set-Cookie", cookie.ToString())
            return res
    }

    [<Function "OAuthCallback">]
    member _.OAuthCallback (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", "applications/{applicationId}/oauth-callback")>] req: HttpRequestData,
        applicationId: string
    ) = task {
        let hostAuthority = req.Url.GetLeftPart UriPartial.Authority

        let code = req.Query.GetValues("code") |> Seq.tryHead
        let state = req.Query.GetValues("state") |> Seq.tryHead
        let cookie =
            req.Cookies
            |> Seq.cast<IHttpCookie>
            |> Seq.tryFind (fun c -> c.Name = CLIENT_STATE_COOKIE_NAME)
            |> Option.map (fun c -> c :?> HttpCookie)

        let hash = Option.map hashCookie state

        match code, state, cookie with
        | None, _, _ ->
            return! req.CreateResponse HttpStatusCode.BadRequest
            |> withJson {| message = "Missing code" |}

        | _, None, _ ->
            return! req.CreateResponse HttpStatusCode.BadRequest
            |> withJson {| message = "Missing state" |}

        | _, _, None ->
            return! req.CreateResponse HttpStatusCode.BadRequest
            |> withJson {| message = "Missing cookie" |}

        | code, _, _ when code <> hash ->
            return! req.CreateResponse HttpStatusCode.BadRequest
            |> withJson {| message = "State does not match" |}
            
        | Some code, _, _ ->
            let! res = mediator.Send(CreateUserCommand(code, applicationId, hostAuthority))
            match res with
            | Error CreateUserCommandError.UnknownApplication ->
                return! req.CreateResponse HttpStatusCode.NotFound
                |> withJson {| message = "Application does not exist" |}

            | Error CreateUserCommandError.OAuthTokenGenerationFailed ->
                return! req.CreateResponse HttpStatusCode.FailedDependency
                |> withJson {| message = "Failed to generative token from provided code" |}

            | Error CreateUserCommandError.DiscordUserFetchFailed ->
                return! req.CreateResponse HttpStatusCode.FailedDependency
                |> withJson {| message = "Failed to fetch user from Discord" |}

            | Error CreateUserCommandError.DatabaseUpdateFailed ->
                return! req.CreateResponse HttpStatusCode.InternalServerError
                |> withJson {| message = "Unexpectedly failed to save user internally" |}

            | Ok _ ->
                return! req.CreateResponse HttpStatusCode.OK
                |> withText "Successfully linked, you can now return to Discord."

                // TODO: Redirect to some success page or respond with a nice page directly  
    }
