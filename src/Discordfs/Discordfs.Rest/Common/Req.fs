[<AutoOpen>]
module Discordfs.Rest.Common.Req

open System.Net.Http

let req = Http.RequestBuilder(Constants.DISCORD_API_URL)
