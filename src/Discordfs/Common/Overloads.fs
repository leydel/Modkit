[<AutoOpen>]
module Overloads

let (>>=) v f = Option.bind f v
let (>>.) v f = Option.map f v
let (>>?) o v = Option.defaultValue v o
