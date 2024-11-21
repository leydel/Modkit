[<AutoOpen>]
module IHttpClientFactory

open Discordfs.Rest.Modules
open System.Net.Http

type System.Net.Http.IHttpClientFactory with
    member this.CreateBotClient (name, token) =
        let httpClient = this.CreateClient name

        HttpClient.toBotClient token httpClient

    member this.CreateBotClient token =
        let httpClient = this.CreateClient() 
        HttpClient.toBotClient token httpClient

    member this.CreateOAuthClient (name, token) =
        let httpClient = this.CreateClient name 
        HttpClient.toOAuthClient token httpClient
            
    member this.CreateOAuthClient token =
        let httpClient = this.CreateClient() 
        HttpClient.toOAuthClient token httpClient
