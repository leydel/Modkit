namespace Modkit.Diacord.Core.Providers

open Modkit.Diacord.Core.Structures
open System.Net.Http
open System.Threading.Tasks

type IStateProvider =
    abstract member get:
        guildId: string ->
        Task<DiacordState>

type StateProvider (httpClientFactory: IHttpClientFactory, discordBotToken: string) =
    interface IStateProvider with
        member _.get guildId = task {
            //let! roles = restClient.Guilds.GetGuildRoles guildId
            //let! emojis = restClient.Emojis.ListGuildEmojis guildId
            //let! stickers = restClient.Stickers.ListGuildStickers guildId
            //let! channels = restClient.Guilds.GetGuildChannels guildId
            //
            //return {
            //    Roles = roles;
            //    Emojis = emojis;
            //    Stickers = stickers;
            //    Channels = channels;
            //}

            return {
                Roles = [];
                Emojis = [];
                Stickers = [];
                Channels = [];
            }

            // TODO: Once above resource modules are created, rewrite this using them and in a functional style
        }
