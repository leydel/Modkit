namespace Modkit.Diacord.Core.Providers

open Discordfs.Rest.Clients
open Modkit.Diacord.Core.Structures
open System.Threading.Tasks

type IStateProvider =
    abstract member get:
        guildId: string ->
        Task<DiacordState>

type StateProvider (restClient: IRestClient) =
    interface IStateProvider with
        member _.get guildId = task {
            let! roles = restClient.Guilds.GetGuildRoles guildId
            let! emojis = restClient.Emojis.ListGuildEmojis guildId
            let! stickers = restClient.Stickers.ListGuildStickers guildId
            let! channels = restClient.Guilds.GetGuildChannels guildId

            return {
                Roles = roles;
                Emojis = emojis;
                Stickers = stickers;
                Channels = channels;
            }
        }
