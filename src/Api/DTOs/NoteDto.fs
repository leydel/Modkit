namespace Modkit.Api.DTOs

open Modkit.Api.Models
open System
open System.Text.Json.Serialization

type NoteDto = {
    [<JsonName "id">] Id: string
    [<JsonName "user_id">] UserId: string
    [<JsonName "message">] Message: string
    [<JsonName "created_at">] CreatedAt: DateTime
}
with
    static member from (model: Note) = {
        Id = model.Id;
        UserId = model.UserId;
        Message = model.Message;
        CreatedAt = model.CreatedAt;
    }
