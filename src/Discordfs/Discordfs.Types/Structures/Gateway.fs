namespace Discordfs.Types

open System
open System.Text.Json.Serialization

#nowarn "49"

type ClientStatus = {
    [<JsonPropertyName "desktop">] Desktop: string option
    [<JsonPropertyName "mobile">] Mobile: string option
    [<JsonPropertyName "web">] Web: string option
}

// https://discord.com/developers/docs/topics/gateway#session-start-limit-object-session-start-limit-structure
type SessionStartLimit = {
    [<JsonPropertyName "total">] Total: int
    [<JsonPropertyName "remaining">] Remaining: int
    [<JsonPropertyName "reset_after">] ResetAfter: int
    [<JsonPropertyName "max_concurrency">] MaxConcurrency: int
}

// https://discord.com/developers/docs/topics/gateway-events#identify-identify-connection-properties
type ConnectionProperties = {
    [<JsonPropertyName "os">] OperatingSystem: string
    [<JsonPropertyName "browser">] Browser: string
    [<JsonPropertyName "device">] Device: string
}
with
    static member build(
        OperatingSystem: string,
        Browser: string,
        Device: string
    ) = {
        OperatingSystem = OperatingSystem;
        Browser = Browser;
        Device = Device;
    }

    static member build(OperatingSystem: string) =
        ConnectionProperties.build(OperatingSystem, "Discordfs", "Discordfs")

    static member build() =
        let operatingSystem =
            match Environment.OSVersion.Platform with
            | PlatformID.Win32NT -> "Windows"
            | PlatformID.Unix -> "Linux"
            | _ -> "Unknown OS"

        ConnectionProperties.build(operatingSystem)
