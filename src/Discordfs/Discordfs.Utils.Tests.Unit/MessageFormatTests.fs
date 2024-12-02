namespace Discordfs.Utils

open Microsoft.VisualStudio.TestTools.UnitTesting
open System

[<TestClass>]
type TimestampStyleTests () =
    [<TestMethod>]
    member _.ToString_ReturnsStringRepresentationofStyle () =
        // Arrange
        let styles = [
            (TimestampStyle.SHORT_TIME, "t");
            (TimestampStyle.LONG_TIME, "T");
            (TimestampStyle.SHORT_DATE, "d");
            (TimestampStyle.LONG_DATE, "D");
            (TimestampStyle.SHORT_DATE_TIME, "f");
            (TimestampStyle.LONG_DATE_TIME, "F");
            (TimestampStyle.RELATIVE_TIME, "R");
        ]

        // Act
        let res = styles |> List.map (fun (style, str) -> (style.ToString(), str))

        // Assert
        for value, str in res do Assert.AreEqual<string>(str, value)

    [<TestMethod>]
    member _.zero_ReturnsCorrectDefaultValue () =
        // Arrange
        let expected = TimestampStyle.SHORT_DATE_TIME

        // Act
        let actual = TimestampStyle.zero()

        // Assert
        Assert.AreEqual<TimestampStyle>(expected, actual)

[<TestClass>]
type GuildNavigationTypeTests () =
    [<TestMethod>]
    member _.ToString_ReturnsStringRepresentationOfType () =
        // Arrange
        let types = [
            (GuildNavigationType.CUSTOMIZE, "customize");
            (GuildNavigationType.BROWSE, "browse");
            (GuildNavigationType.GUIDE, "guide");
            (GuildNavigationType.LINKED_ROLES, "linked-roles");
            (GuildNavigationType.LINKED_ROLE "id", "linked-roles:id");
        ]

        // Act
        let res = types |> List.map (fun (navType, str) -> (navType.ToString(), str))

        // Assert
        for value, str in res do Assert.AreEqual<string>(str, value)

[<TestClass>]
type MessageFormatTests () =
    [<TestMethod>]
    [<DataRow("1", "<@1>")>]
    [<DataRow("1234567890", "<@1234567890>")>]
    member _.user_ReturnsFormatted (userId: string, expected: string) =
        // Arrange

        // Act
        let actual = MessageFormat.user userId

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    [<DataRow("1", "<#1>")>]
    [<DataRow("1234567890", "<#1234567890>")>]
    member _.channel_ReturnsFormatted (channelId: string, expected: string) =
        // Arrange

        // Act
        let actual = MessageFormat.channel channelId

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    [<DataRow("1", "<@&1>")>]
    [<DataRow("1234567890", "<@&1234567890>")>]
    member _.role_ReturnsFormatted (roleId: string, expected: string) =
        // Arrange

        // Act
        let actual = MessageFormat.role roleId

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    [<DataRow("name", "1", "</name:1>")>]
    [<DataRow("example", "1234567890", "</example:1234567890>")>]
    member _.slashCommand_ReturnsFormatted (name: string, id: string, expected: string) =
        // Arrange

        // Act
        let actual = MessageFormat.slashCommand name id

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    [<DataRow("name", "1", "<:name:1>")>]
    [<DataRow("example", "1234567890", "<:example:1234567890>")>]
    member _.customEmoji_ReturnsFormatted (name: string, id: string, expected: string) =
        // Arrange

        // Act
        let actual = MessageFormat.customEmoji name id

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    [<DataRow("name", "1", "<a:name:1>")>]
    [<DataRow("example", "1234567890", "<a:example:1234567890>")>]
    member _.customAnimatedEmoji_ReturnsFormatted (name: string, id: string, expected: string) =
        // Arrange

        // Act
        let actual = MessageFormat.customAnimatedEmoji name id

        // Assert
        Assert.AreEqual<string>(expected, actual)

    [<TestMethod>]
    member _.timestamp_ReturnsFormatted () =
        // Arrange
        let date = DateTime(2024, 11, 28)
        let timestamp = Math.Floor(date.Subtract(DateTime.UnixEpoch).TotalMilliseconds)

        let styles = [
            (TimestampStyle.SHORT_TIME, $"<t:{timestamp}:t>");
            (TimestampStyle.LONG_TIME, $"<t:{timestamp}:T>");
            (TimestampStyle.SHORT_DATE, $"<t:{timestamp}:d>");
            (TimestampStyle.LONG_DATE, $"<t:{timestamp}:D>");
            (TimestampStyle.SHORT_DATE_TIME, $"<t:{timestamp}:f>");
            (TimestampStyle.LONG_DATE_TIME, $"<t:{timestamp}:F>");
            (TimestampStyle.RELATIVE_TIME, $"<t:{timestamp}:R>");
        ]

        // Act
        let res = styles |> List.map (fun (style, str) -> (MessageFormat.timestamp date style, str))

        // Assert
        for value, str in res do Assert.AreEqual<string>(str, value)

    [<TestMethod>]
    [<DataRow("1")>]
    [<DataRow("1234567890")>]
    member _.guildNavigation_ReturnsFormatted (id: string) =
        // Arrange
        let types = [
            (GuildNavigationType.CUSTOMIZE, $"<{id}:customize>");
            (GuildNavigationType.BROWSE, $"<{id}:browse>");
            (GuildNavigationType.GUIDE, $"<{id}:guide>");
            (GuildNavigationType.LINKED_ROLES, $"<{id}:linked-roles>");
            (GuildNavigationType.LINKED_ROLE "id", $"<{id}:linked-roles:id>");
        ]

        // Act
        let res = types |> List.map (fun (navType, str) -> (MessageFormat.guildNavigation id navType, str))

        // Assert
        for value, str in res do Assert.AreEqual<string>(str, value)
