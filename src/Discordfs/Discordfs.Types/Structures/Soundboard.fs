namespace Discordfs.Types

open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/soundboard#soundboard-sound-object-soundboard-sound-structure
type SoundboardSound = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "sound_id">] SoundId: string
    [<JsonPropertyName "volume">] Volume: double
    [<JsonPropertyName "emoji_id">] EmojiId: string option
    [<JsonPropertyName "emoji_name">] EmojiName: string option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "available">] Available: bool
    [<JsonPropertyName "user">] User: User
}
