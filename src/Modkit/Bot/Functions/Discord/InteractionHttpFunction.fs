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
open System.Threading.Tasks

type InteractionHttpFunction (queueServiceClientFactory: IAzureClientFactory<QueueServiceClient>) =
    let queueServiceClient = queueServiceClientFactory.CreateClient Constants.InteractionQueueClientName

    [<Function(nameof InteractionHttpFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", "interactions")>] req: HttpRequestData,
        [<FromBody>] event: InteractionReceiveEvent,
        ctx: FunctionContext,
        options: IOptions<DiscordOptions>
    ) = task {
        let logger = ctx.GetLogger<InteractionHttpFunction>()

        let! json = req.ReadAsStringAsync()
        let publicKey = options.Value.PublicKey
        let signature = req.Headers.TryGetValues "X-Signature-Ed25519" |> fun (_, s) -> Seq.tryHead s >>? ""
        let timestamp = req.Headers.TryGetValues "X-Signature-Timestamp" |> fun (_, s) -> Seq.tryHead s >>? ""

        match Ed25519.verify timestamp json signature publicKey with
        | false ->
            ctx.GetLogger().LogInformation $"Failed to verify ed25519 on function invocation {ctx.InvocationId}"
            return req.CreateResponse HttpStatusCode.Unauthorized

        | true ->
            match event with
            | InteractionReceiveEvent.PING _ ->
                logger.LogInformation $"Responding to ping interaction on function invocation {ctx.InvocationId}"
                return req.CreateResponse HttpStatusCode.OK |> Response.withJson { Type = InteractionCallbackType.PONG; Data = None }

            | InteractionReceiveEvent.APPLICATION_COMMAND interaction ->
                let name = interaction.Data >>. _.Name

                let queueName =
                    match name with
                    | Some v when v = PingCommandQueueFunction.Metadata.Name -> Some (Constants.PingCommandQueueName)
                    | _ -> None

                match queueName with
                | Some queue ->
                    do! queueServiceClient.GetQueueClient(queue).SendMessageAsync json :> Task

                    logger.LogInformation $"Queued message to handle application command interaction {interaction.Id} on function invocation {ctx.InvocationId}"
                    return req.CreateResponse HttpStatusCode.Accepted

                | None -> 
                    logger.LogError $"Could not find queue for application command interaction called \"{name}\" on function invocation {ctx.InvocationId}"
                    return req.CreateResponse HttpStatusCode.OK
                    |> Response.withJson {
                        Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE
                        Data = MessageInteractionResponse.create (content = "Error: Command not found.")
                    }

            | InteractionReceiveEvent.APPLICATION_COMMAND_AUTOCOMPLETE interaction ->
                ctx.GetLogger().LogError $"Unhandled interaction type on function invocation {ctx.InvocationId}"
                return req.CreateResponse HttpStatusCode.InternalServerError

            | InteractionReceiveEvent.MESSAGE_COMPONENT interaction ->
                ctx.GetLogger().LogError $"Unhandled interaction type on function invocation {ctx.InvocationId}"
                return req.CreateResponse HttpStatusCode.InternalServerError

            | InteractionReceiveEvent.MODAL_SUBMIT interaction ->
                ctx.GetLogger().LogError $"Unhandled interaction type on function invocation {ctx.InvocationId}"
                return req.CreateResponse HttpStatusCode.InternalServerError

            // We'll see how queues go with latency. They may need to be replaced with something else. Durable tasks
            // are being used for gateway events because they don't have a time limit, and even if they're a few
            // seconds late, that isn't that bad. Realistically, it'd be firing regularly enough for it to not matter
            // anyway. The other concern with this is how many orchestrators/entities existing may impact a function
            // app. I would expect it is fine, but would be worth double checking. If storage queues don't suffice for
            // interactions, either basic or standard service busses would also work, and if orchestrators are actually
            // really quick, they might even work out for interactions too. Lots of testing to do!
    }
