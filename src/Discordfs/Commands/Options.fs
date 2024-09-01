namespace Modkit.Discordfs.Commands

open Modkit.Discordfs.Types

module Options =
    let (|Channel|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.String channelId);
                Type = ApplicationCommandOptionType.CHANNEL;
            }) when option.Name = name -> Some channelId
            | _ -> None
        )
        
        // TODO: Create versions of above for all option types
