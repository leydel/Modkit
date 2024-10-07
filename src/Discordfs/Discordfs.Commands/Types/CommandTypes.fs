namespace Discordfs.Commands.Types

open Discordfs.Types
open System.Text.Json.Serialization

#nowarn "49"

type InteractionCallback = {
    [<JsonPropertyName "type">] Type: InteractionCallbackType
    [<JsonPropertyName "data">] Data: InteractionCallbackData option
}
with
    static member build(
        Type: InteractionCallbackType,
        ?Data: InteractionCallbackData
    ) = {
        Type = Type;
        Data = Data;
    }
