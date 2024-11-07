namespace Discordfs.Types

open System
open System.Text.Json
open System.Text.Json.Serialization

type Invite = {
    [<JsonPropertyName "type">] Type: InviteType
    [<JsonPropertyName "code">] Code: string
    [<JsonPropertyName "guild">] Guild: Guild option
    [<JsonPropertyName "channel">] Channel: PartialChannel option
    [<JsonPropertyName "inviter">] Inviter: PartialUser option
    [<JsonPropertyName "target_type">] TargetType: InviteTargetType option
    [<JsonPropertyName "target_user">] TargetUser: User option
    [<JsonPropertyName "target_application">] TargetApplication: PartialApplication option
    [<JsonPropertyName "approximate_presence_count">] ApproximatePresenceCount: int option
    [<JsonPropertyName "approximate_member_count">] ApproximateMemberCount: int option
    [<JsonPropertyName "expires_at">] ExpiresAt: DateTime
    [<JsonPropertyName "guild_scheduled_event">] GuildScheduledEvent: GuildScheduledEvent option
}

type InviteMetadata = {
    [<JsonPropertyName "uses">] Uses: int
    [<JsonPropertyName "max_uses">] MaxUses: int
    [<JsonPropertyName "max_age">] MaxAge: int
    [<JsonPropertyName "temporary">] Temporary: bool
    [<JsonPropertyName "created_at">] CreatedAt: DateTime
}

[<JsonConverter(typeof<InviteWithmetadataConverter>)>]
type InviteWithMetadata = {
    Invite: Invite
    Metadata: InviteMetadata
}

and InviteWithmetadataConverter () =
    inherit JsonConverter<InviteWithMetadata> ()
    
    override _.Read (reader, typeToConvert, options) =
        let success, document = JsonDocument.TryParseValue &reader
        if not success then raise (JsonException())

        let json = document.RootElement.GetRawText()

        {
            Invite = Json.deserializeF json;
            Metadata = Json.deserializeF json;
        }

    override _.Write (writer, value, options) =
        let invite = Json.serializeF value.Invite
        let metadata = Json.serializeF value.Metadata

        writer.WriteRawValue (Json.merge invite metadata)
