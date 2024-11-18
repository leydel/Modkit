namespace Discordfs.Commands.Structures

open Discordfs.Rest
open Discordfs.Types
open System.Collections.Generic

type CommandData = {
    Name:                     string
    NameLocalizations:        IDictionary<string, string> option
    Description:              string option
    DescriptionLocalizations: IDictionary<string, string> option
    Options:                  ApplicationCommandOption list option
    DefaultMemberPermissions: string option option
    IntegrationTypes:         ApplicationIntegrationType list option
    Contexts:                 InteractionContextType list option
    Type:                     ApplicationCommandType option
    Nsfw:                     bool option
}
with
    static member build (
        name, ?nameLocalizations, ?description, ?descriptionLocalizations, ?options, ?defaultMemberPermissions,
        ?integrationTypes, ?contexts, ?``type``, ?nsfw 
    ) = {
        Name = name
        NameLocalizations = nameLocalizations
        Description = description
        DescriptionLocalizations = descriptionLocalizations
        Options = options
        DefaultMemberPermissions = defaultMemberPermissions
        IntegrationTypes = integrationTypes
        Contexts = contexts
        Type = ``type``
        Nsfw = nsfw
    }

module CommandData =
    let toGlobalApplicationCommandPayload (data: CommandData) =
        CreateGlobalApplicationCommandPayload(
            name = data.Name,
            name_localizations = data.NameLocalizations,
            ?description = data.Description,
            description_localizations = data.DescriptionLocalizations,
            ?options = data.Options,
            ?default_member_permissions = data.DefaultMemberPermissions,
            ?integration_types = data.IntegrationTypes,
            ?``type`` = data.Type,
            ?nsfw = data.Nsfw
        )

    let toGuildApplicationCommandPayload (data: CommandData) =
        CreateGuildApplicationCommandPayload(
            name = data.Name,
            name_localizations = data.NameLocalizations,
            ?description = data.Description,
            description_localizations = data.DescriptionLocalizations,
            ?options = data.Options,
            ?default_member_permissions = data.DefaultMemberPermissions,
            ?``type`` = data.Type,
            ?nsfw = data.Nsfw
        )
