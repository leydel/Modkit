namespace Modkit.Diacord.Core.Providers

open Modkit.Diacord.Core.Structures
open Modkit.Discordfs.Services
open System.Threading.Tasks

type IStateProvider =
    abstract member get:
        guildId: string ->
        Task<DiacordState>

type StateProvider (httpService: IHttpService) =
    interface IStateProvider with
        member _.get guildId = task {
            let! roles = httpService.Guilds.GetGuildRoles guildId
            let! emojis = httpService.Emojis.ListGuildEmojis guildId
            let! stickers = httpService.Stickers.ListGuildStickers guildId
            let! channels = httpService.Guilds.GetGuildChannels guildId

            return {
                Roles = roles;
                Emojis = emojis;
                Stickers = stickers;
                Channels = channels;
            }
        }
