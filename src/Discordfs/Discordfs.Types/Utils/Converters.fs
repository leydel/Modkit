namespace Discordfs.Types.Utils

open System
open System.Text.Json
open System.Text.Json.Serialization

module Converters =
    type UnixEpoch () =
        inherit JsonConverter<DateTime> () with
            override __.Read (reader: byref<Utf8JsonReader>, typeToConvert: Type, options: JsonSerializerOptions) =
                DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).DateTime

            override __.Write (writer: Utf8JsonWriter, value: DateTime, options: JsonSerializerOptions) =
                DateTimeOffset(value).ToUnixTimeMilliseconds() |> writer.WriteNumberValue
