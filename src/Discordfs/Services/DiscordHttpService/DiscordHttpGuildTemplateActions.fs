namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
open System.Threading.Tasks

type CreateGuildFromTemplate (
    name:  string,
    ?icon: string
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            required "name" name
            optional "icon" icon
        }

type CreateGuildTemplate (
    name:         string,
    ?description: string
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            required "name" name
            optional "description" description
        }

type ModifyGuildTemplate (
    ?name:        string,
    ?description: string
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            optional "name" name
            optional "description" description
        }

type IDiscordHttpGuildTemplateActions =
    abstract member GetGuildTemplate:
        templateCode: string ->
        Task<GuildTemplate>

    abstract member CreateGuildFromTemplate:
        templateCode: string ->
        content: CreateGuildFromTemplate ->
        Task<Guild>

    abstract member GetGuildTemplates:
        guildId: string ->
        Task<GuildTemplate list>

    abstract member CreateGuildTemplate:
        guildId: string ->
        content: CreateGuildTemplate ->
        Task<GuildTemplate>

    abstract member SyncGuildTemplate:
        guildId: string ->
        templateCode: string ->
        Task<GuildTemplate>

    abstract member ModifyGuildTemplate:
        guildId: string ->
        templateCode: string ->
        content: ModifyGuildTemplate ->
        Task<GuildTemplate>

    abstract member DeleteGuildTemplate:
        guildId: string ->
        templateCode: string ->
        Task<GuildTemplate>

type DiscordHttpGuildTemplateActions (httpClientFactory, token) =
    interface IDiscordHttpGuildTemplateActions with
        member _.GetGuildTemplate templateCode =
            req {
                get $"guilds/templates/{templateCode}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateGuildFromTemplate templateCode content =
            req {
                post $"guilds/templates/{templateCode}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildTemplates guildId =
            req {
                get $"guilds/{guildId}/templates"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateGuildTemplate guildId content =
            req {
                post $"guilds/{guildId}/templates"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.SyncGuildTemplate guildId templateCode =
            req {
                put $"guilds/{guildId}/templates/{templateCode}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildTemplate guildId templateCode content =
            req {
                patch $"guilds/{guildId}/templates/{templateCode}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteGuildTemplate guildId templateCode =
            req {
                delete $"guilds/{guildId}/templates/{templateCode}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
