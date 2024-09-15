namespace Modkit.Discordfs.Common

open FSharp.Json
open System.Net.Http
open System.Text.Json
open System.Threading.Tasks

module Res =
    // TODO: Mark `body` obsolete and remove once all converted to STJ
    let body<'a> (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        let! body = res.Content.ReadAsStringAsync()
        return Json.deserialize<'a> body
    }

    let json (resTask: Task<HttpResponseMessage>) = task {
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
    