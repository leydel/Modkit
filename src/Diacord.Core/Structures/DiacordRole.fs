namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System
type DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute
open System.Text.Json.Serialization

[<CustomEquality>]
[<NoComparison>]
type DiacordRole = {
    [<JsonPropertyName("name")>]
    [<JsonRequired>]
    Name: string

    [<JsonPropertyName("color")>]
    Color: int option

    [<JsonPropertyName("hoist")>]
    [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>] // TODO: Test this
    [<DefaultValue(false)>]
    Hoist: bool

    [<JsonPropertyName("icon")>]
    Icon: string option

    [<JsonPropertyName("unicode_emoji")>]
    UnicodeEmoji: string option

    [<JsonPropertyName("permissions")>]
    Permissions: string list option // TODO: Figure out how to set a default value of an empty array

    [<JsonPropertyName("mentionable")>]
    [<JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)>] // TODO: Test this
    [<DefaultValue(false)>]
    Mentionable: bool
}
with
    static member from (role: Role) = {
        Name = role.Name;
        Color = match role.Color with | 0 -> None | _ -> Some role.Color;
        Hoist = role.Hoist;
        Icon = role.Icon;
        UnicodeEmoji = role.UnicodeEmoji;
        Permissions = None; // TODO: Convert permissions bitfield into permission string list
        Mentionable = role.Mentionable;
    }

    static member diff (r1: DiacordRole) (r2: DiacordRole) =
        List.collect Option.toList <| [
            Diff.from "name" (Some r1.Name) (Some r2.Name);
            Diff.from "color" r1.Color r2.Color;
            Diff.from "hoist" (Some r1.Hoist) (Some r2.Hoist);
            Diff.from "icon" r1.Icon r2.Icon;
            Diff.from "unicode_emoji" r1.UnicodeEmoji r2.UnicodeEmoji;
            Diff.from "permissions" r1.Permissions r2.Permissions;
            Diff.from "mentionable" (Some r1.Mentionable) (Some r2.Mentionable);
        ]

    interface IEquatable<DiacordRole> with
        override this.Equals other =
            List.isEmpty <| DiacordRole.diff this other
