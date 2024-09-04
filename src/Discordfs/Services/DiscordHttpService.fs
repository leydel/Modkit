namespace Modkit.Discordfs.Services

open FSharp.Json
open Modkit.Discordfs.Types
open System
open System.Net.Http
open System.Threading.Tasks
open System.Web

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
        threadId: string option ->
        id: string ->
        token: string ->
        payload: ExecuteWebhook ->
        Task<Message>

    abstract member GetFollowUpMessage:
        messageId: string ->
        threadId: string option ->
        id: string ->
        token: string ->
        Task<Message>

    abstract member EditFollowUpMessage:
        messageId: string ->
        threadId: string option ->
        id: string ->
        token: string ->
        payload: EditWebhookMessage ->
        Task<Message>

    abstract member DeleteFollowUpMessage:
        messageId: string ->
        id: string ->
        token: string ->
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
        id: string ->
        Task<ApplicationRoleConnectionMetadata list>
        
    // TODO: Double check the payload type for this
    abstract member UpdateApplicationRoleConnectionMetadataRecords:
        id: string ->
        payload: ApplicationRoleConnectionMetadata list ->
        Task<ApplicationRoleConnectionMetadata list>
        
    abstract member GetCurrentUserApplicationRoleConnection:
        id: string ->
        oauth2AccessToken: string ->
        Task<ApplicationRoleConnection>

    // TODO: Double check the payload type for this
    abstract member UpdateCurrentUserApplicationRoleConnection:
        id: string ->
        oauth2AccessToken: string ->
        payload: ApplicationRoleConnection ->
        Task<ApplicationRoleConnection>


type DiscordHttpService (httpClientFactory: IHttpClientFactory, token: string) =
    static member DISCORD_API_URL = "https://discord.com/api/"

    member _.request<'T> (method: HttpMethod) (endpoint: string) =
        new HttpRequestMessage(method, DiscordHttpService.DISCORD_API_URL + endpoint)

    member _.query (key: string) (value: string) (req: HttpRequestMessage) =
        let uriBuilder = UriBuilder(req.RequestUri)
        let query = HttpUtility.ParseQueryString(uriBuilder.Query)
        query.Add(key, value)
        uriBuilder.Query <- query.ToString()
        req.RequestUri <- uriBuilder.Uri
        req

    member _.body (payload: 'a) (req: HttpRequestMessage) =
        req.Content <- new StringContent (Json.serializeU payload)
        req

    member _.header (key: string) (value: string) (req: HttpRequestMessage) =
        req.Headers.Add(key, value)
        req

    member this.botAuthorization (req: HttpRequestMessage) =
        this.header "Authorization" $"Bearer {token}" req

    member this.oauthAuthorization (token: string) (req: HttpRequestMessage) =
        this.header "Authorization" $"Bearer {token}" req

    member _.result (req: HttpRequestMessage) = task {
        let! res = httpClientFactory.CreateClient().SendAsync req
        let! body = res.Content.ReadAsStringAsync()
        return Json.deserialize<'a> body
    }

    member _.unit (req: HttpRequestMessage) = task {
        do! httpClientFactory.CreateClient().SendAsync req :> Task
    }

    interface IDiscordHttpService with 
        member this.CreateGlobalApplicationCommand applicationId payload =
            this.request
                HttpMethod.Post
                $"applications/{applicationId}/commands"
            |> this.botAuthorization
            |> this.body payload
            |> this.result

        member this.BulkOverwriteGlobalApplicationCommands applicationId payload =
            this.request
                HttpMethod.Patch
                $"applications/{applicationId}/commands"
            |> this.botAuthorization
            |> this.body payload
            |> this.result

        member this.CreateChannelInvite channelId payload =
            this.request
                HttpMethod.Post
                $"channels/{channelId}/invites"
            |> this.botAuthorization
            |> this.body payload
            |> this.result

        member this.CreateInteractionResponse id token payload =
            this.request
                HttpMethod.Post
                $"interactions/{id}/{token}/callback"
            |> this.botAuthorization
            |> this.body payload
            |> this.unit

        member this.GetOriginalInteractionResponse id token =
            this.request
                HttpMethod.Get
                $"webhooks/{id}/{token}/messages/@original"
            |> this.botAuthorization
            |> this.result

        member this.EditOriginalInteractionResponse id token payload =
            this.request
                HttpMethod.Patch
                $"webhooks/{id}/{token}/messages/@original"
            |> this.botAuthorization
            |> this.body payload
            |> this.result

        member this.DeleteOriginalInteractionResponse id token =
            this.request
                HttpMethod.Delete
                $"webhooks/{id}/{token}/messages/@original"
            |> this.botAuthorization
            |> this.unit

        member this.CreateFollowUpMessage threadId id token payload =
            this.request
                HttpMethod.Post
                $"webhooks/{id}/{token}"
            |> this.botAuthorization
            |> Option.foldBack (this.query "thread_id") threadId
            |> this.body payload
            |> this.result

        member this.GetFollowUpMessage messageId threadId id token =
            this.request
                HttpMethod.Get
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> this.botAuthorization
            |> Option.foldBack (this.query "thread_id") threadId
            |> this.result

        member this.EditFollowUpMessage messageId threadId id token payload =
            this.request
                HttpMethod.Patch
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> this.botAuthorization
            |> Option.foldBack (this.query "thread_id") threadId
            |> this.body payload
            |> this.result

        member this.DeleteFollowUpMessage messageId id token =
            this.request
                HttpMethod.Delete
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> this.botAuthorization
            |> this.unit

        member this.GetGateway version encoding compression =
            this.request
                HttpMethod.Get
                $"gateway"
            |> this.query "v" version
            |> this.query "encoding" (encoding.ToString())
            |> Option.foldBack (this.query "compress") (Option.map _.ToString() compression)
            |> this.result

        member this.GetGatewayBot version encoding compression =
            this.request
                HttpMethod.Get
                $"gateway/bot"
            |> this.botAuthorization
            |> this.query "v" version
            |> this.query "encoding" (encoding.ToString())
            |> Option.foldBack (this.query "compress") (Option.map _.ToString() compression)
            |> this.result

        member this.GetApplicationRoleConnectionMetadataRecords id =
            this.request
                HttpMethod.Get
                $"/applications/{id}/role-connections/metadata"
            |> this.botAuthorization
            |> this.result

        member this.UpdateApplicationRoleConnectionMetadataRecords id payload =
            this.request
                HttpMethod.Put
                $"/applications/{id}/role-connections/metadata"
            |> this.botAuthorization
            |> this.body payload
            |> this.result

        member this.GetCurrentUserApplicationRoleConnection id token =
            this.request
                HttpMethod.Get
                $"/users/@me/applications/{id}/role-connection"
            |> this.oauthAuthorization token
            |> this.result

        member this.UpdateCurrentUserApplicationRoleConnection id token payload =
            this.request
                HttpMethod.Get
                $"/users/@me/applications/{id}/role-connection"
            |> this.oauthAuthorization token
            |> this.body payload
            |> this.result
