namespace Discordfs.Rest

open Discordfs.Rest.Common
open Discordfs.Types
open System.Collections.Generic
open System.Text.Json
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

type ModifyChannelPayload =
    | GroupDm of ModifyGroupDmChannelPayload
    | Guild of ModifyGuildChannelPayload
    | Thread of ModifyThreadChannelPayload
with
    member this.Payload =
        match this with
        | ModifyChannelPayload.GroupDm groupdm -> groupdm :> Payload
        | ModifyChannelPayload.Guild guild -> guild :> Payload
        | ModifyChannelPayload.Thread thread -> thread :> Payload

and ModifyGroupDmChannelPayload(
    ?name: string,
    ?icon: string
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "icon" icon
        }

and ModifyGuildChannelPayload(
    ?name:                               string,
    ?``type``:                           ChannelType,
    ?position:                           int option,
    ?topic:                              string option,
    ?nsfw:                               bool option,
    ?rate_limit_per_user:                int option,
    ?bitrate:                            int option,
    ?user_limit:                         int option,
    ?permission_overwrites:              PartialPermissionOverwrite list option,
    ?parent_id:                          string option,
    ?rtc_region:                         string option,
    ?video_quality_mode:                 VideoQualityMode option,
    ?default_auto_archive_duration:      int option,
    ?flags:                              int,
    ?available_tags:                     ChannelTag list,
    ?default_reaction_emoji:             DefaultReaction option,
    ?default_thread_rate_limit_per_user: int,
    ?default_sort_order:                 ChannelSortOrder option,
    ?default_forum_layout:               ChannelForumLayout
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "type" ``type``
            optional "position" position
            optional "topic" topic
            optional "nsfw" nsfw
            optional "rate_limit_per_user" rate_limit_per_user
            optional "bitrate" bitrate
            optional "user_limit" user_limit
            optional "permission_overwrites" permission_overwrites
            optional "parent_id" parent_id
            optional "rtc_region" rtc_region
            optional "video_quality_mode" video_quality_mode
            optional "default_auto_archive_duration" default_auto_archive_duration
            optional "flags" flags
            optional "available_tags" available_tags
            optional "default_reaction_emoji" default_reaction_emoji
            optional "default_thread_rate_limit_per_user" default_thread_rate_limit_per_user
            optional "default_sort_order" default_sort_order
            optional "default_forum_layout" default_forum_layout
        }

and ModifyThreadChannelPayload (
    ?name:                  string,
    ?archived:              bool,
    ?auto_archive_duration: int,
    ?locked:                bool,
    ?invitable:             bool,
    ?rate_limit_per_user:   int option,
    ?flags:                 int,
    ?applied_tags:          string list
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "archived" archived
            optional "auto_archive_duration" auto_archive_duration
            optional "locked" locked
            optional "invitable" invitable
            optional "rate_limit_per_user" rate_limit_per_user
            optional "flags" flags
            optional "applied_tags" applied_tags
        }

type EditChannelPermissionsPayload (
    ``type``: EditChannelPermissionsType,
    ?allow:   string option,
    ?deny:    string option
) =
    inherit Payload() with
        override _.Content = json {
            required "type" ``type``
            optional "allow" allow
            optional "deny" deny
        }

type CreateChannelInvitePayload (
    target_type:            InviteTargetType,
    ?max_age:               int,
    ?max_uses:              int,
    ?temporary:             bool,
    ?unique:                bool,
    ?target_user_id:        string,
    ?target_application_id: string
) =
    inherit Payload() with
        override _.Content = json {
            optional "max_age" max_age
            optional "max_uses" max_uses
            optional "temporary" temporary
            optional "unique" unique
            required "target_type" target_type
            optional "target_user_id" target_user_id
            optional "target_application_id" target_application_id
        }

type FollowAnnouncementChannelPayload (
    webhook_channel_id: string
) =
    inherit Payload() with
        override _.Content = json {
            required "webhook_channel_id" webhook_channel_id
        }

type GroupDmAddRecipientPayload (
    access_token: string,
    ?nick: string
) =
    inherit Payload() with
        override _.Content = json {
            required "access_token" access_token
            optional "nick" nick
        }

type StartThreadFromMessagePayload (
    name:                   string,
    ?auto_archive_duration: int,
    ?rate_limit_per_user:   int option
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "auto_archive_duration" auto_archive_duration
            optional "rate_limit_per_user" rate_limit_per_user
        }

type StartThreadWithoutMessagePayload (
    name:                   string,
    ?auto_archive_duration: int,
    ?``type``:              ThreadType,
    ?invitable:             bool,
    ?rate_limit_per_user:   int option
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "auto_archive_duration" auto_archive_duration
            optional "type" ``type``
            optional "invitable" invitable
            optional "rate_limit_per_user" rate_limit_per_user
        }

type ForumAndMediaThreadMessageParams = {
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "embeds">] Embeds: Embed list option
    [<JsonPropertyName "allowed_mentions">] AllowedMentions: AllowedMentions option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "sticker_ids">] StickerIds: string list option
    [<JsonPropertyName "attachments">] Attachments: PartialAttachment list option
    [<JsonPropertyName "flags">] Flags: int option
}

type StartThreadInForumOrMediaChannelPayload (
    name:                   string,
    message:                ForumAndMediaThreadMessageParams,
    ?auto_archive_duration: int,
    ?applied_tags:          string list,
    ?files:                 IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                required "name" name
                optional "auto_archive_duration" auto_archive_duration
                required "message" message
                optional "applied_tags" applied_tags
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type StartThreadInForumOrMediaChannelOkResponseExtraFields = {
    [<JsonPropertyName "message">] Message: Message option
}

[<JsonConverter(typeof<StartThreadInForumOrMediaChannelOkResponseConverter>)>]
type StartThreadInForumOrMediaChannelOkResponse = {
    Channel: Channel
    ExtraFields: StartThreadInForumOrMediaChannelOkResponseExtraFields
}

and StartThreadInForumOrMediaChannelOkResponseConverter () =
    inherit JsonConverter<StartThreadInForumOrMediaChannelOkResponse> ()

    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            Channel = Json.deserializeF json;
            ExtraFields = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let channel = Json.serializeF value.Channel
        let extraFields = Json.serializeF value.ExtraFields

        writer.WriteRawValue (Json.merge channel extraFields)

type ListPublicArchivedThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
    [<JsonPropertyName "has_more">] HasMore: bool
}

type ListPrivateArchivedThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
    [<JsonPropertyName "has_more">] HasMore: bool
}

type ListJoinedPrivateArchivedThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: ThreadMember list
    [<JsonPropertyName "has_more">] HasMore: bool
}

// ----- Emoji -----

type CreateGuildEmojiPayload(
    name:  string,
    image: string,
    roles: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "image" image
            required "roles" roles
        }

type ModifyGuildEmojiPayload(
    ?name:  string,
    ?roles: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "roles" roles
        }

type CreateApplicationEmojiPayload(
    name:  string,
    image: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "image" image
        }

type ModifyApplicationEmojiPayload(
    name: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
        }

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
