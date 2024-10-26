namespace Discordfs.Webhook.Payloads

open Discordfs.Types
open Discordfs.Webhook.Common
open Discordfs.Webhook.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<InteractionResponseConverter>)>]
type InteractionResponse =
    | Pong of InteractionResponsePayload<Empty>
    | ChannelMessageWithSource of InteractionResponsePayload<MessageInteractionCallback>
    | DeferredChannelMessageWithSource of InteractionResponsePayload<Empty>
    | DeferredUpdateMessage of InteractionResponsePayload<Empty>
    | UpdateMessage of InteractionResponsePayload<MessageInteractionCallback>
    | ApplicationCommandAutocompleteResult of InteractionResponsePayload<AutocompleteInteractionCallback>
    | Modal of InteractionResponsePayload<ModalInteractionCallback>
    | LaunchActivity of InteractionResponsePayload<Empty>

and InteractionResponseConverter () =
    inherit JsonConverter<InteractionResponse> ()

    override __.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue(&reader)
        if not success then raise (JsonException())

        let interactionCallbackType =
            document.RootElement.GetProperty "type"
            |> _.GetInt32()
            |> enum<InteractionCallbackType>

        let json = document.RootElement.GetRawText()

        match interactionCallbackType with
        | InteractionCallbackType.PONG -> Pong <| Json.deserializeF json
        | InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE -> ChannelMessageWithSource <| Json.deserializeF json
        | InteractionCallbackType.DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE -> DeferredChannelMessageWithSource <| Json.deserializeF json
        | InteractionCallbackType.DEFERRED_UPDATE_MESSAGE -> DeferredUpdateMessage <| Json.deserializeF json
        | InteractionCallbackType.UPDATE_MESSAGE -> UpdateMessage <| Json.deserializeF json
        | InteractionCallbackType.APPLICATION_COMMAND_AUTOCOMPLETE_RESULT -> ApplicationCommandAutocompleteResult <| Json.deserializeF json
        | InteractionCallbackType.MODAL -> Modal <| Json.deserializeF json
        | InteractionCallbackType.LAUNCH_ACTIVITY -> LaunchActivity <| Json.deserializeF json
        | _ -> failwith "Unexpected InteractionCallbackType provided"
                
    override __.Write (writer, value, options) =
        match value with
        | Pong p -> Json.serializeF p |> writer.WriteRawValue
        | ChannelMessageWithSource c -> Json.serializeF c |> writer.WriteRawValue
        | DeferredChannelMessageWithSource d -> Json.serializeF d |> writer.WriteRawValue
        | DeferredUpdateMessage d -> Json.serializeF d |> writer.WriteRawValue
        | UpdateMessage u -> Json.serializeF u |> writer.WriteRawValue
        | ApplicationCommandAutocompleteResult a -> Json.serializeF a |> writer.WriteRawValue
        | Modal m -> Json.serializeF m |> writer.WriteRawValue
        | LaunchActivity l -> Json.serializeF l |> writer.WriteRawValue
