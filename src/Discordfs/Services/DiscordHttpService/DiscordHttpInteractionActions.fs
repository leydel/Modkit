namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpInteractionActions =
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

type DiscordHttpInteractionActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpInteractionActions with
        member _.CreateInteractionResponse id token payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"interactions/{id}/{token}/callback"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.GetOriginalInteractionResponse id token =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/@original"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.EditOriginalInteractionResponse id token payload =
            Req.create
                HttpMethod.Patch
                Constants.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/@original"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.DeleteOriginalInteractionResponse id token =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/@original"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.CreateFollowUpMessage id token threadId payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"webhooks/{id}/{token}"
            |> Req.bot token
            |> Req.queryOpt "thread_id" threadId
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.GetFollowUpMessage id token messageId threadId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> Req.bot token
            |> Req.queryOpt "thread_id" threadId
            |> Req.send httpClientFactory
            |> Res.body

        member _.EditFollowUpMessage id token messageId threadId payload =
            Req.create
                HttpMethod.Patch
                Constants.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> Req.bot token
            |> Req.queryOpt "thread_id" threadId
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.DeleteFollowUpMessage id token messageId =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"webhooks/{id}/{token}/messages/{messageId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore
