namespace Modkit.Roles.Presentation.Controllers

open System.Net

open Discordfs.Types
open Discordfs.Webhook.Modules
open Discordfs.Webhook.Types
open MediatR
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http

open Modkit.Roles.Domain.Entities
open Modkit.Roles.Application.Queries

type InteractionController (
    mediator: IMediator
) =
    [<Function "PostInteraction">]
    member _.PostInteraction (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", "applications/{applicationId}/interactions")>] req: HttpRequestData,
        [<FromBody>] event: InteractionReceiveEvent,
        applicationId: string
    ) = task {
        let! application = mediator.Send(GetApplicationFromIdQuery applicationId)
        match application with
        | Error _ ->
            let res = req.CreateResponse(HttpStatusCode.NotFound)
            do! res.WriteAsJsonAsync({| message = "Application does not exist" |})
            return res

        | Ok app ->
            let tryGetHeader name (headers: HttpHeadersCollection) =
                match headers.Contains name with
                | true -> headers.GetValues name |> Seq.tryHead
                | false -> None

            let! body = req.ReadAsStringAsync()
            let signature = req.Headers |> tryGetHeader "X-Signature-Ed25519" >>? ""
            let timestamp = req.Headers |> tryGetHeader "X-Signature-Timestamp" >>? ""

            match Ed25519.verify body signature timestamp app.PublicKey with
            | false ->
                return req.CreateResponse HttpStatusCode.Unauthorized

            | true ->
                match event with
                | InteractionReceiveEvent.PING _ ->
                    let res = req.CreateResponse HttpStatusCode.OK
                    do! res.WriteAsJsonAsync { Type = InteractionCallbackType.PONG; Data = None }
                    return res

                // TODO: Handle interaction events as required (through mediator)

                | _ ->
                    return req.CreateResponse HttpStatusCode.InternalServerError
    }
