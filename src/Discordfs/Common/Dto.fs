namespace Modkit.Discordfs.Common

open Modkit.Discordfs.Utils
open System.Collections.Generic
open System.Text.Json.Nodes

type Dto () =
    member val Properties: IDictionary<string, obj> = Dictionary()

    static member property (key: string) (value: 'a) (dto: Dto) =
        dto.Properties.Add(key, value)
        dto

    static member propertyIf (key: string) (value: 'a option) (dto: Dto) =
        if value.IsSome then
            dto.Properties.Add(key, value)
        dto

    static member json (dto: Dto) =
        let obj = JsonObject()

        for property in dto.Properties do
            obj.Add(property.Key, JsonValue.Create property.Value)
        
        FsJson.serialize (JsonValue.Create obj)

    static member jsonPartial (key: string) (dto: Dto) =
        match dto.Properties.TryGetValue key with
        | false, _ -> failwith "Could not find partial to serialize"
        | true, value -> FsJson.serialize (JsonValue.Create value)
