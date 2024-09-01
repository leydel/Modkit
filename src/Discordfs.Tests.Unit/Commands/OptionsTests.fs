namespace Modkit.Discordfs.Commands

open Microsoft.VisualStudio.TestTools.UnitTesting
open Modkit.Discordfs.Types

[<TestClass>]
type OptionsTests () =
    [<TestMethod>]
    member _.Channel_FindsCorrectChannel () =
        let name = "channel"
        let value = "channelId"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.CHANNEL,
                Value = CommandInteractionDataOptionValue.String value
            )
        ]

        match options with
        | Options.Channel name channelId -> Assert.AreEqual(value, channelId)
        | _ -> failwith "Could not find channel"

    [<TestMethod>]
    member _.Channel_ReturnsNoneIfOptionIncorrectType () =
        let name = "channel"

        let options = [
            CommandInteractionDataOption.build(
                Name = name,
                Type = ApplicationCommandOptionType.ROLE,
                Value = CommandInteractionDataOptionValue.String "roleId"
            )
        ]

        match options with
        | Options.Channel "channel" _ -> failwith "Unexpectedly found non-existent channel"
        | _ -> ()

    [<TestMethod>]
    member _.Channel_ReturnsNoneIfOptionNonExistent () =
        let options: CommandInteractionDataOption list = []
        
        match options with
        | Options.Channel "channel" _ -> failwith "Unexpectedly found non-existent channel"
        | _ -> ()
