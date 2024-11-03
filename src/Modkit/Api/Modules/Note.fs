namespace Modkit.Api.Modules

open System
open System.Text.Json
open System.Text.Json.Serialization

type Note = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "guildId">] GuildId: string
    [<JsonPropertyName "memberId">] MemberId: string
    [<JsonPropertyName "messageId">] MessageId: string option
    [<JsonPropertyName "content">] Content: string
    [<JsonPropertyName "_ts">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] CreatedAt: DateTime
}

type NoteDto = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "member_id">] MemberId: string
    [<JsonPropertyName "message_id">] MessageId: string option
    [<JsonPropertyName "content">] Content: string
    [<JsonPropertyName "created_at">] [<JsonConverter(typeof<Converters.UnixEpoch>)>] CreatedAt: DateTime
}

module Note =
    let toDto (model: Note): NoteDto = {
        Id = model.Id;
        GuildId = model.GuildId;
        MemberId = model.MemberId;
        MessageId = model.MessageId;
        Content = model.Content;
        CreatedAt = model.CreatedAt;
    }

    let fromDto (dto: NoteDto): Note = {
        Id = dto.Id;
        GuildId = dto.GuildId;
        MemberId = dto.MemberId;
        MessageId = dto.MessageId;
        Content = dto.Content;
        CreatedAt = dto.CreatedAt;        
    }
