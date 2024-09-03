namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Types
open System
open System.Net.WebSockets
open System.Threading
open System.Threading.Tasks

type IDiscordGatewayService =
    abstract member Connect:
        unit ->
        Task<unit>

type DiscordGatewayService (discordHttpService: IDiscordHttpService) =
    static member GatewayVersion: string = "10"

    static member GatewayEncoding: GatewayEncoding = GatewayEncoding.JSON

    static member GatewayCompression: GatewayCompression option = None

    member val private _cts: CancellationTokenSource =
        let cts = new CancellationTokenSource ()
        cts.CancelAfter(TimeSpan.FromSeconds 5)
        cts

    member val private _ws: ClientWebSocket option = None with get, set

    member _.GetGatewayUrl () = task {
        let! gateway = (discordHttpService.GetGateway
            DiscordGatewayService.GatewayVersion
            DiscordGatewayService.GatewayEncoding
            DiscordGatewayService.GatewayCompression)

        return gateway.Url
    }

    interface IDiscordGatewayService with 
        member this.Connect () = task {
            let! url = this.GetGatewayUrl()

            let ws = new ClientWebSocket()
            do! ws.ConnectAsync(Uri url, this._cts.Token)

            this._ws <- Some ws
        }

    interface IDisposable with
        member this.Dispose () =
            this._cts.Dispose()

            match this._ws with
            | None -> ()
            | Some ws -> ws.Dispose()

    // TODO: Handle sending/receiving events and the connection lifecycle: https://discord.com/developers/docs/topics/gateway#connection-lifecycle
    