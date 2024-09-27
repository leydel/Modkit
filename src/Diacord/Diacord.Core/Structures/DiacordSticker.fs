namespace Modkit.Diacord.Core.Structures

open Discordfs.Types
open Modkit.Diacord.Core.Types
open System.Collections.Generic
open System.Text.Json.Serialization

type DiacordSticker = {
    [<JsonPropertyName "diacord_id">] [<JsonRequired>] DiacordId: string
    [<JsonPropertyName "name">] [<JsonRequired>] Name: string
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "tags">] [<JsonRequired>] Tags: string
}
with
    static member from (sticker: Sticker) = {
        DiacordId = sticker.Id;
        Name = sticker.Name;
        Description = sticker.Description;
        Tags = sticker.Tags;
    }

    static member diff (mappings: IDictionary<string, string>) ((a: DiacordSticker option), (b: Sticker option)) =
        DiffNode.leaf a b [
            Diff.from "name" (a >>. _.Name) (b >>. _.Name);
            Diff.from "description" (a >>= _.Description) (b >>= _.Description);
            Diff.from "tags" (a >>. _.Tags) (b >>. _.Tags);
        ]
