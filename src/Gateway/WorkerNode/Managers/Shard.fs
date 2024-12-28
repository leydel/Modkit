namespace Modkit.Gateway.WorkerNode.Managers

open Discordfs.Gateway.Clients
open Discordfs.Gateway.Types
open System.Threading
open System.Threading.Tasks

type IShard =
    abstract member StartAsync: unit -> Task<unit>

    abstract member RequestStop: unit -> Task<unit>

type Shard (
    gatewayUrl: string,
    identify: IdentifySendEvent,
    handler: (string -> Task<unit>)
) =
    let cts = new CancellationTokenSource()

    member val Client: IGatewayClient = new GatewayClient()

    interface IShard with
        member this.StartAsync () = task {
            do! this.Client.Connect gatewayUrl identify handler cts.Token
        }

        member _.RequestStop () = task {
            do! cts.CancelAsync()
        }
