module System.Threading.Tasks.Task

let map f t = task {
    let! res = t
    return f res
}

let mapT f t = task {
    let! res = t
    return! f res
}

let wait t = task {
    do! t :> Task
}

let apply f =
    fun v -> f v |> Task.FromResult
