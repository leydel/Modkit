namespace Discordfs.Webhook.Payloads

open Discordfs.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<InteractionReceiveEventConverter>)>]
type InteractionReceiveEvent =
    | PING                             of Interaction
    | APPLICATION_COMMAND              of Interaction
    | MESSAGE_COMPONENT                of Interaction
    | APPLICATION_COMMAND_AUTOCOMPLETE of Interaction
    | MODAL_SUBMIT                     of Interaction

and InteractionReceiveEventConverter () =
    inherit JsonConverter<InteractionReceiveEvent> ()

    override __.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue(&reader)
        if not success then raise (JsonException())

        let interactionType =
            document.RootElement.GetProperty "type"
            |> _.GetInt32()
            |> enum<InteractionType>

        let json = document.RootElement.GetRawText()

        match interactionType with
        | InteractionType.PING -> PING <| Json.deserializeF json
        | InteractionType.APPLICATION_COMMAND -> APPLICATION_COMMAND <| Json.deserializeF json
        | InteractionType.MESSAGE_COMPONENT -> MESSAGE_COMPONENT <| Json.deserializeF json
        | InteractionType.APPLICATION_COMMAND_AUTOCOMPLETE -> APPLICATION_COMMAND_AUTOCOMPLETE <| Json.deserializeF json
        | InteractionType.MODAL_SUBMIT -> MODAL_SUBMIT <| Json.deserializeF json
        | _ -> failwith "Unexpected InteractionType provided" // TODO: Handle gracefully for unfamiliar events
                
    override __.Write (writer, value, options) =
        match value with
        | PING p -> Json.serializeF p |> writer.WriteRawValue
        | APPLICATION_COMMAND a -> Json.serializeF a |> writer.WriteRawValue
        | MESSAGE_COMPONENT m -> Json.serializeF m |> writer.WriteRawValue
        | APPLICATION_COMMAND_AUTOCOMPLETE a -> Json.serializeF a |> writer.WriteRawValue
        | MODAL_SUBMIT m -> Json.serializeF m |> writer.WriteRawValue
        