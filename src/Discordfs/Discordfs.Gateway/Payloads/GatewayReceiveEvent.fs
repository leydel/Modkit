namespace Discordfs.Gateway.Payloads

open Discordfs.Types
open System.Text.Json
open System.Text.Json.Serialization

[<JsonConverter(typeof<GatewayReceiveEventConverter>)>]
type GatewayReceiveEvent =
    | HEARTBEAT                              of GatewayEventPayload<HeartbeatReceiveEvent>
    | HEARTBEAT_ACK                          of GatewayEventPayload<HeartbeatAckReceiveEvent>
    | HELLO                                  of GatewayEventPayload<HelloReceiveEvent>
    | READY                                  of GatewayEventPayload<ReadyReceiveEvent>
    | RESUMED                                of GatewayEventPayload<ReadyReceiveEvent>
    | RECONNECT                              of GatewayEventPayload<ReconnectReceiveEvent>
    | INVALID_SESSION                        of GatewayEventPayload<InvalidSessionReceiveEvent>
    | APPLICATION_COMMAND_PERMISSIONS_UPDATE of GatewayEventPayload<ApplicationCommandPermissionsUpdateReceiveEvent>
    | AUTO_MODERATION_RULE_CREATE            of GatewayEventPayload<AutoModerationRuleCreateReceiveEvent>
    | AUTO_MODERATION_RULE_UPDATE            of GatewayEventPayload<AutoModerationRuleUpdateReceiveEvent>
    | AUTO_MODERATION_RULE_DELETE            of GatewayEventPayload<AutoModerationRuleDeleteReceiveEvent>
    | AUTO_MODERATION_ACTION_EXECUTION       of GatewayEventPayload<AutoModerationActionExecutionReceiveEvent>
    | CHANNEL_CREATE                         of GatewayEventPayload<ChannelCreateReceiveEvent>
    | CHANNEL_UPDATE                         of GatewayEventPayload<ChannelUpdateReceiveEvent>
    | CHANNEL_DELETE                         of GatewayEventPayload<ChannelDeleteReceiveEvent>
    | THREAD_CREATE                          of GatewayEventPayload<ChannelCreateReceiveEvent>
    | THREAD_UPDATE                          of GatewayEventPayload<ChannelUpdateReceiveEvent>
    | THREAD_DELETE                          of GatewayEventPayload<ThreadDeleteReceiveEvent>
    | THREAD_LIST_SYNC                       of GatewayEventPayload<ThreadListSyncReceiveEvent>
    | ENTITLEMENT_CREATE                     of GatewayEventPayload<EntitlementCreateReceiveEvent>
    | ENTITLEMENT_UPDATE                     of GatewayEventPayload<EntitlementUpdateReceiveEvent>
    | ENTITLEMENT_DELETE                     of GatewayEventPayload<EntitlementDeleteReceiveEvent>
    | GUILD_CREATE                           of GatewayEventPayload<GuildCreateReceiveEvent>
    | GUILD_UPDATE                           of GatewayEventPayload<GuildUpdateReceiveEvent>
    | GUILD_DELETE                           of GatewayEventPayload<GuildDeleteReceiveEvent>
    | GUILD_BAN_ADD                          of GatewayEventPayload<GuildBanAddReceiveEvent>
    | GUILD_BAN_REMOVE                       of GatewayEventPayload<GuildBanRemoveReceiveEvent>
    | GUILD_EMOJIS_UPDATE                    of GatewayEventPayload<GuildEmojisUpdateReceiveEvent>
    | GUILD_STICKERS_UPDATE                  of GatewayEventPayload<GuildStickersUpdateReceiveEvent>
    | GUILD_INTEGRATIONS_UPDATE              of GatewayEventPayload<GuildIntegrationsUpdateReceiveEvent>
    | GUILD_MEMBER_ADD                       of GatewayEventPayload<GuildMemberAddReceiveEvent>
    | GUILD_MEMBER_REMOVE                    of GatewayEventPayload<GuildMemberRemoveReceiveEvent>
    | GUILD_MEMBER_UPDATE                    of GatewayEventPayload<GuildMemberUpdateReceiveEvent>
    | GUILD_MEMBERS_CHUNK                    of GatewayEventPayload<GuildMembersChunkReceiveEvent>
    | GUILD_ROLE_CREATE                      of GatewayEventPayload<GuildRoleCreateReceiveEvent>
    | GUILD_ROLE_UPDATE                      of GatewayEventPayload<GuildRoleUpdateReceiveEvent>
    | GUILD_ROLE_DELETE                      of GatewayEventPayload<GuildRoleDeleteReceiveEvent>
    | GUILD_SCHEDULED_EVENT_CREATE           of GatewayEventPayload<GuildScheduledEventCreateReceiveEvent>
    | GUILD_SCHEDULED_EVENT_UPDATE           of GatewayEventPayload<GuildScheduledEventUpdateReceiveEvent>
    | GUILD_SCHEDULED_EVENT_DELETE           of GatewayEventPayload<GuildScheduledEventDeleteReceiveEvent>
    | GUILD_SCHEDULED_EVENT_USER_ADD         of GatewayEventPayload<GuildScheduledEventUserAddReceiveEvent>
    | GUILD_SCHEDULED_EVENT_USER_REMOVE      of GatewayEventPayload<GuildScheduledEventUserRemoveReceiveEvent>
    | GUILD_SOUNDBOARD_SOUND_CREATE          of GatewayEventPayload<GuildSoundboardSoundCreateReceiveEvent>
    | GUILD_SOUNDBOARD_SOUND_UPDATE          of GatewayEventPayload<GuildSoundboardSoundUpdateReceiveEvent>
    | GUILD_SOUNDBOARD_SOUND_DELETE          of GatewayEventPayload<GuildSoundboardSoundDeleteReceiveEvent>
    | GUILD_SOUNDBOARD_SOUNDS_UPDATE         of GatewayEventPayload<GuildSoundboardSoundsUpdateReceiveEvent>
    | GUILD_SOUNDBOARD_SOUNDS                of GatewayEventPayload<GuildSoundboardSoundsReceiveEvent>
    | INTEGRATION_CREATE                     of GatewayEventPayload<IntegrationCreateReceiveEvent>
    | INTEGRATION_UPDATE                     of GatewayEventPayload<IntegrationUpdateReceiveEvent>
    | INTEGRATION_DELETE                     of GatewayEventPayload<IntegrationDeleteReceiveEvent>
    | INVITE_CREATE                          of GatewayEventPayload<InviteCreateReceiveEvent>
    | INVITE_DELETE                          of GatewayEventPayload<InviteDeleteReceiveEvent>
    | MESSAGE_CREATE                         of GatewayEventPayload<MessageCreateReceiveEvent>
    | MESSAGE_UPDATE                         of GatewayEventPayload<MessageUpdateReceiveEvent>
    | MESSAGE_DELETE                         of GatewayEventPayload<MessageDeleteReceiveEvent>
    | MESSAGE_DELETE_BULK                    of GatewayEventPayload<MessageDeleteBulkReceiveEvent>
    | MESSAGE_REACTION_ADD                   of GatewayEventPayload<MessageReactionAddReceiveEvent>
    | MESSAGE_REACTION_REMOVE                of GatewayEventPayload<MessageReactionRemoveReceiveEvent>
    | MESSAGE_REACTION_REMOVE_ALL            of GatewayEventPayload<MessageReactionRemoveAllReceiveEvent>
    | MESSAGE_REACTION_REMOVE_EMOJI          of GatewayEventPayload<MessageReactionRemoveEmojiReceiveEvent>
    | PRESENCE_UPDATE                        of GatewayEventPayload<PresenceUpdateReceiveEvent>
    | TYPING_START                           of GatewayEventPayload<TypingStartReceiveEvent>
    | USER_UPDATE                            of GatewayEventPayload<UserUpdateReceiveEvent>
    | VOICE_CHANNEL_EFFECT_SEND              of GatewayEventPayload<VoiceChannelEffectSendReceiveEvent>
    | VOICE_STATE_UPDATE                     of GatewayEventPayload<VoiceStateUpdateReceiveEvent>
    | VOICE_SERVER_UPDATE                    of GatewayEventPayload<VoiceServerUpdateReceiveEvent>
    | WEBHOOKS_UPDATE                        of GatewayEventPayload<WebhooksUpdateReceiveEvent>
    | INTERACTION_CREATE                     of GatewayEventPayload<InteractionCreateReceiveEvent>
    | STAGE_INSTANCE_CREATE                  of GatewayEventPayload<StageInstanceCreateReceiveEvent>
    | STAGE_INSTANCE_UPDATE                  of GatewayEventPayload<StageInstanceUpdateReceiveEvent>
    | STAGE_INSTANCE_DELETE                  of GatewayEventPayload<StageInstanceDeleteReceiveEvent>
    | SUBSCRIPTION_CREATE                    of GatewayEventPayload<SubscriptionCreateReceiveEvent>
    | SUBSCRIPTION_UPDATE                    of GatewayEventPayload<SubscriptionUpdateReceiveEvent>
    | SUBSCRIPTION_DELETE                    of GatewayEventPayload<SubscriptionDeleteReceiveEvent>
    | MESSAGE_POLL_VOTE_ADD                  of GatewayEventPayload<MessagePollVoteAddReceiveEvent>
    | MESSAGE_POLL_VOTE_REMOVE               of GatewayEventPayload<MessagePollVoteRemoveReceiveEvent>

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
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_EMOJIS_UPDATE) -> GUILD_EMOJIS_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_STICKERS_UPDATE) -> GUILD_STICKERS_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_INTEGRATIONS_UPDATE) -> GUILD_INTEGRATIONS_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_MEMBER_ADD) -> GUILD_MEMBER_ADD <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_MEMBER_REMOVE) -> GUILD_MEMBER_REMOVE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_MEMBER_UPDATE) -> GUILD_MEMBER_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_MEMBERS_CHUNK) -> GUILD_MEMBERS_CHUNK <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_ROLE_CREATE) -> GUILD_ROLE_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_ROLE_UPDATE) -> GUILD_ROLE_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_ROLE_DELETE) -> GUILD_ROLE_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SCHEDULED_EVENT_CREATE) -> GUILD_SCHEDULED_EVENT_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SCHEDULED_EVENT_UPDATE) -> GUILD_SCHEDULED_EVENT_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SCHEDULED_EVENT_DELETE) -> GUILD_SCHEDULED_EVENT_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SCHEDULED_EVENT_USER_ADD) -> GUILD_SCHEDULED_EVENT_USER_ADD <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SCHEDULED_EVENT_USER_REMOVE) -> GUILD_SCHEDULED_EVENT_USER_REMOVE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SOUNDBOARD_SOUND_CREATE) -> GUILD_SOUNDBOARD_SOUND_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SOUNDBOARD_SOUND_UPDATE) -> GUILD_SOUNDBOARD_SOUND_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SOUNDBOARD_SOUND_DELETE) -> GUILD_SOUNDBOARD_SOUND_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SOUNDBOARD_SOUNDS_UPDATE) -> GUILD_SOUNDBOARD_SOUNDS_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof GUILD_SOUNDBOARD_SOUNDS) -> GUILD_SOUNDBOARD_SOUNDS <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof INTEGRATION_CREATE) -> INTEGRATION_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof INTEGRATION_UPDATE) -> INTEGRATION_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof INTEGRATION_DELETE) -> INTEGRATION_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof INVITE_CREATE) -> INVITE_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof INVITE_DELETE) -> INVITE_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_CREATE) -> MESSAGE_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_UPDATE) -> MESSAGE_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_DELETE) -> MESSAGE_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_DELETE_BULK) -> MESSAGE_DELETE_BULK <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_REACTION_ADD) -> MESSAGE_REACTION_ADD <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_REACTION_REMOVE) -> MESSAGE_REACTION_REMOVE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_REACTION_REMOVE_ALL) -> MESSAGE_REACTION_REMOVE_ALL <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_REACTION_REMOVE_EMOJI) -> MESSAGE_REACTION_REMOVE_EMOJI <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof PRESENCE_UPDATE) -> PRESENCE_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof TYPING_START) -> TYPING_START <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof USER_UPDATE) -> USER_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof VOICE_CHANNEL_EFFECT_SEND) -> VOICE_CHANNEL_EFFECT_SEND <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof VOICE_STATE_UPDATE) -> VOICE_STATE_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof VOICE_SERVER_UPDATE) -> VOICE_SERVER_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof WEBHOOKS_UPDATE) -> WEBHOOKS_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof INTERACTION_CREATE) -> INTERACTION_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof STAGE_INSTANCE_CREATE) -> STAGE_INSTANCE_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof STAGE_INSTANCE_UPDATE) -> STAGE_INSTANCE_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof STAGE_INSTANCE_DELETE) -> STAGE_INSTANCE_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof SUBSCRIPTION_CREATE) -> SUBSCRIPTION_CREATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof SUBSCRIPTION_UPDATE) -> SUBSCRIPTION_UPDATE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof SUBSCRIPTION_DELETE) -> SUBSCRIPTION_DELETE <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_POLL_VOTE_ADD) -> MESSAGE_POLL_VOTE_ADD <| Json.deserializeF json
        | GatewayOpcode.DISPATCH, Some (nameof MESSAGE_POLL_VOTE_REMOVE) -> MESSAGE_POLL_VOTE_REMOVE <| Json.deserializeF json
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
        | GUILD_EMOJIS_UPDATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_STICKERS_UPDATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_INTEGRATIONS_UPDATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_MEMBER_ADD g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_MEMBER_REMOVE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_MEMBER_UPDATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_MEMBERS_CHUNK g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_ROLE_CREATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_ROLE_UPDATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_ROLE_DELETE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SCHEDULED_EVENT_CREATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SCHEDULED_EVENT_UPDATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SCHEDULED_EVENT_DELETE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SCHEDULED_EVENT_USER_ADD g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SCHEDULED_EVENT_USER_REMOVE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SOUNDBOARD_SOUND_CREATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SOUNDBOARD_SOUND_UPDATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SOUNDBOARD_SOUND_DELETE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SOUNDBOARD_SOUNDS_UPDATE g -> Json.serializeF g |> writer.WriteRawValue
        | GUILD_SOUNDBOARD_SOUNDS g -> Json.serializeF g |> writer.WriteRawValue
        | INTEGRATION_CREATE i -> Json.serializeF i |> writer.WriteRawValue
        | INTEGRATION_UPDATE i -> Json.serializeF i |> writer.WriteRawValue
        | INTEGRATION_DELETE i -> Json.serializeF i |> writer.WriteRawValue
        | INVITE_CREATE i -> Json.serializeF i |> writer.WriteRawValue
        | INVITE_DELETE i -> Json.serializeF i |> writer.WriteRawValue
        | MESSAGE_CREATE m -> Json.serializeF m |> writer.WriteRawValue
        | MESSAGE_UPDATE m -> Json.serializeF m |> writer.WriteRawValue
        | MESSAGE_DELETE m -> Json.serializeF m |> writer.WriteRawValue
        | MESSAGE_DELETE_BULK m -> Json.serializeF m |> writer.WriteRawValue
        | MESSAGE_REACTION_ADD m -> Json.serializeF m |> writer.WriteRawValue
        | MESSAGE_REACTION_REMOVE m -> Json.serializeF m |> writer.WriteRawValue
        | MESSAGE_REACTION_REMOVE_ALL m -> Json.serializeF m |> writer.WriteRawValue
        | MESSAGE_REACTION_REMOVE_EMOJI m -> Json.serializeF m |> writer.WriteRawValue
        | PRESENCE_UPDATE p -> Json.serializeF p |> writer.WriteRawValue
        | TYPING_START t -> Json.serializeF t |> writer.WriteRawValue
        | USER_UPDATE u -> Json.serializeF u |> writer.WriteRawValue
        | VOICE_CHANNEL_EFFECT_SEND v -> Json.serializeF v |> writer.WriteRawValue
        | VOICE_STATE_UPDATE v -> Json.serializeF v |> writer.WriteRawValue
        | VOICE_SERVER_UPDATE v -> Json.serializeF v |> writer.WriteRawValue
        | WEBHOOKS_UPDATE w -> Json.serializeF w |> writer.WriteRawValue
        | INTERACTION_CREATE i -> Json.serializeF i |> writer.WriteRawValue
        | STAGE_INSTANCE_CREATE s -> Json.serializeF s |> writer.WriteRawValue
        | STAGE_INSTANCE_UPDATE s -> Json.serializeF s |> writer.WriteRawValue
        | STAGE_INSTANCE_DELETE s -> Json.serializeF s |> writer.WriteRawValue
        | SUBSCRIPTION_CREATE s -> Json.serializeF s |> writer.WriteRawValue
        | SUBSCRIPTION_UPDATE s -> Json.serializeF s |> writer.WriteRawValue
        | SUBSCRIPTION_DELETE s -> Json.serializeF s |> writer.WriteRawValue
        | MESSAGE_POLL_VOTE_ADD m -> Json.serializeF m |> writer.WriteRawValue
        | MESSAGE_POLL_VOTE_REMOVE m -> Json.serializeF m |> writer.WriteRawValue

module GatewayReceiveEvent =
    let getSequenceNumber (event: GatewayReceiveEvent) =
        let json = Json.serializeF event
        let document = JsonDocument.Parse json

        match document.RootElement.TryGetProperty "s" with
        | true, t -> Some (t.GetInt32())
        | _ -> None

        // TODO: Figure out way to calculate this without serializing and parsing (?)
