namespace Discordfs.Types

open System
open System.Collections.Generic
open System.Text.Json.Serialization

#nowarn "49"

type AllowedMentions = {
    [<JsonPropertyName "parse">] Parse: AllowedMentionsParseType list
    [<JsonPropertyName "roles">] Roles: string list option
    [<JsonPropertyName "users">] Users: string list option
    [<JsonPropertyName "replied_user">] RepliedUser: bool option
}
with
    static member build(
        Parse: AllowedMentionsParseType list,
        ?Roles: string list,
        ?Users: string list,
        ?RepliedUser: bool
    ) = {
        Parse = Parse;
        Roles = Roles;
        Users = Users;
        RepliedUser = RepliedUser;
    }

type RoleSubscriptionData = {
    [<JsonPropertyName "role_subscription_listing_id">] RoleSubscriptionListingId: string
    [<JsonPropertyName "tier_name">] TierName: string
    [<JsonPropertyName "total_months_subscribed">] TotalMonthsSubscribed: int
    [<JsonPropertyName "is_renewal">] IsRenewal: bool
}

type ChannelMention = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "type">] Type: ChannelType
    [<JsonPropertyName "name">] Name: string
}

type MessageActivity = {
    [<JsonPropertyName "type">] Type: MessageActivityType
    [<JsonPropertyName "party_id">] PartyId: string option
}

type MessageReference = {
    [<JsonPropertyName "message_id">] MessageId: string option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "fail_if_not_exists">] FailIfNotExists: bool option
}

type MessageInteractionMetadata = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: InteractionType
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "authorizing_integration_owners">] AuthorizingIntegrationOwners: Map<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>
    [<JsonPropertyName "original_response_message_id">] OriginalResponseMessage: string option
    [<JsonPropertyName "interacted_message_id">] InteractedMessageId: string option
    [<JsonPropertyName "triggering_interaction_metadata">] TriggeringInteractionMetadata: MessageInteractionMetadata option
}

type MessageInteraction = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: InteractionType
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "member">] Member: PartialGuildMember option
}

type MessageCall = {
    [<JsonPropertyName "participants">] Participants: string list
    [<JsonPropertyName "ended_timestamp">] EndedTimestamp: DateTime option
}

type Message = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "author">] Author: User
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "timestamp">] Timestamp: DateTime
    [<JsonPropertyName "edited_timestamp">] EditedTimestamp: DateTime option
    [<JsonPropertyName "tts">] Tts: bool
    [<JsonPropertyName "mention_everyone">] MentionEveryone: bool
    [<JsonPropertyName "mentions">] Mentions: User list
    [<JsonPropertyName "mention_roles">] MentionRoles: string list
    [<JsonPropertyName "mention_channels">] MentionChannels: ChannelMention list
    [<JsonPropertyName "attachments">] Attachments: Attachment list
    [<JsonPropertyName "embeds">] Embeds: Embed list
    [<JsonPropertyName "reactions">] Reactions: Reaction list
    [<JsonPropertyName "nonce">] Nonce: MessageNonce option
    [<JsonPropertyName "pinned">] Pinned: bool
    [<JsonPropertyName "webhook_id">] WebhookId: string option
    [<JsonPropertyName "type">] Type: MessageType
    [<JsonPropertyName "activity">] Activity: MessageActivity option
    [<JsonPropertyName "application">] Application: PartialApplication option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "message_reference">] MessageReference: MessageReference option
    [<JsonPropertyName "message_snapshots">] MessageSnapshots: MessageSnapshot list option
    [<JsonPropertyName "referenced_message">] ReferencedMessage: Message option
    [<JsonPropertyName "interaction_metadata">] InteractionMetadata: MessageInteractionMetadata option
    [<JsonPropertyName "interaction">] Interaction: MessageInteraction option
    [<JsonPropertyName "thread">] Thread: Channel option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "sticker_items">] StickerItems: StickerItem list option
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "role_subscription_data">] RoleSubscriptionData: RoleSubscriptionData option
    [<JsonPropertyName "resolved">] Resolved: ResolvedData option
    [<JsonPropertyName "poll">] Poll: Poll option
    [<JsonPropertyName "call">] Call: MessageCall option
}

and PartialMessage = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "author">] Author: User option
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "timestamp">] Timestamp: DateTime option
    [<JsonPropertyName "edited_timestamp">] EditedTimestamp: DateTime option
    [<JsonPropertyName "tts">] Tts: bool option
    [<JsonPropertyName "mention_everyone">] MentionEveryone: bool option
    [<JsonPropertyName "mentions">] Mentions: User list option
    [<JsonPropertyName "mention_roles">] MentionRoles: string list option
    [<JsonPropertyName "mention_channels">] MentionChannels: ChannelMention list option
    [<JsonPropertyName "attachments">] Attachments: Attachment list option
    [<JsonPropertyName "embeds">] Embeds: Embed list option
    [<JsonPropertyName "reactions">] Reactions: Reaction list option
    [<JsonPropertyName "nonce">] Nonce: MessageNonce option
    [<JsonPropertyName "pinned">] Pinned: bool option
    [<JsonPropertyName "webhook_id">] WebhookId: string option
    [<JsonPropertyName "type">] Type: MessageType option
    [<JsonPropertyName "activity">] Activity: MessageActivity option
    [<JsonPropertyName "application">] Application: PartialApplication option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "message_reference">] MessageReference: MessageReference option
    [<JsonPropertyName "message_snapshots">] MessageSnapshots: MessageSnapshot list option
    [<JsonPropertyName "referenced_message">] ReferencedMessage: Message option
    [<JsonPropertyName "interaction_metadata">] InteractionMetadata: MessageInteractionMetadata option
    [<JsonPropertyName "interaction">] Interaction: MessageInteraction option
    [<JsonPropertyName "thread">] Thread: Channel option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "sticker_items">] StickerItems: StickerItem list option
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "role_subscription_data">] RoleSubscriptionData: RoleSubscriptionData option
    [<JsonPropertyName "resolved">] Resolved: ResolvedData option
    [<JsonPropertyName "poll">] Poll: Poll option
    [<JsonPropertyName "call">] Call: MessageCall option
}

/// A partial message specifically for message snapshots
and SnapshotPartialMessage = {
    [<JsonPropertyName "content">] Content: string option
    [<JsonPropertyName "timestamp">] Timestamp: DateTime
    [<JsonPropertyName "edited_timestamp">] EditedTimestamp: DateTime option
    [<JsonPropertyName "mentions">] Mentions: User list
    [<JsonPropertyName "mention_roles">] MentionRoles: string list
    [<JsonPropertyName "attachments">] Attachments: Attachment list
    [<JsonPropertyName "embeds">] Embeds: Embed list
    [<JsonPropertyName "type">] Type: MessageType
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "components">] Components: Component list option
    [<JsonPropertyName "sticker_items">] StickerItems: StickerItem list option
}

and MessageSnapshot = {
    [<JsonPropertyName "message">] Message: SnapshotPartialMessage
}

and ResolvedData = {
    [<JsonPropertyName "users">] Users: IDictionary<string, User> option
    [<JsonPropertyName "members">] Members: IDictionary<string, PartialGuildMember> option
    [<JsonPropertyName "roles">] Roles: IDictionary<string, Role> option
    [<JsonPropertyName "channels">] Channels: IDictionary<string, PartialChannel> option
    [<JsonPropertyName "messages">] Messages: IDictionary<string, PartialMessage> option
    [<JsonPropertyName "attachments">] Attachments: IDictionary<string, Attachment> option
}