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

type DiscordHttpService (httpClientFactory: IHttpClientFactory, token: string) =
    static member DISCORD_API_URL = "https://discord.com/api/"

    member _.request<'T> (method: HttpMethod) (endpoint: string) =
        let req = new HttpRequestMessage(method, DiscordHttpService.DISCORD_API_URL + endpoint)
        req.Headers.Clear()
        req.Headers.Add("Authorization", $"Bearer {token}")
        req

    member _.query (key: string) (value: string option) (req: HttpRequestMessage) =
        if (value.IsNone) then
            req
        else
            let uriBuilder = UriBuilder(req.RequestUri)
            let query = HttpUtility.ParseQueryString(uriBuilder.Query)
            query.Add(key, value.Value)
            uriBuilder.Query <- query.ToString()
            req.RequestUri <- uriBuilder.Uri
            req

    member _.body (payload: 'a) (req: HttpRequestMessage) =
        req.Content <- new StringContent (Json.serializeU payload)
        req

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
            |> this.body payload
            |> this.result

        member this.BulkOverwriteGlobalApplicationCommands applicationId payload =
            this.request
                HttpMethod.Patch
                $"applications/{applicationId}/commands"
            |> this.body payload
            |> this.result

        member this.CreateChannelInvite channelId payload =
            this.request
                HttpMethod.Post
                $"channels/{channelId}/invites"
            |> this.body payload
            |> this.result

        member this.CreateInteractionResponse id token payload =
            this.request
                HttpMethod.Post
                $"interactions/{id}/{token}/callback"
            |> this.body payload
            |> this.unit

        member this.GetOriginalInteractionResponse id token =
            this.request
                HttpMethod.Get
                $"webhooks/{id}/{token}/messages/@original"
            |> this.result

        member this.EditOriginalInteractionResponse id token payload =
            this.request
                HttpMethod.Patch
                $"webhooks/{id}/{token}/messages/@original"
            |> this.body payload
            |> this.result

        member this.DeleteOriginalInteractionResponse id token =
            this.request
                HttpMethod.Delete
                $"webhooks/{id}/{token}/messages/@original"
            |> this.unit

        member this.CreateFollowUpMessage threadId id token payload =
            this.request
                HttpMethod.Post
                $"webhooks/{id}/{token}"
            |> this.query "thread_id" threadId
            |> this.body payload
            |> this.result

        member this.GetFollowUpMessage messageId threadId id token =
            this.request
                HttpMethod.Get
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> this.query "thread_id" threadId
            |> this.result

        member this.EditFollowUpMessage messageId threadId id token payload =
            this.request
                HttpMethod.Patch
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> this.query "thread_id" threadId
            |> this.body payload
            |> this.result

        member this.DeleteFollowUpMessage messageId id token =
            this.request
                HttpMethod.Delete
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> this.unit

        member this.GetGateway version encoding compression =
            this.request
                HttpMethod.Get
                $"gateway"
            |> this.query "v" (Some version)
            |> this.query "encoding" (Some (encoding.ToString()))
            |> this.query "compress" (match compression with | Some c -> Some (c.ToString()) | None -> None)
            |> this.result

        member this.GetGatewayBot version encoding compression =
            this.request
                HttpMethod.Get
                $"gateway/bot"
            |> this.query "v" (Some version)
            |> this.query "encoding" (Some (encoding.ToString()))
            |> this.query "compress" (match compression with | Some c -> Some (c.ToString()) | None -> None)
            |> this.result
