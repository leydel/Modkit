namespace Modkit.Discordfs.Services

open FSharp.Json
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpService =
    abstract member CreateGlobalApplicationCommand:
        applicationId: string ->
        payload: CreateGlobalApplicationCommand ->
        Task<ApplicationCommand>

    abstract member BulkOverwriteGlobalApplicationCommands:
        applicationId: string -> 
        payload: CreateGlobalApplicationCommand list ->
        Task<ApplicationCommand list>

    abstract member CreateChannelInvite:
        channelId: string ->
        payload: CreateChannelInvite ->
        Task<Invite>

type DiscordHttpService (httpClientFactory: IHttpClientFactory, token: string) =
    member _.send<'T> (method: HttpMethod) (endpoint: string) (body: HttpContent option) = task {
        let client = httpClientFactory.CreateClient()
        let req = new HttpRequestMessage(method, "https://discord.com/api/" + endpoint)
        
        if body.IsSome then
            req.Content <- body.Value

        req.Headers.Clear()
        req.Headers.Add("Authorization", $"Bearer {token}")

        let! res = client.SendAsync req
        let! body = res.Content.ReadAsStringAsync()
        return Json.deserialize<'T> body
    }

    member _.content<'T> (payload: 'T) =
        Some (new StringContent (Json.serialize payload) :> HttpContent)

    interface IDiscordHttpService with 
        member this.CreateGlobalApplicationCommand applicationId payload =
            this.send
                HttpMethod.Post
                $"applications/{applicationId}/commands"
                (this.content payload)

        member this.BulkOverwriteGlobalApplicationCommands applicationId payload =
            this.send
                HttpMethod.Patch
                $"applications/{applicationId}/commands"
                (this.content payload)

        member this.CreateChannelInvite channelId payload =
            this.send
                HttpMethod.Post
                $"channels/{channelId}/invites"
                (this.content payload)

// TODO: Convert this into a factory (?)
