namespace Modkit.Roles.Application.Common

open System.Threading.Tasks

type RailwayResult<'a, 'b, 'c> = Result<('a * 'b), 'c>

module RailwayResult =
    let fromResult r =
        r |> Result.map (fun v -> ((), v))

module Railway =
    type RailwayBuilder () =
        member inline _.Yield (_) = Ok ()
        member inline _.Yield (v: Result<'a, 'b>) = RailwayResult.fromResult v
        member inline _.Yield (v: RailwayResult<'a, 'b, 'c>) = v

        [<CustomOperation>]
        member inline _.next (acc: Result<'a, 'b>, f: 'a -> Result<'c, 'b>) =
            Result.bind (fun v -> f v) acc

        [<CustomOperation>]
        member inline _.next (acc: Result<'a, 'b>, f: 'a -> Task<Result<'c, 'b>>) = task {
            match acc with
            | Error e -> return Error e
            | Ok v -> return! f v
        }

        [<CustomOperation>]
        member inline _.next (acc: Task<Result<'a, 'b>>, f: 'a -> Result<'c, 'b>) = task {
            let! res = acc
            return Result.bind f res
        }

        [<CustomOperation>]
        member inline _.next (acc: Task<Result<'a, 'b>>, f: 'a -> Task<Result<'c, 'b>>) = task {
            match! acc with
            | Error e -> return Error e
            | Ok v -> return! f v
        }

        // TODO: Change above to ahndle RailwayResults to store state (?)

    let railway = RailwayBuilder()

    let test () =
        let first = fun _ -> Ok ""
        let second = fun _ -> Task.FromResult (Error "")
        let third = fun _ -> Ok ()

        railway {
            next first
            next second
            next third
        }
