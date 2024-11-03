namespace Discordfs.Types

open System.Text.Json
open System.Text.Json.Serialization

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-interaction-response-structure
type InteractionResponsePayload<'a> = {
    [<JsonPropertyName "type">] Type: InteractionCallbackType
    [<JsonPropertyName "data">] Data: 'a
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-messages
type MessageInteractionResponse = {
    [<JsonPropertyName "tts">] Tts: bool option
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "embeds">] Embeds: Embed list option
    [<JsonPropertyName "allowed_mentions">] AllowedMentions: AllowedMentions option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "attachments">] Attachments: PartialAttachment list option
    [<JsonPropertyName "poll">] Poll: Poll option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-autocomplete
type AutocompleteInteractionResponse = {
    [<JsonPropertyName "choices">] Choices: ApplicationCommandOptionChoice list
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-modal
type ModalInteractionResponse = {
    [<JsonPropertyName "custom_id">] CustomId: string
    [<JsonPropertyName "title">] Title: string
    [<JsonPropertyName "components">] Components: Component list
}

type PongResponseEvent = Empty

type ChannelMessageWithSourceResponseEvent = MessageInteractionResponse

type DeferredChannelMessageWithSourceResponseEvent = Empty

type DeferredUpdateMessageResponseEvent = Empty

type UpdateMessageResponseEvent = MessageInteractionResponse

type ApplicationCommandAutocompleteResponseEvent = AutocompleteInteractionResponse

type ModalResponseEvent = ModalInteractionResponse

type LaunchActivityResponseEvent = Empty
