namespace Modkit.Api.DTOs

open FSharp.Json
open Modkit.Api.Models
open System

type NoteDto = {
    [<JsonField("id")>]
    Id: string
    
    [<JsonField("user_id")>]
    UserId: string
    
    [<JsonField("message")>]
    Message: string

    [<JsonField("created_at")>]
    CreatedAt: DateTime
}
with
    static member from (model: Note) = {
        Id = model.Id;
        UserId = model.UserId;
        Message = model.Message;
        CreatedAt = model.CreatedAt;
    }
