namespace Discordfs.Commands

open System.Collections.Generic

open Discordfs.Types

type Command = {
    Name: string
    NameLocalizations: IDictionary<string, string> option
    Description: string option
    DescriptionLocalizations: IDictionary<string, string> option
    Options: ApplicationCommandOption list option
    DefaultMemberPermissions: string option option // TODO: Does this need to be double option?
    IntegrationTypes: ApplicationIntegrationType list option
    Contexts: InteractionContextType list option
    Type: ApplicationCommandType option
    Nsfw: bool option
}

module Command =
    let zero name = {
        Name = name
        NameLocalizations = None
        Description = None
        DescriptionLocalizations = None
        Options = None
        DefaultMemberPermissions = None
        IntegrationTypes = None
        Contexts = None
        Type = None
        Nsfw = None
    }

    let setDescription description (command: Command) =
        { command with Description = Some description }

    let addNameLocalizations localizations command =
        let localizations =
            command.NameLocalizations >>? dict []
            |> Seq.map (|KeyValue|)
            |> Seq.append localizations
            |> dict |> Some
        
        { command with NameLocalizations = localizations }

    let addLocalizedName locale name command =
        command |> addNameLocalizations [(locale, name)]

    let addDescriptionLocalizations localizations command =
        let localizations =
            command.DescriptionLocalizations >>? dict []
            |> Seq.map (|KeyValue|)
            |> Seq.append localizations
            |> dict |> Some
        
        { command with DescriptionLocalizations = localizations }

    let addLocalizedDescription locale description command =
        command |> addDescriptionLocalizations [(locale, description)]

    let setDefaultMemberPermissions defaultMemberPermissions (command: Command) =
        { command with DefaultMemberPermissions = Some defaultMemberPermissions }

    let addIntegrationTypes integrationTypes command =
        let integrationTypes =
            command.IntegrationTypes >>? []
            |> List.append integrationTypes
            |> Some

        { command with IntegrationTypes = integrationTypes }

    let addIntegrationType integrationType command =
        command |> addIntegrationTypes [integrationType]

    let addContexts contexts command =
        let contexts =
            command.Contexts >>? []
            |> List.append contexts
            |> Some

        { command with Contexts = contexts }

    let addContext context command =
        command |> addContexts [context]

    let setType commandType (command: Command) =
        { command with Type = Some commandType }

    let setNsfw nsfw (command: Command) =
        { command with Nsfw = Some nsfw }

    let addLocale locale name description command =
        command
        |> addLocalizedName locale name
        |> addLocalizedDescription locale description

    // TODO: Create methods to turn this into guild/user payloads to register (need to figure out how this logic should work)

    let isHandler (interaction: Interaction) (command: Command) =
        match interaction.Data with
        | Some { Name = name; Options = (Some options) } ->
            let rec loop (names: string list) (option: CommandInteractionDataOption) =
                match option.Type, option.Options with
                | ApplicationCommandOptionType.SUB_COMMAND_GROUP, Some options -> options |> List.collect (loop names) |> List.append names
                | ApplicationCommandOptionType.SUB_COMMAND, _ -> [option.Name] |> List.append names
                | _ -> []
            
            let rec compareOptions (path: string list) (options: ApplicationCommandOption list option) =
                match options, path with
                | Some options, path when path.Length > 0 ->
                    match options |> List.tryFind (fun o -> o.Name = (path |> List.head)) with
                    | Some _ when path.Length = 1 -> true
                    | Some { Options = options } -> compareOptions path[1..] options
                    | _ -> false
                | None, path when path.Length = 0 -> true
                | _ -> false

            name = command.Name && compareOptions (List.collect (loop []) options) command.Options
        | _ -> false
        
[<AutoOpen>]
module CommandBuilder =
    type CommandBuilder (name: string) =
        // Computation expression functions
        member _.Yield (_: unit) = Command.zero name
        member _.Yield (x: Command) = x
        
        // Basic building operations
        [<CustomOperation>] member _.nameLocalizations (x, localizations) = x |> Command.addNameLocalizations localizations
        [<CustomOperation>] member _.nameLocalization (x, locale, name) = x |> Command.addLocalizedName locale name
        [<CustomOperation>] member _.description (x, description) = x |> Command.setDescription description
        [<CustomOperation>] member _.descriptionLocalizations (x, localizations) = x |> Command.addDescriptionLocalizations localizations
        [<CustomOperation>] member _.descriptionLocalization (x, locale, name) = x |> Command.addLocalizedDescription locale name
        // TODO: add `options` and `option`
        [<CustomOperation>] member _.defaultMemberPermissions (x, defaultMemberPermissions) = x |> Command.setDefaultMemberPermissions defaultMemberPermissions
        [<CustomOperation>] member _.integrationTypes (x, types: ApplicationIntegrationType list) = x |> Command.addIntegrationTypes types
        [<CustomOperation>] member _.integrationType (x, ``type``: ApplicationIntegrationType) = x |> Command.addIntegrationType ``type``
        [<CustomOperation>] member _.contexts (x, contexts: InteractionContextType list) = x |> Command.addContexts contexts
        [<CustomOperation>] member _.context (x, context: InteractionContextType) = x |> Command.addContext context
        [<CustomOperation>] member _.commandType (x: Command, commandType: ApplicationCommandType) = x |> Command.setType commandType
        [<CustomOperation>] member _.nsfw (x: Command, nsfw: bool) = x |> Command.setNsfw nsfw

        // Custom building operations
        [<CustomOperation>] member _.locale (x, locale, name, description) = x |> Command.addLocale locale name description
        // TODO: Add builders for specific option types

    let command name = CommandBuilder(name)
