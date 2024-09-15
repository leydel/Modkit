namespace Modkit.Diacord.Core.Providers

open Modkit.Diacord.Core.Structures
open Modkit.Discordfs.Services
open System.Threading.Tasks

type IStateProvider =
    abstract member GetState:
        guildId: string ->
        Task<DiacordState>

type StateProvider (discordHttpService: IDiscordHttpService) =
    interface IStateProvider with
        member _.GetState guildId = task {
            let! roles = discordHttpService.Guilds.GetGuildRoles guildId
            let! emojis = discordHttpService.Emojis.ListGuildEmojis guildId
            let! stickers = discordHttpService.Stickers.ListGuildStickers guildId
            let! channels = discordHttpService.Guilds.GetGuildChannels guildId

            return {
                Roles = roles;
                Emojis = emojis;
                Stickers = stickers;
                Channels = channels;
            }
        }
