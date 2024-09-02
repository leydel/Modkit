namespace Modkit.Discordfs.Commands

open Microsoft.VisualStudio.TestTools.UnitTesting
open Modkit.Discordfs.Types

[<TestClass>]
type OptionsTests () =
    [<TestMethod>]
    [<Ignore>]
    member _.SubCommand_FindsCorrectSubCommand () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.SubCommand_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.SubCommand_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.SubCommandGroup_FindsCorrectSubCommandGroup () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.SubCommandGroup_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.SubCommandGroup_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.String_FindsCorrectString () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.String_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.String_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Integer_FindsCorrectInteger () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Integer_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Integer_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Boolean_FindsCorrectBoolean () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Boolean_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Boolean_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.User_FindsCorrectUser () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.User_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.User_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO

    [<TestMethod>]
    member _.Channel_FindsCorrectChannel () =
        // Arrange
        let name = "channel"
        let value = "channelId"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.CHANNEL,
                Value = CommandInteractionDataOptionValue.String value
            )
        ]

        // Act
        let channelId =
            match options with
            | Options.Channel name channelId -> channelId
            | _ -> failwith "Could not find channel"

        // Assert
        Assert.AreEqual(value, channelId)

    [<TestMethod>]
    member _.Channel_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "channel"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.ROLE,
                Value = CommandInteractionDataOptionValue.String "roleId"
            )
        ]

        // Act
        let res =
            match options with
            | Options.Channel "channel" _ -> failwith "Unexpectedly found non-existent channel"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Channel_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.Channel "channel" _ -> failwith "Unexpectedly found non-existent channel"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    [<Ignore>]
    member _.Role_FindsCorrectRole () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Role_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Role_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Mentionable_FindsCorrectMentionable () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Mentionable_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Mentionable_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Number_FindsCorrectNumber () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Number_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Number_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Attachment_FindsCorrectAttachment () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Attachment_ReturnsNoneIfOptionIncorrectType () =
        Assert.Fail() // TODO

    [<TestMethod>]
    [<Ignore>]
    member _.Attachment_ReturnsNoneIfOptionNonExistent () =
        Assert.Fail() // TODO
