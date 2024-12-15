namespace Discordfs.Rest.Modules

open System
open System.Net.Http
open System.Text

type BotClient () =
    inherit HttpClient ()

type OAuthClient () =
    inherit HttpClient ()

type BasicClient () =
    inherit HttpClient ()

type DiscordClient =
    | Bot of BotClient
    | OAuth of OAuthClient
    | Basic of BasicClient
with
    member this.SendAsync req =
        match this with
        | Bot c -> c.SendAsync req
        | OAuth c -> c.SendAsync req
        | Basic c -> c.SendAsync req

module HttpClient =
    let toBotClient (token: string) (client: HttpClient) =
        client.DefaultRequestHeaders.Add("Authorization", "Bot " + token)
        client :?> BotClient

    let toOAuthClient (token: string) (client: HttpClient) =
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token)
        client :?> OAuthClient

    let toBasicClient (clientId: string) (clientSecret: string) (client: HttpClient) =
        let token =
            $"{clientId}:{clientSecret}"
            |> Encoding.UTF8.GetBytes
            |> Convert.ToBase64String

        client.DefaultRequestHeaders.Add("Authorization", "Basic " + token)
        client :?> BasicClient
