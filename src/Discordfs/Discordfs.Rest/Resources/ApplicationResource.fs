namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Threading.Tasks

type GetCurrentApplicationResponse =
    | Ok of Application
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type EditCurrentApplicationPayload (
    ?custom_install_url:               string,
    ?description:                      string,
    ?role_connection_verification_url: string,
    ?install_params:                   OAuth2InstallParams,
    ?integration_types_config:         IDictionary<ApplicationIntegrationType, ApplicationIntegrationTypeConfiguration>,
    ?flags:                            int,
    ?icon:                             string option,
    ?cover_image:                      string option,
    ?interactions_endpoint_url:        string,
    ?tags:                             string list
) =
    inherit Payload() with
        override _.Content = json {
            optional "custom_install_url" custom_install_url
            optional "description" description
            optional "role_connection_verification_url" role_connection_verification_url
            optional "install_params" install_params
            optional "integration_types_config" integration_types_config
            optional "flags" flags
            optional "icon" icon
            optional "cover_image" cover_image
            optional "interactions_endpoint_url" interactions_endpoint_url
            optional "tags" tags
        }

type EditCurrentApplicationResponse =
    | Ok of Application
    | BadRequest of ErrorResponse
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

type GetApplicationActivityInstanceResponse =
    | Ok of ActivityInstance
    | NotFound of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other of HttpStatusCode

module Application =
    let getCurrentApplication
        botToken
        (httpClient: HttpClient) =
            req {
                get "applications/@me"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetCurrentApplicationResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetCurrentApplicationResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetCurrentApplicationResponse.TooManyRequests (Http.toJson res)
                | status -> return GetCurrentApplicationResponse.Other status
            })

    let editCurrentApplication
        (content: EditCurrentApplicationPayload)
        botToken
        (httpClient: HttpClient) =
            req {
                patch "applications/@me"
                bot botToken
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map EditCurrentApplicationResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map EditCurrentApplicationResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map EditCurrentApplicationResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map EditCurrentApplicationResponse.TooManyRequests (Http.toJson res)
                | status -> return EditCurrentApplicationResponse.Other status
            })

    let getApplicationActivityInstance
        (applicationId: string)
        (instanceId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                patch $"applications/{applicationId}/activity-instances/{instanceId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetApplicationActivityInstanceResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetApplicationActivityInstanceResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetApplicationActivityInstanceResponse.TooManyRequests (Http.toJson res)
                | status -> return GetApplicationActivityInstanceResponse.Other status
            })
