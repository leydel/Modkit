namespace Discordfs.Types

open System.Text.Json.Serialization

type OAuth2InstallParams = {
    [<JsonPropertyName "scopes">] Scopes: string list
    [<JsonPropertyName "permissions">] Permissions: string
}

type ApplicationIntegrationTypeConfiguration = {
    [<JsonPropertyName "oauth2_install_params">] Oauth2InstallParams: OAuth2InstallParams option
}

type TeamMember = {
    [<JsonPropertyName "membership_state">] MembershipState: TeamMembershipState
    [<JsonPropertyName "team_id">] TeamId: string
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "role">] Role: string
}

type Team = {
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "members">] Members: TeamMember list
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "owner_user_id">] OwnerUserId: string
}

type Application = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "rpc_origins">] RpcOrigins: string list option
    [<JsonPropertyName "bot_public">] BotPublic: bool
    [<JsonPropertyName "bot_require_code_grant">] BotRequireCodeGrant: bool
    [<JsonPropertyName "bot">] Bot: PartialUser option
    [<JsonPropertyName "terms_of_Service_url">] TermsOfServiceUrl: string option
    [<JsonPropertyName "privacy_policy_url">] PrivacyPolicyUrl: string option
    [<JsonPropertyName "owner">] Owner: PartialUser option
    [<JsonPropertyName "verify_key">] VerifyKey: string
    [<JsonPropertyName "team">] Team: Team option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "guild">] Guild: PartialGuild option
    [<JsonPropertyName "primary_sku_id">] PrimarySkuId: string option
    [<JsonPropertyName "slug">] Slug: string option
    [<JsonPropertyName "cover_image">] CoverImage: string option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "approximate_guild_count">] ApproximateGuildCount: int option
    [<JsonPropertyName "redirect_uris">] RedirectUris: string list option
    [<JsonPropertyName "interactions_endpoint_url">] InteractionsEndpointUrl: string option
    [<JsonPropertyName "role_connections_verification_url">] RoleConnectionsVerificationUrl: string option
    [<JsonPropertyName "tags">] Tags: string list option
    [<JsonPropertyName "install_params">] InstallParams: OAuth2InstallParams option
    [<JsonPropertyName "integration_types_config">] IntegrationTypesConfig: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option
    [<JsonPropertyName "custom_install_url">] CustomInstallUrl: string option
}

type PartialApplication = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "rpc_origins">] RpcOrigins: string list option
    [<JsonPropertyName "bot_public">] BotPublic: bool option
    [<JsonPropertyName "bot_require_code_grant">] BotRequireCodeGrant: bool option
    [<JsonPropertyName "bot">] Bot: PartialUser option
    [<JsonPropertyName "terms_of_Service_url">] TermsOfServiceUrl: string option
    [<JsonPropertyName "privacy_policy_url">] PrivacyPolicyUrl: string option
    [<JsonPropertyName "owner">] Owner: PartialUser option
    [<JsonPropertyName "verify_key">] VerifyKey: string option
    [<JsonPropertyName "team">] Team: Team option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "guild">] Guild: PartialGuild option
    [<JsonPropertyName "primary_sku_id">] PrimarySkuId: string option
    [<JsonPropertyName "slug">] Slug: string option
    [<JsonPropertyName "cover_image">] CoverImage: string option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "approximate_guild_count">] ApproximateGuildCount: int option
    [<JsonPropertyName "redirect_uris">] RedirectUris: string list option
    [<JsonPropertyName "interactions_endpoint_url">] InteractionsEndpointUrl: string option
    [<JsonPropertyName "role_connections_verification_url">] RoleConnectionsVerificationUrl: string option
    [<JsonPropertyName "tags">] Tags: string list option
    [<JsonPropertyName "install_params">] InstallParams: OAuth2InstallParams option
    [<JsonPropertyName "integration_types_config">] IntegrationTypesConfig: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration> option
    [<JsonPropertyName "custom_install_url">] CustomInstallUrl: string option
}
