namespace Discordfs.Types

open System
open System.Text.Json.Serialization

#nowarn "49"

type EmbedFooter = {
    [<JsonPropertyName "text">] Text: string
    [<JsonPropertyName "icon_url">] IconUrl: string option
    [<JsonPropertyName "proxy_icon_url">] ProxyIconUrl: string option
}
with
    static member build(
        Text: string,
        ?IconUrl: string,
        ?ProxyIconUrl: string
    ) = {
        Text = Text;
        IconUrl = IconUrl;
        ProxyIconUrl = ProxyIconUrl;
    }

type EmbedImage = {
    [<JsonPropertyName "url">] Url: string
    [<JsonPropertyName "proxy_url">] ProxyUrl: string option
    [<JsonPropertyName "height">] Height: int option
    [<JsonPropertyName "width">] Width: int option
}
with
    static member build(
        Url: string,
        ?ProxyUrl: string,
        ?Height: int,
        ?Width: int
    ) = {
        Url = Url;
        ProxyUrl = ProxyUrl;
        Height = Height;
        Width = Width;
    }

type EmbedThumbnail = {
    [<JsonPropertyName "url">] Url: string
    [<JsonPropertyName "proxy_url">] ProxyUrl: string option
    [<JsonPropertyName "height">] Height: int option
    [<JsonPropertyName "width">] Width: int option
}
with
    static member build(
        Url: string,
        ?ProxyUrl: string,
        ?Height: int,
        ?Width: int
    ) = {
        Url = Url;
        ProxyUrl = ProxyUrl;
        Height = Height;
        Width = Width;
    }

type EmbedVideo = {
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "proxy_url">] ProxyUrl: string option
    [<JsonPropertyName "height">] Height: int option
    [<JsonPropertyName "width">] Width: int option
}
with
    static member build(
        ?Url: string,
        ?ProxyUrl: string,
        ?Height: int,
        ?Width: int
    ) = {
        Url = Url;
        ProxyUrl = ProxyUrl;
        Height = Height;
        Width = Width;
    }

type EmbedProvider = {
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "url">] Url: string option
}
with
    static member build(
        ?Name: string,
        ?Url: string
    ) = {
        Name = Name;
        Url = Url;
    }

type EmbedAuthor = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "icon_url">] IconUrl: string option
    [<JsonPropertyName "proxy_icon_url">] ProxyIconUrl: string option
}
with
    static member build(
        Name: string,
        ?Url: string,
        ?IconUrl: string,
        ?ProxyIconUrl: string
    ) = {
        Name = Name;
        Url = Url;
        IconUrl = IconUrl;
        ProxyIconUrl = ProxyIconUrl;
    }

type EmbedField = {
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "value">] Value: string
    [<JsonPropertyName "inline">] Inline: bool option
}
with
    static member build(
        Name: string,
        Value: string,
        ?Inline: bool
    ) = {
        Name = Name;
        Value = Value;
        Inline = Inline;
    }

type Embed = {
    [<JsonPropertyName "title">] Title: string option
    [<JsonPropertyName "type">] Type: string option
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "timestamp">] Timestamp: DateTime option
    [<JsonPropertyName "color">] Color: int option
    [<JsonPropertyName "footer">] Footer: EmbedFooter option
    [<JsonPropertyName "image">] Image: EmbedImage option
    [<JsonPropertyName "thumbnail">] Thumbnail: EmbedThumbnail option
    [<JsonPropertyName "video">] Video: EmbedVideo option
    [<JsonPropertyName "provider">] Provider: EmbedProvider option
    [<JsonPropertyName "author">] Author: EmbedAuthor option
    [<JsonPropertyName "fields">] Fields: EmbedField list option
}
with
    static member build(
        ?Title: string,
        ?Type: string,
        ?Description: string,
        ?Url: string,
        ?Timestamp: DateTime,
        ?Color: int,
        ?Footer: EmbedFooter,
        ?Image: EmbedImage,
        ?Thumbnail: EmbedThumbnail,
        ?Video: EmbedVideo,
        ?Provider: EmbedProvider,
        ?Author: EmbedAuthor,
        ?Fields: EmbedField list
    ) = {
        Title = Title;
        Type = Type;
        Description = Description;
        Url = Url;
        Timestamp = Timestamp;
        Color = Color;
        Footer = Footer;
        Image = Image;
        Thumbnail = Thumbnail;
        Video = Video;
        Provider = Provider;
        Author = Author;
        Fields = Fields;
    }