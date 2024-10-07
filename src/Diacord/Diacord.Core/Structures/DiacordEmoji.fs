namespace Modkit.Diacord.Core.Structures

open Discordfs.Types
open Modkit.Diacord.Core.Types
open System.Collections.Generic
open System.Text.Json.Serialization

type DiacordEmoji = {
    [<JsonPropertyName "diacord_id">] [<JsonRequired>] DiacordId: string
    [<JsonPropertyName "name">] [<JsonRequired>] Name: string
    [<JsonPropertyName "roles">] Roles: string list option
}
with
    static member from (emoji: Emoji) = {
        DiacordId = match emoji.Id with | Some id -> id | None -> failwith "Invalid emoji provided";
        Name = match emoji.Name with | Some name -> name | None -> failwith "Invalid emoji provided";
        Roles = emoji.Roles;
    }

    static member diff (mappings: IDictionary<string, string>) ((a: DiacordEmoji option), (b: Emoji option)) =
        DiffNode.leaf a b [
            Diff.from "name" (a >>. _.Name) (b >>= _.Name);
            Diff.from "roles" (a >>= _.Roles) (b >>= _.Roles);
        ]
