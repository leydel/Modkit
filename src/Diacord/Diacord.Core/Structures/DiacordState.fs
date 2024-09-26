namespace Modkit.Diacord.Core.Structures

open Discordfs.Types

type DiacordState = {
    Roles: Role list
    Emojis: Emoji list
    Stickers: Sticker list
    Channels: Channel list
}
