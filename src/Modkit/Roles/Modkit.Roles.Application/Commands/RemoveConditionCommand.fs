namespace Modkit.Roles.Application.Commands

open System.Net.Http

open Discordfs
open Discordfs.Types
open Discordfs.Rest.Modules
open MediatR

open Modkit.Roles.Domain.Entities

open Modkit.Roles.Application.Repositories

type RemoveConditionCommandError =
    | UnknownApplication
    | DiscordRecordFetchFailed
    | DatabaseUpdateFailed
    | DiscordRecordUpdateFailed

type RemoveConditionCommandResponse = Result<unit, RemoveConditionCommandError>

type RemoveConditionCommand (
    applicationId: string,
    key: string
) =
    interface IRequest<RemoveConditionCommandResponse>

    member val ApplicationId = applicationId with get, set
    member val Key = key with get, set

type RemoveConditionCommandHandler (
    applicationRepository: IApplicationRepository,
    httpClientFactory: IHttpClientFactory
) =
    interface IRequestHandler<RemoveConditionCommand, RemoveConditionCommandResponse> with
        member _.Handle (req, ct) = task {
            let! app = applicationRepository.Get req.ApplicationId
            match app with
            | Error _ -> return Error UnknownApplication
            | Ok app ->
                let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient app.Token

                let! records = Application.getRoleConnectionMetadata app.Id client
                match records with
                | None -> return Error DiscordRecordFetchFailed
                | Some records ->
                    let filteredDiscordMetadataRecords = records |> List.filter (fun v -> v.Key <> req.Key)
                    let filteredApplicationMetadataRecords = app.Metadata |> Seq.filter (fun (key, _) -> key <> req.Key)

                    let! res = client |> Application.updateRoleConnectionMetadata app.Id filteredDiscordMetadataRecords
                    match res with
                    | None -> return Error DiscordRecordUpdateFailed
                    | Some _ ->
                        let! updated = applicationRepository.Update app.Id [ApplicationChange.Metadata filteredApplicationMetadataRecords]
                        match updated with
                        | Error _ -> return Error DatabaseUpdateFailed
                        | Ok _ -> return Ok ()
        }
