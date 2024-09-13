namespace Modkit.Discordfs.Services

open System.Net.Http

type IDiscordHttpService =
    abstract member Applications: IDiscordHttpApplicationActions

    abstract member Interactions: IDiscordHttpInteractionActions

    abstract member ApplicationCommands: IDiscordHttpApplicationCommandActions

    abstract member AuditLogs: IDiscordHttpAuditLogActions

    abstract member AutoModeration: IDiscordHttpAutoModerationActions

    abstract member RoleConnections: IDiscordHttpRoleConnectionActions

    abstract member Channels: IDiscordHttpChannelActions

    abstract member Emojis: IDiscordHttpEmojiActions

    abstract member Gateway: IDiscordHttpGatewayActions

    abstract member Guilds: IDiscordHttpGuildActions

    abstract member Stickers: IDiscordHttpStickerActions

type DiscordHttpService (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpService with
        member _.Applications = DiscordHttpApplicationActions(httpClientFactory, token)

        member _.Interactions = DiscordHttpInteractionActions(httpClientFactory, token)

        member _.ApplicationCommands = DiscordHttpApplicationCommandActions(httpClientFactory, token)

        member _.AuditLogs = DiscordHttpAuditLogActions(httpClientFactory, token)

        member _.AutoModeration = DiscordHttpAutoModerationActions(httpClientFactory, token)
        
        member _.RoleConnections = DiscordHttpRoleConnectionActions(httpClientFactory, token)

        member _.Channels = DiscordHttpChannelActions(httpClientFactory, token)

        member _.Emojis = DiscordHttpEmojiActions(httpClientFactory, token)

        member _.Gateway = DiscordHttpGatewayActions(httpClientFactory, token)

        member _.Guilds = DiscordHttpGuildActions(httpClientFactory, token)

        member _.Stickers = DiscordHttpStickerActions(httpClientFactory, token)
