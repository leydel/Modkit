module System.Nullable

let toOption (v: Nullable<'a>) =
    if v.HasValue then Some v.Value
    else None
