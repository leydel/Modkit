namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System
open System.Text.Json.Serialization

[<CustomEquality>]
[<NoComparison>]
type DiacordEmoji = {
    [<JsonName "diacord_id">] [<JsonRequired>] DiacordId: string
    [<JsonName "name">] [<JsonRequired>] Name: string
    [<JsonName "roles">] Roles: string list option
}
with
    static member diff (e1: DiacordEmoji) (e2: Emoji) =
        [
            Diff.from "name" (Some e1.Name) e2.Name;
            Diff.from "roles" e1.Roles e2.Roles;
        ]

    interface IEquatable<Emoji> with
        override this.Equals other =
            List.exists Diff.isUnchanged (DiacordEmoji.diff this other)
