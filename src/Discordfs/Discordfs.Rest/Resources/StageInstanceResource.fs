namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Threading.Tasks

type CreateStageInstance (
    channel_id:                string,
    topic:                     string,
    ?privacy_level:            PrivacyLevelType,
    ?send_start_notification:  bool,
    ?guild_scheduled_event_id: string
) =
    inherit Payload() with
        override _.Content = json {
            required "channel_id" channel_id
            required "topic" topic
            optional "privacy_level" privacy_level
            optional "send_start_notification" send_start_notification
            optional "guild_scheduled_event_id" guild_scheduled_event_id
        }

type ModifyStageInstance (
    ?topic:         string,
    ?privacy_level: PrivacyLevelType
) =
    inherit Payload() with
        override _.Content = json {
            optional "topic" topic
            optional "privacy_level" privacy_level
        }

type IStageInstanceResource =
    // https://discord.com/developers/docs/resources/stage-instance#create-stage-instance
    abstract member CreateStageInstance:
        auditLogReason: string option ->
        content: CreateStageInstance ->
        Task<StageInstance>

    // https://discord.com/developers/docs/resources/stage-instance#get-stage-instance
    abstract member GetStageInstance:
        channelId: string ->
        Task<StageInstance>

    // https://discord.com/developers/docs/resources/stage-instance#modify-stage-instance
    abstract member ModifyStageInstance:
        channelId: string ->
        auditLogReason: string option ->
        content: ModifyStageInstance ->
        Task<StageInstance>

    // https://discord.com/developers/docs/resources/stage-instance#delete-stage-instance
    abstract member DeleteStageInstance:
        channelId: string ->
        auditLogReason: string option ->
        Task<unit>

type StageInstanceResource (httpClientFactory, token) =
    interface IStageInstanceResource with
        member _.CreateStageInstance auditLogReason content =
            req {
                post "stage_instances"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetStageInstance channelId =
            req {
                get $"stage-instances/{channelId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyStageInstance channelId auditLogReason content =
            req {
                patch $"stage-instances/{channelId}"
                bot token
                audit auditLogReason
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteStageInstance channelId auditLogReason =
            req {
                delete $"stage-instances/{channelId}"
                bot token
                audit auditLogReason
            }
            |> Http.send httpClientFactory
            |> Task.wait
