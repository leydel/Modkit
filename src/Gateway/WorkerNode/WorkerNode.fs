namespace Modkit.Gateway.WorkerNode

open Modkit.Gateway.WorkerNode.Factories
open Modkit.Gateway.WorkerNode.Managers

type WorkerNode (
    serviceBusClientFactory: IServiceBusClientFactory,
    shardFactory: IShardFactory
) =
    let rec loop (shards: Shard list) = task {
        // TODO: Connect to orchestrator service bus to await shards to bid on and instantiate

        // TODO: Start shard and store in list

        // TODO: Figure out appropriate way to handle notifying orchestrator when shards released

        return! loop shards
    }

    member _.StartAsync () = task {
        do! loop []
    }
