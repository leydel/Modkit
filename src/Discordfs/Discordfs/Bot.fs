namespace Discordfs

open Discordfs.Rest
open Discordfs.Rest.Modules
open Discordfs.Types

type BotEditApplication =
    | CustomInstallUrl of string
    | Description of string
    | RoleConnectionVerificationUrl of string
    | InstallParams of OAuth2InstallParams
    | IntegrationTypesConfig of (ApplicationIntegrationType * ApplicationIntegrationTypeConfiguration) seq
    | Flags of int
    | Icon of string option
    | CoverImage of string option
    | InteractionsEndpointUrl of string
    | AppTags of string list
    | EventWebhooksUrl of string
    | EventWebhooksStatus of WebhookEventStatus
    | EventWebhooksTypes of WebhookEventType list

type BotModifyUser =
    | Username of string
    | Avatar of string option
    | Banner of string option

type BotGetGuildsParams =
    | Origin of PaginationOrigin
    | Limit of int
    | WithCounts of bool

module Bot =
    let getApplication client = task {
        let! res = Rest.getCurrentApplication client
        return DiscordResponse.toOption res
    }

    let editApplication (optionals: BotEditApplication list) client = task {
        let customInstallUrl = optionals |> List.tryPick (function | BotEditApplication.CustomInstallUrl v -> Some v | _ -> None)
        let description = optionals |> List.tryPick (function | BotEditApplication.Description v -> Some v | _ -> None)
        let roleConnectionVerificationUrl = optionals |> List.tryPick (function | BotEditApplication.RoleConnectionVerificationUrl v -> Some v | _ -> None)
        let installParams = optionals |> List.tryPick (function | BotEditApplication.InstallParams v -> Some v | _ -> None)
        let integrationTypesConfig = optionals |> List.tryPick (function | BotEditApplication.IntegrationTypesConfig v -> Some <| dict v | _ -> None)
        let flags = optionals |> List.tryPick (function | BotEditApplication.Flags v -> Some v | _ -> None)
        let icon = optionals |> List.tryPick (function | BotEditApplication.Icon v -> Some v | _ -> None)
        let coverImage = optionals |> List.tryPick (function | BotEditApplication.CoverImage v -> Some v | _ -> None)
        let interactionsEndpointUrl = optionals |> List.tryPick (function | BotEditApplication.InteractionsEndpointUrl v -> Some v | _ -> None)
        let tags = optionals |> List.tryPick (function | BotEditApplication.AppTags v -> Some v | _ -> None)
        let eventWebhooksUrl = optionals |> List.tryPick (function | BotEditApplication.EventWebhooksUrl v -> Some v | _ -> None)
        let eventWebhooksStatus = optionals |> List.tryPick (function | BotEditApplication.EventWebhooksStatus v -> Some v | _ -> None)
        let eventWebhooksTypes = optionals |> List.tryPick (function | BotEditApplication.EventWebhooksTypes v -> Some v | _ -> None)
        
        let payload = EditCurrentApplicationPayload(
            ?custom_install_url = customInstallUrl,
            ?description = description,
            ?role_connection_verification_url = roleConnectionVerificationUrl,
            ?install_params = installParams,
            ?integration_types_config = integrationTypesConfig,
            ?flags = flags,
            ?icon = icon,
            ?cover_image = coverImage,
            ?interactions_endpoint_url = interactionsEndpointUrl,
            ?tags = tags,
            ?event_webhooks_url = eventWebhooksUrl,
            ?event_webhooks_status = eventWebhooksStatus,
            ?event_webhooks_types = eventWebhooksTypes
        )

        let! res = Rest.editCurrentApplication payload client
        return DiscordResponse.toOption res
    }

    let getUser client = task {
        let! res = Rest.getCurrentUser (DiscordClient.Bot client)
        return DiscordResponse.toOption res
    }

    let modifyUser (optionals: BotModifyUser list) client = task {
        let username = optionals |> List.tryPick (function | BotModifyUser.Username v -> Some v | _ -> None)
        let avatar = optionals |> List.tryPick (function | BotModifyUser.Avatar v -> Some v | _ -> None)
        let banner = optionals |> List.tryPick (function | BotModifyUser.Banner v -> Some v | _ -> None)

        let payload = ModifyCurrentUserPayload(?username = username, ?avatar = avatar, ?banner = banner)

        let! res = Rest.modifyCurrentUser payload client
        return DiscordResponse.toOption res
    }

    let leaveGuild guildId client = task {
        let! res = Rest.leaveGuild guildId client
        return DiscordResponse.toOption res
    }

    let getGuilds (parameters: BotGetGuildsParams list) client = task {
        let before = parameters |> List.tryPick (function | BotGetGuildsParams.Origin (PaginationOrigin.Before v) -> Some v | _ -> None)
        let after = parameters |> List.tryPick (function | BotGetGuildsParams.Origin (PaginationOrigin.After v) -> Some v | _ -> None)
        let limit = parameters |> List.tryPick (function | BotGetGuildsParams.Limit v -> Some v | _ -> None)
        let withCounts = parameters |> List.tryPick (function | BotGetGuildsParams.WithCounts v -> Some v | _ -> None)

        let! res = Rest.getCurrentUserGuilds before after limit withCounts (DiscordClient.Bot client)
        return DiscordResponse.toOption res
    }

    let getGuildMember guildId client = task {
        let! res = Rest.getCurrentUserGuildMember guildId (DiscordClient.Bot client)
        return DiscordResponse.toOption res
    }
