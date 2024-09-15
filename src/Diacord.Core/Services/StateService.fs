namespace Modkit.Diacord.Core.Services

open Modkit.Diacord.Core.Structures
open Modkit.Discordfs.Types
open System.Collections.Generic

type IStateService =
    abstract member Compare:
        state: DiacordState ->
        template: DiacordTemplate ->
        mappings: IDictionary<string, string> ->
        unit

type StateService () =
    member _.map<'a, 'b> (getDiacordId: 'a -> string) (getStateId: 'b -> string) (template: 'a list) (state: 'b list) (mappings: IDictionary<string, string>) =
        let innerJoin (template: 'a list) (state: 'b list) (kvp: KeyValuePair<string, string>) = (
            List.tryFind (fun (t: 'a) -> (getDiacordId t) = kvp.Key) template,
            List.tryFind (fun (s: 'b) -> (getStateId s) = kvp.Value) state
        )

        let leftJoin (mappings: IDictionary<string, string>) (t: 'a) =
            match mappings.Keys.Contains (getDiacordId t) with
            | true -> []
            | false -> [(Some t, None)]

        let rightJoin (mappings: IDictionary<string, string>) (s: 'b) =
            match mappings.Values.Contains (getStateId s) with
            | true -> []
            | false -> [(None, Some s)]

        List.map (innerJoin template state) (Seq.toList mappings)
        |> List.append (List.collect (leftJoin mappings) template)
        |> List.append (List.collect (rightJoin mappings) state)

    interface IStateService with
        member this.Compare state template mappings =
            let templateRoles = Option.defaultValue [] template.Roles
            let templateEmojis = Option.defaultValue [] template.Emojis
            let templateStickers = Option.defaultValue [] template.Stickers
            let templateChannels = Option.defaultValue [] template.Channels

            let roles = this.map<DiacordRole, Role> _.DiacordId _.Id templateRoles state.Roles mappings
            let emojis = this.map<DiacordEmoji, Emoji> _.DiacordId _.Id.Value templateEmojis state.Emojis mappings
            let stickers = this.map<DiacordSticker, Sticker> _.DiacordId _.Id templateStickers state.Stickers mappings
            let channels = this.map<DiacordChannel, Channel> _.DiacordId _.Id templateChannels state.Channels mappings

            // TODO: Using tuples, check what diffs have taken place, and then return result of that

            ()
