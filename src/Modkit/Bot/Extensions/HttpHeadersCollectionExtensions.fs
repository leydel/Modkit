[<AutoOpen>]
module HttpHeadersCollection

type Microsoft.Azure.Functions.Worker.Http.HttpHeadersCollection with
    member this.GetValueOption name =
        match this.Contains name with
        | true -> this.GetValues name |> Seq.tryHead
        | false -> None
