namespace Modkit.Discordfs.Commands

open Modkit.Discordfs.Types

module Options =
    let (|SubCommand|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Options = Some options;
                Type = ApplicationCommandOptionType.SUB_COMMAND;
            }) when option.Name = name -> Some options
            | _ -> None
        )

    let (|SubCommandGroup|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Options = Some options;
                Type = ApplicationCommandOptionType.SUB_COMMAND_GROUP;
            }) when option.Name = name -> Some options
            | _ -> None
        )

    let (|String|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.String value);
                Type = ApplicationCommandOptionType.STRING;
            }) when option.Name = name -> Some value
            | _ -> None
        )

    let (|Integer|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.Int value);
                Type = ApplicationCommandOptionType.INTEGER;
            }) when option.Name = name -> Some value
            | _ -> None
        )

    let (|Boolean|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.Bool value);
                Type = ApplicationCommandOptionType.BOOLEAN;
            }) when option.Name = name -> Some value
            | _ -> None
        )

    let (|User|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.String userId);
                Type = ApplicationCommandOptionType.USER;
            }) when option.Name = name -> Some userId
            | _ -> None
        )

    let (|Channel|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.String channelId);
                Type = ApplicationCommandOptionType.CHANNEL;
            }) when option.Name = name -> Some channelId
            | _ -> None
        )

    let (|Role|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.String roleId);
                Type = ApplicationCommandOptionType.ROLE;
            }) when option.Name = name -> Some roleId
            | _ -> None
        )

    let (|Mentionable|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.String mentionableId);
                Type = ApplicationCommandOptionType.MENTIONABLE;
            }) when option.Name = name -> Some mentionableId
            | _ -> None
        )

    let (|Number|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.Double value);
                Type = ApplicationCommandOptionType.NUMBER;
            }) when option.Name = name -> Some value
            | _ -> None
        )

    let (|Attachment|_|) (name: string) (options: CommandInteractionDataOption list) =
        options |> List.tryPick (fun option ->
            match option with
            | ({
                Value = Some (CommandInteractionDataOptionValue.String attachmentId);
                Type = ApplicationCommandOptionType.ATTACHMENT;
            }) when option.Name = name -> Some attachmentId
            | _ -> None
        )
