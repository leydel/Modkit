namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open System
open System.Text.Json.Serialization

[<CustomEquality>]
[<NoComparison>]
type Role = {
    [<JsonPropertyName("name")>]
    [<JsonRequired>]
    Name: string

    [<JsonPropertyName("color")>]
    Color: int option

    [<JsonPropertyName("hoist")>]
    Hoist: bool option

    [<JsonPropertyName("icon")>]
    Icon: string option

    [<JsonPropertyName("unicode_emoji")>]
    UnicodeEmoji: string option

    [<JsonPropertyName("permissions")>]
    Permissions: string list option

    [<JsonPropertyName("mentionable")>]
    Mentionable: bool option
}
with
    static member diff (r1: Role) (r2: Role) =
        List.collect Option.toList <| [
            Diff.from "name" (Some r1.Name) (Some r2.Name);
            Diff.from "color" r1.Color r2.Color;
            Diff.from "hoist" r1.Hoist r2.Hoist;
            Diff.from "icon" r1.Icon r2.Icon;
            Diff.from "unicode_emoji" r1.UnicodeEmoji r2.UnicodeEmoji;
            Diff.from "permissions" r1.Permissions r2.Permissions;
            Diff.from "mentionable" r1.Mentionable r2.Mentionable;
        ]

    interface IEquatable<Role> with
        override this.Equals other =
            List.isEmpty <| Role.diff this other
