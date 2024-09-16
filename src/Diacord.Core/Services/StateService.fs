namespace Modkit.Diacord.Core.Services

open Modkit.Diacord.Core.Structures
open Modkit.Diacord.Core.Types
open Modkit.Discordfs.Types
open System.Collections.Generic

type IStateService =
    abstract member Compare:
        state: DiacordState ->
        template: DiacordTemplate ->
        mappings: IDictionary<string, string> ->
        DiffNode

type StateService () =
    member _.map<'a, 'b>
        (getLeftId: 'a -> string) (getRightId: 'b -> string) (template: 'a list) (state: 'b list) (strict: bool)
        (mappings: IDictionary<string, string>) =
            let innerJoin (template: 'a list) (state: 'b list) (kvp: KeyValuePair<string, string>) = (
                List.tryFind (fun (t: 'a) -> (getLeftId t) = kvp.Key) template,
                List.tryFind (fun (s: 'b) -> (getRightId s) = kvp.Value) state
            )

            let leftJoin (mappings: IDictionary<string, string>) (t: 'a) =
                match mappings.Keys.Contains (getLeftId t) with
                | true -> []
                | false -> [(Some t, None)]

            let rightJoin (mappings: IDictionary<string, string>) (s: 'b) =
                match mappings.Values.Contains (getRightId s) with
                | true -> []
                | false -> [(None, Some s)]

            let ignoreIfNotStrict (strict: bool) (a: 'a option, b: 'b option) =
                match a, b with
                | None, Some _ -> strict
                | _ -> true

            List.map (innerJoin template state) (Seq.toList mappings)
            |> List.append (List.collect (leftJoin mappings) template)
            |> List.append (List.collect (rightJoin mappings) state)
            |> List.filter (ignoreIfNotStrict strict)

    interface IStateService with
        member this.Compare state template mappings =
            let strictRoles = template.Settings |> Option.map _.StrictRoles |> Option.defaultValue false
            let strictEmojis = template.Settings |> Option.map _.StrictEmojis |> Option.defaultValue false
            let strictStickers = template.Settings |> Option.map _.StrictStickers |> Option.defaultValue false
            let strictChannels = template.Settings |> Option.map _.StrictChannels |> Option.defaultValue false

            let templateRoles = Option.defaultValue [] template.Roles
            let templateEmojis = Option.defaultValue [] template.Emojis
            let templateStickers = Option.defaultValue [] template.Stickers
            let templateChannels = Option.defaultValue [] template.Channels

            let roles = this.map<DiacordRole, Role> _.DiacordId _.Id templateRoles state.Roles strictRoles mappings
            let emojis = this.map<DiacordEmoji, Emoji> _.DiacordId _.Id.Value templateEmojis state.Emojis strictEmojis mappings
            let stickers = this.map<DiacordSticker, Sticker> _.DiacordId _.Id templateStickers state.Stickers strictStickers mappings
            let channels = this.map<DiacordChannel, Channel> _.DiacordId _.Id templateChannels state.Channels strictChannels mappings

            DiffNode.Root [
                DiffNode.Branch("roles", roles |> List.map (DiacordRole.diff mappings));
                DiffNode.Branch("emojis", emojis |> List.map (DiacordEmoji.diff mappings));
                DiffNode.Branch("stickers", stickers |> List.map (DiacordSticker.diff mappings));
                DiffNode.Branch("channels", channels |> List.map (DiacordChannel.diff mappings));
            ]
