namespace Discordfs.Commands.Structures

open Discordfs.Types
open System.Collections.Generic
open System.Threading.Tasks

#nowarn "49"

type CommandData = {
    Name: string
    NameLocalizations: IDictionary<string, string> option
    Description: string
    DescriptionLocalizations: IDictionary<string, string> option
    Options: ApplicationCommandOption list option
    DefaultMemberPermissions: string option
    DmPermissions: bool option
    IntegrationTypes: ApplicationIntegrationType list option
    Type: ApplicationCommandType
    Nsfw: bool option
}
with
    static member build (
        Type: ApplicationCommandType,
        Name: string,
        Description: string,
        ?NameLocalizations: IDictionary<string, string>,
        ?DescriptionLocalizations: IDictionary<string, string>,
        ?Options: ApplicationCommandOption list,
        ?DefaultMemberPermissions: string,
        ?DmPermissions: bool,
        ?IntegrationTypes: ApplicationIntegrationType list,
        ?Nsfw: bool
    ) = {
        Name = Name
        NameLocalizations = NameLocalizations
        Description = Description
        DescriptionLocalizations = DescriptionLocalizations
        Options = Options
        DefaultMemberPermissions = DefaultMemberPermissions
        DmPermissions = DmPermissions
        IntegrationTypes = IntegrationTypes
        Type = Type
        Nsfw = Nsfw
    }

[<AbstractClass>]
type Command () =
    abstract member Data: CommandData

    abstract member Execute:
        interaction: Interaction ->
        Task<Result<Discordfs.Commands.Types.InteractionCallback, string>>
