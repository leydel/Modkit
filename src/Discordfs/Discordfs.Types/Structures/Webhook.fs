namespace Discordfs.Types

open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/webhook#webhook-object-webhook-structure
type Webhook = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "webhook_type">] Type: WebhookType
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "channel_id">] ChannelId: string option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "name">] Name: string option
    [<JsonPropertyName "avatar">] Avatar: string option
    [<JsonPropertyName "token">] Token: string option
    [<JsonPropertyName "application_id">] ApplicationId: string option
    [<JsonPropertyName "source_guild">] SourceGuild: PartialGuild option
    [<JsonPropertyName "source_channel">] SourceChannel: PartialChannel option
    [<JsonPropertyName "url">] Url: string option
}
