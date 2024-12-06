namespace Modkit.Roles.Functions

open Discordfs.Types
open Discordfs.Webhook.Modules
open Discordfs.Webhook.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Modkit.Roles.Common
open Modkit.Roles.Types
open System.Net

type InteractionHttpFunction (
    logger: ILogger<InteractionHttpFunction>
) =
    [<Function(nameof InteractionHttpFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", "applications/{applicationId}/interactions")>] req: HttpRequestData,
        [<CosmosDBInput(containerName = ROLE_APP_CONTAINER_NAME, databaseName = DATABASE_NAME, Id = "{applicationId}", PartitionKey = "{applicationId}")>] app: RoleApp option,
        [<FromBody>] event: InteractionReceiveEvent,
        applicationId: string
    ) = task {
        match app with
        | None ->
            logger.LogInformation("Failed to find application {ApplicationId}", applicationId)

            let res = req.CreateResponse HttpStatusCode.NotFound
            do! res.WriteAsJsonAsync {| message = "Application not found" |}
            return res

        | Some app ->
            let tryGetHeader name (headers: HttpHeadersCollection) =
                match headers.Contains name with
                | true -> headers.GetValues name |> Seq.tryHead
                | false -> None

            let! body = req.ReadAsStringAsync()
            let signature = req.Headers |> tryGetHeader "X-Signature-Ed25519" >>? ""
            let timestamp = req.Headers |> tryGetHeader "X-Signature-Timestamp" >>? ""

            match Ed25519.verify body signature timestamp app.PublicKey with
            | false ->
                logger.LogInformation("Failed to verify ed25519 for application {ApplicationId}", applicationId)
                return req.CreateResponse HttpStatusCode.Unauthorized

            | true ->
                match event with
                | InteractionReceiveEvent.PING _ ->
                    logger.LogInformation("Responding to ping interaction for application {ApplicationId}", applicationId)
                    
                    let res = req.CreateResponse HttpStatusCode.OK
                    do! res.WriteAsJsonAsync { Type = InteractionCallbackType.PONG; Data = None }
                    return res

                // TODO: Handle interaction events as required

                | _ ->
                    logger.LogError("Unexpected interaction event received for application {ApplicationId}", applicationId)
                    return req.CreateResponse HttpStatusCode.InternalServerError
    }
