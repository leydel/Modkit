namespace Discordfs.Gateway.Payloads

open Discordfs.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<GatewayReceiveEventConverter>)>]
type GatewayReceiveEvent =
    | Hello of GatewayEventPayload<Hello>
    | Heartbeat of GatewayEventPayload<Empty>
    | HeartbeatAck of GatewayEventPayload<Empty>
    | Ready of GatewayEventPayload<Ready>
    | Resumed of GatewayEventPayload<Empty>
    | Reconnect of GatewayEventPayload<Empty>
    | InvalidSession of GatewayEventPayload<InvalidSession>
with
    static member getSequenceNumber (event: GatewayReceiveEvent) =
        let json = Json.serializeF event
        let document = JsonDocument.Parse json

        match document.RootElement.TryGetProperty "s" with
        | true, t -> Some (t.GetInt32())
        | _ -> None

        // TODO: Figure out way to calculate this without serializing and parsing (?)

and GatewayReceiveEventConverter () =
    inherit JsonConverter<GatewayReceiveEvent> ()

    override __.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let opcode =
            document.RootElement.GetProperty "op"
            |> _.GetInt32()
            |> enum<GatewayOpcode>

        let eventName =
            match document.RootElement.TryGetProperty "t" with
            | true, t -> Some (t.GetRawText())
            | _ -> None
            

        let json = document.RootElement.GetRawText()

        match opcode, eventName with
        | GatewayOpcode.HELLO, None -> Hello <| Json.deserializeF json
        | GatewayOpcode.HEARTBEAT, None -> Heartbeat <| Json.deserializeF json
        | GatewayOpcode.HEARTBEAT_ACK, None -> HeartbeatAck <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some "READY" -> Ready <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some "RESUMED" -> Resumed <| Json.deserializeF json
        | GatewayOpcode.RECONNECT, None -> Reconnect <| Json.deserializeF json
        | GatewayOpcode.INVALID_SESSION, None -> InvalidSession <| Json.deserializeF json
        | _ -> failwith "Unexpected GatewayOpcode and/or EventName provided"
                
    override __.Write (writer, value, options) =
        match value with
        | Hello h -> Json.serializeF h |> writer.WriteRawValue
        | Heartbeat h -> Json.serializeF h |> writer.WriteRawValue
        | HeartbeatAck h -> Json.serializeF h |> writer.WriteRawValue
        | Ready r -> Json.serializeF r |> writer.WriteRawValue
        | Resumed r -> Json.serializeF r |> writer.WriteRawValue
        | Reconnect r -> Json.serializeF r |> writer.WriteRawValue
        | InvalidSession i -> Json.serializeF i |> writer.WriteRawValue
