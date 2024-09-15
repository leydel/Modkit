namespace Modkit.Diacord.Core.Structures

open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System
open System.Text.Json.Serialization

[<CustomEquality>]
[<NoComparison>]
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
    static member diff (s1: DiacordChannel) (s2: Channel) =
        List.collect Option.toList <| [
            Diff.from "type" (Some s1.Type) (Some s2.Type);
            Diff.from "name" (Some s1.Name) (Some s2.Name);
            Diff.from "topic" s1.Topic s2.Topic;
            Diff.from "bitrate" s1.Bitrate s2.Bitrate
            Diff.from "user_limit" s1.UserLimit s2.UserLimit
            Diff.from "rate_limit_per_user" s1.RateLimitPerUser s2.RateLimitPerUser
            Diff.from "position" s1.Position s2.Position
            Diff.from "parent_id" s1.ParentId s2.ParentId
            Diff.from "nsfw" s1.Nsfw s2.Nsfw
            Diff.from "rtc_region" s1.RtcRegion s2.RtcRegion
            Diff.from "video_quality_mode" s1.VideoQualityMode s2.VideoQualityMode
            Diff.from "default_auto_archive_duration" s1.DefaultAutoArchiveDuration s2.DefaultAutoArchiveDuration
            Diff.from "default_reaction_emoji" s1.DefaultReactionEmoji s2.DefaultReactionEmoji
            Diff.from "available_tags" s1.AvailableTags s2.AvailableTags
            Diff.from "default_sort_order" s1.DefaultSortOrder s2.DefaultSortOrder
            Diff.from "default_forum_layout" s1.DefaultForumLayout s2.DefaultForumLayout
            Diff.from "default_thread_rate_limit_per_user" s1.DefaultThreadRateLimitPerUser s2.DefaultThreadRateLimitPerUser
        ]

    interface IEquatable<Channel> with
        override this.Equals other =
            List.isEmpty <| DiacordChannel.diff this other
