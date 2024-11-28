namespace Discordfs.Utils

open System

type Snowflake = {
    Increment: int
    WorkerId: int
    ProcessId: int
    Timestamp: DateTime
}

module Snowflake =
    [<Literal>]
    let INCREMENT_MASK  = 0b00000000_00000000_00000000_00000000_00000000_00000000_00001111_11111111L

    [<Literal>]
    let WORKER_ID_MASK  = 0b00000000_00000000_00000000_00000000_00000000_00000001_11110000_00000000L

    [<Literal>]
    let PROCESS_ID_MASK = 0b00000000_00000000_00000000_00000000_00000000_00111110_00000000_00000000L

    [<Literal>]
    let TIMESTAMP_MASK  = 0b11111111_11111111_11111111_11111111_11111111_11000000_00000000_00000000L

    /// Parse a snowflake to read its inner data.
    let tryParse (snowflake: string) =
        match Int64.TryParse snowflake with
        | false, _ -> None
        | true, id -> Some {
            Increment = int (id &&& INCREMENT_MASK >>> 0)
            WorkerId = int (id &&& WORKER_ID_MASK >>> 12)
            ProcessId = int (id &&& PROCESS_ID_MASK >>> 17)
            Timestamp = DateTime.UnixEpoch.AddMilliseconds (float (DISCORD_EPOCH + (id &&& TIMESTAMP_MASK >>> 22)))
        }

    // Create a snowflake based on the given tiemstamp for use in pagination requests.
    let tryCreate (dateTimestamp: DateTime) =
        match DateTimeOffset dateTimestamp |> _.ToUnixTimeMilliseconds() with
        | unix when unix < DISCORD_EPOCH -> None
        | unix -> Some ((unix - DISCORD_EPOCH) <<< 22)
