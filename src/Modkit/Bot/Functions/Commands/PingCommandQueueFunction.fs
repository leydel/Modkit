namespace Modkit.Bot.Functions

open Discordfs.Commands.Structures
open Discordfs.Rest.Resources
open Discordfs.Rest.Types
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
        let logger = ctx.GetLogger<PingCommandQueueFunction>()
        let client = (options.Value.BotToken, httpClientFactory.CreateClient())

        try
            let content = CreateInteractionResponsePayload({
                Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE;
                Data = MessageInteractionResponse.create (content = "Pong!")
            })

            do! client
                ||> Interaction.createInteractionResponse interaction.Id interaction.Token (Some false) content
                ?> DiscordResponse.unwrap :> Task

            logger.LogInformation $"Successfully ponged interaction {interaction.Id} on function invocation {ctx.InvocationId}"
        with _ ->
            logger.LogError $"Failed to respond to interaction {interaction.Id} on function invocation {ctx.InvocationId}"
    }
