﻿namespace Modkit.Diacord.Core.Structures

open Discordfs.Types
open Modkit.Diacord.Core.Types
open System.Collections.Generic
open System.Text.Json.Serialization

[<JsonConverter(typeof<DiacordGenericChannelConverter>)>]
type DiacordGenericChannel =
    | Category of DiacordCategory
    | Channel of DiacordChannel
with
    static member from (channel: Channel) =
        match channel.Type with
        | ChannelType.GUILD_CATEGORY -> DiacordGenericChannel.Category <| DiacordCategory.from channel
        | _ -> DiacordGenericChannel.Channel <| DiacordChannel.from channel

    static member group (genericChannels: DiacordGenericChannel list) =
        genericChannels |> List.collect (fun channel ->
            match channel with
            | DiacordGenericChannel.Category c ->
                let channels = genericChannels |> List.collect (fun channel ->
                    match channel with
                    | DiacordGenericChannel.Channel c -> [c]
                    | _ -> []
                )

                [DiacordGenericChannel.Category <| DiacordCategory.populate channels c]
            | DiacordGenericChannel.Channel c ->
                match c.ParentId with
                | Some _ -> []
                | None -> [DiacordGenericChannel.Channel c]
        )

    static member ungroup (genericChannels: DiacordGenericChannel list) =
        genericChannels |> List.collect (fun channel ->
            match channel with
            | DiacordGenericChannel.Category c ->
                let category = DiacordGenericChannel.Category <| DiacordCategory.unpopulate c
                let children = List.map DiacordGenericChannel.Channel c.Channels
                [category] @ children
            | DiacordGenericChannel.Channel c ->
                [DiacordGenericChannel.Channel c]
        )

    static member id (genericChannel: DiacordGenericChannel) =
        match genericChannel with
        | DiacordGenericChannel.Category c -> c.DiacordId
        | DiacordGenericChannel.Channel c -> c.DiacordId

    static member diff (mappings: IDictionary<string, string>) ((a: DiacordGenericChannel option), (b: Channel option)) =
        match a, b with
        | Some (DiacordGenericChannel.Category ca), c ->
            DiacordCategory.diff mappings (Some ca, c)
        | Some (DiacordGenericChannel.Channel ch), c ->
            DiacordChannel.diff mappings (Some ch, c)
        | None, Some c ->
            match c.Type with
            | ChannelType.GUILD_CATEGORY ->
                DiacordCategory.diff mappings (None, Some c)
            | _ ->
                DiacordChannel.diff mappings (None, Some c)
        | None, None ->
            DiacordChannel.diff mappings (None, None)

and DiacordGenericChannelConverter () =
    static member Todo: bool = false
    //inherit JsonConverter<DiacordGenericChannel> () with
    //    override _.Read (reader, typeToConvert, options) =
    //        ()

    //    override _.Write (writer, value, options) =
    //        ()

    // TODO: Implement serializer

and DiacordCategory = {
    [<JsonPropertyName "diacord_id">] DiacordId: string
    [<JsonPropertyName "channels">] Channels: DiacordChannel list
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "position">] Position: int option
    // TODO: Add `permission_overwrites`
    [<JsonPropertyName "nsfw">] Nsfw: bool option
}
with
    static member from (channel: Channel) =
        match channel.Type with
        | ChannelType.GUILD_CATEGORY ->
            let name =
                match channel.Name with
                | Some name -> name
                | None -> failwith "Invalid channel provided to DiacordCategory"

            {
                DiacordId = channel.Id;
                Channels = [];
                Name = name;
                Position = channel.Position;
                Nsfw = channel.Nsfw; 
            }
        | _ ->
            failwith "Provided channel to DiacordCategory is not a category"

    static member populate (channels: DiacordChannel list) (category: DiacordCategory) = {
        DiacordId = category.DiacordId;
        Channels = channels |> List.filter (fun c -> c.ParentId = Some category.DiacordId);
        Name = category.Name;
        Position = category.Position;
        Nsfw = category.Nsfw; 
    }

    static member unpopulate (category: DiacordCategory) = {
        DiacordId = category.DiacordId;
        Channels = [];
        Name = category.Name;
        Position = category.Position;
        Nsfw = category.Nsfw; 
    }

    static member diff (mappings: IDictionary<string, string>) ((a: DiacordCategory option), (b: Channel option)) =
        let id =
            match a, b with
            | None, None -> failwith "Diff called when no channel or diacord category present"
            | Some a, None -> a.DiacordId
            | None, Some b -> b.Id
            | Some a, Some b -> a.DiacordId

        // TODO: Figure out how this ID should work

        let channels =
            match a with
            | Some ca ->
                match ca.Channels.Length with
                | 0 -> []
                | _ -> [List.map (fun ch -> DiacordChannel.diff mappings (Some ch, b)) ca.Channels |> Diff.object "channels"] // TODO: `b` here is meant to be the actual channel not the category
            | _ -> []

        Diff.object id (channels @ [
            Diff.value "name" (a >>. _.Name) (b >>= _.Name);
            Diff.value "position" (a >>= _.Position) (b >>= _.Position);
            Diff.value "nsfw" (a >>= _.Nsfw) (b >>= _.Nsfw);
        ])

and DiacordChannel = {
    [<JsonPropertyName "diacord_id">] [<JsonRequired>] DiacordId: string
    [<JsonPropertyName "type">] [<JsonRequired>] Type: ChannelType
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "topic">] Topic: string option
    [<JsonPropertyName "bitrate">] Bitrate: int option
    [<JsonPropertyName "user_limit">] UserLimit: int option
    [<JsonPropertyName "rate_limit_per_user">] RateLimitPerUser: int option
    [<JsonPropertyName "position">] Position: int option
    // TODO: Add `permission_overwrites`
    [<JsonPropertyName "parent_id">] ParentId: string option
    [<JsonPropertyName "nsfw">] Nsfw: bool option
    [<JsonPropertyName "rtc_region">] RtcRegion: string option
    [<JsonPropertyName "video_quality_mode">] VideoQualityMode: VideoQualityMode option
    [<JsonPropertyName "default_auto_archive_duration">] DefaultAutoArchiveDuration: AutoArchiveDurationType option
    [<JsonPropertyName "default_reaction_emoji">] DefaultReactionEmoji: DefaultReaction option
    [<JsonPropertyName "available_tags">] AvailableTags: ChannelTag list option
    [<JsonPropertyName "default_sort_order">] DefaultSortOrder: ChannelSortOrder option
    [<JsonPropertyName "default_forum_layout">] DefaultForumLayout: ChannelForumLayout option
    [<JsonPropertyName "default_thread_rate_limit_per_user">] DefaultThreadRateLimitPerUser: int option
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
        let id =
            match a, b with
            | None, None -> failwith "Diff called when no channel or diacord channel present"
            | Some a, None -> a.DiacordId
            | None, Some b -> b.Id
            | Some a, Some b -> a.DiacordId

        // TODO: Figure out how this ID should work

        let parentId =
            match a >>= _.ParentId with
            | None -> None
            | Some parentId ->
                match mappings.TryGetValue parentId with
                | false, _ -> None
                | true, id -> Some id

        Diff.object id [
            Diff.value "type" (a >>. _.Type) (b >>. _.Type);
            Diff.value "name" (a >>. _.Name) (b >>. _.Name);
            Diff.value "topic" (a >>= _.Topic) (b >>= _.Topic);
            Diff.value "bitrate" (a >>= _.Bitrate) (b >>= _.Bitrate);
            Diff.value "user_limit" (a >>= _.UserLimit) (b >>= _.UserLimit);
            Diff.value "rate_limit_per_user" (a >>= _.RateLimitPerUser) (b >>= _.RateLimitPerUser);
            Diff.value "position" (a >>= _.Position) (b >>= _.Position);
            Diff.value "parent_id" parentId (b >>= _.ParentId);
            Diff.value "nsfw" (a >>= _.Nsfw) (b >>= _.Nsfw);
            Diff.value "rtc_region" (a >>= _.RtcRegion) (b >>= _.RtcRegion);
            Diff.value "video_quality_mode" (a >>= _.VideoQualityMode) (b >>= _.VideoQualityMode);
            Diff.value "default_auto_archive_duration" (a >>= _.DefaultAutoArchiveDuration) (b >>= _.DefaultAutoArchiveDuration);
            Diff.value "default_reaction_emoji" (a >>= _.DefaultReactionEmoji) (b >>= _.DefaultReactionEmoji);
            Diff.value "available_tags" (a >>= _.AvailableTags) (b >>= _.AvailableTags);
            Diff.value "default_sort_order" (a >>= _.DefaultSortOrder) (b >>= _.DefaultSortOrder);
            Diff.value "default_forum_layout" (a >>= _.DefaultForumLayout) (b >>= _.DefaultForumLayout);
            Diff.value "default_thread_rate_limit_per_user" (a >>= _.DefaultThreadRateLimitPerUser) (b >>= _.DefaultThreadRateLimitPerUser);
        ]
