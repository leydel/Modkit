namespace Modkit.Discordfs.Resources

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
open System.Threading.Tasks

type IOauth2Resource =
    // https://discord.com/developers/docs/topics/oauth2#get-current-bot-application-information
    abstract member GetCurrentBotApplicationInformation:
        oauth2AccessToken: string option -> // TODO: Test if this supports oauth2 access token and/or bot token
        Task<Application>

    // https://discord.com/developers/docs/topics/oauth2#get-current-authorization-information
    abstract member GetCurrentAuthorizationInformation:
        oauth2AccessToken: string ->
        Task<GetCurrentAuthorizationInformationResponse>

type OAuth2Resource (httpClientFactory, token) =
    interface IOauth2Resource with
        member _.GetCurrentBotApplicationInformation oauth2AccessToken =
            match oauth2AccessToken with
            | Some oauth2AccessToken ->
                req {
                    get "oauth2/applications/@me"
                    oauth oauth2AccessToken
                }
            | None ->
                req {
                    get "oauth2/applications/@me"
                    bot token
                }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetCurrentAuthorizationInformation oauth2AccessToken =
            req {
                get "oauth2/@me"
                oauth oauth2AccessToken
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
