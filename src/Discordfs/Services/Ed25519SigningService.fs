namespace Modkit.Discordfs.Services

open System
open System.Text
open TweetNaclSharp

type ISigningService =
    abstract member Verify:
        timestamp: string ->
        body: string ->
        signature: string ->
        publicKey: string ->
        bool

type Ed25519SigningService () =
    interface ISigningService with
        member _.Verify timestamp body signature publicKey =
            let msg = timestamp + body |> Encoding.UTF8.GetBytes
            let ``sig`` = signature |> Convert.FromHexString
            let key = publicKey |> Convert.FromHexString
            Nacl.SignDetachedVerify(msg, ``sig``, key)
