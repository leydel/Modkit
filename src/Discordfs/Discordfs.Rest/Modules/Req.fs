[<AutoOpen>]
module Discordfs.Rest.Modules.Req

open Discordfs.Rest.Common
open System.Net.Http

let req = RequestBuilder(Constants.DISCORD_API_URL)
