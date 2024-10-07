namespace Discordfs.Rest.Common

open Discordfs.Types.Utils
open System.Net.Http
open System.Threading.Tasks

[<System.Obsolete>]
module Res =
    let json<'a> (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        let! body = res.Content.ReadAsStringAsync()
        return FsJson.deserialize<'a> body
    }

    let raw (resTask: Task<HttpResponseMessage>) = task {
        let! res = resTask
        return! res.Content.ReadAsStringAsync()
    }

    let ignore (resTask: Task<HttpResponseMessage>) = task {
        do! resTask :> Task
    }
    