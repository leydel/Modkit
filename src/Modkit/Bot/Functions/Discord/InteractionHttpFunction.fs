namespace Modkit.Bot.Functions

open Azure.Storage.Queues
open Discordfs.Types
open Discordfs.Webhook.Modules
open Discordfs.Webhook.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Azure
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Api.Common
open Modkit.Bot.Common
open Modkit.Bot.Configuration
open System.Net
open System.Text.Json

type InteractionHttpFunction (queueServiceClientFactory: IAzureClientFactory<QueueServiceClient>) =
    let queueServiceClient = queueServiceClientFactory.CreateClient Constants.InteractionQueueClientName

    [<Function(nameof InteractionHttpFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", "interactions")>] req: HttpRequestData,
        ctx: FunctionContext,
        options: IOptions<DiscordOptions>
    ) = task {
        let! json = req.ReadAsStringAsync()
        let publicKey = options.Value.PublicKey
        let signature = req.Headers.TryGetValues "X-Signature-Ed25519" |> fun (_, s) -> Seq.tryHead s >>? ""
        let timestamp = req.Headers.TryGetValues "X-Signature-Timestamp" |> fun (_, s) -> Seq.tryHead s >>? ""

        match Ed25519.verify timestamp json signature publicKey with
        | true ->
            match Json.deserializeF<InteractionReceiveEvent> json with
            | InteractionReceiveEvent.PING _ ->
                ctx.GetLogger().LogInformation $"Responding to ping interaction on function invocation {ctx.FunctionId}"
                return req.CreateResponse HttpStatusCode.OK |> Response.withJson { Type = InteractionCallbackType.PONG; Data = None }

            // TODO: Handle different interaction types here, then offload to queues through queueServiceClient

            // We'll see how queues go with latency. They may need to be replaced with something else. Durable tasks
            // are being used for gateway events because they don't have a time limit, and even if they're a few
            // seconds late, that isn't that bad. Realistically, it'd be firing regularly enough for it to not matter
            // anyway. The other concern with this is how many orchestrators/entities existing may impact a function
            // app. I would expect it is fine, but would be worth double checking. If storage queues don't suffice for
            // interactions, either basic or standard service busses would also work, and if orchestrators are actually
            // really quick, they might even work out for interactions too. Lots of testing to do!

            | _ ->
                ctx.GetLogger().LogError $"Unhandled interaction on function invocation {ctx.FunctionId}"
                return req.CreateResponse HttpStatusCode.InternalServerError

        | false ->
            ctx.GetLogger().LogInformation $"Failed to verify ed25519 on function invocation {ctx.FunctionId}"
            return req.CreateResponse HttpStatusCode.Unauthorized
    }
