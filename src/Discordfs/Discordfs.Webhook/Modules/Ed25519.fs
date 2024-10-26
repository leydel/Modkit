module Discordfs.Webhook.Modules.Ed25519

open System
open System.Text
open TweetNaclSharp

let verify (timestamp: string) (body: string) (signature: string) (publicKey: string) =
    let msg = timestamp + body |> Encoding.UTF8.GetBytes
    let ``sig`` = signature |> Convert.FromHexString
    let key = publicKey |> Convert.FromHexString
    Nacl.SignDetachedVerify(msg, ``sig``, key)
