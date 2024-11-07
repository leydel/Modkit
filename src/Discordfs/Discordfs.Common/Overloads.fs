[<AutoOpen>]
module Overloads

let inline (>>=) v f = Option.bind f v
let inline (>>.) v f = Option.map f v
let inline (>>?) o v = Option.defaultValue v o

let inline (||>) (a, b) f = f a b
