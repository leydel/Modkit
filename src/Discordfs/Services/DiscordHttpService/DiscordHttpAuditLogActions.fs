namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpAuditLogActions =
    // https://discord.com/developers/docs/resources/audit-log#get-guild-audit-log
    abstract member GetGuildAuditLog:
        guildId: string ->
        userId: string option ->
        actionType: AuditLogEventType option ->
        before: string option ->
        after: string option ->
        limit: int option ->
        Task<AuditLog>

type DiscordHttpAuditLogActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpAuditLogActions with
        member _.GetGuildAuditLog guildId userId actionType before after limit =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"guilds/{guildId}/audit-logs"
            |> Req.bot token
            |> Req.queryOpt "user_id" userId
            |> Req.queryOpt "action_type" (Option.map (fun a -> (int a).ToString()) actionType)
            |> Req.queryOpt "before" before
            |> Req.queryOpt "after" after
            |> Req.queryOpt "limit" (Option.map _.ToString() limit)
            |> Req.send httpClientFactory
            |> Res.body
