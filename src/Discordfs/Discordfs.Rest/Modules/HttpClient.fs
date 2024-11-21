namespace Discordfs.Rest.Modules

open System.Net.Http

type BotClient () = inherit HttpClient ()
type OAuthClient () = inherit HttpClient ()

module HttpClient =
    let toBotClient (token: string) (client: HttpClient) =
        client.DefaultRequestHeaders.Add("Authorization", $"Bot {token}")
        client :?> BotClient

    let toOAuthClient (token: string) (client: HttpClient) =
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}")
        client :?> OAuthClient
