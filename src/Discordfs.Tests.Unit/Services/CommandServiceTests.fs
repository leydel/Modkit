namespace Modkit.Discordfs.Services

open Microsoft.VisualStudio.TestTools.UnitTesting
open Modkit.Discordfs.Commands
open Modkit.Discordfs.Types

type SampleCommand () =
    inherit Command<unit> ()

    override _.Data = CreateGlobalApplicationCommand.build(
        Name = "sample",
        Description = "Sample command for testing the command service"
    )

    override _.Validate _ =
        Ok ()

    override _.Execute _ = task {
        return Ok (InteractionCallback.build(
            Type = InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE,
            Data = InteractionCallbackMessageData.buildBase(
                Content = "Hello world"
            )
        ))
    }


[<TestClass>]
type CommandServiceTests () =
    [<TestMethod>]
    member _.getCommandName_GetsExistingName () =
        // Arrange
        let commandService = CommandService []

        let interaction = Interaction.Deserialize """{"type":3,"data":{"name":"sample"}}""" |> Option.get

        // Act
        let name = commandService.getCommandName interaction

        // Assert
        Assert.IsTrue(name.IsSome)
        Assert.AreEqual("sample", name.Value)


    [<TestMethod>]
    member _.getCommandName_DoesNotGetNonExistentName () =
        // Arrange
        let commandService = CommandService []

        let interaction = Interaction.Deserialize """{"type":3}""" |> Option.get

        // Act
        let name = commandService.getCommandName interaction

        // Assert
        Assert.IsTrue(name.IsNone)

    [<TestMethod>]
    member _.getCommand_GetsExistingCommand () =
        // Arrange
        let commands = [SampleCommand()]
        let commandService = CommandService commands

        // Act
        let command = commandService.getCommand "sample"

        // Assert
        Assert.IsTrue(command.IsSome)
        Assert.AreEqual("sample", command.Value.Data.Name)

    [<TestMethod>]
    member _.getCommand_DoesNotGetNonExistentCommand () =
        // Arrange
        let commandService = CommandService []

        // Act
        let command = commandService.getCommand "sample"

        // Assert
        Assert.IsTrue(command.IsNone)

    [<TestMethod>]
    member _.Execute_RunsSuccessfulCommand () = task {
        // Arrange
        let commands = [SampleCommand()]
        let commandService: ICommandService = CommandService commands

        let interaction = Interaction.Deserialize """{"type":3,"data":{"name":"sample"}}""" |> Option.get

        // Act
        let! res = commandService.Execute interaction

        // Assert
        match res with
        | Error err -> failwith err
        | Ok res -> Assert.AreEqual(InteractionCallbackType.CHANNEL_MESSAGE_WITH_SOURCE, res.Type)
    }

    [<TestMethod>]
    member _.Execute_ReturnsErrorOnUnknownCommand () = task {
        // Arrange
        let commandService: ICommandService = CommandService []

        let interaction = Interaction.Deserialize """{"type":3,"data":{"name":"sample"}}""" |> Option.get

        // Act
        let! res = commandService.Execute interaction

        // Assert
        match res with
        | Ok _ -> failwith "Supposed to fail"
        | Error err -> Assert.AreEqual("Unknown command", err)
    }

    [<TestMethod>]
    member _.Execute_ReturnsErrorOnMissingCommandName () = task {
        // Arrange
        let commandService: ICommandService = CommandService []

        let interaction = Interaction.Deserialize """{"type":3}""" |> Option.get

        // Act
        let! res = commandService.Execute interaction

        // Assert
        match res with
        | Ok _ -> failwith "Supposed to fail"
        | Error err -> Assert.AreEqual("Missing command name in interaction data", err)
    }
