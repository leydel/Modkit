namespace Discordfs.Types

open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/sticker#sticker-object-sticker-structure
type Sticker = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "pack_id">] PackId: string option
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "description">] Description: string option
    [<JsonPropertyName "tags">] Tags: string
    [<JsonPropertyName "type">] Type: StickerType
    [<JsonPropertyName "format_type">] FormatType: StickerFormatType
    [<JsonPropertyName "available">] Available: bool option
    [<JsonPropertyName "guild_id">] GuildId: string option
    [<JsonPropertyName "user">] User: User option
    [<JsonPropertyName "sort_value">] SortValue: int option
}

// https://discord.com/developers/docs/resources/sticker#sticker-item-object-sticker-item-structure
type StickerItem = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "format_type">] FormatType: StickerFormatType
}

// https://discord.com/developers/docs/resources/sticker#sticker-pack-object
type StickerPack = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "stickers">] Stickers: Sticker list
    [<JsonPropertyName "name">] Name: string
    [<JsonPropertyName "sku_id">] SkuId: string
    [<JsonPropertyName "cover_sticker_id">] CoverStickerId: string option
    [<JsonPropertyName "description">] Description: string
    [<JsonPropertyName "banner_asset_id">] BannerAssetId: string option
}
