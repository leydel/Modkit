namespace Modkit.Roles.Functions

open Discordfs.Rest
open Discordfs.Rest.Modules
open Microsoft.Azure.Cosmos
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open Modkit.Roles.Common
open Modkit.Roles.Types
open System
open System.Net
open System.Net.Http
open System.Text.Json.Serialization
open System.Threading.Tasks

type PostApplicationPayload = {
    [<JsonPropertyName "token">] Token: string
    [<JsonPropertyName "publicKey">] PublicKey: string
}

type ApplicationController (
    logger: ILogger<ApplicationController>,
    httpClientFactory: IHttpClientFactory
) =
    [<Function "PostApplication">]
    member _.PostApplication (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "applications")>] req: HttpRequestData,
        [<CosmosDBInput(containerName = ROLE_APP_CONTAINER_NAME, databaseName = DATABASE_NAME)>] container: Container,
        [<FromBody>] payload: PostApplicationPayload
    ) = task {
        let host = req.Url.GetLeftPart(UriPartial.Authority)
        let client = httpClientFactory.CreateClient() |> HttpClient.toBotClient payload.Token

        let! currentApplication = client |> Rest.getCurrentApplication
        match currentApplication with
        | Error _ ->
            logger.LogInformation("Failed to get current application using provided token")
            
            let res = req.CreateResponse(HttpStatusCode.BadRequest)
            do! res.WriteAsJsonAsync({| message = "Invalid token provided" |})
            return res

        | Ok { Data = app } ->
            logger.LogInformation("Successfully fetched application {ApplicationId} using provided token", app.Id)

            let editCurrentApplicationPayload = EditCurrentApplicationPayload(
                description = DEFAULT_APP_DESCRIPTION,
                role_connection_verification_url = host + $"/applications/{app.Id}/linked-role",
                interactions_endpoint_url = host + $"/applications/{app.Id}/interactions"
            )

            let! res = client |> Rest.editCurrentApplication editCurrentApplicationPayload
            match res with
            | Error _ ->
                logger.LogError("Failed to update application {ApplicationId}", app.Id)

                let res = req.CreateResponse(HttpStatusCode.InternalServerError)
                do! res.WriteAsJsonAsync({| message = "Unexpectedly unable to update application on Discord" |})
                return res

            | Ok _ ->
                try
                    let roleApp: RoleApp = { Id = app.Id; Token = payload.Token; PublicKey = payload.PublicKey } // TODO: Encrypt token at rest
                    do! container.UpsertItemAsync(roleApp, PartitionKey app.Id) :> Task

                    logger.LogInformation("Successfully updated application {ApplicationId}", app.Id)
                    
                    let res = req.CreateResponse(HttpStatusCode.Created)
                    res.Headers.Add("Location", host + $"/applications/{app.Id}")
                    do! res.WriteAsJsonAsync({| id = app.Id |})
                    return res

                with | ex ->
                    logger.LogError(ex, "Failed to upsert application {ApplicationId}", app.Id)

                    let res = req.CreateResponse(HttpStatusCode.InternalServerError)
                    do! res.WriteAsJsonAsync({| message = "Unexpectedly failed to save application" |})
                    return res
    }
