namespace Discordfs.Types

open System.Collections.Generic
open System.Text.Json.Serialization

#nowarn "49"

type ApplicationCommandOptionChoice = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: Dictionary<string, string> option
    [<JsonPropertyName "value">] Value: ApplicationCommandOptionChoiceValue
}

type ApplicationCommandOption = {
    [<JsonPropertyName "type">] Type: ApplicationCommandOptionType
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: Dictionary<string, string> option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "description_localizations">] DescriptionLocalizations: Dictionary<string, string> option
    [<JsonPropertyName "required">] Required: bool option
    [<JsonPropertyName "choices">] Choices: ApplicationCommandOptionChoice list option
    [<JsonPropertyName "options">] Options: ApplicationCommandOption list option
    [<JsonPropertyName "channel_types">] ChannelTypes: ChannelType list option
    [<JsonPropertyName "min_value">] MinValue: ApplicationCommandMinValue option
    [<JsonPropertyName "max_value">] MaxValue: ApplicationCommandMaxValue option
    [<JsonPropertyName "min_length">] MinLength: int option
    [<JsonPropertyName "max_length">] MaxLength: int option
    [<JsonPropertyName "autocomplete">] Autocomplete: bool option
}
with
    static member build(
        Type: ApplicationCommandOptionType,
        Name: string,
        Description: string,
        ?NameLocalizations: Dictionary<string, string>,
        ?DescriptionLocalizations: Dictionary<string, string>,
        ?Required: bool,
        ?Choices: ApplicationCommandOptionChoice list,
        ?Options: ApplicationCommandOption list,
        ?ChannelTypes: ChannelType list,
        ?MinValue: ApplicationCommandMinValue,
        ?MaxValue: ApplicationCommandMaxValue,
        ?MinLength: int,
        ?MaxLength: int,
        ?Autocomplete: bool
    ) = {
        Type = Type;
        Name = Name;
        NameLocalizations = NameLocalizations;
        Description = Description;
        DescriptionLocalizations = DescriptionLocalizations;
        Required = Required;
        Choices = Choices;
        Options = Options;
        ChannelTypes = ChannelTypes;
        MinValue = MinValue;
        MaxValue = MaxValue;
        MinLength = MinLength;
        MaxLength = MaxLength;
        Autocomplete = Autocomplete;
    }

type ApplicationCommand = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: ApplicationCommandType option
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: IDictionary<string, string> option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "description_localizations">] DescriptionLocalizations: IDictionary<string, string> option
    [<JsonPropertyName "options">] Options: ApplicationCommandOption list option
    [<JsonPropertyName "default_member_permissions">] DefaultMemberPermissions: string option
    [<JsonPropertyName "nsfw">] Nsfw: bool option
    [<JsonPropertyName "integration_types">] IntegrationTypes: ApplicationIntegrationType list option
    [<JsonPropertyName "contexts">] Contexts: InteractionContextType list option
    [<JsonPropertyName "version">] Version: string
    [<JsonPropertyName "handler">] Handler: ApplicationCommandHandlerType option

    // Only present under certain conditions: https://discord.com/developers/docs/interactions/application-commands#retrieving-localized-commands
    [<JsonPropertyName "name_localized">] NameLocalized: string option
    [<JsonPropertyName "description_localized">] DescriptionLocalized: string option

    // TODO: Create separate type with these special properties? Like invite metadata?
}

// https://discord.com/developers/docs/interactions/application-commands#application-command-permissions-object-application-command-permissions-structure
type ApplicationCommandPermission = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: ApplicationCommandPermissionType
    [<JsonPropertyName "permission">] Permission: bool
}

// https://discord.com/developers/docs/interactions/application-commands#application-command-permissions-object-guild-application-command-permissions-structure
type GuildApplicationCommandPermissions = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "permissions">] Permissions: ApplicationCommandPermission list
}
