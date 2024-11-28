module Discordfs.Utils.Snowflake

open System

/// Parse a snowflake to read its inner data.
let tryParse (snowflake: string) =
    match Int64.TryParse snowflake with
    | false, _ -> None
    | true, id ->
        let increment = id &&& 0b00000000_00000000_00000000_00000000_00000000_00000000_00001111_11111111L
        let workerId  = id &&& 0b00000000_00000000_00000000_00000000_00000000_00000001_11110000_00000000L
        let processId = id &&& 0b00000000_00000000_00000000_00000000_00000000_00111110_00000000_00000000L
        let timestamp = id &&& 0b11111111_11111111_11111111_11111111_11111111_11000000_00000000_00000000L

        let dateTimestamp = DateTime.UnixEpoch.AddMilliseconds (float (DISCORD_EPOCH + timestamp))

        Some {|
            Increment = increment
            WorkerId = workerId
            ProcessId = processId
            Timestamp = dateTimestamp
        |}

// Create a snowflake based on the given tiemstamp for use in pagination requests.
let tryCreate (dateTimestamp: DateTime) =
    match DateTimeOffset dateTimestamp |> _.ToUnixTimeMilliseconds() with
    | unix when unix < DISCORD_EPOCH -> None
    | unix -> Some ((unix - DISCORD_EPOCH) <<< 22)
