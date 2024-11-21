namespace Discordfs.Rest.Modules

open System.Net.Http

type BotClient () =
    inherit HttpClient ()

type OAuthClient () =
    inherit HttpClient ()

type DiscordClient =
    | Bot of BotClient
    | OAuth of OAuthClient
with
    member this.SendAsync req =
        match this with
        | Bot c -> c.SendAsync req
        | OAuth c -> c.SendAsync req

module HttpClient =
    let toBotClient (token: string) (client: HttpClient) =
        client.DefaultRequestHeaders.Add("Authorization", $"Bot {token}")
        client :?> BotClient

    let toOAuthClient (token: string) (client: HttpClient) =
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}")
        client :?> OAuthClient
