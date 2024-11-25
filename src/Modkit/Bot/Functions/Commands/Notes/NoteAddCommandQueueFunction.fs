namespace Modkit.Bot.Functions

open Discordfs.Commands.Patterns
open Discordfs.Rest
open Discordfs.Types
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open Modkit.Api.Client
open Modkit.Api.Functions.Http
open Modkit.Bot.Configuration
open System.Net.Http
open System.Threading.Tasks

module NoteAddCommand =
    [<Literal>]
    let queueName = "NoteAddCommand"

type NoteAddCommandQueueFunction (logger: ILogger<NoteAddCommandQueueFunction>) =
    // TODO: Figure out how to create command metadata for subcommands so this can be `/note add [user] [content]`

    let validate (interaction: Interaction) =
        // TODO: Create validation computation expression that results in `Result<ValidOptionsHere, string list>`
        
        let args =
            match (interaction.Data >>= _.Options), interaction.GuildId with
            | Some options, Some guildId ->
                match options with
                | Options.User "user" userId & Options.String "content" content ->
                    Some {| UserId = userId; Content = content; GuildId = guildId |}
                | _ -> None
            | _ -> None

        match args with
        | Some args -> Ok args
        | None -> Error "Missing arguments"

    member _.Run (
        [<QueueTrigger(NoteAddCommand.queueName)>] interaction: Interaction,
        options: IOptions<DiscordOptions>,
        httpClientFactory: IHttpClientFactory
    ) = task {
        logger.LogInformation("Handling note add command for interaction {InteractionId}", interaction.Id)

        let api = httpClientFactory.CreateApiClient()
        let client = httpClientFactory.CreateBotClient options.Value.BotToken

        // TODO: Create task pipeline that does each step and skips to interaction response once any step returns a result (?)

        match validate interaction with
        | Error err ->
            logger.LogError(exn err, "Failed to add note due to invalid command arguments")

            let message = CreateInteractionResponsePayload({
                Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE;
                Data = MessageInteractionResponse.create (content = err) // TODO: Design proper interaction response
            })

            do! client |> Rest.createInteractionResponse interaction.Id interaction.Token (Some false) message :> Task

        | Ok args ->
            let payload = { MessageId = None; Content = args.Content; }
            let! noteResponse = api |> Api.addMemberNote args.GuildId args.UserId payload

            match noteResponse with
            | Error status ->
                logger.LogError("Failed to add note due to status code {StatusCode}", status)

                let message = CreateInteractionResponsePayload({
                    Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE;
                    Data = MessageInteractionResponse.create (content = "Failed to save note") // TODO: Design proper interaction response
                })

                do! client |> Rest.createInteractionResponse interaction.Id interaction.Token (Some false) message :> Task

            | Ok note ->
                logger.LogInformation("Successfully created note {NoteID} for user {UserId} in guild {GuildId}", note.Id, args.UserId, args.GuildId)

                let message = CreateInteractionResponsePayload({
                    Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE;
                    Data = MessageInteractionResponse.create (content = "Note saved") // TODO: Design proper interaction response
                })
                
                do! client |> Rest.createInteractionResponse interaction.Id interaction.Token (Some false) message :> Task

        // TODO: Create discord type utility functions e.g. `Interaction.respond` to simplify some uses (?)
    }
