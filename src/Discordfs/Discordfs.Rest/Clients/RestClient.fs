namespace Discordfs.Rest.Clients

open Discordfs.Rest.Resources
open Microsoft.Extensions.Configuration
open System.Net.Http

type IRestClient =
    abstract member Interactions: IInteractionResource
    abstract member Applications: IApplicationResource
    abstract member ApplicationCommands: IApplicationCommandResource
    abstract member AuditLogs: IAuditLogResource
    abstract member AutoModeration: IAutoModerationResource
    abstract member RoleConnections: IRoleConnectionResource
    abstract member Channels: IChannelResource
    abstract member Emojis: IEmojiResource
    abstract member Entitlements: IEntitlementResource
    abstract member Guilds: IGuildResource
    abstract member GuildScheduledEvents: IGuildScheduledEventResource
    abstract member GuildTemplates: IGuildTemplateResource
    abstract member Invites: IInviteResource
    abstract member Messages: IMessageResource
    abstract member Polls: IPollResource
    abstract member Skus: ISkuResource
    abstract member StageInstances: IStageInstanceResource
    abstract member Stickers: IStickerResource
    abstract member Subscriptions: ISubscriptionResource
    abstract member Users: IUserResource
    abstract member Voice: IVoiceResource
    // TODO: Webhook
    abstract member Gateway: IGatewayResource
    abstract member OAuth2: IOAuth2Resource

type RestClient (configuration: IConfiguration, httpClientFactory: IHttpClientFactory) =
    let discordBotToken = configuration.GetValue "DiscordBotToken"

    interface IRestClient with
        member _.Interactions = InteractionResource(httpClientFactory, discordBotToken)
        member _.Applications = ApplicationResource(httpClientFactory, discordBotToken)
        member _.ApplicationCommands = ApplicationCommandResource(httpClientFactory, discordBotToken)
        member _.AuditLogs = AuditLogResource(httpClientFactory, discordBotToken)
        member _.AutoModeration = AutoModerationResource(httpClientFactory, discordBotToken)
        member _.RoleConnections = RoleConnectionResource(httpClientFactory, discordBotToken)
        member _.Channels = ChannelResource(httpClientFactory, discordBotToken)
        member _.Emojis = EmojiResource(httpClientFactory, discordBotToken)
        member _.Entitlements = EntitlementResource(httpClientFactory, discordBotToken)
        member _.Guilds = GuildResource(httpClientFactory, discordBotToken)
        member _.GuildScheduledEvents = GuildScheduledEventResource(httpClientFactory, discordBotToken)
        member _.GuildTemplates = GuildTemplateResource(httpClientFactory, discordBotToken)
        member _.Invites = InviteResource(httpClientFactory, discordBotToken)
        member _.Messages = MessageResource(httpClientFactory, discordBotToken)
        member _.Polls = PollResource(httpClientFactory, discordBotToken)
        member _.Skus = SkuResource(httpClientFactory, discordBotToken)
        member _.StageInstances = StageInstanceResource(httpClientFactory, discordBotToken)
        member _.Stickers = StickerResource(httpClientFactory, discordBotToken)
        member _.Subscriptions = SubscriptionResource(httpClientFactory, discordBotToken)
        member _.Users = UserResource(httpClientFactory, discordBotToken)
        member _.Voice = VoiceResource(httpClientFactory, discordBotToken)
        // TODO: Webhook
        member _.Gateway = GatewayResource(httpClientFactory, discordBotToken)
        member _.OAuth2 = OAuth2Resource(httpClientFactory, discordBotToken)
