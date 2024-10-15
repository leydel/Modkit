namespace System.Text.Json

open System
open System.Text.Json
open System.Text.Json.Serialization

module Json =
    let private options =
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
