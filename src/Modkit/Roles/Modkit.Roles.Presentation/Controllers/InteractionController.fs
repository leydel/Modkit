namespace Modkit.Roles.Presentation.Controllers

open System.Net

open Discordfs.Types
open Discordfs.Webhook.Types
open MediatR
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http

open Modkit.Roles.Presentation.Modules
open Modkit.Roles.Presentation.Middleware

type InteractionController (mediator: ISender) =
    [<Function "PostInteraction">]
    [<VerifyEd25519>]
    member _.PostInteraction (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", "applications/{applicationId}/interactions")>] req: HttpRequestData,
        [<FromBody>] event: InteractionReceiveEvent,
        applicationId: string
    ) = task {
        match event with
        | InteractionReceiveEvent.PING _ ->
            return! req.CreateResponse HttpStatusCode.OK
            |> withJson { Type = InteractionCallbackType.PONG; Data = None }

        // TODO: Handle interaction events as required (through mediator)

        | _ ->
            return req.CreateResponse HttpStatusCode.InternalServerError
    }
