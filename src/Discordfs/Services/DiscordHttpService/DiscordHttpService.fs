namespace Modkit.Discordfs.Services

open System.Net.Http

type IDiscordHttpService =
    abstract member Applications: IDiscordHttpApplicationCommandActions

    abstract member Interactions: IDiscordHttpInteractionActions

    abstract member ApplicationCommands: IDiscordHttpApplicationCommandActions

    abstract member AutoModeration: IDiscordHttpAutoModerationActions

    abstract member RoleConnections: IDiscordHttpRoleConnectionActions

    abstract member Channels: IDiscordHttpChannelActions

    abstract member Gateway: IDiscordHttpGatewayActions

type DiscordHttpService (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpService with
        member _.Applications = DiscordHttpApplicationCommandActions(httpClientFactory, token)

        member _.Interactions = DiscordHttpInteractionActions(httpClientFactory, token)

        member _.ApplicationCommands = DiscordHttpApplicationCommandActions(httpClientFactory, token)

        member _.AutoModeration = DiscordHttpAutoModerationActions(httpClientFactory, token)
        
        member _.RoleConnections = DiscordHttpRoleConnectionActions(httpClientFactory, token)

        member _.Channels = DiscordHttpChannelActions(httpClientFactory, token)

        member _.Gateway = DiscordHttpGatewayActions(httpClientFactory, token)
