namespace Discordfs.Utils

open Microsoft.VisualStudio.TestTools.UnitTesting
open System

[<TestClass>]
type SnowflakeTests () =
    [<TestMethod>]
    [<DataRow("")>]
    [<DataRow("123abc")>]
    [<DataRow("invalid")>]
    [<DataRow("!!!")>]
    member _.tryParse_InvalidSnowflake_ReturnsNone (invalidSnowflake: string) =
        // Arrange
        
        // Act
        let res = Snowflake.tryParse invalidSnowflake

        // Assert
        Assert.AreEqual<Snowflake option>(None, res)

    [<TestMethod>]
    [<DataRow(0b00000000_00000000_00000000_00000000_00000011_11001110_00110000_00000001, 1, 3, 7, 15L)>]
    [<DataRow(0b00000000_00000000_00000000_00000000_00000111_10011100_01100000_00000010, 2, 6, 14, 30L)>]
    [<DataRow(0b00000000_00000000_00000000_00000000_00001111_00111000_11000000_00000100, 4, 12, 28, 60L)>]
    member _.tryParse_ValidSnowflake_ReturnsCorrectValues (snowflake: int64, inc: int, wkr: int, prc: int, ts: int64) =
        // Arrange
        let expectedTimestamp = DateTime.UnixEpoch.AddMilliseconds (float (DISCORD_EPOCH + ts))

        let expected = {
            Increment = inc
            WorkerId = wkr
            ProcessId = prc
            Timestamp = expectedTimestamp
        }
        
        // Act
        let res = Snowflake.tryParse (snowflake.ToString())

        // Assert
        match res with
        | None ->
            Assert.Fail()
        | Some snowflake ->
            Assert.AreEqual<int>(expected.Increment, snowflake.Increment)
            Assert.AreEqual<int>(expected.WorkerId, snowflake.WorkerId)
            Assert.AreEqual<int>(expected.ProcessId, snowflake.ProcessId)
            Assert.AreEqual<DateTime>(expected.Timestamp, snowflake.Timestamp)

    [<TestMethod>]
    [<DataRow(-1L)>]
    [<DataRow(-1000L)>]
    [<DataRow(-1000000L)>]
    member _.tryCreate_DateBeforeDiscordEpoch_ReturnsNone (ts: int64) =
        // Arrange
        let date = DateTime.UnixEpoch.AddMilliseconds (float (DISCORD_EPOCH + ts))

        // Act
        let res = Snowflake.tryCreate date

        // Assert
        Assert.AreEqual<int64 option>(None, res)

    [<TestMethod>]
    [<DataRow(1L)>]
    [<DataRow(1000L)>]
    [<DataRow(1000000L)>]
    member _.tryCreate_ValidDate_ReturnsSnowflakeAsLong (ts: int64) =
        // Arrange
        let date = DateTime.UnixEpoch.AddMilliseconds (float (DISCORD_EPOCH + ts))

        // Act
        let res = Snowflake.tryCreate date

        // Assert
        match res with
        | None ->
            Assert.Fail()
        | Some snowflake ->
            Assert.IsTrue(snowflake &&& Snowflake.INCREMENT_MASK >>> 0 = 0L)
            Assert.IsTrue(snowflake &&& Snowflake.WORKER_ID_MASK >>> 12 = 0L)
            Assert.IsTrue(snowflake &&& Snowflake.PROCESS_ID_MASK >>> 17 = 0L)
            Assert.IsTrue(snowflake &&& Snowflake.TIMESTAMP_MASK >>> 22 = ts)
