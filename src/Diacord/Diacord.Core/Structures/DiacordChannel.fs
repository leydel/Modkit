namespace Modkit.Diacord.Core.Structures

open Discordfs.Types
open Modkit.Diacord.Core.Types
open System.Collections.Generic
open System.Text.Json.Serialization

type DiacordChannel = {
    [<JsonName "diacord_id">] [<JsonRequired>] DiacordId: string
    [<JsonName "type">] [<JsonRequired>] Type: ChannelType
    [<JsonName "name">] Name: string option
    [<JsonName "topic">] Topic: string option
    [<JsonName "bitrate">] Bitrate: int option
    [<JsonName "user_limit">] UserLimit: int option
    [<JsonName "rate_limit_per_user">] RateLimitPerUser: int option
    [<JsonName "position">] Position: int option
    // TODO: Add `permission_overwrites`
    [<JsonName "parent_id">] ParentId: string option
    [<JsonName "nsfw">] Nsfw: bool option
    [<JsonName "rtc_region">] RtcRegion: string option
    [<JsonName "video_quality_mode">] VideoQualityMode: VideoQualityMode option
    [<JsonName "default_auto_archive_duration">] DefaultAutoArchiveDuration: AutoArchiveDurationType option
    [<JsonName "default_reaction_emoji">] DefaultReactionEmoji: DefaultReaction option
    [<JsonName "available_tags">] AvailableTags: ChannelTag list option
    [<JsonName "default_sort_order">] DefaultSortOrder: ChannelSortOrder option
    [<JsonName "default_forum_layout">] DefaultForumLayout: ChannelForumLayout option
    [<JsonName "default_thread_rate_limit_per_user">] DefaultThreadRateLimitPerUser: int option
}
with
    static member from (channel: Channel) = {
        DiacordId = channel.Id;
        Type = channel.Type;
        Name = channel.Name;
        Topic = channel.Topic;
        Bitrate = channel.Bitrate;
        UserLimit = channel.UserLimit;
        RateLimitPerUser = channel.RateLimitPerUser;
        Position = channel.Position;
        ParentId = channel.ParentId;
        Nsfw = channel.Nsfw;
        RtcRegion = channel.RtcRegion;
        VideoQualityMode = channel.VideoQualityMode;
        DefaultAutoArchiveDuration = channel.DefaultAutoArchiveDuration;
        DefaultReactionEmoji = channel.DefaultReactionEmoji;
        AvailableTags = channel.AvailableTags;
        DefaultSortOrder = channel.DefaultSortOrder;
        DefaultForumLayout = channel.DefaultForumLayout;
        DefaultThreadRateLimitPerUser = channel.DefaultThreadRateLimitPerUser;
    }

    static member diff (mappings: IDictionary<string, string>) ((a: DiacordChannel option), (b: Channel option)) =
        // TODO: Add `permission_overwrites`

        let parentId =
            match a >>= _.ParentId with
            | None -> None
            | Some parentId ->
                match mappings.TryGetValue parentId with
                | false, _ -> None
                | true, id -> Some id

        DiffNode.leaf a b [
            Diff.from "type" (a >>. _.Type) (b >>. _.Type);
            Diff.from "name" (a >>. _.Name) (b >>. _.Name);
            Diff.from "topic" (a >>= _.Topic) (b >>= _.Topic);
            Diff.from "bitrate" (a >>= _.Bitrate) (b >>= _.Bitrate);
            Diff.from "user_limit" (a >>= _.UserLimit) (b >>= _.UserLimit);
            Diff.from "rate_limit_per_user" (a >>= _.RateLimitPerUser) (b >>= _.RateLimitPerUser);
            Diff.from "position" (a >>= _.Position) (b >>= _.Position);
            Diff.from "parent_id" parentId (b >>= _.ParentId);
            Diff.from "nsfw" (a >>= _.Nsfw) (b >>= _.Nsfw);
            Diff.from "rtc_region" (a >>= _.RtcRegion) (b >>= _.RtcRegion);
            Diff.from "video_quality_mode" (a >>= _.VideoQualityMode) (b >>= _.VideoQualityMode);
            Diff.from "default_auto_archive_duration" (a >>= _.DefaultAutoArchiveDuration) (b >>= _.DefaultAutoArchiveDuration);
            Diff.from "default_reaction_emoji" (a >>= _.DefaultReactionEmoji) (b >>= _.DefaultReactionEmoji);
            Diff.from "available_tags" (a >>= _.AvailableTags) (b >>= _.AvailableTags);
            Diff.from "default_sort_order" (a >>= _.DefaultSortOrder) (b >>= _.DefaultSortOrder);
            Diff.from "default_forum_layout" (a >>= _.DefaultForumLayout) (b >>= _.DefaultForumLayout);
            Diff.from "default_thread_rate_limit_per_user" (a >>= _.DefaultThreadRateLimitPerUser) (b >>= _.DefaultThreadRateLimitPerUser);
        ]
