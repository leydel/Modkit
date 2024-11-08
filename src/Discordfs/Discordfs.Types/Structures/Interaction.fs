namespace Discordfs.Types

open System.Collections.Generic
open System.Text.Json.Serialization

#nowarn "49"

type CommandInteractionDataOption = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: ApplicationCommandOptionType
    [<JsonPropertyName "value">] Value: CommandInteractionDataOptionValue option
    [<JsonPropertyName "options">] Options: CommandInteractionDataOption list option
    [<JsonPropertyName "focused">] Focused: bool option
}
with
    static member build(
        Name: string,
        Type: ApplicationCommandOptionType,
        ?Value: CommandInteractionDataOptionValue,
        ?Options: CommandInteractionDataOption list,
        ?Focused: bool
    ) = {
        Name = Name;
        Type = Type;
        Value = Value;
        Options = Options;
        Focused = Focused;
    }

type InteractionData = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: ApplicationCommandType
    [<JsonPropertyName "resolved">] Resolved: ResolvedData option
    [<JsonPropertyName "options">] Options: CommandInteractionDataOption list option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "target_it">] TargetId: string option
}
with
    static member build(
        Id: string,
        Name: string,
        Type: ApplicationCommandType,
        ?Resolved: ResolvedData,
        ?Options: CommandInteractionDataOption list,
        ?GuildId: string,
        ?TargetId: string
    ) = {
        Id = Id;
        Name = Name;
        Type = Type;
        Resolved = Resolved;
        Options = Options;
        GuildId = GuildId;
        TargetId = TargetId;
    }

type Interaction = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "type">] Type: InteractionType
    [<JsonPropertyName "data">] Data: InteractionData option
    [<JsonPropertyName "guild">] Guild: PartialGuild option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "channel">] Channel: PartialChannel option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "member">] Member: GuildMember option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "token">] Token: string
    [<JsonPropertyName "version">] Version: int
    [<JsonPropertyName "message">] Message: Message option
    [<JsonPropertyName "app_permissions">] AppPermissions: string
    [<JsonPropertyName "locale">] Locale: string option
    [<JsonPropertyName "guild_locale">] GuildLocale: string option
    [<JsonPropertyName "entitlements">] Entitlements: Entitlement list
    [<JsonPropertyName "authorizing_integration_owners">] AuthorizingIntegrationOwners: IDictionary<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>
    [<JsonPropertyName "context">] Context: InteractionContextType option
}
with
    static member build(
        Id: string,
        ApplicationId: string,
        Type: InteractionType,
        Token: string,
        Version: int,
        AppPermissions: string,
        Entitlements: Entitlement list,
        AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>,
        ?Data: InteractionData,
        ?Guild: PartialGuild,
        ?GuildId: string,
        ?Channel: PartialChannel,
        ?ChannelId: string,
        ?Member: GuildMember,
        ?User: User,
        ?Message: Message,
        ?Locale: string,
        ?GuildLocale: string,
        ?Context: InteractionContextType
    ) = {
        Id = Id;
        ApplicationId = ApplicationId;
        Type = Type;
        Data = Data;
        Guild = Guild;
        GuildId = GuildId;
        Channel = Channel;
        ChannelId = ChannelId;
        Member = Member;
        User = User;
        Token = Token;
        Version = Version;
        Message = Message;
        AppPermissions = AppPermissions;
        Locale = Locale;
        GuildLocale = GuildLocale;
        Entitlements = Entitlements;
        AuthorizingIntegrationOwners = AuthorizingIntegrationOwners;
        Context = Context;
    }

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-interaction-response-structure
type InteractionResponsePayload<'a> = {
    [<JsonPropertyName "type">] Type: InteractionCallbackType
    [<JsonPropertyName "data">] Data: 'a
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-messages
type MessageInteractionResponse = {
    [<JsonPropertyName "tts">] Tts: bool option
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "embeds">] Embeds: Embed list option
    [<JsonPropertyName "allowed_mentions">] AllowedMentions: AllowedMentions option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "attachments">] Attachments: PartialAttachment list option
    [<JsonPropertyName "poll">] Poll: Poll option
}
with
    static member create (?tts, ?content, ?embeds, ?allowedMentions, ?flags, ?components, ?attachments, ?poll) = {
        Tts = tts;
        Content = content;
        Embeds = embeds;
        AllowedMentions = allowedMentions;
        Flags = flags;
        Components = components;
        Attachments = attachments;
        Poll = poll;
    }

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-autocomplete
type AutocompleteInteractionResponse = {
    [<JsonPropertyName "choices">] Choices: ApplicationCommandOptionChoice list
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-response-object-modal
type ModalInteractionResponse = {
    [<JsonPropertyName "custom_id">] CustomId: string
    [<JsonPropertyName "title">] Title: string
    [<JsonPropertyName "components">] Components: Component list
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-callback-interaction-callback-object
type InteractionCallback = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: InteractionType
    [<JsonPropertyName "activity_instance_id">] ActivityInstanceId: string option
    [<JsonPropertyName "response_message_id">] ResponseMessageId: string option
    [<JsonPropertyName "response_message_loading">] ResponseMessageLoading: bool option
    [<JsonPropertyName "response_message_ephemeral">] ResponseMessageEphemeral: bool option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-callback-interaction-callback-resource-object
type InteractionCallbackResource = {
    [<JsonPropertyName "type">] Type: InteractionCallbackType
    [<JsonPropertyName "activity_instance">] ActivityInstance: ActivityInstance option
    [<JsonPropertyName "message">] Message: Message option
}

// https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-callback-interaction-callback-response-object
type InteractionCallbackResponse = {
    [<JsonPropertyName "data">] Data: InteractionCallback
    [<JsonPropertyName "resource">] Resource: InteractionCallbackResource option
}
