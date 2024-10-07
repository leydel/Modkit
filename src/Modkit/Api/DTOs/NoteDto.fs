namespace Modkit.Api.DTOs

open Modkit.Api.Models
open System
open System.Text.Json.Serialization

type NoteDto = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "user_id">] UserId: string
    [<JsonPropertyName "message">] Message: string
    [<JsonPropertyName "created_at">] CreatedAt: DateTime
}
with
    static member from (model: Note) = {
        Id = model.Id;
        UserId = model.UserId;
        Message = model.Message;
        CreatedAt = model.CreatedAt;
    }
