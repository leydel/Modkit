namespace Modkit.Bot.Functions

open Discordfs.Commands
open Discordfs.Rest
open Discordfs.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Bot.Common
open Modkit.Bot.Configuration
open System.Net.Http

type PingCommandQueueFunction (logger: ILogger<PingCommandQueueFunction>) =
    static member Metadata = command "ping" {
        description "Test if the bot is online"
    }

    // TODO: Separate command data definition from function, to neatly handle subcommands and groups

    member _.Run (
        [<QueueTrigger(Constants.PingCommandQueueName)>] interaction: Interaction,
        ctx: FunctionContext,
        options: IOptions<DiscordOptions>,
        httpClientFactory: IHttpClientFactory
    ) = task {
        logger.LogInformation("Handling ping command for interaction {InteractionId}", interaction.Id)

        let client = httpClientFactory.CreateBotClient options.Value.BotToken

        let content = CreateInteractionResponsePayload({
            Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE;
            Data = MessageInteractionResponse.create (content = "Pong!")
        })

        do! client
            |> Rest.createInteractionResponse interaction.Id interaction.Token (Some false) content
            ?> (fun res ->
                match res with
                | Ok _ -> logger.LogInformation "Successfully ponged interaction"
                | Error { Data = err } -> logger.LogError(DiscordError.toExn err, "Failed to respond due to Discord error")
            )
    }
