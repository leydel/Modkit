namespace Discordfs.Types

open System.Text.Json.Serialization

type ReactionCountDetails = {
    [<JsonPropertyName "burst">] Burst: int
    [<JsonPropertyName "normal">] Normal: int
}

type Reaction = {
    [<JsonPropertyName "count">] Count: int
    [<JsonPropertyName "count_details">] CountDetails: ReactionCountDetails
    [<JsonPropertyName "me">] Me: bool
    [<JsonPropertyName "me_burst">] MeBurst: bool
    [<JsonPropertyName "emoji">] Emoji: PartialEmoji
    [<JsonPropertyName "burst_colors">] BurstColors: int list
}
