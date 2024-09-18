﻿namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System.Collections.Generic
open System.Text.Json.Serialization

type DiacordRole = {
    [<JsonName "diacord_id">] [<JsonRequired>] DiacordId: string
    [<JsonName "name">] [<JsonRequired>] Name: string
    [<JsonName "color">] Color: int option
    [<JsonName "hoist">] Hoist: bool option
    [<JsonName "icon">] Icon: string option
    [<JsonName "unicode_emoji">] UnicodeEmoji: string option
    // TODO: Add `permissions`
    [<JsonName "mentionable">] Mentionable: bool option
}
with
    static member diff (mappings: IDictionary<string, string>) ((a: DiacordRole option), (b: Role option)) =
        let (>>=) ma f = Option.bind f ma
        let (>>.) ma f = Option.map f ma

        let color =
            match (a >>= _.Color) with
            | None -> Some 0
            | v -> v

        let hoist =
            match (a >>= _.Hoist) with
            | None -> Some false
            | v -> v

        // TODO: Add `permissions`

        let mentionable =
            match (a >>= _.Mentionable) with
            | None -> Some false
            | v -> v

        DiffNode.leaf a b [
            Diff.from "name" (a >>. _.Name) (b >>. _.Name);
            Diff.from "color" color (b >>. _.Color);
            Diff.from "hoist" hoist (b >>. _.Hoist);
            Diff.from "icon" (a >>= _.Icon) (b >>= _.Icon);
            Diff.from "unicode_emoji" (a >>= _.UnicodeEmoji) (b >>= _.UnicodeEmoji);
            Diff.from "mentionable" mentionable (b >>. _.Mentionable);
        ]