namespace Modkit.Roles.Presentation.Controllers

open System.Net

open Discordfs.Types
open Discordfs.Webhook.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http

open Modkit.Roles.Application.Services

open Modkit.Roles.Presentation.Modules
open Modkit.Roles.Presentation.Middleware

type InteractionController (interactionService: IInteractionService) =
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

        | InteractionReceiveEvent.APPLICATION_COMMAND interaction ->
            do! interactionService.HandleApplicationCommand interaction
            return req.CreateResponse HttpStatusCode.Accepted

        | InteractionReceiveEvent.APPLICATION_COMMAND_AUTOCOMPLETE interaction ->
            do! interactionService.HandleApplicationCommandAutocomplete interaction
            return req.CreateResponse HttpStatusCode.Accepted

        | InteractionReceiveEvent.MESSAGE_COMPONENT interaction ->
            do! interactionService.HandleMessageComponent interaction
            return req.CreateResponse HttpStatusCode.Accepted

        | InteractionReceiveEvent.MODAL_SUBMIT interaction ->
            do! interactionService.HandleModalSubmit interaction
            return req.CreateResponse HttpStatusCode.Accepted
    }
