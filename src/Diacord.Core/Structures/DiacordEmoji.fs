﻿namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System
type DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute
open System.Text.Json.Serialization

[<CustomEquality>]
[<NoComparison>]
type DiacordEmoji = {
    [<JsonPropertyName("diacord_id")>]
    [<JsonRequired>]
    DiacordId: string

    [<JsonPropertyName("name")>]
    [<JsonRequired>]
    Name: string

    [<JsonPropertyName("color")>]
    Roles: string list option
}
with
    static member diff (e1: DiacordEmoji) (e2: Emoji) =
        List.collect Option.toList <| [
            Diff.from "name" (Some e1.Name) e2.Name;
            Diff.from "roles" e1.Roles e2.Roles;
        ]

    interface IEquatable<Emoji> with
        override this.Equals other =
            List.isEmpty <| DiacordEmoji.diff this other
