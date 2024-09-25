namespace Modkit.Discordfs.Resources

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
open System.Collections.Generic
open System.Threading.Tasks

type ModifyCurrentUser (
    ?username: string,
    ?avatar:   string option,
    ?banner:   string option
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            optional "username" username
            optional "avatar" avatar
            optional "banner" banner
        }

type CreateDm (
    recipient_id: string
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            required "recipient_id" recipient_id
        }

type CreateGroupDm (
    access_tokens: string list,
    nicks:         IDictionary<string, string> // TODO: Check if this is optional
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            required "access_tokens" access_tokens
            required "nicks" nicks
        }

type UpdateCurrentUserApplicationRoleConnection (
    ?platform_name:     string,
    ?platform_username: string,
    ?metadata:          ApplicationRoleConnectionMetadata
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            optional "platform_name" platform_name
            optional "platform_username" platform_username
            optional "metadata" metadata // TODO: Test how "stringified values" are supposed to work
        }

type IUserResource =
    // https://discord.com/developers/docs/resources/user#get-current-user
    abstract member GetCurrentUser:
        oauth2AccessToken: string option ->
        Task<User>

    // https://discord.com/developers/docs/resources/user#get-user
    abstract member GetUser:
        userId: string ->
        Task<User>

    // https://discord.com/developers/docs/resources/user#modify-current-user
    abstract member ModifyCurrentUser:
        content: ModifyCurrentUser ->
        Task<User>

    // https://discord.com/developers/docs/resources/user#get-current-user-guilds
    abstract member GetCurrentUserGuilds:
        oauth2AccessToken: string option ->
        before: string option ->
        after: string option ->
        limit: int option ->
        withCounts: bool option ->
        Task<Guild list>

    // https://discord.com/developers/docs/resources/user#get-current-user-guild-member
    abstract member GetCurrentUserGuildMember:
        guildId: string ->
        oauth2AccessToken: string option ->
        Task<GuildMember>

    // https://discord.com/developers/docs/resources/user#leave-guild
    abstract member LeaveGuild:
        guildId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/user#create-dm
    abstract member CreateDm:
        content: CreateDm ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/user#create-group-dm
    abstract member CreateGroupDm:
        content: CreateGroupDm ->
        Task<Channel>

    // TODO: Check if above request is deprecated (says it was intended to be used by now-deprecated sdk)

    // https://discord.com/developers/docs/resources/user#get-current-user-connections
    abstract member GetCurrentUserConnections:
        oauth2AccessToken: string ->
        Task<Connection list>

    // https://discord.com/developers/docs/resources/user#get-current-user-connections
    abstract member GetCurrentUserApplicationRoleConnection:
        applicationId: string ->
        oauth2AccessToken: string ->
        Task<ApplicationRoleConnection>

    // https://discord.com/developers/docs/resources/user#update-current-user-application-role-connection
    abstract member UpdateCurrentUserApplicationRoleConnection:
        applicationId: string ->
        oauth2AccessToken: string ->
        content: UpdateCurrentUserApplicationRoleConnection ->
        Task<ApplicationRoleConnection>

type UserResource (httpClientFactory, token) =
    interface IUserResource with
        member _.GetCurrentUser oauth2AccessToken =
            match oauth2AccessToken with
            | Some oauth2AccessToken ->
                req {
                    get "users/@me"
                    oauth oauth2AccessToken
                }
            | None ->
                req {
                    get "users/@me"
                    bot token
                }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetUser userId =
            req {
                get $"users/{userId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyCurrentUser content =
            req {
                patch "users/@me"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetCurrentUserGuilds oauth2AccessToken before after limit withCounts =
            match oauth2AccessToken with
            | Some oauth2AccessToken ->
                req {
                    get "users/@me/guilds"
                    oauth oauth2AccessToken
                    query "before" before
                    query "after" after
                    query "limit" (limit >>. _.ToString())
                    query "with_counts" (withCounts >>. _.ToString())
                }
            | None ->
                req {
                    get "users/@me/guilds"
                    bot token
                    query "before" before
                    query "after" after
                    query "limit" (limit >>. _.ToString())
                    query "with_counts" (withCounts >>. _.ToString())
                }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetCurrentUserGuildMember guildId oauth2AccessToken =
            match oauth2AccessToken with
            | Some oauth2AccessToken ->
                req {
                    get $"users/@me/guilds/{guildId}/member"
                    oauth oauth2AccessToken
                }
            | None ->
                req {
                    get $"users/@me/guilds/{guildId}/member"
                    bot token
                }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.LeaveGuild guildId =
            req {
                delete $"users/@me/guilds/{guildId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.CreateDm content =
            req {
                post "users/@me/channels"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateGroupDm content =
            req {
                post "users/@me/channels"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetCurrentUserConnections oauth2AccessToken =
            req {
                get "users/@me/connections"
                oauth oauth2AccessToken
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetCurrentUserApplicationRoleConnection applicationId oauth2AccessToken =
            req {
                get $"users/@me/applications/{applicationId}/role-connection"
                oauth oauth2AccessToken
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
            
        member _.UpdateCurrentUserApplicationRoleConnection applicationId oauth2AccessToken content =
            req {
                put $"users/@me/applications/{applicationId}/role-connection"
                oauth oauth2AccessToken
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
