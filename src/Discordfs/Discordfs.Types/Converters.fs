namespace Discordfs.Types

open System
open System.Text.Json
open System.Text.Json.Serialization

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
                