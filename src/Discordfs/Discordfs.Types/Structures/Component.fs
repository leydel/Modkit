namespace Discordfs.Types

open System.Text.Json
open System.Text.Json.Serialization

type SelectMenuOption = {
    [<JsonPropertyName "label">] Label: string
    [<JsonPropertyName "value">] Value: string
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "emoji">] Emoji: Emoji option
    [<JsonPropertyName "default">] Default: bool option
}

type SelectMenuDefaultValue = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: string
}

[<JsonConverter(typeof<ComponentConverter>)>]
type Component =
    | ACTION_ROW of ActionRowComponent
    | BUTTON of ButtonComponent
    | SELECT_MENU of SelectMenuComponent
    | TEXT_INPUT of TextInputComponent

and ComponentConverter () =
    inherit JsonConverter<Component> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue(&reader)
        if not success then raise (JsonException())

        let componentType = document.RootElement.GetProperty "type" |> _.GetInt32() |> enum<ComponentType>
        let json = document.RootElement.GetRawText()

        match componentType with
        | ComponentType.ACTION_ROW -> Component.ACTION_ROW <| Json.deserializeF<ActionRowComponent> json
        | ComponentType.BUTTON -> Component.BUTTON <| Json.deserializeF<ButtonComponent> json
        | ComponentType.STRING_SELECT
        | ComponentType.USER_SELECT
        | ComponentType.ROLE_SELECT
        | ComponentType.MENTIONABLE_SELECT
        | ComponentType.CHANNEL_SELECT -> Component.SELECT_MENU <| Json.deserializeF<SelectMenuComponent> json
        | ComponentType.TEXT_INPUT -> Component.TEXT_INPUT <| Json.deserializeF<TextInputComponent> json
        | _ -> failwith "Unexpected ComponentType provided"

    override _.Write (writer, value, options) =
        match value with
        | Component.ACTION_ROW a -> Json.serializeF a |> writer.WriteRawValue
        | Component.BUTTON b -> Json.serializeF b |> writer.WriteRawValue
        | Component.SELECT_MENU s -> Json.serializeF s |> writer.WriteRawValue
        | Component.TEXT_INPUT t -> Json.serializeF t |> writer.WriteRawValue

and ActionRowComponent = {
    [<JsonPropertyName "type">] Type: ComponentType
    [<JsonPropertyName "components">] Components: Component list
}

and ButtonComponent = {
    [<JsonPropertyName "type">] Type: ComponentType
    [<JsonPropertyName "style">] Style: ButtonStyle
    [<JsonPropertyName "label">] Label: string
    [<JsonPropertyName "emoji">] Emoji: Emoji option
    [<JsonPropertyName "custom_id">] CustomId: string option
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "disabled">] Disabled: bool option
}

and SelectMenuComponent = {
    [<JsonPropertyName "type">] Type: ComponentType
    [<JsonPropertyName "custom_id">] CustomId: string
    [<JsonPropertyName "options">] Options: SelectMenuOption list option
    [<JsonPropertyName "channel_types">] ChannelTypes: ChannelType list option
    [<JsonPropertyName "placeholder">] Placeholder: string option
    [<JsonPropertyName "default_values">] DefaultValues: SelectMenuDefaultValue option
    [<JsonPropertyName "min_values">] MinValues: int option
    [<JsonPropertyName "max_values">] MaxValues: int option
    [<JsonPropertyName "disabled">] Disabled: bool option
}

and TextInputComponent = {
    [<JsonPropertyName "type">] Type: ComponentType
    [<JsonPropertyName "custom_id">] CustomId: string
    [<JsonPropertyName "style">] Style: TextInputStyle
    [<JsonPropertyName "label">] Label: string
    [<JsonPropertyName "min_length">] MinLength: int option
    [<JsonPropertyName "max_length">] MaxLength: int option
    [<JsonPropertyName "required">] Required: bool option
    [<JsonPropertyName "value">] Value: string option
    [<JsonPropertyName "placeholder">] Placeholder: string option
}
