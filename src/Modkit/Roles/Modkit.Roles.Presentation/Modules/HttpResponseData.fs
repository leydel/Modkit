[<AutoOpen>]
module Modkit.Roles.Presentation.Modules.HttpResponseData

open Microsoft.Azure.Functions.Worker.Http

let withHeader key (value: string) (res: HttpResponseData) =
    res.Headers.Add(key, value)
    res    

let withJson<'a> (data: 'a) (res: HttpResponseData) = task {
    do! res.WriteAsJsonAsync data
    return res
}

let withText text (res: HttpResponseData) = task {
    do! res.WriteStringAsync text
    return res
}
