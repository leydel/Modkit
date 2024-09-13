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

            return {
                Roles = List.map DiacordRole.from roles;
            }
        }
