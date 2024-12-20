﻿namespace Modkit.Roles.Application.Common

open System.Threading.Tasks

type Railway<'a, 'b> =
    | Sync of Result<'a, 'b>
    | Task of Task<Result<'a, 'b>>
    | Async of Async<Result<'a, 'b>>

[<AutoOpen>]
module Railway =
    let toTask (v: Railway<'a, 'b>) =
        match v with
        | Railway.Sync v -> task { return v }
        | Railway.Task v -> v
        | Railway.Async v -> v |> Async.StartAsTask

    let toAsync (v: Railway<'a, 'b>) =
        match v with
        | Railway.Sync v -> async { return v }
        | Railway.Task v -> v |> Async.AwaitTask
        | Railway.Async v -> v

    let bind (f: 'a -> Railway<'c, 'b>) (v: Railway<'a, 'b>) =
        match v with
        | Railway.Sync v ->
            match v with
            | Error e -> Error e |> Railway.Sync
            | Ok v -> f v

        | Railway.Task t ->
            task {
                match! t with
                | Error e -> return Error e
                | Ok v -> return! f v |> toTask
            }
            |> Railway.Task

        | Railway.Async a ->
            async {
                match! a with
                | Error e -> return Error e
                | Ok v -> return! f v |> toAsync
            }
            |> Railway.Async

    type RailwayBuilder<'b> () =
        member _.Zero () = Ok ()
        member _.Bind (v: Railway<'a, 'b>, f: 'a -> Railway<'c, 'b>) = bind f v
        member _.Return (v: 'a) = Railway.Sync (Ok v)

    let railway<'b> = RailwayBuilder<'b>()
