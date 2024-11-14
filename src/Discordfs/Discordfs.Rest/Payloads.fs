namespace Discordfs.Rest

open Discordfs.Rest.Common
open Discordfs.Types
open System.Collections.Generic
open System.Text.Json.Serialization

// ----- Interaction -----

type CreateInteractionResponsePayload<'a> (
    payload: InteractionResponsePayload<'a>, // TODO: Figure out nicer way to handle this (message, modal, autocomplete)
    ?files: IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = JsonPayload payload

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type EditOriginalInteractionResponsePayload (
    ?content: string,
    ?embeds: Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components: Component list,
    ?attachments: PartialAttachment list,
    ?poll: Poll,
    ?files: IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "poll" poll
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type CreateFollowUpMessagePayload (
    ?content: string,
    ?tts: bool,
    ?embeds: Embed list,
    ?allowed_mentions: AllowedMentions,
    ?components: Component list,
    ?attachments: PartialAttachment list,
    ?flags: int,
    ?poll: Poll,
    ?files: IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "tts" tts
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "flags" flags
                optional "poll" poll                
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type EditFollowUpMessagePayload (
    ?content: string option,
    ?embeds: Embed list option,
    ?allowed_mentions: AllowedMentions option,
    ?components: Component list option,
    ?attachments: PartialAttachment list option,
    ?poll: Poll option,
    ?files: IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "embeds" embeds
                optional "allowed_mentions" allowed_mentions
                optional "components" components
                optional "attachments" attachments
                optional "poll" poll                
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }
            
// ----- Application Command -----

type CreateGlobalApplicationCommandPayload (
    name:                        string,
    ?name_localizations:         IDictionary<string, string> option,
    ?description:                string,
    ?description_localizations:  IDictionary<string, string> option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?integration_types:          ApplicationIntegrationType list,
    ?contexts:                   InteractionContextType list,
    ?``type``:                   ApplicationCommandType,
    ?nsfw:                       bool
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "name_localizations" name_localizations
            optional "description" description
            optional "description_localizations" description_localizations
            optional "options" options
            optional "default_member_permissions" default_member_permissions
            optional "integration_types" integration_types
            optional "contexts" contexts
            optional "type" ``type``
            optional "nsfw" nsfw
        }

type EditGlobalApplicationCommandPayload (
    ?name:                       string,
    ?name_localizations:         IDictionary<string, string> option,
    ?description:                string,
    ?description_localizations:  IDictionary<string, string> option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?integration_types:          ApplicationIntegrationType list,
    ?contexts:                   InteractionContextType list,
    ?nsfw:                       bool
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "name_localizations" name_localizations
            optional "description" description
            optional "description_localizations" description_localizations
            optional "options" options
            optional "default_member_permissions" default_member_permissions
            optional "integration_types" integration_types
            optional "contexts" contexts
            optional "nsfw" nsfw
        }

type BulkOverwriteApplicationCommand = {
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "name_localizations">] NameLocalizations: IDictionary<string, string> option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "description_localizations">] DescriptionLocalizations: IDictionary<string, string> option
    [<JsonPropertyName "options">] Options: ApplicationCommandOption list option
    [<JsonPropertyName "default_member_permissions">] DefaultMemberPermissions: string option
    [<JsonPropertyName "integration_types">] IntegrationTypes: ApplicationIntegrationType list option
    [<JsonPropertyName "contexts">] Contexts: InteractionContextType list option
    [<JsonPropertyName "type">] Type: ApplicationCommandType option
    [<JsonPropertyName "nsfw">] Nsfw: bool option
}
        
type BulkOverwriteGlobalApplicationCommandsPayload (
    commands: BulkOverwriteApplicationCommand list
) =
    inherit Payload() with
        override _.Content = JsonListPayload commands

type CreateGuildApplicationCommandPayload (
    name:                        string,
    ?name_localizations:         IDictionary<string, string> option,
    ?description:                string,
    ?description_localizations:  IDictionary<string, string> option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?``type``:                   ApplicationCommandType,
    ?nsfw:                       bool
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "name_localizations" name_localizations
            optional "description" description
            optional "description_localizations" description_localizations
            optional "options" options
            optional "default_member_permissions" default_member_permissions
            optional "type" ``type``
            optional "nsfw" nsfw
        }

type EditGuildApplicationCommandPayload (
    ?name:                       string,
    ?name_localizations:         IDictionary<string, string> option,
    ?description:                string,
    ?description_localizations:  IDictionary<string, string> option,
    ?options:                    ApplicationCommandOption list,
    ?default_member_permissions: string option,
    ?nsfw:                       bool
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "name_localizations" name_localizations
            optional "description" description
            optional "description_localizations" description_localizations
            optional "options" options
            optional "default_member_permissions" default_member_permissions
            optional "nsfw" nsfw
        }

type BulkOverwriteGuildApplicationCommands (
    commands: BulkOverwriteApplicationCommand list
) =
    inherit Payload() with
        override _.Content = JsonListPayload commands

type EditApplicationCommandPermissions (
    permissions: ApplicationCommandPermission list
) =
    inherit Payload() with
        override _.Content = json {
            required "permissions" permissions
        }

// ----- Application -----

type EditCurrentApplicationPayload (
    ?custom_install_url:               string,
    ?description:                      string,
    ?role_connection_verification_url: string,
    ?install_params:                   OAuth2InstallParams,
    ?integration_types_config:         IDictionary<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>,
    ?flags:                            int,
    ?icon:                             string option,
    ?cover_image:                      string option,
    ?interactions_endpoint_url:        string,
    ?tags:                             string list
) =
    inherit Payload() with
        override _.Content = json {
            optional "custom_install_url" custom_install_url
            optional "description" description
            optional "role_connection_verification_url" role_connection_verification_url
            optional "install_params" install_params
            optional "integration_types_config" integration_types_config
            optional "flags" flags
            optional "icon" icon
            optional "cover_image" cover_image
            optional "interactions_endpoint_url" interactions_endpoint_url
            optional "tags" tags
        }

// ----- Audit Log -----

// ----- Auto Moderation -----

type CreateAutoModerationRulePayload (
    name:              string,
    event_type:        AutoModerationEventType,
    trigger_type:      AutoModerationTriggerType,
    actions:           AutoModerationAction list,
    ?trigger_metadata: AutoModerationTriggerMetadata,
    ?enabled:          bool,
    ?exempt_roles:     string list,
    ?exempt_channels:  string list
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "event_type" event_type
            required "trigger_type" trigger_type
            optional "trigger_metadata" trigger_metadata
            required "actions" actions
            optional "enabled" enabled
            optional "exempt_roles" exempt_roles
            optional "exempt_channels" exempt_channels
        }

type ModifyAutoModerationRulePayload (
    ?name:             string,
    ?event_type:       AutoModerationEventType,
    ?trigger_metadata: AutoModerationTriggerMetadata,
    ?actions:          AutoModerationAction list,
    ?enabled:          bool,
    ?exempt_roles:     string list,
    ?exempt_channels:  string list
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "event_type" event_type
            optional "trigger_metadata" trigger_metadata
            optional "actions" actions
            optional "enabled" enabled
            optional "exempt_roles" exempt_roles
            optional "exempt_channels" exempt_channels
        }

// ----- Channel -----

// TODO: Implement

// ----- Emoji -----

// TODO: Implement

// ----- Entitlement -----

// TODO: Implement

// ----- Guild -----

// TODO: Implement

// ----- Guild Scheduled Event -----

// TODO: Implement

// ----- Guild Template -----

// TODO: Implement

// ----- Invite -----

// TODO: Implement

// ----- Message -----

// TODO: Implement

// ----- Poll -----

// TODO: Implement

// ----- Role Connection -----

// TODO: Implement

// ----- Sku -----

// TODO: Implement

// ----- Soundboard -----

// TODO: Implement

// ----- Stage Instance -----

// TODO: Implement

// ----- Sticker -----

// TODO: Implement

// ----- Subscription -----

// TODO: Implement

// ----- User -----

// TODO: Implement

// ----- Voice -----

// TODO: Implement

// ----- Webhook -----

// TODO: Implement

// ----- Gateway -----

// TODO: Implement

// ----- OAuth2 -----

// TODO: Implement
