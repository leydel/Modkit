namespace Modkit.Roles.Presentation.Controllers

open System
open System.Net
open System.Text.Json.Serialization

open MediatR
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http

open Modkit.Roles.Application.Commands

open Modkit.Roles.Presentation.Modules

type PostApplicationPayload = {
    [<JsonPropertyName "token">] Token: string
    [<JsonPropertyName "clientSecret">] ClientSecret: string
}

type ApplicationController (mediator: ISender) =
    [<Function "PostApplication">]
    member _.PostApplication (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "applications")>] req: HttpRequestData,
        [<FromBody>] payload: PostApplicationPayload
    ) = task {
        let hostAuthority = req.Url.GetLeftPart UriPartial.Authority
        let! createResult = mediator.Send(CreateApplicationCommand(payload.Token, payload.ClientSecret, hostAuthority))
        
        match createResult with
        | Error CreateApplicationCommandError.InvalidToken ->
            return! req.CreateResponse HttpStatusCode.BadRequest
            |> withJson {| message = "Invalid token provided" |}

        | Error CreateApplicationCommandError.DiscordAppUpdateFailed ->
            return! req.CreateResponse HttpStatusCode.FailedDependency
            |> withJson {| message = "Discord rejected application update" |}

        | Error CreateApplicationCommandError.DatabaseUpdateFailed ->
            return! req.CreateResponse HttpStatusCode.InternalServerError
            |> withJson {| message = "Unexpectedly failed to setup application internally" |}

        | Ok app ->
            return! req.CreateResponse HttpStatusCode.Created
            |> withHeader "Location" (hostAuthority + $"/applications/{app.Id}")
            |> withJson {| id = app.Id |} // TODO: Create proper dto for this
    }
