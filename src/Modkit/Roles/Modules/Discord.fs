module Modkit.Roles.Modules.Discord

open System
open System.Collections.Specialized

let buildOAuthUrl (applicationId: string) (state: string) =
    let builder = new UriBuilder("https://discord.com/api/oauth2/authorize")

    let query = NameValueCollection()
    query.Add("client_id", applicationId)
    query.Add("redirect_uri", "https://modkit.io/oauth-callback")
    query.Add("response_type", "code")
    query.Add("state", state)
    query.Add("scope", "identify role_connections.write")
    query.Add("prompt", "consent")

    builder.Query <- query.ToString()
    builder.Uri.ToString()

    // TODO: This belongs in Discordfs along with other oauth2 implementations
