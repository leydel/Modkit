namespace Modkit.Discordfs.Resources

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IAutoModerationResource =
    // https://discord.com/developers/docs/resources/auto-moderation#get-auto-moderation-rule
    abstract member GetAutoModerationRule:
        guildId: string ->
        autoModerationRuleId: string ->
        Task<AutoModerationRule>

    // https://discord.com/developers/docs/resources/auto-moderation#create-auto-moderation-rule
    abstract member CreateAutoModerationRule:
        guildId: string ->
        auditLogReason: string option ->
        name: string ->
        eventType: AutoModerationEventType ->
        triggerType: AutoModerationTriggerType ->
        triggerMetadata: AutoModerationTriggerMetadata option ->
        actions: AutoModerationAction list ->
        enabled: bool option ->
        exemptRoles: string list option ->
        exemptChannels: string list option ->
        Task<AutoModerationRule>

    // https://discord.com/developers/docs/resources/auto-moderation#modify-auto-moderation-rule
    abstract member ModifyAutoModerationRule:
        guildId: string ->
        autoModerationRuleId: string ->
        auditLogReason: string option ->
        name: string option ->
        eventType: AutoModerationEventType option ->
        triggerType: AutoModerationTriggerType option ->
        triggerMetadata: AutoModerationTriggerMetadata option ->
        actions: AutoModerationAction list option ->
        enabled: bool option ->
        exemptRoles: string list option ->
        exemptChannels: string list option ->
        Task<AutoModerationRule>

    // https://discord.com/developers/docs/resources/auto-moderation#delete-auto-moderation-rule
    abstract member DeleteAutoModerationRule:
        guildId: string ->
        autoModerationRuleId: string ->
        auditLogReason: string option ->
        Task<unit>

type AutoModerationResource (httpClientFactory: IHttpClientFactory, token: string) =
    interface IAutoModerationResource with
        member _.GetAutoModerationRule
            guildId autoModerationRuleId =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.json

        member _.CreateAutoModerationRule
            guildId auditLogReason name eventType triggerType triggerMetadata actions enabled exemptRoles
            exemptChannels =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/auto-moderation/rules"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                    Dto()
                    |> Dto.property "name" name
                    |> Dto.property "event_type" eventType
                    |> Dto.property "trigger_type" triggerType
                    |> Dto.propertyIf "trigger_metadata" triggerMetadata
                    |> Dto.property "actions" actions
                    |> Dto.property "enabled" enabled
                    |> Dto.propertyIf "exempt_roles" exemptRoles
                    |> Dto.propertyIf "exempt_channels" exemptChannels
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.ModifyAutoModerationRule
            guildId autoModerationRuleId auditLogReason name eventType triggerType triggerMetadata actions enabled
            exemptRoles exemptChannels =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.json (
                        Dto()
                        |> Dto.property "name" name
                        |> Dto.property "event_type" eventType
                        |> Dto.property "trigger_type" triggerType
                        |> Dto.propertyIf "trigger_metadata" triggerMetadata
                        |> Dto.property "actions" actions
                        |> Dto.property "enabled" enabled
                        |> Dto.propertyIf "exempt_roles" exemptRoles
                        |> Dto.propertyIf "exempt_channels" exemptChannels
                        |> Dto.json
                    )
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteAutoModerationRule
            guildId autoModerationRuleId auditLogReason =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
                |> Req.bot token
                |> Req.audit auditLogReason
                |> Req.send httpClientFactory
                |> Res.ignore
