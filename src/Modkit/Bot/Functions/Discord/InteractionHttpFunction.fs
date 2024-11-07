namespace Modkit.Bot.Functions

open Discordfs.Types
open Discordfs.Webhook.Modules
open Discordfs.Webhook.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Api.Common
open Modkit.Bot.Configuration
open System.Net
open System.Text.Json

type InteractionHttpFunction () =
    [<Function(nameof InteractionHttpFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", "interactions")>] req: HttpRequestData,
        [<OrchestrationTrigger>]
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
            | _ ->
                ctx.GetLogger().LogError $"Unhandled interaction on function invocation {ctx.FunctionId}"
                return req.CreateResponse HttpStatusCode.InternalServerError
        | false ->
            ctx.GetLogger().LogInformation $"Failed to verify ed25519 on function invocation {ctx.FunctionId}"
            return req.CreateResponse HttpStatusCode.Unauthorized
    }
