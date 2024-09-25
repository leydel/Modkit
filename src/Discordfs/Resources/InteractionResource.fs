namespace Modkit.Discordfs.Resources

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IInteractionResource =
    abstract member CreateInteractionResponse:
        id: string ->
        token: string ->
        ``type``: InteractionType ->
        data: InteractionCallbackData ->
        Task<unit>

    abstract member GetOriginalInteractionResponse:
        id: string ->
        token: string ->
        threadId: string option ->
        Task<Message>

    abstract member EditOriginalInteractionResponse:
        id: string ->
        token: string ->
        threadId: string option ->
        content: string option option ->
        embeds: Embed list option option ->
        allowedMentions: AllowedMentions option option ->
        components: Component list option option ->
        // TODO: Add `files`, `payload_json`, `attachments` support
        // TODO: Check what types should be `option`
        Task<Message>

    abstract member DeleteOriginalInteractionResponse:
        id: string ->
        token: string ->
        Task<unit>

    abstract member CreateFollowUpMessage:
        id: string ->
        token: string ->
        threadId: string option ->
        content: string option ->
        username: string ->
        avatarUrl: string ->
        tts: bool ->
        embeds: Embed list option ->
        allowedMentions: AllowedMentions option ->
        components: Component list option ->
        // TODO: Add `files`, `payload_json`, `attachments` support
        flags: int ->
        threadName: string option ->
        appliedTags: string list option ->
        poll: Poll option ->
        // TODO: Check what types should be `option`
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
        content: string option option ->
        embeds: Embed list option option ->
        allowedMentions: AllowedMentions option option ->
        components: Component list option option ->
        // TODO: Add `files`, `payload_json`, `attachments` support
        // TODO: Check what types should be `option`
        Task<Message>

    abstract member DeleteFollowUpMessage:
        id: string ->
        token: string ->
        messageId: string ->
        Task<unit>

type InteractionResource (httpClientFactory: IHttpClientFactory, token: string) =
    interface IInteractionResource with
        member _.CreateInteractionResponse
            id token ``type`` data =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"interactions/{id}/{token}/callback"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.property "type" ``type``
                    |> Dto.property "data" data
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.GetOriginalInteractionResponse
            id token threadId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"webhooks/{id}/{token}/messages/@original"
                |> Req.bot token
                |> Req.queryOpt "thread_id" threadId
                |> Req.send httpClientFactory
                |> Res.json

        member _.EditOriginalInteractionResponse
            id token content embeds allowedMentions components =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"webhooks/{id}/{token}/messages/@original"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "content" content
                    |> Dto.propertyIf "embeds" embeds
                    |> Dto.propertyIf "allowed_mentions" allowedMentions
                    |> Dto.propertyIf "components" components
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteOriginalInteractionResponse
            id token =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"webhooks/{id}/{token}/messages/@original"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.CreateFollowUpMessage
            id token threadId content username avatarUrl tts embeds allowedMentions components flags threadName
            appliedTags poll =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"webhooks/{id}/{token}"
                |> Req.bot token
                |> Req.queryOpt "thread_id" threadId
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "content" content
                    |> Dto.property "username" username
                    |> Dto.property "avatar_url" avatarUrl
                    |> Dto.property "tts" tts
                    |> Dto.propertyIf "embeds" embeds
                    |> Dto.propertyIf "allowed_mentions" allowedMentions
                    |> Dto.propertyIf "components" components
                    |> Dto.property "flags" flags
                    |> Dto.propertyIf "thread_name" threadName
                    |> Dto.propertyIf "applied_tags" appliedTags
                    |> Dto.propertyIf "poll" poll
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.GetFollowUpMessage
            id token messageId threadId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"webhooks/{id}/{token}/messages/{messageId}"
                |> Req.bot token
                |> Req.queryOpt "thread_id" threadId
                |> Req.send httpClientFactory
                |> Res.json

        member _.EditFollowUpMessage
            id token messageId threadId content embeds allowedMentions components =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"webhooks/{id}/{token}/messages/{messageId}"
                |> Req.bot token
                |> Req.queryOpt "thread_id" threadId
                |> Req.json (
                    Dto()
                    |> Dto.propertyIf "content" content
                    |> Dto.propertyIf "embeds" embeds
                    |> Dto.propertyIf "allowed_mentions" allowedMentions
                    |> Dto.propertyIf "components" components
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteFollowUpMessage
            id token messageId =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"webhooks/{id}/{token}/messages/{messageId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore
