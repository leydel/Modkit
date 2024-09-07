namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpAutoModerationActions =
    // https://discord.com/developers/docs/resources/auto-moderation#get-auto-moderation-rule
    abstract member GetAutoModerationRule:
        guildId: string ->
        autoModerationRuleId: string ->
        Task<AutoModerationRule>

    // https://discord.com/developers/docs/resources/auto-moderation#create-auto-moderation-rule
    abstract member CreateAutoModerationRule:
        guildId: string ->
        auditLogReason: string option ->
        payload: CreateAutoModerationRule ->
        Task<AutoModerationRule>

    // https://discord.com/developers/docs/resources/auto-moderation#modify-auto-moderation-rule
    abstract member ModifyAutoModerationRule:
        guildId: string ->
        autoModerationRuleId: string ->
        auditLogReason: string option ->
        payload: ModifyAutoModerationRule ->
        Task<AutoModerationRule>

    // https://discord.com/developers/docs/resources/auto-moderation#delete-auto-moderation-rule
    abstract member DeleteAutoModerationRule:
        guildId: string ->
        autoModerationRuleId: string ->
        auditLogReason: string option ->
        Task<unit>

type DiscordHttpAutoModerationActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpAutoModerationActions with
        member _.GetAutoModerationRule guildId autoModerationRuleId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.CreateAutoModerationRule guildId auditLogReason payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/auto-moderation/rules"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.ModifyAutoModerationRule guildId autoModerationRuleId auditLogReason payload =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.DeleteAutoModerationRule guildId autoModerationRuleId auditLogReason =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.send httpClientFactory
            |> Res.ignore
