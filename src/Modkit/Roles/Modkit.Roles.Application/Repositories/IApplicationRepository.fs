namespace Modkit.Roles.Application.Repositories

open System.Net
open System.Threading.Tasks

open Modkit.Roles.Domain.Types
open Modkit.Roles.Domain.Entities

type ApplicationChange =
    | Token of string
    | ClientSecret of string
    | Metadata of (string * MetadataType) seq

[<Interface>]
type IApplicationRepository =
    abstract member Put:
        application: Application ->
        Task<Result<Application, HttpStatusCode>>

    abstract member Get:
        applicationId: string ->
        Task<Result<Application, HttpStatusCode>>

    abstract member Update:
        applicationId: string ->
        changes: ApplicationChange list ->
        Task<Result<Application, HttpStatusCode>>

    abstract member Delete:
        applicationId: string ->
        Task<Result<unit, HttpStatusCode>>
