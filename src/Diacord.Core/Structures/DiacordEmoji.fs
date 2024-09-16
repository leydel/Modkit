namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System.Collections.Generic
open System.Text.Json.Serialization

type DiacordEmoji = {
    [<JsonName "diacord_id">] [<JsonRequired>] DiacordId: string
    [<JsonName "name">] [<JsonRequired>] Name: string
    [<JsonName "roles">] Roles: string list option
}
with
    static member diff (mappings: IDictionary<string, string>) ((a: DiacordEmoji option), (b: Emoji option)) =
        let (>>=) ma f = Option.bind f ma
        let (>>.) ma f = Option.map f ma

        DiffNode.leaf a b [
            Diff.from "name" (a >>. _.Name) (b >>= _.Name);
            Diff.from "roles" (a >>= _.Roles) (b >>= _.Roles);
        ]
