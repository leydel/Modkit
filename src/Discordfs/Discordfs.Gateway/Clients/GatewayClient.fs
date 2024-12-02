namespace Discordfs.Gateway.Clients

open Discordfs.Gateway.Modules
open Discordfs.Gateway.Types
open System
open System.Net.WebSockets
open System.Threading
open System.Threading.Tasks

type IGatewayClient =
    abstract member Connect:
        gatewayUrl: string ->
        identify: IdentifySendEvent ->
        handler: (GatewayReceiveEvent -> Task<unit>) ->
        ct: CancellationToken ->
        Task<unit>

    abstract member RequestGuildMembers:
        RequestGuildMembersSendEvent ->
        Task<Result<unit, unit>>

    abstract member RequestSoundboardSounds:
        RequestSoundboardSoundsSendEvent ->
        Task<Result<unit, unit>>

    abstract member UpdateVoiceState:
        UpdateVoiceStateSendEvent ->
        Task<Result<unit, unit>>

    abstract member UpdatePresence:
        UpdatePresenceSendEvent ->
        Task<Result<unit, unit>>

type GatewayClient () =
    let _ws: ClientWebSocket option ref = ref None

    interface IGatewayClient with
        member _.Connect gatewayUrl identify handler ct =
            // TODO: Implement cancellation token
            Gateway.reconnect gatewayUrl None identify handler _ws

        member _.RequestGuildMembers payload = task {
            match _ws.Value with
            | Some ws ->
                do! Gateway.requestGuildMembers payload ws
                return Ok ()
            | None ->
                return Error ()
        }

        member _.RequestSoundboardSounds payload = task {
            match _ws.Value with
            | Some ws ->
                do! Gateway.requestSoundboardSounds payload ws
                return Ok ()
            | None ->
                return Error ()
        }

        member _.UpdateVoiceState payload = task {
            match _ws.Value with
            | Some ws ->
                do! Gateway.updateVoiceState payload ws
                return Ok ()
            | None ->
                return Error ()
        }

        member _.UpdatePresence payload = task {
            match _ws.Value with
            | Some ws ->
                do! Gateway.updatePresence payload ws
                return Ok ()
            | None ->
                return Error ()
        }

    interface IDisposable with
        member _.Dispose () =
            match _ws.Value with
            | Some ws -> ws.Dispose()
            | None -> ()
