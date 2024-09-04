namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpApplicationCommandActions =
    abstract member CreateGlobalApplicationCommand:
        applicationId: string ->
        payload: CreateGlobalApplicationCommand ->
        Task<ApplicationCommand>

    abstract member BulkOverwriteGlobalApplicationCommands:
        applicationId: string -> 
        payload: CreateGlobalApplicationCommand list ->
        Task<ApplicationCommand list>

type DiscordHttpApplicationCommandActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpApplicationCommandActions with
        member _.CreateGlobalApplicationCommand applicationId payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"applications/{applicationId}/commands"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.BulkOverwriteGlobalApplicationCommands applicationId payload =
            Req.create
                HttpMethod.Patch
                Constants.DISCORD_API_URL
                $"applications/{applicationId}/commands"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body
