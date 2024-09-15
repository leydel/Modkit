namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System
open System.Text.Json.Serialization

[<CustomEquality>]
[<NoComparison>]
type DiacordSticker = {
    [<JsonName "diacord_id">] [<JsonRequired>] DiacordId: string
    [<JsonName "name">] [<JsonRequired>] Name: string
    [<JsonName "description">] Description: string option
    [<JsonName "tags">] [<JsonRequired>] Tags: string
}
with
    static member diff (s1: DiacordSticker) (s2: Sticker) =
        [
            Diff.from "name" (Some s1.Name) (Some s2.Name);
            Diff.from "description" s1.Description s2.Description;
            Diff.from "tags" (Some s1.Tags) (Some s2.Tags);
        ]

    interface IEquatable<Sticker> with
        override this.Equals other =
            List.exists Diff.isUnchanged (DiacordSticker.diff this other)
            