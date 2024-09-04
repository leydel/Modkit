namespace Modkit.Discordfs.Services

open System.Net.Http

type IDiscordHttpService =
    abstract member ApplicationCommands: IDiscordHttpApplicationCommandActions

    abstract member Channels: IDiscordHttpChannelActions

    abstract member Interactions: IDiscordHttpInteractionActions

    abstract member Gateway: IDiscordHttpGatewayActions

    abstract member RoleConnections: IDiscordHttpRoleConnectionActions

type DiscordHttpService (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpService with
        member _.ApplicationCommands = DiscordHttpApplicationCommandActions(httpClientFactory, token)

        member _.Channels = DiscordHttpChannelActions(httpClientFactory, token)

        member _.Interactions = DiscordHttpInteractionActions(httpClientFactory, token)

        member _.Gateway = DiscordHttpGatewayActions(httpClientFactory, token)

        member _.RoleConnections = DiscordHttpRoleConnectionActions(httpClientFactory, token)
