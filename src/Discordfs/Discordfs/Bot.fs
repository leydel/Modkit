namespace Discordfs

open Discordfs.Rest
open Discordfs.Rest.Modules

type BotModify =
    | Username of string
    | Avatar of string option
    | Banner of string option

type BotGetGuildsParams =
    | Origin of PaginationOrigin
    | Limit of int
    | WithCounts of bool

module Bot =
    let get client = task {
        let! res = Rest.getCurrentUser (DiscordClient.Bot client)
        return DiscordResponse.toOption res
    }

    let modify (optionals: BotModify list) client = task {
        let username = optionals |> List.tryPick (function | BotModify.Username v -> Some v | _ -> None)
        let avatar = optionals |> List.tryPick (function | BotModify.Avatar v -> Some v | _ -> None)
        let banner = optionals |> List.tryPick (function | BotModify.Banner v -> Some v | _ -> None)

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
