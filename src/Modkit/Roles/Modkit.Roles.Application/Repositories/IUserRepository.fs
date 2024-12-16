namespace Modkit.Roles.Application.Repositories

open System
open System.Net
open System.Threading.Tasks

open Modkit.Roles.Domain.Entities

type UserChange =
    | Token of accessToken: string * accessTokenExpiry: DateTime * refreshToken: string
    | Metadata of (string * int) seq

[<Interface>]
type IUserRepository =
    abstract member Put:
        user: User ->
        Task<Result<User, HttpStatusCode>>

    abstract member Get:
        userId: string ->
        applicationId: string ->
        Task<Result<User, HttpStatusCode>>

    abstract member Update:
        userId: string ->
        applicationId: string ->
        changes: UserChange list ->
        Task<Result<User, HttpStatusCode>>

    abstract member Delete:
        userId: string ->
        applicationId: string ->
        Task<Result<unit, HttpStatusCode>>
