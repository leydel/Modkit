namespace Modkit.Diacord.Core.Commands

open Modkit.Diacord.Core.Providers
open Modkit.Diacord.Core.Structures
open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System.Collections.Generic
open System.Threading.Tasks

type ICompareCommand =
    abstract member run:
        guildId: string ->
        template: DiacordTemplate ->
        Task<DiffNode>

type CompareCommand (stateProvider: IStateProvider, mappingProvider: IMappingProvider) =
    member _.map<'a, 'b>
        (getLeftId: 'a -> string) (getRightId: 'b -> string) (template: 'a list) (state: 'b list) (strict: bool)
        (mappings: IDictionary<string, string>) =
            // Map template to state
            let innerJoin (template: 'a list) (state: 'b list) (kvp: KeyValuePair<string, string>) = (
                List.tryFind (fun (t: 'a) -> (getLeftId t) = kvp.Key) template,
                List.tryFind (fun (s: 'b) -> (getRightId s) = kvp.Value) state
            )

            // Add templates not contained in mappings
            let leftJoin (mappings: IDictionary<string, string>) (t: 'a) =
                match mappings.Keys.Contains (getLeftId t) with
                | true -> []
                | false -> [(Some t, None)]

            // Add states not contained in mappings
            let rightJoin (mappings: IDictionary<string, string>) (s: 'b) =
                match mappings.Values.Contains (getRightId s) with
                | true -> []
                | false -> [(None, Some s)]

            // Apply filter to apply strict changes if strict mode enabled
            let ignoreIfNotStrict (strict: bool) (a: 'a option, b: 'b option) =
                match a, b with
                | None, Some _ -> strict
                | _ -> true

            // Construct mapping between template and state
            List.map (innerJoin template state) (Seq.toList mappings)
            |> List.append (List.collect (leftJoin mappings) template)
            |> List.append (List.collect (rightJoin mappings) state)
            |> List.filter (ignoreIfNotStrict strict)

    interface ICompareCommand with
        member this.run guildId template = task {
            // Fetch data from API and Discord
            let! state = stateProvider.get guildId
            let! mapping = mappingProvider.get guildId

            let mappings =
                match mapping with
                | Error _ -> Dictionary<string, string>() :> IDictionary<string, string>
                | Ok mappings -> mappings

            // Determine command settings
            let strictRoles = template.Settings |> Option.map _.StrictRoles |> Option.defaultValue false
            let strictEmojis = template.Settings |> Option.map _.StrictEmojis |> Option.defaultValue false
            let strictStickers = template.Settings |> Option.map _.StrictStickers |> Option.defaultValue false
            let strictChannels = template.Settings |> Option.map _.StrictChannels |> Option.defaultValue false

            // Get templates to compare
            let templateRoles = Option.defaultValue [] template.Roles
            let templateEmojis = Option.defaultValue [] template.Emojis
            let templateStickers = Option.defaultValue [] template.Stickers
            let templateChannels = Option.defaultValue [] template.Channels

            // Compare templates to state
            let roles = this.map<DiacordRole, Role> _.DiacordId _.Id templateRoles state.Roles strictRoles mappings
            let emojis = this.map<DiacordEmoji, Emoji> _.DiacordId _.Id.Value templateEmojis state.Emojis strictEmojis mappings
            let stickers = this.map<DiacordSticker, Sticker> _.DiacordId _.Id templateStickers state.Stickers strictStickers mappings
            let channels = this.map<DiacordChannel, Channel> _.DiacordId _.Id templateChannels state.Channels strictChannels mappings

            // Create diff tree comparing template to state
            return DiffNode.Root [
                DiffNode.Branch("roles", roles |> List.map (DiacordRole.diff mappings));
                DiffNode.Branch("emojis", emojis |> List.map (DiacordEmoji.diff mappings));
                DiffNode.Branch("stickers", stickers |> List.map (DiacordSticker.diff mappings));
                DiffNode.Branch("channels", channels |> List.map (DiacordChannel.diff mappings));
            ]
        }
