namespace Discordfs.Types.Utils

open System.Text.Json
open System.Text.Json.Serialization

module FsJson =
    let serialize<'T> (value: 'T) =
        JsonSerializer.Serialize(value)

    let deserialize<'T> (json: string) =
        JsonSerializer.Deserialize<'T>(json)
