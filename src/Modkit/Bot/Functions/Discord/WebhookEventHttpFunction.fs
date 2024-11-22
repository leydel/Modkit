namespace Modkit.Bot.Functions

open Discordfs.Webhook.Modules
open Discordfs.Webhook.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.DurableTask.Client
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Bot.Configuration
open System.Net
open System.Threading.Tasks

type WebhookEventHttpFunction (
    logger: ILogger<WebhookEventHttpFunction>
) =
    [<Function(nameof WebhookEventHttpFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", "events")>] req: HttpRequestData,
        [<FromBody>] webhookEvent: WebhookEvent,
        [<DurableClient>] orchestrationClient: DurableTaskClient,
        ctx: FunctionContext,
        options: IOptions<DiscordOptions>
    ) = task {
        let! json = req.ReadAsStringAsync()
        let publicKey = options.Value.PublicKey
        let signature = req.Headers.TryGetValues "X-Signature-Ed25519" |> fun (_, s) -> Seq.tryHead s >>? ""
        let timestamp = req.Headers.TryGetValues "X-Signature-Timestamp" |> fun (_, s) -> Seq.tryHead s >>? ""

        match Ed25519.verify timestamp json signature publicKey with
        | false ->
            logger.LogInformation $"Failed to verify ed25519"
            return req.CreateResponse HttpStatusCode.Unauthorized

        | true ->
            let run (name: string) (event: 'a) =
                logger.LogInformation("Processing webhook event with {OrchestratorName}", name)
                orchestrationClient.ScheduleNewOrchestrationInstanceAsync(name, input = event) :> Task

            match webhookEvent with
            | WebhookEvent.PING _ ->
                logger.LogInformation "Responding to ping webhook event"

            | WebhookEvent.APPLICATION_AUTHORIZED { Event = event } ->
                do! run (nameof ApplicationAuthorizedWebhookEventOrchestratorFunction) event.Data

            | WebhookEvent.ENTITLEMENT_CREATE { Event = event } ->
                do! run (nameof EntitlementCreateWebhookEventOrchestratorFunction) event.Data

            return req.CreateResponse HttpStatusCode.NoContent
    }
