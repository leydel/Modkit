namespace Discordfs

open Discordfs.Rest
open Discordfs.Types

type GuildGetAuditLogParams =
    | UserId of string
    | ActionType of AuditLogEventType
    | Origin of PaginationOrigin
    | Limit of int

type GuildCreateAutoModerationRule =
    | TriggerMetadata of AutoModerationTriggerMetadata
    | Enabled of bool
    | ExemptRoles of string list
    | ExemptChannels of string list

type GuildModifyAutoModerationRule =
    | Name of string
    | EventType of AutoModerationEventType
    | TriggerMetadata of AutoModerationTriggerMetadata
    | Actions of AutoModerationAction list
    | Enabled of bool
    | ExemptRoles of string list
    | ExemptChannels of string list

module Guild =
    let getAuditLog guildId (parameters: GuildGetAuditLogParams list) client = task {
        let userId = parameters |> List.tryPick (function | GuildGetAuditLogParams.UserId v -> Some v | _ -> None)
        let actionType = parameters |> List.tryPick (function | GuildGetAuditLogParams.ActionType v -> Some v | _ -> None)
        let before = parameters |> List.tryPick (function | GuildGetAuditLogParams.Origin (PaginationOrigin.Before v) -> Some v | _ -> None)
        let after = parameters |> List.tryPick (function | GuildGetAuditLogParams.Origin (PaginationOrigin.After v) -> Some v | _ -> None)
        let limit = parameters |> List.tryPick (function | GuildGetAuditLogParams.Limit v -> Some v | _ -> None)

        let! res = Rest.getGuildAuditLog guildId userId actionType before after limit client
        return DiscordResponse.toOption res
    }

    let listAutoModerationRules guildId client = task {
        let! res = Rest.listAutoModerationRulesForGuild guildId client
        return DiscordResponse.toOption res
    }

    let getAutoModerationRule guildId ruleId client = task {
        let! res = Rest.getAutoModerationRule guildId ruleId client
        return DiscordResponse.toOption res
    }

    let createAutoModerationRule guildId name eventType triggerType actions (optionals: GuildCreateAutoModerationRule list) auditLogReason client = task {
        let triggerMetadata = optionals |> List.tryPick (function | GuildCreateAutoModerationRule.TriggerMetadata v -> Some v | _ -> None)
        let enabled = optionals |> List.tryPick (function | GuildCreateAutoModerationRule.Enabled v -> Some v | _ -> None)
        let exemptRoles = optionals |> List.tryPick (function | GuildCreateAutoModerationRule.ExemptRoles v -> Some v | _ -> None)
        let exemptChannels = optionals |> List.tryPick (function | GuildCreateAutoModerationRule.ExemptChannels v -> Some v | _ -> None)
        
        let payload = CreateAutoModerationRulePayload(
            name,
            eventType,
            triggerType,
            actions,
            ?trigger_metadata = triggerMetadata,
            ?enabled = enabled,
            ?exempt_roles = exemptRoles,
            ?exempt_channels = exemptChannels
        )

        let! res = Rest.createAutoModerationRule guildId auditLogReason payload client
        return DiscordResponse.toOption res
    }

    let modifyAutoModerationRule guildId ruleId (optionals: GuildModifyAutoModerationRule list) auditLogReason client = task {
        let name = optionals |> List.tryPick (function | GuildModifyAutoModerationRule.Name v -> Some v | _ -> None)
        let eventType = optionals |> List.tryPick (function | GuildModifyAutoModerationRule.EventType v -> Some v | _ -> None)
        let triggerMetadata = optionals |> List.tryPick (function | GuildModifyAutoModerationRule.TriggerMetadata v -> Some v | _ -> None)
        let actions = optionals |> List.tryPick (function | GuildModifyAutoModerationRule.Actions v -> Some v | _ -> None)
        let enabled = optionals |> List.tryPick (function | GuildModifyAutoModerationRule.Enabled v -> Some v | _ -> None)
        let exemptRoles = optionals |> List.tryPick (function | GuildModifyAutoModerationRule.ExemptRoles v -> Some v | _ -> None)
        let exemptChannels = optionals |> List.tryPick (function | GuildModifyAutoModerationRule.ExemptChannels v -> Some v | _ -> None)

        let payload = ModifyAutoModerationRulePayload(
            ?name = name,
            ?event_type = eventType,
            ?trigger_metadata = triggerMetadata,
            ?actions = actions,
            ?enabled = enabled,
            ?exempt_roles = exemptRoles,
            ?exempt_channels = exemptChannels
        )

        let! res = Rest.modifyAutoModerationRule guildId ruleId auditLogReason payload client
        return DiscordResponse.toOption res
    }

    let deleteAutoModerationRule guildId ruleId auditLogReason client = task {
        let! res = Rest.deleteAutoModerationRule guildId ruleId auditLogReason client
        return DiscordResponse.toOption res
    }
