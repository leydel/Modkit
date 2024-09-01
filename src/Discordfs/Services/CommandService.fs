namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Types
open Modkit.Discordfs.Commands
open System.Threading.Tasks

type ICommandService =
    abstract member Execute: interaction: Interaction -> Task<Result<InteractionCallback, string>>

type CommandService (commands: Command<_> list) = // TODO: Figure out how to support any child of `Command`
    member _.getCommandName (interaction: Interaction) =
        match interaction with
        | { Data = None } -> None
        | { Data = Some { Name = name } } -> Some name

    member _.getCommand (name: string) =
        commands |> List.tryFind (fun c -> c.Data.Name = name)

    interface ICommandService with
        member this.Execute interaction =
            match this.getCommandName interaction with
            | None -> Task.FromResult <| Error "Missing command name in interaction data"
            | Some name ->
                match this.getCommand name with
                | None -> Task.FromResult <| Error "Unknown command"
                | Some command -> command.Run interaction
