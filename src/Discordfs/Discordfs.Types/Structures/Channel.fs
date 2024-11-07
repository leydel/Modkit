namespace Discordfs.Types

open System
open System.Text.Json.Serialization

type DefaultReaction = {
    [<JsonPropertyName "emoji_id">] EmojiId: string option
    [<JsonPropertyName "emoji_name">] EmojiName: string option
}

type ThreadMetadata = {
    [<JsonPropertyName "archived">] Archived: bool
    [<JsonPropertyName "auto_archive_duration">] AutoArchiveDuration: int
    [<JsonPropertyName "archive_timestamp">] ArchiveTimestamp: DateTime
    [<JsonPropertyName "locked">] Locked: bool
    [<JsonPropertyName "invitable">] Invitable: bool option
    [<JsonPropertyName "create_timestamp">] CreateTimestamp: DateTime option
}

type ThreadMember = {
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "user_id">] UserId: string option
    [<JsonPropertyName "join_timestamp">] JoinTimestamp: DateTime
    [<JsonPropertyName "flags">] Flags: int
    [<JsonPropertyName "member">] Member: GuildMember option
}

type ChannelTag = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "moderated">] Moderated: bool
    [<JsonPropertyName "emoji_id">] EmojiId: string option
    [<JsonPropertyName "emoji_name">] EmojiName: string option
}

type PermissionOverwrite = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: PermissionOverwriteType
    [<JsonPropertyName "allow">] Allow: string
    [<JsonPropertyName "deny">] Deny: string
}

type PartialPermissionOverwrite = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: PermissionOverwriteType
    [<JsonPropertyName "allow">] Allow: string option
    [<JsonPropertyName "deny">] Deny: string option
}

type Channel = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: ChannelType
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "permission_overwrites">] PermissionOverwrites: PermissionOverwrite list option
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "topic">] Topic: string option
    [<JsonPropertyName "nsfw">] Nsfw: bool option
    [<JsonPropertyName "last_message_id">] LastMessageId: string option
    [<JsonPropertyName "bitrate">] Bitrate: int option
    [<JsonPropertyName "user_limit">] UserLimit: int option
    [<JsonPropertyName "rate_limit_per_user">] RateLimitPerUser: int option
    [<JsonPropertyName "recipients">] Recipients: User list option
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "owner_id">] OwnerId: string option
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "managed">] Managed: bool option
    [<JsonPropertyName "parent_id">] ParentId: string option
    [<JsonPropertyName "last_pin_timestamp">] LastPinTimestamp: DateTime option
    [<JsonPropertyName "rtc_region">] RtcRegion: string option
    [<JsonPropertyName "video_quality_mode">] VideoQualityMode: VideoQualityMode option
    [<JsonPropertyName "message_count">] MessageCount: int option
    [<JsonPropertyName "member_count">] MemberCount: int option
    [<JsonPropertyName "thread_metadata">] ThreadMetadata: ThreadMetadata option
    [<JsonPropertyName "member">] Member: ThreadMember option
    [<JsonPropertyName "default_auto_archive_duration">] DefaultAutoArchiveDuration: AutoArchiveDurationType option
    [<JsonPropertyName "permissions">] Permissions: string option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "total_messages_sent">] TotalMessagesSent: int option
    [<JsonPropertyName "available_tags">] AvailableTags: ChannelTag list option
    [<JsonPropertyName "applied_tags">] AppliedTags: int list option
    [<JsonPropertyName "default_reaction_emoji">] DefaultReactionEmoji: DefaultReaction option
    [<JsonPropertyName "default_thread_rate_limit_per_user">] DefaultThreadRateLimitPerUser: int option
    [<JsonPropertyName "default_sort_order">] DefaultSortOrder: ChannelSortOrder option
    [<JsonPropertyName "default_forum_layout">] DefaultForumLayout: ChannelForumLayout option
}

// TODO: Create DU for different channel types

type PartialChannel = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "type">] Type: ChannelType option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "position">] Position: int option
    [<JsonPropertyName "permission_overwrites">] PermissionOverwrites: PermissionOverwrite list option
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "topic">] Topic: string option
    [<JsonPropertyName "nsfw">] Nsfw: bool option
    [<JsonPropertyName "last_message_id">] LastMessageId: string option
    [<JsonPropertyName "bitrate">] Bitrate: int option
    [<JsonPropertyName "user_limit">] UserLimit: int option
    [<JsonPropertyName "rate_limit_per_user">] RateLimitPerUser: int option
    [<JsonPropertyName "recipients">] Recipients: User list option
    [<JsonPropertyName "icon">] Icon: string option
    [<JsonPropertyName "owner_id">] OwnerId: string option
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "managed">] Managed: bool option
    [<JsonPropertyName "parent_id">] ParentId: string option
    [<JsonPropertyName "last_pin_timestamp">] LastPinTimestamp: DateTime option
    [<JsonPropertyName "rtc_region">] RtcRegion: string option
    [<JsonPropertyName "video_quality_mode">] VideoQualityMode: VideoQualityMode option
    [<JsonPropertyName "message_count">] MessageCount: int option
    [<JsonPropertyName "member_count">] MemberCount: int option
    [<JsonPropertyName "thread_metadata">] ThreadMetadata: ThreadMetadata option
    [<JsonPropertyName "member">] Member: ThreadMember option
    [<JsonPropertyName "default_auto_archive_duration">] DefaultAutoArchiveDuration: AutoArchiveDurationType option
    [<JsonPropertyName "permissions">] Permissions: string option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "total_messages_sent">] TotalMessagesSent: int option
    [<JsonPropertyName "available_tags">] AvailableTags: ChannelTag list option
    [<JsonPropertyName "applied_tags">] AppliedTags: int list option
    [<JsonPropertyName "default_reaction_emoji">] DefaultReactionEmoji: DefaultReaction option
    [<JsonPropertyName "default_thread_rate_limit_per_user">] DefaultThreadRateLimitPerUser: int option
    [<JsonPropertyName "default_sort_order">] DefaultSortOrder: ChannelSortOrder option
    [<JsonPropertyName "default_forum_layout">] DefaultForumLayout: ChannelForumLayout option
}

type FollowedChannel = {
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "webhook_id">] WebhookId: string
}
