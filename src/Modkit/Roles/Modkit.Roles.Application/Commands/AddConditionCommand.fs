namespace Modkit.Roles.Application.Commands

open System.Net.Http

open Discordfs
open Discordfs.Types
open Discordfs.Rest.Modules
open MediatR

open Modkit.Roles.Domain.Types
open Modkit.Roles.Domain.Entities

open Modkit.Roles.Application.Repositories

type AddConditionCommandError =
    | UnknownApplication
    | TooManyConditions
    | ConditionKeyAlreadyExists
    | DiscordRecordFetchFailed
    | DiscordRecordUpdateFailed
    | DatabaseUpdateFailed

type AddConditionCommandResponse = Result<(string * MetadataType), AddConditionCommandError>

type AddConditionCommand (
    applicationId: string,
    key: string,
    metadataType: MetadataType,
    name: string,
    description: string
) =
    interface IRequest<AddConditionCommandResponse>

    member val ApplicationId = applicationId with get, set
    member val Key = key with get, set
    member val MetadataType = metadataType with get, set
    member val Name = name with get, set
    member val Description = description with get, set

type AddConditionCommandHandler (
    applicationRepository: IApplicationRepository,
    httpClientFactory: IHttpClientFactory
) =
    interface IRequestHandler<AddConditionCommand, AddConditionCommandResponse> with
        member _.Handle (req, ct) = task {
            let! app = applicationRepository.Get req.ApplicationId
            match app with
            | Error _ -> return Error UnknownApplication
            | Ok app when Seq.length app.Metadata > Application.MAX_ROLE_CONNNECTION_METADATA - 1 -> return Error TooManyConditions
            | Ok app when app.Metadata |> Seq.exists (fun (key, _) -> key = req.Key) -> return Error ConditionKeyAlreadyExists
            | Ok app ->
                let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient app.Token

                let! records = Application.getRoleConnectionMetadata app.Id client
                match records with
                | None -> return Error DiscordRecordFetchFailed
                | Some records ->
                    let newDiscordMetadataRecord = {
                        Type =
                            match req.MetadataType with
                            | MetadataType.RANGE_MAX -> ApplicationRoleConnectionMetadataType.INTEGER_LESS_THAN_OR_EQUAL
                            | MetadataType.RANGE_MIN -> ApplicationRoleConnectionMetadataType.INTEGER_GREATER_THAN_OR_EQUAL
                            | MetadataType.PICK -> ApplicationRoleConnectionMetadataType.INTEGER_EQUAL
                            | MetadataType.PICK_NOT -> ApplicationRoleConnectionMetadataType.INTEGER_NOT_EQUAL
                            | MetadataType.IS -> ApplicationRoleConnectionMetadataType.BOOLEAN_EQUAL
                            | MetadataType.IS_NOT -> ApplicationRoleConnectionMetadataType.BOOLEAN_NOT_EQUAL
                            | _ -> failwith "Unexpected MetadataType provided"
                        Key = req.Key
                        Name = req.Name
                        Description = req.Description
                        NameLocalizations = None
                        DescriptionLocalizations = None
                    }

                    let newApplicationMetadataRecord = (req.Key, req.MetadataType)

                    // TODO: Add support for localization (separate commands?)

                    let discordMetadata = records @ [newDiscordMetadataRecord]
                    let applicationMetadata = (Seq.toList app.Metadata) @ [newApplicationMetadataRecord]

                    let! res = client |> Application.updateRoleConnectionMetadata app.Id discordMetadata
                    match res with
                    | None -> return Error DiscordRecordUpdateFailed
                    | Some _ ->
                        let! updated = applicationRepository.Update app.Id [ApplicationChange.Metadata applicationMetadata]
                        match updated with
                        | Error _ -> return Error DatabaseUpdateFailed
                        | Ok _ -> return Ok newApplicationMetadataRecord
        }
