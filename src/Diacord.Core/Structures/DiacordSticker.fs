namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System
type DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute
open System.Text.Json.Serialization

[<CustomEquality>]
[<NoComparison>]
type DiacordSticker = {
    [<JsonPropertyName("diacord_id")>]
    [<JsonRequired>]
    DiacordId: string

    [<JsonPropertyName("name")>]
    [<JsonRequired>]
    Name: string

    [<JsonPropertyName("description")>]
    Description: string option

    [<JsonPropertyName("tags")>]
    [<JsonRequired>]
    Tags: string
}
with
    static member diff (s1: DiacordSticker) (s2: Sticker) =
        List.collect Option.toList <| [
            Diff.from "name" (Some s1.Name) (Some s2.Name);
            Diff.from "description" s1.Description s2.Description;
            Diff.from "tags" (Some s1.Tags) (Some s2.Tags);
        ]

    interface IEquatable<Sticker> with
        override this.Equals other =
            List.isEmpty <| DiacordSticker.diff this other
