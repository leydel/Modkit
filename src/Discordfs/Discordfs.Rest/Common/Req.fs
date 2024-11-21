[<AutoOpen>]
module Discordfs.Rest.Common.Req

open System.Net.Http

let req = RequestBuilder(Constants.DISCORD_API_URL)
