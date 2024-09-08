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
            |> Req.queryOpt "action_type" (match actionType with | Some a -> Some ((int a).ToString()) | None -> None)
            |> Req.queryOpt "before" before
            |> Req.queryOpt "after" after
            |> Req.queryOpt "limit" (match limit with | Some l -> Some (l.ToString()) | None -> None)
            |> Req.send httpClientFactory
            |> Res.body
