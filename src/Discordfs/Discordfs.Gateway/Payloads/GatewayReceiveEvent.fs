﻿namespace Discordfs.Gateway.Payloads

open Discordfs.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<GatewayReceiveEventConverter>)>]
type GatewayReceiveEvent =
    | HEARTBEAT                              of GatewayEventPayload<Empty>
    | HEARTBEAT_ACK                          of GatewayEventPayload<Empty>
    | HELLO                                  of GatewayEventPayload<Hello>
    | READY                                  of GatewayEventPayload<Ready>
    | RESUMED                                of GatewayEventPayload<Empty>
    | RECONNECT                              of GatewayEventPayload<Empty>
    | INVALID_SESSION                        of GatewayEventPayload<InvalidSession>
    | APPLICATION_COMMAND_PERMISSIONS_UPDATE of GatewayEventPayload<ApplicationCommandPermissionsUpdate>
    | AUTO_MODERATION_RULE_CREATE            of GatewayEventPayload<AutoModerationRuleCreate>
    | AUTO_MODERATION_RULE_UPDATE            of GatewayEventPayload<AutoModerationRuleUpdate>
    | AUTO_MODERATION_RULE_DELETE            of GatewayEventPayload<AutoModerationRuleDelete>
    | AUTO_MODERATION_ACTION_EXECUTION       of GatewayEventPayload<AutoModerationActionExecution>
    | CHANNEL_CREATE                         of GatewayEventPayload<ChannelCreate>
    | CHANNEL_UPDATE                         of GatewayEventPayload<ChannelUpdate>
    | CHANNEL_DELETE                         of GatewayEventPayload<ChannelDelete>
    | THREAD_CREATE                          of GatewayEventPayload<ChannelCreate> 
    | THREAD_UPDATE                          of GatewayEventPayload<ChannelUpdate>
    | THREAD_DELETE                          of GatewayEventPayload<ThreadDelete>
    | THREAD_LIST_SYNC                       of GatewayEventPayload<ThreadListSync>
    | ENTITLEMENT_CREATE                     of GatewayEventPayload<EntitlementCreate>
    | ENTITLEMENT_UPDATE                     of GatewayEventPayload<EntitlementUpdate>
    | ENTITLEMENT_DELETE                     of GatewayEventPayload<EntitlementDelete>
    | GUILD_CREATE                           of GatewayEventPayload<GuildCreate>
    | GUILD_UPDATE                           of GatewayEventPayload<GuildUpdate>
    | GUILD_DELETE                           of GatewayEventPayload<GuildDelete>
    | GUILD_BAN_ADD                          of GatewayEventPayload<GuildBanAdd>
    | GUILD_BAN_REMOVE                       of GatewayEventPayload<GuildBanRemove>
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
        | GatewayOpcode.HEARTBEAT, None -> HEARTBEAT <| Json.deserializeF json
        | GatewayOpcode.HEARTBEAT_ACK, None -> HEARTBEAT_ACK <| Json.deserializeF json
        | GatewayOpcode.HELLO, None -> HELLO <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof READY) -> READY <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof RESUMED) -> RESUMED <| Json.deserializeF json
        | GatewayOpcode.RECONNECT, None -> RECONNECT <| Json.deserializeF json
        | GatewayOpcode.INVALID_SESSION, None -> INVALID_SESSION <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof APPLICATION_COMMAND_PERMISSIONS_UPDATE) -> APPLICATION_COMMAND_PERMISSIONS_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof AUTO_MODERATION_RULE_CREATE) -> AUTO_MODERATION_RULE_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof AUTO_MODERATION_RULE_UPDATE) -> AUTO_MODERATION_RULE_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof AUTO_MODERATION_RULE_DELETE) -> AUTO_MODERATION_RULE_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof AUTO_MODERATION_ACTION_EXECUTION) -> AUTO_MODERATION_ACTION_EXECUTION <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof CHANNEL_CREATE) -> CHANNEL_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof CHANNEL_UPDATE) -> CHANNEL_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof CHANNEL_DELETE) -> CHANNEL_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof THREAD_CREATE) -> THREAD_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof THREAD_UPDATE) -> THREAD_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof THREAD_DELETE) -> THREAD_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof THREAD_LIST_SYNC) -> THREAD_LIST_SYNC <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof ENTITLEMENT_CREATE) -> ENTITLEMENT_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof ENTITLEMENT_UPDATE) -> ENTITLEMENT_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof ENTITLEMENT_DELETE) -> ENTITLEMENT_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_CREATE) -> GUILD_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_UPDATE) -> GUILD_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_DELETE) -> GUILD_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_BAN_ADD) -> GUILD_BAN_ADD <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_BAN_REMOVE) -> GUILD_BAN_REMOVE <| Json.deserializeF json
        | _ -> failwith "Unexpected GatewayOpcode and/or EventName provided" // TODO: Handle gracefully so bot doesnt crash on unfamiliar events
                
    override __.Write (writer, value, options) =
        match value with
        | HEARTBEAT h -> Json.serializeF h |> writer.WriteRawValue
        | HEARTBEAT_ACK h -> Json.serializeF h |> writer.WriteRawValue
        | HELLO h -> Json.serializeF h |> writer.WriteRawValue
        | READY r -> Json.serializeF r |> writer.WriteRawValue
        | RESUMED r -> Json.serializeF r |> writer.WriteRawValue
        | RECONNECT r -> Json.serializeF r |> writer.WriteRawValue
        | INVALID_SESSION i -> Json.serializeF i |> writer.WriteRawValue
        | APPLICATION_COMMAND_PERMISSIONS_UPDATE a -> Json.serializeF a |> writer.WriteRawValue
        | AUTO_MODERATION_RULE_CREATE a -> Json.serializeF a |> writer.WriteRawValue
        | AUTO_MODERATION_RULE_UPDATE a -> Json.serializeF a |> writer.WriteRawValue
        | AUTO_MODERATION_RULE_DELETE a -> Json.serializeF a |> writer.WriteRawValue
        | AUTO_MODERATION_ACTION_EXECUTION a -> Json.serializeF a |> writer.WriteRawValue
        | CHANNEL_CREATE c -> Json.serializeF c |> writer.WriteRawValue
        | CHANNEL_UPDATE c -> Json.serializeF c |> writer.WriteRawValue
        | CHANNEL_DELETE c -> Json.serializeF c |> writer.WriteRawValue
        | THREAD_CREATE t -> Json.serializeF t |> writer.WriteRawValue
        | THREAD_UPDATE t -> Json.serializeF t |> writer.WriteRawValue
        | THREAD_DELETE t -> Json.serializeF t |> writer.WriteRawValue
        | THREAD_LIST_SYNC t -> Json.serializeF t |> writer.WriteRawValue
        | ENTITLEMENT_CREATE e -> Json.serializeF e |> writer.WriteRawValue
        | ENTITLEMENT_UPDATE e -> Json.serializeF e |> writer.WriteRawValue
        | ENTITLEMENT_DELETE e -> Json.serializeF e |> writer.WriteRawValue
        | GUILD_CREATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_UPDATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_DELETE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_BAN_ADD g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_BAN_REMOVE g -> Json.serializeF g |> writer.WriteRawValue
