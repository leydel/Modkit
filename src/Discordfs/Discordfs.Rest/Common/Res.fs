namespace Discordfs.Rest.Common

open System.Net.Http
open System.Text.Json
open System.Threading.Tasks

[<System.Obsolete>]
module Res =
    let json<'a> (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        let! body = res.Content.ReadAsStringAsync()
        return JsonSerializer.Deserialize<'a> body
    }

    let raw (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        return! res.Content.ReadAsStringAsync()
    }

    let ignore (resTask: Task<HttpResponseMessage>) = task {
        do! resTask :> Task
    }
    