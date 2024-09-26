namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Threading.Tasks

// https://discord.com/developers/docs/resources/poll#get-answer-voters
type IPollResource =
    abstract member GetAnswerVoters:
        channelId: string ->
        messageId: string ->
        answerId: string ->
        after: string option ->
        limit: int option ->
        Task<GetAnswerVotersResponse>

    // https://discord.com/developers/docs/resources/poll#end-poll
    abstract member EndPoll:
        channelId: string ->
        messageId: string ->
        Task<Message>

type PollResource (httpClientFactory, token) =
    interface IPollResource with
        member _.GetAnswerVoters channelId messageId answerId after limit =
            req {
                get $"channels/{channelId}/polls/{messageId}/answers/{answerId}"
                bot token
                query "after" after
                query "limit" (limit >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.EndPoll channelId messageId =
            req {
                post $"channels/{channelId}/polls/{messageId}/expire"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
