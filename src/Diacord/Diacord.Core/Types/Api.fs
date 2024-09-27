namespace Modkit.Diacord.Core.Types

open System.Collections.Generic
open System.Text.Json.Serialization

type ApiDiacordMapping = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "mappings">] Mappings: IDictionary<string, string>
}
