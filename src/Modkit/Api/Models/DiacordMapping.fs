namespace Modkit.Api.Models

open System.Collections.Generic

type DiacordMapping = {
    GuildId: string
    Mappings: IDictionary<string, string>
}
