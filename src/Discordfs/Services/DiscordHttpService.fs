namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpService =
    abstract member CreateGlobalApplicationCommand:
        applicationId: string ->
        payload: CreateGlobalApplicationCommand ->
        Task<ApplicationCommand>

    abstract member BulkOverwriteGlobalApplicationCommands:
        applicationId: string -> 
        payload: CreateGlobalApplicationCommand list ->
        Task<ApplicationCommand list>

    abstract member CreateChannelInvite:
        channelId: string ->
        payload: CreateChannelInvite ->
        Task<Invite>

    abstract member CreateInteractionResponse:
        id: string ->
        token: string ->
        payload: InteractionCallback ->
        Task<unit>

    abstract member GetOriginalInteractionResponse:
        id: string ->
        token: string ->
        Task<Message>

    abstract member EditOriginalInteractionResponse:
        id: string ->
        token: string ->
        payload: EditWebhookMessage ->
        Task<Message>

    abstract member DeleteOriginalInteractionResponse:
        id: string ->
        token: string ->
        Task<unit>

    abstract member CreateFollowUpMessage:
        id: string ->
        token: string ->
        threadId: string option ->
        payload: ExecuteWebhook ->
        Task<Message>

    abstract member GetFollowUpMessage:
        id: string ->
        token: string ->
        messageId: string ->
        threadId: string option ->
        Task<Message>

    abstract member EditFollowUpMessage:
        id: string ->
        token: string ->
        messageId: string ->
        threadId: string option ->
        payload: EditWebhookMessage ->
        Task<Message>

    abstract member DeleteFollowUpMessage:
        id: string ->
        token: string ->
        messageId: string ->
        Task<unit>

    abstract member GetGateway:
        version: string ->
        encoding: GatewayEncoding ->
        compression: GatewayCompression option ->
        Task<GetGateway>

    abstract member GetGatewayBot:
        version: string ->
        encoding: GatewayEncoding ->
        compression: GatewayCompression option ->
        Task<GetGatewayBot>

    abstract member GetApplicationRoleConnectionMetadataRecords:
        applicationId: string ->
        Task<ApplicationRoleConnectionMetadata list>
        
    // TODO: Double check the payload type for this
    abstract member UpdateApplicationRoleConnectionMetadataRecords:
        applicationId: string ->
        payload: ApplicationRoleConnectionMetadata list ->
        Task<ApplicationRoleConnectionMetadata list>
        
    abstract member GetCurrentUserApplicationRoleConnection:
        applicationId: string ->
        oauth2AccessToken: string ->
        Task<ApplicationRoleConnection>

    // TODO: Double check the payload type for this
    abstract member UpdateCurrentUserApplicationRoleConnection:
        applicationId: string ->
        oauth2AccessToken: string ->
        payload: ApplicationRoleConnection ->
        Task<ApplicationRoleConnection>


type DiscordHttpService (httpClientFactory: IHttpClientFactory, token: string) =
    static member DISCORD_API_URL = "https://discord.com/api/"

    interface IDiscordHttpService with
        member _.CreateGlobalApplicationCommand applicationId payload =
            Req.create
                HttpMethod.Post
                DiscordHttpService.DISCORD_API_URL
                $"applications/{applicationId}/commands"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.BulkOverwriteGlobalApplicationCommands applicationId payload =
            Req.create
                HttpMethod.Patch
                DiscordHttpService.DISCORD_API_URL
                $"applications/{applicationId}/commands"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.CreateChannelInvite channelId payload =
            Req.create
                HttpMethod.Post
                DiscordHttpService.DISCORD_API_URL
                $"channels/{channelId}/invites"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.CreateInteractionResponse id token payload =
            Req.create
                HttpMethod.Post
                DiscordHttpService.DISCORD_API_URL
                $"interactions/{id}/{token}/callback"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.GetOriginalInteractionResponse id token =
            Req.create
                HttpMethod.Get
                DiscordHttpService.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/@original"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.EditOriginalInteractionResponse id token payload =
            Req.create
                HttpMethod.Patch
                DiscordHttpService.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/@original"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.DeleteOriginalInteractionResponse id token =
            Req.create
                HttpMethod.Delete
                DiscordHttpService.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/@original"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.CreateFollowUpMessage id token threadId payload =
            Req.create
                HttpMethod.Post
                DiscordHttpService.DISCORD_API_URL
                $"webhooks/{id}/{token}"
            |> Req.bot token
            |> Req.queryOpt "thread_id" threadId
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.GetFollowUpMessage id token messageId threadId =
            Req.create
                HttpMethod.Get
                DiscordHttpService.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> Req.bot token
            |> Req.queryOpt "thread_id" threadId
            |> Req.send httpClientFactory
            |> Res.body

        member _.EditFollowUpMessage id token messageId threadId payload =
            Req.create
                HttpMethod.Patch
                DiscordHttpService.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> Req.bot token
            |> Req.queryOpt "thread_id" threadId
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.DeleteFollowUpMessage id token messageId =
            Req.create
                HttpMethod.Delete
                DiscordHttpService.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.GetGateway version encoding compression =
            Req.create
                HttpMethod.Get
                DiscordHttpService.DISCORD_API_URL
                $"gateway"
            |> Req.query "v" version
            |> Req.query "encoding" (encoding.ToString())
            |> Req.queryOpt "compress" (Option.map _.ToString() compression)
            |> Req.send httpClientFactory
            |> Res.body

        member _.GetGatewayBot version encoding compression =
            Req.create
                HttpMethod.Get
                DiscordHttpService.DISCORD_API_URL
                $"gateway/bot"
            |> Req.bot token
            |> Req.query "v" version
            |> Req.query "encoding" (encoding.ToString())
            |> Req.queryOpt "compress" (Option.map _.ToString() compression)
            |> Req.send httpClientFactory
            |> Res.body

        member _.GetApplicationRoleConnectionMetadataRecords applicationId =
            Req.create
                HttpMethod.Get
                DiscordHttpService.DISCORD_API_URL
                $"/applications/{applicationId}/role-connections/metadata"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.UpdateApplicationRoleConnectionMetadataRecords applicationId payload =
            Req.create
                HttpMethod.Put
                DiscordHttpService.DISCORD_API_URL
                $"/applications/{applicationId}/role-connections/metadata"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.GetCurrentUserApplicationRoleConnection applicationId token =
            Req.create
                HttpMethod.Get
                DiscordHttpService.DISCORD_API_URL
                $"/users/@me/applications/{applicationId}/role-connection"
            |> Req.oauth token
            |> Req.send httpClientFactory
            |> Res.body

        member _.UpdateCurrentUserApplicationRoleConnection applicationId token payload =
            Req.create
                HttpMethod.Put
                DiscordHttpService.DISCORD_API_URL
                $"/users/@me/applications/{applicationId}/role-connection"
            |> Req.oauth token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body
