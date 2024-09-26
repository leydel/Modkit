namespace Discordfs.Types.Utils

open System.Text.Json
open System.Text.Json.Serialization

module FsJson =
    let options = JsonFSharpOptions.Default().WithUnionUntagged().ToJsonSerializerOptions()

    let serialize<'T> (value: 'T) =
        JsonSerializer.Serialize(value, options)

    let deserialize<'T> (json: string) =
        JsonSerializer.Deserialize<'T>(json, options)
