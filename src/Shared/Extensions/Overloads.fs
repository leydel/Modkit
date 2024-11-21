[<AutoOpen>]
module Overloads

open System
open System.Threading.Tasks

// General purpose overloads
[<Obsolete>]
let inline (||>) (a, b) f = f a b
let inline (!) f = f |> ignore

// Option overloads
let inline (>>=) v f = Option.bind f v
let inline (>>.) v f = Option.map f v
let inline (>>?) o v = Option.defaultValue v o

// Task overloads
let inline (?>) v f = Task.map f v
let inline (<?) f v = v ?> f
let inline (?>>) v f = Task.mapT f v
let inline (<<?) f v = v ?>> f
let inline (?) f = f :> Task
