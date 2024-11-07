namespace Discordfs.Types

open System
open System.Text.Json
open System.Text.Json.Serialization

#nowarn "49"

type ActivityTimestamps = {
    [<JsonPropertyName "start">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] Start: DateTime option
    [<JsonPropertyName "end">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] End: DateTime option
}

type ActivityEmoji = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "animated">] Animated: bool option
}

type ActivityParty = {
    [<JsonPropertyName "id">] Id: string option
    [<JsonPropertyName "size">] Size: (int * int) option
}

type ActivityAssets = {
    [<JsonPropertyName "large_image">] LargeImage: string option
    [<JsonPropertyName "large_text">] LargeText: string option
    [<JsonPropertyName "small_image">] SmallImage: string option
    [<JsonPropertyName "small_text">] SmallText: string option
}

type ActivitySecrets = {
    [<JsonPropertyName "join">] Join: string option
    [<JsonPropertyName "spectate">] Spectate: string option
    [<JsonPropertyName "matcch">] Match: string option
}

type ActivityButton = {
    [<JsonPropertyName "label">] Label: string
    [<JsonPropertyName "url">] Url: string
}

// https://discord.com/developers/docs/topics/gateway-events#activity-object-activity-structure
type Activity = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "type">] Type: ActivityType
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "created_at">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] CreatedAt: DateTime option
    [<JsonPropertyName "timestamps">] Timestamps: ActivityTimestamps option
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "details">] Details: string option
    [<JsonPropertyName "state">] State: string option
    [<JsonPropertyName "emoji">] Emoji: ActivityEmoji option
    [<JsonPropertyName "party">] Party: ActivityParty option
    [<JsonPropertyName "assets">] Assets: ActivityAssets option
    [<JsonPropertyName "secrets">] Secrets: ActivitySecrets option
    [<JsonPropertyName "instance">] Instance: bool option
    [<JsonPropertyName "flags">] Flags: int option
    [<JsonPropertyName "buttons">] Buttons: ActivityButton list option
}
with
    static member build (
        Type: ActivityType,
        Name: string,
        ?Url: string,
        ?CreatedAt: DateTime,
        ?Timestamps: ActivityTimestamps,
        ?ApplicationId: string,
        ?Details: string,
        ?State: string,
        ?Emoji: ActivityEmoji,
        ?Party: ActivityParty,
        ?Assets: ActivityAssets,
        ?Secrets: ActivitySecrets,
        ?Instance: bool,
        ?Flags: int,
        ?Buttons: ActivityButton list
    ) = {
        Name = Name;
        Type = Type;
        Url = Url;
        CreatedAt = CreatedAt;
        Timestamps = Timestamps;
        ApplicationId = ApplicationId;
        Details = Details;
        State = State;
        Emoji = Emoji;
        Party = Party;
        Assets = Assets;
        Secrets = Secrets;
        Instance = Instance;
        Flags = Flags;
        Buttons = Buttons;
    }
    
// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-location-object
type ActivityLocation = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "kind">] Kind: ActivityLocationKind
    [<JsonPropertyName "channel_id">] ChannelId: string
    [<JsonPropertyName "guild_id">] GuildId: string option
}

// https://discord.com/developers/docs/resources/application#get-application-activity-instance-activity-instance-object
type ActivityInstance = {
    [<JsonPropertyName "application_id">] ApplicationId: string
    [<JsonPropertyName "instance_id">] InstanceId: string
    [<JsonPropertyName "launch_id">] LaunchId: string
    [<JsonPropertyName "location">] Location: ActivityLocation
    [<JsonPropertyName "users">] Users: string list
}
