namespace Modkit.Discordfs.Resources

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
open System.Threading.Tasks

type IInviteResource =
    abstract member GetInvite:
        inviteCode: string ->
        withCounts: bool option ->
        withExpiration: bool option ->
        guildScheduledEventId: string option ->
        Task<Invite>

    abstract member DeleteInvite:
        inviteCode: string ->
        auditLogReason: string option ->
        Task<Invite>

type InviteResource (httpClientFactory, token) =
    interface IInviteResource with
        member _.GetInvite inviteCode withCounts withExpiration guildScheduledEventId =
            req {
                get $"invites/{inviteCode}"
                bot token
                query "with_counts" (withCounts >>. _.ToString())
                query "with_expiration" (withExpiration >>. _.ToString())
                query "guild_scheduled_event_id" guildScheduledEventId
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteInvite inviteCode auditLogReason =
            req {
                delete $"invites/{inviteCode}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
