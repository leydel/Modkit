namespace Modkit.Bot.Functions

open Discordfs.Commands.Structures
open Discordfs.Rest.Resources
open Discordfs.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Bot.Common
open Modkit.Bot.Configuration
open System.Net.Http
open System.Threading.Tasks

type PingCommandQueueFunction () =
    static member Metadata = CommandData.build(
        name = "ping",
        description = "Test if the bot is online",
        ``type`` = ApplicationCommandType.CHAT_INPUT
    )

    member _.Run (
        [<QueueTrigger(Constants.PingCommandQueueName)>] interaction: Interaction,
        ctx: FunctionContext,
        options: IOptions<DiscordOptions>,
        httpClientFactory: IHttpClientFactory
    ) = task {
        let client = (options.Value.BotToken, httpClientFactory.CreateClient())
        let logger = ctx.GetLogger<PingCommandQueueFunction>()

        let content = CreateInteractionResponsePayload({
            Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE;
            Data = MessageInteractionResponse.create (content = "Pong!")
        })

        do!
            client
            ||> Interaction.createInteractionResponse interaction.Id interaction.Token (Some false) content
            |> Task.map (fun res ->
                match res with
                | CreateInteractionResponseResponse.NoContent ->
                    logger.LogInformation $"Successfully ponged interaction {interaction.Id} on function invocation {ctx.InvocationId}"
                | _ ->
                    logger.LogError $"Failed to respond to interaction {interaction.Id} on function invocation {ctx.InvocationId}"
            )
    }
