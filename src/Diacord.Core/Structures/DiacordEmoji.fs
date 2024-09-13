namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System
type DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute
open System.Text.Json.Serialization

[<CustomEquality>]
[<NoComparison>]
type DiacordEmoji = {
    [<JsonPropertyName("name")>]
    [<JsonRequired>]
    Name: string

    [<JsonPropertyName("color")>]
    Roles: string list option
}
with
    static member from (emoji: Emoji) = {
        Name = Option.get emoji.Name;
        Roles = emoji.Roles;
    }

    static member diff (e1: DiacordEmoji) (e2: DiacordEmoji) =
        List.collect Option.toList <| [
            Diff.from "name" (Some r1.Name) (Some r2.Name);
            Diff.from "roles" e1.Roles e2.Roles;
        ]

    interface IEquatable<DiacordEmoji> with
        override this.Equals other =
            List.isEmpty <| DiacordEmoji.diff this other
