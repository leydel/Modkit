namespace Modkit.Roles.Application.Services

open System.Net.Http
open System.Threading.Tasks

open Discordfs.Rest
open Discordfs.Rest.Modules
open Discordfs.Types
open MediatR
open Microsoft.Extensions.Logging

open Modkit.Roles.Application.Repositories

[<Interface>]
type IInteractionService =
    abstract member HandleApplicationCommand:             Interaction -> Task<unit>
    abstract member HandleApplicationCommandAutocomplete: Interaction -> Task<unit>
    abstract member HandleMessageComponent:               Interaction -> Task<unit>
    abstract member HandleModalSubmit:                    Interaction -> Task<unit>        

type InteractionService (
    applicationRepository: IApplicationRepository,
    httpClientFactory: IHttpClientFactory,
    mediator: ISender,
    logger: ILogger<InteractionService>
) =
    let getBotClient applicationId = task {
        let! application = applicationRepository.Get applicationId
        return application |> Result.map (fun app -> httpClientFactory.CreateClient() |> HttpClient.toBotClient app.Token)
    }

    let sendErrorResponse (callbackType: InteractionCallbackType) data (interaction: Interaction) client = task {
        let content = CreateInteractionResponsePayload({ Type = callbackType; Data = data })
        do! client |> Rest.createInteractionResponse interaction.Id interaction.Token (Some false) content :> Task
    }

    let sendErrorMessageResponse content (interaction: Interaction) client = task {
        let data = MessageInteractionResponse.create(content = content, flags = int MessageFlag.EPHEMERAL)
        do! client |> sendErrorResponse InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE data interaction
    }

    interface IInteractionService with
        member _.HandleApplicationCommand interaction = task {
            let! client = getBotClient interaction.ApplicationId
            match client with
            | Error _ -> logger.LogError("Unable to find client for application {ApplicationId} to respond to command", interaction.ApplicationId)
            | Ok client ->
                // TODO: Handle application commands here, fallback to below
                
                logger.LogError("Unhandled application command interaction for application {ApplicationId}", interaction.ApplicationId)
                do! client |> sendErrorMessageResponse "Error: Unable to process command" interaction
        }
        
        member _.HandleApplicationCommandAutocomplete interaction = task {
            let! client = getBotClient interaction.ApplicationId
            match client with
            | Error _ -> logger.LogError("Unable to find client for application {ApplicationId} to respond to modal", interaction.ApplicationId)
            | Ok client ->
                // TODO: Handle autocomplete commands here, fallback to below
                
                logger.LogError("Unhandled autocomplete interaction for application {ApplicationId}", interaction.ApplicationId)
                do! client |> sendErrorResponse InteractionCallbackType.APPLICATION_COMMAND_AUTOCOMPLETE_RESULT ({ Choices = [] }) interaction
        }
        
        member _.HandleMessageComponent interaction = task {
            let! client = getBotClient interaction.ApplicationId
            match client with
            | Error _ -> logger.LogError("Unable to find client for application {ApplicationId} to respond to message component", interaction.ApplicationId)
            | Ok client ->
                // TODO: Handle message component commands here, fallback to below
                
                logger.LogError("Unhandled message component interaction for application {ApplicationId}", interaction.ApplicationId)
                do! client |> sendErrorMessageResponse "Error: Unable to process message component" interaction
        }
        
        member _.HandleModalSubmit interaction = task {
            let! client = getBotClient interaction.ApplicationId
            match client with
            | Error _ -> logger.LogError("Unable to find client for application {ApplicationId} to respond to modal", interaction.ApplicationId)
            | Ok client ->
                // TODO: Handle modal commands here, fallback to below
                
                logger.LogError("Unhandled modal interaction for application {ApplicationId}", interaction.ApplicationId)
                do! client |> sendErrorMessageResponse "Error: Unable to process modal" interaction
        }
