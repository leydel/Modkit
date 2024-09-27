namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Threading.Tasks

type CreateAutoModerationRule (
    name: string,
    event_type: AutoModerationEventType,
    trigger_type: AutoModerationTriggerType,
    actions: AutoModerationAction list,
    ?trigger_metadata: AutoModerationTriggerMetadata,
    ?enabled: bool,
    ?exempt_roles: string list,
    ?exempt_channels: string list
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "event_type" event_type
            required "trigger_type" trigger_type
            optional "trigger_metadata" trigger_metadata
            required "actions" actions
            optional "enabled" enabled
            optional "exempt_roles" exempt_roles
            optional "exempt_channels" exempt_channels
        }

type ModifyAutoModerationRule (
    ?name: string,
    ?event_type: AutoModerationEventType,
    ?trigger_metadata: AutoModerationTriggerMetadata,
    ?actions: AutoModerationAction list,
    ?enabled: bool,
    ?exempt_roles: string list,
    ?exempt_channels: string list
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "event_type" event_type
            optional "trigger_metadata" trigger_metadata
            optional "actions" actions
            optional "enabled" enabled
            optional "exempt_roles" exempt_roles
            optional "exempt_channels" exempt_channels
        }

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
        content: CreateAutoModerationRule ->
        Task<AutoModerationRule>

    // https://discord.com/developers/docs/resources/auto-moderation#modify-auto-moderation-rule
    abstract member ModifyAutoModerationRule:
        guildId: string ->
        autoModerationRuleId: string ->
        auditLogReason: string option ->
        content: ModifyAutoModerationRule ->
        Task<AutoModerationRule>

    // https://discord.com/developers/docs/resources/auto-moderation#delete-auto-moderation-rule
    abstract member DeleteAutoModerationRule:
        guildId: string ->
        autoModerationRuleId: string ->
        auditLogReason: string option ->
        Task<unit>

type AutoModerationResource (httpClientFactory, token) =
    interface IAutoModerationResource with
        member _.GetAutoModerationRule guildId autoModerationRuleId =
            req {
                get $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateAutoModerationRule guildId auditLogReason content =
            req {
                post $"guilds/{guildId}/auto-moderation/rules"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyAutoModerationRule guildId autoModerationRuleId auditLogReason content =
            req {
                patch $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteAutoModerationRule guildId autoModerationRuleId auditLogReason =
            req {
                delete $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
