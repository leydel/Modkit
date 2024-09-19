namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open Modkit.Discordfs.Utils
open System.Collections.Generic
open System.Net.Http
open System.Threading.Tasks



type IDiscordHttpApplicationCommandActions =
    abstract member CreateGlobalApplicationCommand:
        applicationId: string ->
        payload: CreateGlobalApplicationCommandPayload ->
        Task<ApplicationCommand>

    abstract member BulkOverwriteGlobalApplicationCommands:
        applicationId: string -> 
        payload: ApplicationCommand list ->
        Task<ApplicationCommand list>

    // TODO: Implement remaining endpoints

type DiscordHttpApplicationCommandActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpApplicationCommandActions with
        member _.CreateGlobalApplicationCommand
            applicationId payload =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/commands"
                |> Req.bot token
                |> Req.json (payload.ToString())
                |> Req.send httpClientFactory
                |> Res.json

        member _.BulkOverwriteGlobalApplicationCommands
            applicationId payload =
                Req.create
                    HttpMethod.Patch
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/commands"
                |> Req.bot token
                |> Req.json (FsJson.serialize payload)
                |> Req.send httpClientFactory
                |> Res.json
