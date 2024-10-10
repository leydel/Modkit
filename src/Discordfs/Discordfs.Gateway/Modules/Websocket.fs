namespace Discordfs.Gateway.Modules

open System
open System.IO
open System.Net.WebSockets
open System.Text
open System.Threading

type WebsocketReadResponse =
    | Message of string
    | Close of int option

module Websocket =
    let readNext ws = task {
        let rec loop (ms: MemoryStream) (ws: ClientWebSocket) = task {
            let buffer = Array.zeroCreate<byte> 4096 |> ArraySegment
            let! res = ws.ReceiveAsync(buffer, CancellationToken.None)
            ms.Write(buffer.Array, buffer.Offset, res.Count)

            match res.EndOfMessage with
            | false ->
                return! loop ms ws
            | true ->
                use sr = new StreamReader(ms)
                let! message = sr.ReadToEndAsync()

                match res.MessageType with
                | WebSocketMessageType.Close ->
                    let status =
                        Nullable.toOption res.CloseStatus
                        |> Option.map int

                    return WebsocketReadResponse.Close status
                | _ ->
                    return WebsocketReadResponse.Message message
        }

        use ms = new MemoryStream()
        return! loop ms ws
    }

    let write (message: string) (ws: ClientWebSocket) = task {
        let rec loop (bytes: byte array) (size: int) (offset: int) (ws: ClientWebSocket) = task {

            let isEndOfMessage = offset + size >= bytes.Length
            let count = Math.Min(size, bytes.Length - offset)
            let buffer = ArraySegment(bytes, offset, count)

            do! ws.SendAsync(buffer, WebSocketMessageType.Text, isEndOfMessage, CancellationToken.None)

            match isEndOfMessage with
            | false -> return! loop bytes size (offset + size) ws
            | true -> return ()
        }

        do! loop (Encoding.UTF8.GetBytes message) 4096 0 ws
    }
