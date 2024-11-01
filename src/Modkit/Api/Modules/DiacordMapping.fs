namespace Modkit.Api.Modules

open System.Collections.Generic
open System.Text.Json.Serialization

type DiacordMapping = {
    [<JsonPropertyName "guildId">] GuildId: string
    [<JsonPropertyName "mapping">] Mapping: IDictionary<string, string>
}

type DiacordMappingDto = {
    [<JsonPropertyName "guild_id">] GuildId: string
    [<JsonPropertyName "mapping">] Mapping: IDictionary<string, string>
}

module DiacordMapping =
    let toDto (model: DiacordMapping): DiacordMappingDto = {
        GuildId = model.GuildId;
        Mapping = model.Mapping;
    }

    let fromDto (dto: DiacordMappingDto): DiacordMapping = {
        GuildId = dto.GuildId;
        Mapping = dto.Mapping;       
    }
