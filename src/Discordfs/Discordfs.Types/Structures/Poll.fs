namespace Discordfs.Types

open System
open System.Text.Json.Serialization

type PollMedia = {
    [<JsonPropertyName "text">] Text: string option
    [<JsonPropertyName "emoji">] Emoji: PartialEmoji option
}

type PollAnswer = {
    [<JsonPropertyName "answer_id">] AnswerId: int
    [<JsonPropertyName "poll_media">] PollMedia: PollMedia
}

type PollAnswerCount = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "count">] Count: int
    [<JsonPropertyName "me_voted">] MeVoted: bool
}

type PollResults = {
    [<JsonPropertyName "is_finalized">] IsFinalized: bool
    [<JsonPropertyName "answer_counts">] AnswerCounts: PollAnswerCount list
}

type Poll = {
    [<JsonPropertyName "question">] Question: PollMedia
    [<JsonPropertyName "answers">] Answers: PollAnswer list
    [<JsonPropertyName "expiry">] Expiry: DateTime option
    [<JsonPropertyName "allow_multiselect">] AllowMultiselect: bool
    [<JsonPropertyName "layout_type">] LayoutType: PollLayoutType
    [<JsonPropertyName "results">] Results: PollResults option
}
