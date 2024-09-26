namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Threading.Tasks

type IAuditLogResource =
    // https://discord.com/developers/docs/resources/audit-log#get-guild-audit-log
    abstract member GetGuildAuditLog:
        guildId: string ->
        userId: string option ->
        actionType: AuditLogEventType option ->
        before: string option ->
        after: string option ->
        limit: int option ->
        Task<AuditLog>

type AuditLogResource (httpClientFactory, token) =
    interface IAuditLogResource with
        member _.GetGuildAuditLog guildId userId actionType before after limit =
            req {
                get "guilds/{guildId}/audit-logs"
                bot token
                query "user_id" userId
                query "action_type" (actionType >>. _.ToString())
                query "before" before
                query "after" after
                query "limit" (limit >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
