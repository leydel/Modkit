namespace Modkit.Discordfs.Services

open Microsoft.Extensions.Configuration
open Modkit.Discordfs.Resources
open System.Net.Http

type IHttpService =
    abstract member Interactions: IInteractionResource
    abstract member Applications: IApplicationResource
    abstract member ApplicationCommands: IApplicationCommandResource
    abstract member AuditLogs: IAuditLogResource
    abstract member AutoModeration: IAutoModerationResource
    abstract member RoleConnections: IRoleConnectionResource
    abstract member Channels: IChannelResource
    abstract member Emojis: IEmojiResource
    abstract member Entitlements: IEntitlementResource
    abstract member Gateway: IGatewayResource
    abstract member Guilds: IGuildResource
    abstract member GuildScheduledEvents: IGuildScheduledEventResource
    abstract member GuildTemplates: IGuildTemplateResource
    abstract member Invites: IInviteResource
    // TODO: Messsage
    // TODO: Poll
    abstract member Skus: ISkuResource
    // TODO: StageInstance
    abstract member Stickers: IStickerResource
    // TODO: Subscription
    // TODO: User
    // TODO: Voice
    // TODO: Webhook

type HttpService (configuration: IConfiguration, httpClientFactory: IHttpClientFactory) =
    let discordBotToken = configuration.GetValue "DiscordBotToken"

    interface IHttpService with
        member _.Interactions = InteractionResource(httpClientFactory, discordBotToken)
        member _.Applications = ApplicationResource(httpClientFactory, discordBotToken)
        member _.ApplicationCommands = ApplicationCommandResource(httpClientFactory, discordBotToken)
        member _.AuditLogs = AuditLogResource(httpClientFactory, discordBotToken)
        member _.AutoModeration = AutoModerationResource(httpClientFactory, discordBotToken)
        member _.RoleConnections = RoleConnectionResource(httpClientFactory, discordBotToken)
        member _.Channels = ChannelResource(httpClientFactory, discordBotToken)
        member _.Emojis = EmojiResource(httpClientFactory, discordBotToken)
        member _.Entitlements = EntitlementResource(httpClientFactory, discordBotToken)
        member _.Gateway = GatewayResource(httpClientFactory, discordBotToken)
        member _.Guilds = GuildResource(httpClientFactory, discordBotToken)
        member _.GuildScheduledEvents = GuildScheduledEventResource(httpClientFactory, discordBotToken)
        member _.GuildTemplates = GuildTemplateResource(httpClientFactory, discordBotToken)
        member _.Invites = InviteResource(httpClientFactory, discordBotToken)
        member _.Skus = SkuResource(httpClientFactory, discordBotToken)
        member _.Stickers = StickerResource(httpClientFactory, discordBotToken)
