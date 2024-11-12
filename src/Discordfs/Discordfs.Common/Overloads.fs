[<AutoOpen>]
module Overloads

open System.Threading.Tasks

let inline (>>=) v f = Option.bind f v
let inline (>>.) v f = Option.map f v
let inline (>>?) o v = Option.defaultValue v o

let inline (||>) (a, b) f = f a b
let inline (!) f = f |> ignore

let inline (?>) v f = Task.map f v
let inline (<?) f v = v ?> f
let inline (?>>) v f = Task.mapT f v
let inline (<<?) f v = v ?>> f

let inline (?) f = f :> Task
