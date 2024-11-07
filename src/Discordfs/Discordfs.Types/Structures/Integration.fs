namespace Discordfs.Types

open System
open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/guild#integration-account-object-integration-account-structure
type GuildIntegrationAccount = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
}

// https://discord.com/developers/docs/resources/guild#integration-application-object-integration-application-structure
type GuildIntegrationApplication = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "bot">] Bot: User option
}

// https://discord.com/developers/docs/resources/guild#integration-object-integration-structure
type GuildIntegration = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: GuildIntegrationType
    [<JsonPropertyName "enabled">] Enabled: bool
    [<JsonPropertyName "syncing">] Syncing: bool option
    [<JsonPropertyName "role_id">] RoleId: string option
    [<JsonPropertyName "enable_emoticons">] EnableEmoticons: bool option
    [<JsonPropertyName "expire_behavior">] ExpireBehavior: IntegrationExpireBehaviorType option
    [<JsonPropertyName "expire_grace_period">] ExpireGracePeriod: int option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "account">] Account: GuildIntegrationAccount
    [<JsonPropertyName "synced_at">] SyncedAt: DateTime option
    [<JsonPropertyName "subscriber_count">] SubscriberCount: int option
    [<JsonPropertyName "revoked">] Revoked: bool option
    [<JsonPropertyName "application">] Application: GuildIntegrationApplication option
    [<JsonPropertyName "scopes">] Scopes: OAuth2Scope list option
}

type PartialGuildIntegration = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "type">] Type: GuildIntegrationType option
    [<JsonPropertyName "enabled">] Enabled: bool option
    [<JsonPropertyName "syncing">] Syncing: bool option
    [<JsonPropertyName "role_id">] RoleId: string option
    [<JsonPropertyName "enable_emoticons">] EnableEmoticons: bool option
    [<JsonPropertyName "expire_behavior">] ExpireBehavior: IntegrationExpireBehaviorType option
    [<JsonPropertyName "expire_grace_period">] ExpireGracePeriod: int option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "account">] Account: GuildIntegrationAccount option
    [<JsonPropertyName "synced_at">] SyncedAt: DateTime option
    [<JsonPropertyName "subscriber_count">] SubscriberCount: int option
    [<JsonPropertyName "revoked">] Revoked: bool option
    [<JsonPropertyName "application">] Application: GuildIntegrationApplication option
    [<JsonPropertyName "scopes">] Scopes: OAuth2Scope list option
}

// TODO: Name, type, account, application may be present even in partial? Shown in audit log entry example. Should check a connection to confirm

// https://discord.com/developers/docs/resources/user#connection-object-connection-structure
type Connection = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: ConnectionServiceType
    [<JsonPropertyName "revoked">] Revoked: bool option
    [<JsonPropertyName "integrations">] Integrations: PartialGuildIntegration list option
    [<JsonPropertyName "verified">] Verified: bool
    [<JsonPropertyName "friend_sync">] FriendSync: bool
    [<JsonPropertyName "show_activity">] ShowActivity: bool
    [<JsonPropertyName "two_way_link">] TwoWayLink: bool
    [<JsonPropertyName "visibility">] Visibility: ConnectionVisibilityType
}
