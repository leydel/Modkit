namespace Modkit.Discordfs.Commands

open Microsoft.VisualStudio.TestTools.UnitTesting
open Modkit.Discordfs.Types

[<TestClass>]
type OptionsTests () =
    [<TestMethod>]
    member _.SubCommand_FindsCorrectSubCommand () =
        // Arrange
        let name = "command"

        let expectedSubcommandOptions = [
            CommandInteractionDataOption.build(
                Name = "option",
                Type = ApplicationCommandOptionType.STRING,
                Value = CommandInteractionDataOptionValue.String "option"
            )
        ]

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.SUB_COMMAND,
                Options = expectedSubcommandOptions
            )
        ]

        // Act
        let subcommandOptions =
            match options with
            | Options.SubCommand name options -> options
            | _ -> failwith "Could not find subcommand"

        // Assert
        Assert.AreSame(expectedSubcommandOptions, subcommandOptions)

    [<TestMethod>]
    member _.SubCommand_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "subcommand"

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
            | Options.SubCommand name _ -> failwith "Unexpectedly found non-existent subcommand"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.SubCommand_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "subcommand"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.SubCommand name _ -> failwith "Unexpectedly found non-existent subcommand"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.SubCommandGroup_FindsCorrectSubCommandGroup () =
        // Arrange
        let name = "command"

        let expectedSubcommandOptions = [
            CommandInteractionDataOption.build(
                Name = "option",
                Type = ApplicationCommandOptionType.STRING,
                Value = CommandInteractionDataOptionValue.String "option"
            )
        ]

        let expectedSubcommandGroupOptions = [
            CommandInteractionDataOption.build(
                Name = "subcommand",
                Type = ApplicationCommandOptionType.SUB_COMMAND,
                Options = expectedSubcommandOptions
            )
        ]

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.SUB_COMMAND_GROUP,
                Options = expectedSubcommandGroupOptions
            )
        ]

        // Act
        let subcommandGroupOptions =
            match options with
            | Options.SubCommandGroup name options -> options
            | _ -> failwith "Could not find subcommand group"

        // Assert
        Assert.AreSame(expectedSubcommandGroupOptions, subcommandGroupOptions)

    [<TestMethod>]
    member _.SubCommandGroup_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "subcommandGroup"

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
            | Options.SubCommandGroup name _ -> failwith "Unexpectedly found non-existent subcommand group"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.SubCommandGroup_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "subcommandGroup"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.SubCommand name _ -> failwith "Unexpectedly found non-existent subcommand group"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.String_FindsCorrectString () =
        // Arrange
        let name = "string"
        let value = "string"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.STRING,
                Value = CommandInteractionDataOptionValue.String value
            )
        ]

        // Act
        let string =
            match options with
            | Options.String name string -> string
            | _ -> failwith "Could not find string"

        // Assert
        Assert.AreEqual<string>(value, string)

    [<TestMethod>]
    member _.String_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "string"

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
            | Options.String name _ -> failwith "Unexpectedly found non-existent string"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.String_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "string"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.String name _ -> failwith "Unexpectedly found non-existent string"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Integer_FindsCorrectInteger () =
        // Arrange
        let name = "integer"
        let value = 1

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.INTEGER,
                Value = CommandInteractionDataOptionValue.Int value
            )
        ]

        // Act
        let integer =
            match options with
            | Options.Integer name integer -> integer
            | _ -> failwith "Could not find integer"

        // Assert
        Assert.AreEqual<int>(value, integer)

    [<TestMethod>]
    member _.Integer_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "integer"

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
            | Options.Integer name _ -> failwith "Unexpectedly found non-existent integer"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Integer_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "integer"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.Integer name _ -> failwith "Unexpectedly found non-existent integer"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Boolean_FindsCorrectBoolean () =
        // Arrange
        let name = "boolean"
        let value = true

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.BOOLEAN,
                Value = CommandInteractionDataOptionValue.Bool value
            )
        ]

        // Act
        let boolean =
            match options with
            | Options.Boolean name boolean -> boolean
            | _ -> failwith "Could not find boolean"

        // Assert
        Assert.AreEqual<bool>(value, boolean)

    [<TestMethod>]
    member _.Boolean_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "boolean"

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
            | Options.Boolean name _ -> failwith "Unexpectedly found non-existent boolean"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Boolean_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "boolean"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.Boolean name _ -> failwith "Unexpectedly found non-existent boolean"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.User_FindsCorrectUser () =
        // Arrange
        let name = "user"
        let value = "userId"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.USER,
                Value = CommandInteractionDataOptionValue.String value
            )
        ]

        // Act
        let userId =
            match options with
            | Options.User name userId -> userId
            | _ -> failwith "Could not find user"

        // Assert
        Assert.AreEqual<string>(value, userId)

    [<TestMethod>]
    member _.User_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "user"

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
            | Options.User name _ -> failwith "Unexpectedly found non-existent user"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.User_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "user"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.User name _ -> failwith "Unexpectedly found non-existent user"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

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
        Assert.AreEqual<string>(value, channelId)

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
        let name = "channel"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.Channel name _ -> failwith "Unexpectedly found non-existent channel"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Role_FindsCorrectRole () =
        // Arrange
        let name = "role"
        let value = "roleId"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.ROLE,
                Value = CommandInteractionDataOptionValue.String value
            )
        ]

        // Act
        let roleId =
            match options with
            | Options.Role name roleId -> roleId
            | _ -> failwith "Could not find role"

        // Assert
        Assert.AreEqual<string>(value, roleId)

    [<TestMethod>]
    member _.Role_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "role"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.CHANNEL,
                Value = CommandInteractionDataOptionValue.String "channelId"
            )
        ]

        // Act
        let res =
            match options with
            | Options.Role name _ -> failwith "Unexpectedly found non-existent role"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Role_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "role"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.Role name _ -> failwith "Unexpectedly found non-existent role"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Mentionable_FindsCorrectMentionable () =
        // Arrange
        let name = "mentionable"
        let value = "mentionableId"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.MENTIONABLE,
                Value = CommandInteractionDataOptionValue.String value
            )
        ]

        // Act
        let mentionableId =
            match options with
            | Options.Mentionable name mentionableId -> mentionableId
            | _ -> failwith "Could not find mentionable"

        // Assert
        Assert.AreEqual<string>(value, mentionableId)

    [<TestMethod>]
    member _.Mentionable_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "mentionable"

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
            | Options.Mentionable name _ -> failwith "Unexpectedly found non-existent mentionable"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Mentionable_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "mentionable"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.Mentionable name _ -> failwith "Unexpectedly found non-existent mentionable"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Number_FindsCorrectNumber () =
        // Arrange
        let name = "number"
        let value = 12.34

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.NUMBER,
                Value = CommandInteractionDataOptionValue.Double value
            )
        ]

        // Act
        let double =
            match options with
            | Options.Number name double -> double
            | _ -> failwith "Could not find number"

        // Assert
        Assert.AreEqual<double>(value, double)

    [<TestMethod>]
    member _.Number_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "number"

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
            | Options.Number name _ -> failwith "Unexpectedly found non-existent number"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Number_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "number"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.Number name _ -> failwith "Unexpectedly found non-existent number"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Attachment_FindsCorrectAttachment () =
        // Arrange
        let name = "attachment"
        let value = "attachmentId"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.ATTACHMENT,
                Value = CommandInteractionDataOptionValue.String value
            )
        ]

        // Act
        let attachmentId =
            match options with
            | Options.Attachment name attachmentId -> attachmentId
            | _ -> failwith "Could not find attachment"

        // Assert
        Assert.AreEqual<string>(value, attachmentId)

    [<TestMethod>]
    member _.Attachment_ReturnsNoneIfOptionIncorrectType () =
        // Arrange
        let name = "attachment"

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
            | Options.Attachment name _ -> failwith "Unexpectedly found non-existent attachment"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Attachment_ReturnsNoneIfOptionNonExistent () =
        // Arrange
        let name = "attachment"

        let options: CommandInteractionDataOption list = []
        
        // Act
        let res =
            match options with
            | Options.Attachment name _ -> failwith "Unexpectedly found non-existent attachment"
            | _ -> ()

        // Assert
        Assert.IsNull(res)

    [<TestMethod>]
    member _.Options_FetchesMultipleOptions () =
        // Arrange
        let options = [
            CommandInteractionDataOption.build(
                Name = "role1",
                Type = ApplicationCommandOptionType.ROLE,
                Value = CommandInteractionDataOptionValue.String "roleId1"
            );
            CommandInteractionDataOption.build(
                Name = "role2",
                Type = ApplicationCommandOptionType.ROLE,
                Value = CommandInteractionDataOptionValue.String "roleId2"
            );
            CommandInteractionDataOption.build(
                Name = "channel",
                Type = ApplicationCommandOptionType.CHANNEL,
                Value = CommandInteractionDataOptionValue.String "channelId"
            );
        ]

        // Act
        let res =
            match options with
            | Options.Role "role1" roleId1 & Options.Role "role2" roleId2 & Options.Channel "channel" channelId ->
                {|
                    roleId1 = roleId1;
                    roleId2 = roleId2;
                    channelId = channelId;
                |}
            | _ -> failwith "Could not find all options"

        // Assert
        Assert.AreEqual<string>("roleId1", res.roleId1)
        Assert.AreEqual<string>("roleId2", res.roleId2)
        Assert.AreEqual<string>("channelId", res.channelId)

    [<TestMethod>]
    member _.Options_FailsWhenAtLeastOneOptionNotAvailable () =
        // Arrange
        let options = [
            CommandInteractionDataOption.build(
                Name = "role1",
                Type = ApplicationCommandOptionType.ROLE,
                Value = CommandInteractionDataOptionValue.String "roleId1"
            );
            CommandInteractionDataOption.build(
                Name = "channel",
                Type = ApplicationCommandOptionType.CHANNEL,
                Value = CommandInteractionDataOptionValue.String "channelId"
            );
        ]

        // Act
        let res () =
            match options with
            | Options.Role "role1" _ & Options.Role "role2" _ & Options.Channel "channel" _ -> ()
            | _ -> failwith "Could not find all options"

        // Assert
        Assert.ThrowsException(res) |> ignore
        ()
