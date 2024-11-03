namespace Discordfs.Webhook.Types

open Discordfs.Types
open System.Text.Json

type PongResponseEvent = Empty

type ChannelMessageWithSourceResponseEvent = MessageInteractionResponse

type DeferredChannelMessageWithSourceResponseEvent = Empty

type DeferredUpdateMessageResponseEvent = Empty

type UpdateMessageResponseEvent = MessageInteractionResponse

type ApplicationCommandAutocompleteResponseEvent = AutocompleteInteractionResponse

type ModalResponseEvent = ModalInteractionResponse

type LaunchActivityResponseEvent = Empty
