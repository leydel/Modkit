namespace Discordfs.Rest

open Discordfs.Rest.Common
open Discordfs.Types
open System
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

type ListApplicationEmojisOkResponse = {
    [<JsonPropertyName "items">] Items: Emoji list
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

type CreateTestEntitlementPayload (
    sku_id:     string,
    owner_id:   string,
    owner_type: EntitlementOwnerType
) =
    inherit Payload() with
        override _.Content = json {
            required "sku_id" sku_id
            required "owner_id" owner_id
            required "owner_type" owner_type
        }

// ----- Guild -----

type CreateGuildPayload(
    name:                           string,
    ?icon:                          string,
    ?verification_level:            GuildVerificationLevel,
    ?default_message_notifications: GuildMessageNotificationLevel,
    ?explicit_content_filter:       GuildExplicitContentFilterLevel,
    ?roles:                         Role list,
    ?channels:                      PartialChannel list,
    ?afk_channel_id:                string,
    ?afk_timeout:                   int,
    ?system_channel_id:             string,
    ?system_channel_flags:          int
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "icon" icon
            optional "verification_level" verification_level
            optional "default_message_notifications" default_message_notifications
            optional "explicit_content_filter" explicit_content_filter
            optional "roles" roles
            optional "channels" channels
            optional "afk_channel_id" afk_channel_id
            optional "afk_timeout" afk_timeout
            optional "system_channel_id" system_channel_id
            optional "system_channel_flags" system_channel_flags
        }

type ModifyGuildPayload(
    ?name:                          string,
    ?verification_level:            GuildVerificationLevel option,
    ?default_message_notifications: GuildMessageNotificationLevel option,
    ?explicit_content_filter:       GuildExplicitContentFilterLevel option,
    ?afk_channel_id:                string option,
    ?afk_timeout:                   int,
    ?icon:                          string option,
    ?owner_id:                      string,
    ?splash:                        string option,
    ?discovery_splash:              string option,
    ?banner:                        string option,
    ?system_channel_id:             string option,
    ?system_channel_flags:          int,
    ?rules_channel_id:              string option,
    ?public_updates_channel_id:     string option,
    ?preferred_locale:              string option,
    ?features:                      GuildFeature list,
    ?description:                   string option,
    ?premium_progress_bar_enabled:  bool,
    ?safety_alerts_channel_id:      string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "verification_level" verification_level
            optional "default_message_notifications" default_message_notifications
            optional "explicit_content_filter" explicit_content_filter
            optional "afk_channel_id" afk_channel_id
            optional "afk_timeout" afk_timeout
            optional "icon" icon
            optional "owner_id" owner_id
            optional "splash" splash
            optional "discovery_splash" discovery_splash
            optional "banner" banner
            optional "system_channel_id" system_channel_id
            optional "system_channel_flags" system_channel_flags
            optional "rules_channel_id" rules_channel_id
            optional "public_updates_channel_id" public_updates_channel_id
            optional "preferred_locale" preferred_locale
            optional "features" (features >>. List.map _.ToString())
            optional "description" description
            optional "premium_progress_bar_enabled" premium_progress_bar_enabled
            optional "safety_alerts_channel_id" safety_alerts_channel_id
        }

type CreateGuildChannelPayload(
    name:                                string,
    ?``type``:                           ChannelType option,
    ?topic:                              string option,
    ?bitrate:                            int option,
    ?user_limit:                         int option,
    ?rate_limit_per_user:                int option,
    ?position:                           int option,
    ?permission_overwrites:              PartialPermissionOverwrite list option,
    ?parent_id:                          string option,
    ?nsfw:                               bool option,
    ?rtc_region:                         string option,
    ?video_quality_mode:                 VideoQualityMode option,
    ?default_auto_archive_duration:      int option,
    ?default_reaction_emoji:             DefaultReaction option,
    ?available_tags:                     ChannelTag list option,
    ?default_sort_order:                 ChannelSortOrder option,
    ?default_forum_layout:               ChannelForumLayout option,
    ?default_thread_rate_limit_per_user: int option
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "type" ``type``
            optional "topic" topic
            optional "bitrate" bitrate
            optional "user_limit" user_limit
            optional "rate_limit_per_user" rate_limit_per_user
            optional "position" position
            optional "permission_overwrites" permission_overwrites
            optional "parent_id" parent_id
            optional "nsfw" nsfw
            optional "rtc_region" rtc_region
            optional "video_quality_mode" video_quality_mode
            optional "default_auto_archive_duration" default_auto_archive_duration
            optional "default_reaction_emoji" default_reaction_emoji
            optional "available_tags" available_tags
            optional "default_sort_order" default_sort_order
            optional "default_forum_layout" default_forum_layout
            optional "default_thread_rate_limit_per_user" default_thread_rate_limit_per_user
        }


type ModifyGuildChannelPosition = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "lock_permissions">] LockPermissions: bool option
    [<JsonPropertyName "parent_id">] ParentId: string option
}

type ModifyGuildChannelPositionsPayload(
    positions: ModifyGuildChannelPosition list
) =
    inherit Payload() with
        override _.Content =
            JsonListPayload positions

type ListActiveGuildThreadsOkResponse = {
    [<JsonPropertyName "threads">] Threads: Channel list
    [<JsonPropertyName "members">] Members: GuildMember list
}

type AddGuildMemberPayload(
    access_token: string,
    ?nick:        string,
    ?roles:       string list,
    ?mute:        bool,
    ?deaf:        bool
) =
    inherit Payload() with
        override _.Content = json {
            required "access_token" access_token
            optional "nick" nick
            optional "roles" roles
            optional "mute" mute
            optional "deaf" deaf
        }

type ModifyGuildMemberPayload(
    ?nick:                         string option,
    ?roles:                        string list option,
    ?mute:                         bool option,
    ?deaf:                         bool option,
    ?channel_id:                   string option,
    ?communication_disabled_until: DateTime option,
    ?flags:                        int option
) =
    inherit Payload() with
        override _.Content = json {
            optional "nick" nick
            optional "roles" roles
            optional "mute" mute
            optional "deaf" deaf
            optional "channel_id" channel_id
            optional "communication_disabled_until" communication_disabled_until
            optional "flags" flags
        }

type ModifyCurrentMemberPayload(
    ?nick: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "nick" nick
        }

type CreateGuildBanPayload(
    ?delete_message_days:    int,
    ?delete_message_seconds: int
) =
    inherit Payload() with
        override _.Content = json {
            optional "delete_message_days" delete_message_days
            optional "delete_message_seconds" delete_message_seconds
        }

type BulkGuildBanPayload(
    user_ids:                string list,
    ?delete_message_seconds: int
) =
    inherit Payload() with
        override _.Content = json {
            required "user_ids" user_ids
            optional "delete_message_seconds" delete_message_seconds
        }

type BulkGuildBanOkResponse = {
    [<JsonPropertyName "banned_users">] BannedUsers: string list
    [<JsonPropertyName "failed_users">] FailedUsers: string list
}
    
type CreateGuildRolePayload(
    ?name:          string,
    ?permissions:   string,
    ?color:         int,
    ?hoist:         bool,
    ?icon:          string option,
    ?unicode_emoji: string option,
    ?mentionable:   bool
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "permissions" permissions
            optional "color" color
            optional "hoist" hoist
            optional "icon" icon
            optional "unicode_emoji" unicode_emoji
            optional "mentionable" mentionable
        }

type ModifyGuildRolePosition = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "position">] Position: int option
}

type ModifyGuildRolePositionsPayload(
    positions: ModifyGuildRolePosition list
) =
    inherit Payload() with
        override _.Content =
            JsonListPayload positions

type ModifyGuildRolePayload(
    ?name:          string option,
    ?permissions:   string option,
    ?color:         int option,
    ?hoist:         bool option,
    ?icon:          string option,
    ?unicode_emoji: string option,
    ?mentionable:   bool option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "permissions" permissions
            optional "color" color
            optional "hoist" hoist
            optional "icon" icon
            optional "unicode_emoji" unicode_emoji
            optional "mentionable" mentionable
        }

type ModifyGuildMfaLevelPayload(
    level: GuildMfaLevel
) =
    inherit Payload() with
        override _.Content = json {
            required "level" level
        }

type GetGuildPruneCountOkResponse = {
    [<JsonPropertyName "pruned">] Pruned: int
}

type BeginGuildPrunePayload(
    ?days: int,
    ?compute_prune_count: bool,
    ?include_roles: string list,
    ?reason: string
) =
    inherit Payload() with
        override _.Content = json {
            optional "days" days
            optional "compute_prune_count" compute_prune_count
            optional "include_roles" include_roles
            optional "reason" reason
        }

type BeginGuildPruneOkResponse = {
    [<JsonPropertyName "pruned">] Pruned: int option
}

type ModifyGuildWidgetPayload(
    ?enabled:    bool,
    ?channel_id: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "enabled" enabled
            optional "channel_id" channel_id
        }

type GetGuildVanityUrlOkResponse = {
    [<JsonPropertyName "code">] Code: string option
    [<JsonPropertyName "uses">] Uses: int
}

type ModifyGuildWelcomeScreenPayload(
    ?enabled:          bool option,
    ?welcome_channels: WelcomeScreenChannel list option,
    ?description:      string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "enabled" enabled
            optional "welcome_channels" welcome_channels
            optional "description" description
        }

type ModifyGuildOnboardingPayload(
    prompts:             GuildOnboardingPrompt list,
    default_channel_ids: string list,
    enabled:             bool,
    mode:                OnboardingMode
) =
    inherit Payload() with
        override _.Content = json {
            required "prompts" prompts
            required "default_channel_ids" default_channel_ids
            required "enabled" enabled
            required "mode" mode
        }

// ----- Guild Scheduled Event -----

type CreateGuildScheduledEventPayload (
    name:                 string,
    privacy_level:        PrivacyLevelType,
    scheduled_start_time: DateTime,
    entity_type:          ScheduledEntityType,
    ?channel_id:          string,
    ?entity_metadata:     EntityMetadata,
    ?scheduled_end_time:  DateTime,
    ?description:         string,
    ?image:               string,
    ?recurrence_rule:     RecurrenceRule
) =
    inherit Payload() with
        override _.Content = json {
            optional "channel_id" channel_id
            optional "entity_metadata" entity_metadata
            required "name" name
            required "privacy_level" privacy_level
            required "scheduled_start_time" scheduled_start_time
            optional "scheduled_end_time" scheduled_end_time
            optional "description" description
            required "entity_type" entity_type
            optional "image" image
            optional "recurrence_rule" recurrence_rule
        }

type ModifyGuildScheduledEventPayload (
    ?channel_id:           string option,
    ?entity_metadata:      EntityMetadata option,
    ?name:                 string,
    ?privacy_level:        PrivacyLevelType,
    ?scheduled_start_time: DateTime,
    ?scheduled_end_time:   DateTime,
    ?description:          string option,
    ?entity_type:          ScheduledEntityType,
    ?image:                string,
    ?recurrence_rule:      RecurrenceRule option
) =
    inherit Payload() with
        override _.Content = json {
            optional "channel_id" channel_id
            optional "entity_metadata" entity_metadata
            optional "name" name
            optional "privacy_level" privacy_level
            optional "scheduled_start_time" scheduled_start_time
            optional "scheduled_end_time" scheduled_end_time
            optional "description" description
            optional "entity_type" entity_type
            optional "image" image
            optional "recurrence_rule" recurrence_rule
        }

// ----- Guild Template -----

type CreateGuildFromTemplatePayload (
    name:  string,
    ?icon: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "icon" icon
        }

type CreateGuildTemplatePayload (
    name:         string,
    ?description: string
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            optional "description" description
        }

type ModifyGuildTemplatePayload (
    ?name:        string,
    ?description: string
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "description" description
        }

// ----- Invite -----

// ----- Message -----

type CreateMessagePayload (
    ?content:           string,
    ?nonce:             MessageNonce,
    ?tts:               bool,
    ?embeds:            Embed list,
    ?allow_mentions:    AllowedMentions,
    ?message_reference: MessageReference,
    ?components:        Component list,
    ?sticker_ids:       string list,
    ?attachments:       PartialAttachment list,
    ?flags:             int,
    ?enforce_nonce:     bool,
    ?poll:              Poll,
    ?files:             IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "nonce" (match nonce with | Some (MessageNonce.Number n) -> Some n | _ -> None)
                optional "nonce" (match nonce with | Some (MessageNonce.String s) -> Some s | _ -> None)
                optional "tts" tts
                optional "embeds" embeds
                optional "allowed_mentions" allow_mentions
                optional "message_reference" message_reference
                optional "components" components
                optional "sticker_ids" sticker_ids
                optional "attachments" attachments
                optional "flags" flags
                optional "enforce_nonce" enforce_nonce
                optional "poll" poll
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type EditMessagePayload (
    ?content:        string option,
    ?embeds:         Embed list option,
    ?flags:          int option,
    ?allow_mentions: AllowedMentions option,
    ?components:     Component list option,
    ?attachments:    PartialAttachment list option,
    ?files:          IDictionary<string, IPayloadBuilder>
) =
    inherit Payload() with
        override _.Content =
            let payload_json = json {
                optional "content" content
                optional "embeds" embeds
                optional "flags" flags
                optional "allowed_mentions" allow_mentions
                optional "components" components
                optional "attachments" attachments
            }

            match files with
            | None -> payload_json
            | Some f -> multipart {
                part "payload_json" payload_json
                files f
            }

type BulkDeleteMessagesPayload (
    messages: string list
) =
    inherit Payload() with
        override _.Content = json {
            required "messages" messages
        }

// ----- Poll -----

type GetAnswerVotersOkResponse = {
    [<JsonPropertyName "users">] Users: User list
}

// ----- Role Connection -----

type UpdateApplicationRoleConnectionMetadataRecordsPayload (
    ``type``: ApplicationRoleConnectionMetadataType,
    key: string,
    name: string,
    description: string,
    ?name_localizations: IDictionary<string, string>,
    ?description_localizations: IDictionary<string, string>
) =
    inherit Payload() with
        override _.Content = json {
            required "type" ``type``
            required "key" key
            required "name" name
            optional "name_localizations" name_localizations
            required "description" description
            optional "description_localizations" description_localizations
        }

// ----- Sku -----

// ----- Soundboard -----

type SendSoundboardSoundPayload (
    sound_id:         string,
    ?source_guild_id: string
) =
    inherit Payload() with
        override _.Content = json {
            required "sound_id" sound_id
            optional "source_guild_id" source_guild_id
        }

type CreateGuildSoundboardSoundPayload (
    name: string,
    sound: string,
    ?volume: double option,
    ?emoji_id: string option,
    ?emoji_name: string option
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "sound" sound
            optional "volume" volume
            optional "emoji_id" emoji_id
            optional "emoji_name" emoji_name
        }

type ModifyGuildSoundboardSoundPayload (
    ?name: string,
    ?volume: double option,
    ?emoji_id: string option,
    ?emoji_name: string option
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "volume" volume
            optional "emoji_id" emoji_id
            optional "emoji_name" emoji_name
        }

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

type GetGatewayOkResponse = {
    [<JsonPropertyName "url">] Url: string
}

type GetGatewayBotOkResponse = {
    [<JsonPropertyName "url">] Url: string
    [<JsonPropertyName "shards">] Shards: int
    [<JsonPropertyName "session_start_limit">] SessionStartLimit: SessionStartLimit
}

// ----- OAuth2 -----

type GetCurrentAuthorizationInformationOkResponse = {
    [<JsonPropertyName "application">] Application: PartialApplication
    [<JsonPropertyName "scopes">] Scopes: OAuth2Scope list
    [<JsonPropertyName "expires">] Expires: DateTime
    [<JsonPropertyName "user">] User: User option
}
