namespace Modkit.Diacord.Core.Types

open System.Collections.Generic
open System.Text.Json.Serialization

type ApiDiacordMapping = {
    [<JsonName "guild_id">] GuildId: string
    [<JsonName "mappings">] Mappings: IDictionary<string, string>
}
