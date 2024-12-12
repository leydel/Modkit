namespace Modkit.Roles.Application.Queries

open System.Net.Http

open Discordfs.Rest
open Discordfs.Rest.Modules
open Discordfs.Types
open MediatR

type GetApplicationFromTokenQueryResponse = Result<Application, unit>

type GetApplicationFromTokenQuery (token: string) =
    interface IRequest<GetApplicationFromTokenQueryResponse>

    member val Token = token with get, set

type GetApplicationFromTokenQueryHandler (
    httpClientFactory: IHttpClientFactory
) =
    interface IRequestHandler<GetApplicationFromTokenQuery, GetApplicationFromTokenQueryResponse> with
        member _.Handle (req, ct) = task {
            let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient req.Token

            let! currentApplication = client |> Rest.getCurrentApplication
            match currentApplication with
            | Error _ -> return Error ()
            | Ok { Data = app } -> return Ok app
        }
