namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IDiscordHttpApplicationActions =
    // https://discord.com/developers/docs/resources/application#get-current-application
    abstract member GetCurrentApplication:
        unit ->
        Task<Application>

    // https://discord.com/developers/docs/resources/application#edit-current-application
    abstract member EditCurrentApplication:
        payload: EditCurrentApplication ->
        Task<Application>

    // https://discord.com/developers/docs/resources/application#get-application-activity-instance
    abstract member GetApplicationActivityInstance:
        applicationId: string ->
        instanceId: string ->
        Task<ActivityInstance>

type DiscordHttpApplicationActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpApplicationActions with
        member _.GetCurrentApplication () =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"applications/@me"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.EditCurrentApplication payload =
            Req.create
                HttpMethod.Patch
                Constants.DISCORD_API_URL
                $"applications/@me"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.GetApplicationActivityInstance applicationId instanceId =
            Req.create
                HttpMethod.Patch
                Constants.DISCORD_API_URL
                $"applications/{applicationId}/activity-instances/{instanceId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body
            