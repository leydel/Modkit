namespace Discordfs.Types

open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/audit-log#audit-log-change-object
type AuditLogChange = {
    [<JsonPropertyName "new_value">] NewValue: obj option
    [<JsonPropertyName "old_value">] OldValue: obj option
    [<JsonPropertyName "key">] Key: string
    // TODO: Determine what possible types the values can be and create discriminated union for them
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-optional-audit-entry-info
type AuditLogEntryOptionalInfo = {
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "auto_moderation_rule_name">] AutoModerationRuleName: string option
    [<JsonPropertyName "auto_moderation_rule_trigger_type">] AutoModerationRuleTriggerType: string option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "count">] Count: string option
    [<JsonPropertyName "delete_member_days">] DeleteMemberDays: string option
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "members_removed">] MembersRemoved: string option
    [<JsonPropertyName "message_id">] MessageId: string option
    [<JsonPropertyName "role_name">] RoleName: string option
    [<JsonPropertyName "type">] Type: string option
    [<JsonPropertyName "integration_type">] IntegrationType: string option
    // TODO: Determine if the documentation is incorrect about everything being strings
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-entry-object-audit-log-entry-structure
type AuditLogEntry = {
    [<JsonPropertyName "target_id">] TargetId: string option
    [<JsonPropertyName "changes">] Changes: AuditLogChange list option
    [<JsonPropertyName "user_id">] UserId: string option
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "action_type">] ActionType: AuditLogEventType
    [<JsonPropertyName "options">] Options: AuditLogEntryOptionalInfo option
    [<JsonPropertyName "reason">] Reason: string option
}

// https://discord.com/developers/docs/resources/audit-log#audit-log-object-audit-log-structure
type AuditLog = {
    [<JsonPropertyName "application_commands">] ApplicationCommands: ApplicationCommand list
    [<JsonPropertyName "audit_log_entries">] AuditLogEntries: AuditLogEntry list
    [<JsonPropertyName "auto_moderation_rules">] AutoModerationRules: AutoModerationRule list
    [<JsonPropertyName "guild_scheduled_events">] GuildScheduledEvents: GuildScheduledEvent list
    [<JsonPropertyName "integrations">] Integrations: PartialGuildIntegration list
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "users">] Users: User list
    [<JsonPropertyName "webhooks">] Webhooks: Webhook list
}
