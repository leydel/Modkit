module Modkit.Website.App

open Browser.Dom
open Feliz

[<ReactComponent>]
let App () = React.fragment [
    Html.p "Hello world"
]

ReactDOM
    .createRoot(document.getElementById "root")
    .render(App())
