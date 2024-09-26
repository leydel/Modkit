namespace Modkit.Api.Models

open System

type Note = {
    Id: string
    UserId: string
    Message: string
    CreatedAt: DateTime
}
