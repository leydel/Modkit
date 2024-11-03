namespace Discordfs.Webhook.Payloads

open Discordfs.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<InteractionRespondEventConverter>)>]
type InteractionRespondEvent =
    | PONG                                    of InteractionResponsePayload<PongResponseEvent>
    | CHANNEL_MESSAGE_WITH_SOURCE             of InteractionResponsePayload<ChannelMessageWithSourceResponseEvent>
    | DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE    of InteractionResponsePayload<DeferredChannelMessageWithSourceResponseEvent>
    | DEFERRED_UPDATE_MESSAGE                 of InteractionResponsePayload<DeferredUpdateMessageResponseEvent>
    | UPDATE_MESSAGE                          of InteractionResponsePayload<UpdateMessageResponseEvent>
    | APPLICATION_COMMAND_AUTOCOMPLETE_RESULT of InteractionResponsePayload<ApplicationCommandAutocompleteResponseEvent>
    | MODAL                                   of InteractionResponsePayload<ModalResponseEvent>
    | LAUNCH_ACTIVITY                         of InteractionResponsePayload<LaunchActivityResponseEvent>

and InteractionRespondEventConverter () =
    inherit JsonConverter<InteractionRespondEvent> ()

    override __.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue(&reader)
        if not success then raise (JsonException())

        let interactionType =
            document.RootElement.GetProperty "type"
            |> _.GetInt32()
            |> enum<InteractionCallbackType>

        let json = document.RootElement.GetRawText()

        match interactionType with
        | InteractionCallbackType.PONG -> PONG <| Json.deserializeF json
        | InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE -> CHANNEL_MESSAGE_WITH_SOURCE <| Json.deserializeF json
        | InteractionCallbackType.DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE -> DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE <| Json.deserializeF json
        | InteractionCallbackType.DEFERRED_UPDATE_MESSAGE -> DEFERRED_UPDATE_MESSAGE <| Json.deserializeF json
        | InteractionCallbackType.UPDATE_MESSAGE -> UPDATE_MESSAGE <| Json.deserializeF json
        | InteractionCallbackType.APPLICATION_COMMAND_AUTOCOMPLETE_RESULT -> APPLICATION_COMMAND_AUTOCOMPLETE_RESULT <| Json.deserializeF json
        | InteractionCallbackType.MODAL -> MODAL <| Json.deserializeF json
        | InteractionCallbackType.LAUNCH_ACTIVITY -> LAUNCH_ACTIVITY <| Json.deserializeF json
        | _ -> failwith "Unexpected InteractionCallbackType provided" // TODO: Handle gracefully for unfamiliar events
                
    override __.Write (writer, value, options) =
        match value with
        | PONG p -> Json.serializeF p |> writer.WriteRawValue
        | CHANNEL_MESSAGE_WITH_SOURCE c -> Json.serializeF c |> writer.WriteRawValue
        | DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE d -> Json.serializeF d |> writer.WriteRawValue
        | DEFERRED_UPDATE_MESSAGE d -> Json.serializeF d |> writer.WriteRawValue
        | UPDATE_MESSAGE u -> Json.serializeF u |> writer.WriteRawValue
        | APPLICATION_COMMAND_AUTOCOMPLETE_RESULT a -> Json.serializeF a |> writer.WriteRawValue
        | MODAL m -> Json.serializeF m |> writer.WriteRawValue
        | LAUNCH_ACTIVITY l -> Json.serializeF l |> writer.WriteRawValue
