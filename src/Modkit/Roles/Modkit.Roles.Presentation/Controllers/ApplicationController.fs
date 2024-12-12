namespace Modkit.Roles.Presentation.Controllers

open System
open System.Net
open System.Text.Json.Serialization

open MediatR
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http

open Modkit.Roles.Application.Commands
open Modkit.Roles.Application.Queries

type PostApplicationPayload = {
    [<JsonPropertyName "token">] Token: string
}

type ApplicationController (
    mediator: IMediator
) =
    [<Function "PostApplication">]
    member _.PostApplication (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "applications")>] req: HttpRequestData,
        [<FromBody>] payload: PostApplicationPayload
    ) = task {
        let hostAuthority = req.Url.GetLeftPart(UriPartial.Authority)

        let! appResult = mediator.Send(GetApplicationFromTokenQuery(payload.Token))
        match appResult with
        | Error _ ->
            let res = req.CreateResponse HttpStatusCode.BadRequest
            do! res.WriteAsJsonAsync {| message = "Invalid token provided" |}
            return res

        | Ok app ->
            let! createResult = mediator.Send(CreateApplicationCommand(app.Id, payload.Token, app.VerifyKey, hostAuthority))
            match createResult with
            | Error _ ->
                let res = req.CreateResponse HttpStatusCode.InternalServerError
                do! res.WriteAsJsonAsync {| message = "Unexpectedly failed to setup application" |}
                return res

            | Ok app ->
                let res = req.CreateResponse HttpStatusCode.Created
                res.Headers.Add("Location", hostAuthority + $"/applications/{app.Id}")
                do! res.WriteAsJsonAsync {| id = app.Id |} // TODO: Create proper dto for this
                return res

        // TODO: Rewrite using ROP
    }
