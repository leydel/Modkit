namespace Modkit.Roles.Application.Commands

open MediatR

open Modkit.Roles.Domain.Types
open Modkit.Roles.Domain.Entities

open Modkit.Roles.Application.Common
open Modkit.Roles.Application.Repositories

type SetUserConditionCommandError =
    | UnknownApplication
    | InvalidConditionKey
    | InvlaidConditionValue
    | UnknownUser
    | DatabaseUpdateFailed

type SetUserConditionCommandResponse = Result<User, SetUserConditionCommandError>

type SetUserConditionCommand (
    applicationId: string,
    userId: string,
    conditionKey: string,
    conditionValue: int
) =
    interface IRequest<SetUserConditionCommandResponse>

    member val ApplicationId = applicationId with get, set
    member val UserId = userId with get, set
    member val ConditionKey = conditionKey with get, set
    member val ConditionValue = conditionValue with get, set

type SetUserConditionCommandHandler (
    applicationRepository: IApplicationRepository,
    userRepository: IUserRepository
) =
    let getApplication applicationId = task {
        let! application = applicationRepository.Get applicationId
        return application |> Result.mapError (fun _ -> UnknownApplication)
    }

    let validateCondition (application: Application) conditionKey conditionValue =
        let metadataType =
            application.Metadata
            |> Seq.tryFind (fun (k, v) -> k = conditionKey)
            |> Option.map (fun (k, v) -> v)

        let isNotBinary value =
            value <> 0 && value <> 1

        match metadataType with
        | None -> Error InvalidConditionKey
        | Some MetadataType.IS when isNotBinary conditionValue -> Error InvlaidConditionValue
        | Some MetadataType.IS_NOT when isNotBinary conditionValue -> Error InvlaidConditionValue
        | _ -> Ok ()

    let getUser applicationId userId = task {
        let! user = userRepository.Get userId applicationId
        return user |> Result.mapError (fun _ -> UnknownUser)
    }

    let setUserCondition (user: User) conditionKey conditionValue = task {
        let metadata =
            user.Metadata
            |> Seq.where (fun (k, _) -> k <> conditionKey)
            |> Seq.append [(conditionKey, conditionValue)]

        let! updatedUser = userRepository.Update user.Id user.ApplicationId [UserChange.Metadata metadata]
        return updatedUser |> Result.mapError (fun _ -> DatabaseUpdateFailed)
    }

    interface IRequestHandler<SetUserConditionCommand, SetUserConditionCommandResponse> with
        member _.Handle (req, ct) = Railway.toTask <| railway {
            let! application = getApplication req.ApplicationId
            do! validateCondition application req.ConditionKey req.ConditionValue
            let! user = getUser application.Id req.UserId
            return! setUserCondition user req.ConditionKey req.ConditionValue
        }
        