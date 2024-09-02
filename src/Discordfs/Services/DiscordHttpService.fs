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

    abstract member CreateInteractionResponse:
        id: string ->
        token: string ->
        payload: InteractionCallback ->
        Task<unit>

type DiscordHttpService (httpClientFactory: IHttpClientFactory, token: string) =
    static member DISCORD_API_URL = "https://discord.com/api/"

    member _.request<'T> (method: HttpMethod) (endpoint: string) =
        let req = new HttpRequestMessage(method, DiscordHttpService.DISCORD_API_URL + endpoint)
        req.Headers.Clear()
        req.Headers.Add("Authorization", $"Bearer {token}")
        req

    member _.body (payload: 'a) (req: HttpRequestMessage) =
        req.Content <- new StringContent (Json.serializeU payload)
        req

    member _.result (req: HttpRequestMessage) = task {
        let! res = httpClientFactory.CreateClient().SendAsync req
        let! body = res.Content.ReadAsStringAsync()
        return Json.deserialize<'a> body
    }

    member _.unit (req: HttpRequestMessage) = task {
        let! _ = httpClientFactory.CreateClient().SendAsync req
        return ()
    }

    interface IDiscordHttpService with 
        member this.CreateGlobalApplicationCommand applicationId payload =
            this.request
                HttpMethod.Post
                $"applications/{applicationId}/commands"
            |> this.body payload
            |> this.result

        member this.BulkOverwriteGlobalApplicationCommands applicationId payload =
            this.request
                HttpMethod.Patch
                $"applications/{applicationId}/commands"
            |> this.body payload
            |> this.result

        member this.CreateChannelInvite channelId payload =
            this.request
                HttpMethod.Post
                $"channels/{channelId}/invites"
            |> this.body payload
            |> this.result

        member this.CreateInteractionResponse id token payload =
            this.request
                HttpMethod.Post
                $"interactions/{id}/{token}/callback"
            |> this.body payload
            |> this.unit
