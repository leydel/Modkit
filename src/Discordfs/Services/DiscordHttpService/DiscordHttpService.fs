namespace Modkit.Discordfs.Services

open Microsoft.Extensions.Configuration
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
    abstract member Entitlements: IDiscordHttpEntitlementActions
    abstract member Gateway: IDiscordHttpGatewayActions
    abstract member Guilds: IDiscordHttpGuildActions
    abstract member GuildScheduledEvents: IDiscordHttpGuildScheduledEventActions
    abstract member GuildTemplates: IDiscordHttpGuildTemplateActions
    abstract member Invites: IDiscordHttpInviteActions
    abstract member Stickers: IDiscordHttpStickerActions

type DiscordHttpService (configuration: IConfiguration, httpClientFactory: IHttpClientFactory) =
    let discordBotToken = configuration.GetValue "DiscordBotToken"

    interface IDiscordHttpService with
        member _.Applications = DiscordHttpApplicationActions(httpClientFactory, discordBotToken)
        member _.Interactions = DiscordHttpInteractionActions(httpClientFactory, discordBotToken)
        member _.ApplicationCommands = DiscordHttpApplicationCommandActions(httpClientFactory, discordBotToken)
        member _.AuditLogs = DiscordHttpAuditLogActions(httpClientFactory, discordBotToken)
        member _.AutoModeration = DiscordHttpAutoModerationActions(httpClientFactory, discordBotToken)
        member _.RoleConnections = DiscordHttpRoleConnectionActions(httpClientFactory, discordBotToken)
        member _.Channels = DiscordHttpChannelActions(httpClientFactory, discordBotToken)
        member _.Emojis = DiscordHttpEmojiActions(httpClientFactory, discordBotToken)
        member _.Entitlements = DiscordHttpEntitlementActions(httpClientFactory, discordBotToken)
        member _.Gateway = DiscordHttpGatewayActions(httpClientFactory, discordBotToken)
        member _.Guilds = DiscordHttpGuildActions(httpClientFactory, discordBotToken)
        member _.GuildScheduledEvents = DiscordHttpGuildScheduledEventActions(httpClientFactory, discordBotToken)
        member _.GuildTemplates = DiscordHttpGuildTemplateActions(httpClientFactory, discordBotToken)
        member _.Invites = DiscordHttpInviteActions(httpClientFactory, discordBotToken)
        member _.Stickers = DiscordHttpStickerActions(httpClientFactory, discordBotToken)
