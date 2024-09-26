namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System
open System.Net.Http
open System.Threading.Tasks

type ModifyCurrentUserVoiceState (
    ?channel_id:                 string,
    ?suppress:                   bool,
    ?request_to_speak_timestamp: DateTime option
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            optional "channel_id" channel_id
            optional "suppress" suppress
            optional "request_to_speak_timestamp" request_to_speak_timestamp
        }

type ModifyUserVoiceState (
    channel_id: string,
    ?suppress:  bool
) =
    inherit Payload(Json) with
        override _.Serialize () = json {
            required "channel_id" channel_id
            optional "suppress" suppress
        }

type IVoiceResource =
    // https://discord.com/developers/docs/resources/voice#list-voice-regions
    abstract member ListVoiceRegions:
        unit ->
        Task<VoiceRegion list>

    // https://discord.com/developers/docs/resources/voice#get-current-user-voice-state
    abstract member GetCurrentUserVoiceState:
        guildId: string ->
        Task<VoiceState>

    // https://discord.com/developers/docs/resources/voice#get-user-voice-state
    abstract member GetUserVoiceState:
        guildId: string ->
        userId: string ->
        Task<VoiceState>

    // https://discord.com/developers/docs/resources/voice#modify-current-user-voice-state
    abstract member ModifyCurrentUserVoiceState:
        guildId: string ->
        content: ModifyCurrentUserVoiceState ->
        Task<unit>

    // https://discord.com/developers/docs/resources/voice#modify-user-voice-state
    abstract member ModifyUserVoiceState:
        guildId: string ->
        userId: string ->
        content: ModifyUserVoiceState ->
        Task<unit>

type VoiceResource (httpClientFactory: IHttpClientFactory, token: string) =
    interface IVoiceResource with
        member _.ListVoiceRegions () =
            req {
                get "voice/regions"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetCurrentUserVoiceState guildId =
            req {
                get $"guilds/{guildId}/voice-states/@me"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetUserVoiceState guildId userId =
            req {
                get $"guilds/{guildId}/voice-states/{userId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ModifyCurrentUserVoiceState guildId content =
            req {
                patch $"guilds/{guildId}/voice-states/@me"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.ModifyUserVoiceState guildId userId content =
            req {
                patch $"guilds/{guildId}/voice-states/{userId}"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.wait
