namespace Discordfs.Webhook.Types

open Discordfs.Types
open System.Text.Json.Serialization

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-interaction-response-structure
type InteractionResponsePayload<'a> = {
    [<JsonPropertyName "type">] Type: InteractionCallbackType
    [<JsonPropertyName "data">] Data: 'a
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-messages
type MessageInteractionCallback = {
    [<JsonPropertyName "tts">] Tts: bool option
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "embeds">] Embeds: Embed list option
    [<JsonPropertyName "allowed_mentions">] AllowedMentions: AllowedMentions option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "attachments">] Attachments: Attachment list option
    [<JsonPropertyName "poll">] Poll: Poll option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-autocomplete
type AutocompleteInteractionCallback = {
    [<JsonPropertyName "choices">] Choices: ApplicationCommandOptionChoice list
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-modal
type ModalInteractionCallback = {
    [<JsonPropertyName "custom_id">] CustomId: string
    [<JsonPropertyName "title">] Title: string
    [<JsonPropertyName "components">] Components: Component list
}