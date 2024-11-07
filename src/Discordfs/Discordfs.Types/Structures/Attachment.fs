namespace Discordfs.Types

open System.Text.Json.Serialization

type Attachment = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "filename">] Filename: string
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "content_type">] ContentType: string option
    [<JsonPropertyName "size">] Size: int
    [<JsonPropertyName "url">] Url: string
    [<JsonPropertyName "proxy_url">] ProxyUrl: string
    [<JsonPropertyName "height">] Height: int option
    [<JsonPropertyName "width">] Width: int option
    [<JsonPropertyName "ephemeral">] Ephemeral: bool option
    [<JsonPropertyName "duration_secs">] DurationSecs: float option
    [<JsonPropertyName "waveform">] Waveform: string option
    [<JsonPropertyName "flags">] Flags: int option
}

type PartialAttachment = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "filename">] Filename: string option
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "content_type">] ContentType: string option
    [<JsonPropertyName "size">] Size: int option
    [<JsonPropertyName "url">] Url: string option
    [<JsonPropertyName "proxy_url">] ProxyUrl: string option
    [<JsonPropertyName "height">] Height: int option
    [<JsonPropertyName "width">] Width: int option
    [<JsonPropertyName "ephemeral">] Ephemeral: bool option
    [<JsonPropertyName "duration_secs">] DurationSecs: float option
    [<JsonPropertyName "waveform">] Waveform: string option
    [<JsonPropertyName "flags">] Flags: int option
}
