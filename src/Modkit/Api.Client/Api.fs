module Modkit.Api.Client.Api

open Modkit.Api.Common
open Modkit.Api.Functions.Http
open Modkit.Api.Modules
open System.Net.Http

let listMemberNotes
    (guildId: string)
    (memberId: string)
    (httpClient: HttpClient) =
        req {
            host (httpClient.BaseAddress.ToString())
            get $"guilds/{guildId}/members/{memberId}/notes"
        }
        |> httpClient.SendAsync
        ?>> ApiResponse.asJson<NoteDto list>

let getMemberNote
    (guildId: string)
    (memberId: string)
    (noteId: string)
    (httpClient: HttpClient) =
        req {
            host (httpClient.BaseAddress.ToString())
            get $"guilds/{guildId}/members/{memberId}/notes/{noteId}"            
        }
        |> httpClient.SendAsync
        ?>> ApiResponse.asJson<NoteDto>

let addMemberNote
    (guildId: string)
    (memberId: string)
    (content: AddMemberNotePayload)
    (httpClient: HttpClient) =
        req {
            host (httpClient.BaseAddress.ToString())
            post $"guilds/{guildId}/members/{memberId}/notes"
            payload (Payload.fromObj content)
        }
        |> httpClient.SendAsync
        ?>> ApiResponse.asJson<NoteDto>

let removeMemberNote
    (guildId: string)
    (memberId: string)
    (noteId: string)
    (httpClient: HttpClient) =
        req {
            host (httpClient.BaseAddress.ToString())
            delete $"guilds/{guildId}/members/{memberId}/notes/{noteId}"            
        }
        |> httpClient.SendAsync
        ?>> ApiResponse.asEmpty

let getDiacordMapping
    (guildId: string)
    (httpClient: HttpClient) =
        req {
            host (httpClient.BaseAddress.ToString())
            get $"guilds/{guildId}/mapping"
        }
        |> httpClient.SendAsync
        ?>> ApiResponse.asJson<DiacordMappingDto>

let putDiacordMapping
    (guildId: string)
    (content: PutDiacordMappingPayload)
    (httpClient: HttpClient) =
        req {
            host (httpClient.BaseAddress.ToString())
            put $"guilds/{guildId}/mapping"
            payload (Payload.fromObj content)
        }
        |> httpClient.SendAsync
        ?>> ApiResponse.asJson<DiacordMappingDto>
        
let removeDiacordMapping
    (guildId: string)
    (httpClient: HttpClient) =
        req {
            host (httpClient.BaseAddress.ToString())
            delete $"guilds/{guildId}/mapping"
        }
        |> httpClient.SendAsync
        ?>> ApiResponse.asEmpty
