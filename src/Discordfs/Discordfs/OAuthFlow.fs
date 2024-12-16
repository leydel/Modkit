namespace Discordfs

open Discordfs.Rest
open Discordfs.Types

type RevokeTokenType =
    | Any of string
    | AccessToken of string
    | RefreshToken of string

module OAuthFlow =
    let authorizationCodeGrant code redirectUri client = task {
        let payload = AuthorizationCodeGrantPayload(code, redirectUri)

        let! res = Rest.authorizationCodeGrant payload client
        return DiscordResponse.toOption res
    }

    let refreshTokenGrant refreshToken client = task {
        let payload = RefreshTokenGrantPayload(refreshToken)
        
        let! res = Rest.refreshTokenGrant payload client
        return DiscordResponse.toOption res
    }

    let revokeToken (token: RevokeTokenType) client = task {
        let payload =
            match token with
            | RevokeTokenType.Any v -> RevokeTokenPayload(v)
            | RevokeTokenType.AccessToken v -> RevokeTokenPayload(v, TokenTypeHint.ACCESS_TOKEN)
            | RevokeTokenType.RefreshToken v -> RevokeTokenPayload(v, TokenTypeHint.REFRESH_TOKEN)
        
        let! res = Rest.revokeToken payload client
        return DiscordResponse.toOption res
    }

    let clientCredentialsGrant scopes client = task {
        let payload = ClientCredentialsGrantPayload(scopes)
        
        let! res = Rest.clientCredentialsGrant payload client
        return DiscordResponse.toOption res
    }
