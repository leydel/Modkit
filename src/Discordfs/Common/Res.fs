namespace Modkit.Discordfs.Common

open FSharp.Json
open System.Net.Http
open System.Threading.Tasks

module Res =
    let body (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        let! body = res.Content.ReadAsStringAsync()
        return Json.deserialize<'a> body
    }

    let ignore (resTask: Task<HttpResponseMessage>) = task {
        do! resTask :> Task
    }
    