namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System
open System.Text.Json.Serialization

[<CustomEquality>]
[<NoComparison>]
type DiacordRole = {
    [<JsonName "diacord_id">] [<JsonRequired>] DiacordId: string
    [<JsonName "name">] [<JsonRequired>] Name: string
    [<JsonName "color">] Color: int option

    [<JsonName "hoist">]
    [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>] // TODO: Test this
    [<System.ComponentModel.DefaultValue false>]
    Hoist: bool

    [<JsonName "icon">] Icon: string option
    [<JsonName "unicode_emoji">] UnicodeEmoji: string option
    [<JsonName "permissions">] Permissions: string list option // TODO: Figure out how to set a default value of an empty array

    [<JsonName "mentionable">]
    [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>] // TODO: Test this
    [<System.ComponentModel.DefaultValue false>]
    Mentionable: bool
}
with
    static member diff (r1: DiacordRole) (r2: Role) =
        let color =
            match r2.Color with
            | 0 -> None
            | _ -> Some r2.Color

        let permissions = None // TODO: Convert permissions bitfield into permission string list

        [
            Diff.from "name" (Some r1.Name) (Some r2.Name);
            Diff.from "color" r1.Color color;
            Diff.from "hoist" (Some r1.Hoist) (Some r2.Hoist);
            Diff.from "icon" r1.Icon r2.Icon;
            Diff.from "unicode_emoji" r1.UnicodeEmoji r2.UnicodeEmoji;
            Diff.from "permissions" r1.Permissions permissions;
            Diff.from "mentionable" (Some r1.Mentionable) (Some r2.Mentionable);
        ]

    interface IEquatable<Role> with
        override this.Equals other =
            List.exists Diff.isUnchanged (DiacordRole.diff this other)
