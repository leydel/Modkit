namespace Modkit.Diacord.Core.Commands

open Modkit.Diacord.Core.Providers
open Modkit.Diacord.Core.Structures
open System.Collections.Generic
open System.Threading.Tasks

type ICreateCommand =
    abstract member run:
        guildId: string ->
        strictRoles: bool ->
        strictEmojis: bool ->
        strictStickers: bool ->
        strictChannels: bool ->
        Task<(DiacordTemplate * IDictionary<string, string>)>

type CreateCommand (stateProvider: IStateProvider) =
    interface ICreateCommand with
        member _.run guildId strictRoles strictEmojis strictStickers strictChannels = task {
            // Fetch state from Discord
            let! state = stateProvider.get guildId

            // Generate template values from state
            let templateRoles = List.map DiacordRole.from state.Roles
            let templateEmojis = List.map DiacordEmoji.from state.Emojis
            let templateStickers = List.map DiacordSticker.from state.Stickers
            let templateChannels = List.map DiacordChannel.from state.Channels

            // Generate template settings
            let toTrueOrNone (value: bool) =
                match value with
                | true -> Some true
                | false -> None

            let settings = {
                StrictRoles = toTrueOrNone strictRoles;
                StrictEmojis = toTrueOrNone strictEmojis;
                StrictStickers = toTrueOrNone strictStickers;
                StrictChannels = toTrueOrNone strictChannels;
            }

            // Create resulting template and mappings
            let template = {
                Settings = Some settings;
                Roles = Some templateRoles;
                Emojis = Some templateEmojis;
                Stickers = Some templateStickers;
                Channels = Some templateChannels;
            }

            let mappings =
                List.empty<string>
                |> List.append (templateRoles |> List.map _.DiacordId)
                |> List.append (templateEmojis |> List.map _.DiacordId)
                |> List.append (templateStickers |> List.map _.DiacordId)
                |> List.append (templateChannels |> List.map _.DiacordId)
                |> List.map (fun id -> (id, id))
                |> dict

            // TODO: Figure out nice way to allow neater mappings. Currently the diacord ID will be the snowflake, which
            // should be able to then be renamed somehow without breaking mappings entirely.

            return (template, mappings)
        }
