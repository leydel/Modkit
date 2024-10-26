namespace Discordfs.Commands.Services

open Discordfs.Commands.Structures
open Discordfs.Types
open Discordfs.Webhook.Payloads
open System.Threading.Tasks

type ICommandService =
    abstract member Commands: Command list

    abstract member Execute:
        interaction: Interaction ->
        Task<Result<InteractionResponse, string>>

type CommandService (commands: Command list) =
    member _.getCommandName (interaction: Interaction) =
        match interaction with
        | { Data = None } -> None
        | { Data = Some { Name = name } } -> Some name

    member _.getCommand (name: string) =
        commands |> List.tryFind (fun c -> c.Data.Name = name)

    interface ICommandService with
        member _.Commands = commands

        member this.Execute interaction =
            match this.getCommandName interaction with
            | None -> Task.FromResult <| Error "Missing command name in interaction data"
            | Some name ->
                match this.getCommand name with
                | None -> Task.FromResult <| Error "Unknown command"
                | Some command -> command.Execute interaction
