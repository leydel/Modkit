namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
open System
open System.Threading.Tasks

type CreateGuildScheduledEvent (
    name:                 string,
    privacy_level:        PrivacyLevelType,
    scheduled_start_time: DateTime,
    entity_type:          ScheduledEntityType,
    ?channel_id:          string,
    ?entity_metadata:     EntityMetadata,
    ?scheduled_end_time:  DateTime,
    ?description:         string,
    ?image:               string,
    ?recurrence_rule:     RecurrenceRule
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            optional "channel_id" channel_id
            optional "entity_metadata" entity_metadata
            required "name" name
            required "privacy_level" privacy_level
            required "scheduled_start_time" scheduled_start_time
            optional "scheduled_end_time" scheduled_end_time
            optional "description" description
            required "entity_type" entity_type
            optional "image" image
            optional "recurrence_rule" recurrence_rule
        }

type ModifyGuildScheduledEvent (
    ?channel_id:           string option,
    ?entity_metadata:      EntityMetadata option,
    ?name:                 string,
    ?privacy_level:        PrivacyLevelType,
    ?scheduled_start_time: DateTime,
    ?scheduled_end_time:   DateTime,
    ?description:          string option,
    ?entity_type:          ScheduledEntityType,
    ?image:                string,
    ?recurrence_rule:      RecurrenceRule option
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            optional "channel_id" channel_id
            optional "entity_metadata" entity_metadata
            optional "name" name
            optional "privacy_level" privacy_level
            optional "scheduled_start_time" scheduled_start_time
            optional "scheduled_end_time" scheduled_end_time
            optional "description" description
            optional "entity_type" entity_type
            optional "image" image
            optional "recurrence_rule" recurrence_rule
        }

type IDiscordHttpGuildScheduledEventActions =
    // https://discord.com/developers/docs/resources/guild-scheduled-event#list-scheduled-events-for-guild
    abstract member ListGuildScheduledEvents:
        guildId: string ->
        withUserCount: bool option ->
        Task<GuildScheduledEvent list>

    // https://discord.com/developers/docs/resources/guild-scheduled-event#create-guild-scheduled-event
    abstract member CreateGuildScheduledEvent:
        guildId: string ->
        auditLogReason: string option ->
        content: CreateGuildScheduledEvent ->
        Task<GuildScheduledEvent>

    // https://discord.com/developers/docs/resources/guild-scheduled-event#get-guild-scheduled-event
    abstract member GetGuildScheduledEvent:
        guildId: string ->
        guildScheduledEventId: string ->
        withUserCount: bool option ->
        Task<GuildScheduledEvent>

    // https://discord.com/developers/docs/resources/guild-scheduled-event#modify-guild-scheduled-event
    abstract member ModifyGuildScheduledEvent:
        guildId: string ->
        guildScheduledEventId: string ->
        auditLogReason: string option ->
        content: ModifyGuildScheduledEvent ->
        Task<GuildScheduledEvent>

    // https://discord.com/developers/docs/resources/guild-scheduled-event#delete-guild-scheduled-event
    abstract member DeleteGuildScheduledEvent:
        guildId: string ->
        guildScheduledEventId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/guild-scheduled-event#get-guild-scheduled-event-users
    abstract member GetGuildScheduledEventUsers:
        guildId: string ->
        guildScheduledEventId: string ->
        limit: int option ->
        withMember: bool option ->
        before: string option ->
        after: string option ->
        Task<GuildScheduledEventUser list>

type DiscordHttpGuildScheduledEventActions (httpClientFactory, token) =
    interface IDiscordHttpGuildScheduledEventActions with
        member _.ListGuildScheduledEvents guildId withUserCount =
            req {
                get $"guilds/{guildId}/scheduled-events"
                bot token
                query "with_user_count" (withUserCount >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.CreateGuildScheduledEvent guildId auditLogReason content =
            req {
                post $"guilds/{guildId}/scheduled-events"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetGuildScheduledEvent guildId guildScheduledEventId withUserCount =
            req {
                get $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
                bot token
                query "with_user_count" (withUserCount >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyGuildScheduledEvent guildId guildScheduledEventId auditLogReason content =
            req {
                patch $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteGuildScheduledEvent guildId guildScheduledEventId =
            req {
                post $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.GetGuildScheduledEventUsers guildId guildScheduledEventId limit withMember before after =
            req {
                get $"guilds/{guildId}/scheduled-events/{guildScheduledEventId}/users"
                bot token
                query "limit" (limit >>. _.ToString())
                query "with_member" (withMember >>. _.ToString())
                query "before" before
                query "after" after
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
