namespace System.Text.Json

open System
open System.Text.Json.Nodes
open System.Text.Json.Serialization

module Json =
    let options =
        JsonFSharpOptions()
            .WithUnionUnwrapFieldlessTags()
            .WithSkippableOptionFields(SkippableOptionFields.Always, deserializeNullAsNone = true)
            .ToJsonSerializerOptions()
    
    let serializeF (value: 'a) =
        JsonSerializer.Serialize(value, options)

    let serialize (value: 'a) =
        try serializeF value |> Some
        with | _ -> None

    let deserializeF<'a> (json: string) =
        JsonSerializer.Deserialize<'a>(json, options)

    let deserialize<'a> (json: string) =
        try deserializeF json |> Some
        with | _ -> None

    let merge (json1: string) (json2: string) =
        let obj1 = JsonNode.Parse(json1).AsObject()
        let obj2 = JsonNode.Parse(json2).AsObject()

        let res = obj1.DeepClone()

        for prop in obj2 do
            res[prop.Key] <- prop.Value.DeepClone()

        res.ToJsonString()

module Converters =
    type UnixEpoch () =
        inherit JsonConverter<DateTime> () with
            override _.Read (reader, typeToConvert, options) =
                DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).DateTime

            override _.Write (writer, value, options) =
                DateTimeOffset(value).ToUnixTimeMilliseconds() |> writer.WriteNumberValue

    type NullUndefinedAsBool() =
        inherit JsonConverter<bool> () with
            override _.Read (reader, typeToConvert, options) =
                match reader.TokenType with
                | JsonTokenType.Null -> true
                | JsonTokenType.None -> false
                | _ -> failwith "Unexpected token received in NullUndefinedAsBoolConverter"

            override _.Write (writer, value, options) =
                raise (NotImplementedException())
                // writer.WriteNullValue() works for true, but false already has the property name written...

/// Used to represent a non-existent value for when used in a generic for JSON serialization.
type Empty = obj option
