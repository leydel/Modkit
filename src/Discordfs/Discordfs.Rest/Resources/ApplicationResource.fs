namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Collections.Generic
open System.Threading.Tasks

type EditCurrentApplication (
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
    inherit Payload(Json) with
        override _.Serialize () = json {
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

type IApplicationResource =
    // https://discord.com/developers/docs/resources/application#get-current-application
    abstract member GetCurrentApplication:
        unit ->
        Task<Application>

    // https://discord.com/developers/docs/resources/application#edit-current-application
    abstract member EditCurrentApplication:
        content: EditCurrentApplication ->
        Task<Application>

    // https://discord.com/developers/docs/resources/application#get-application-activity-instance
    abstract member GetApplicationActivityInstance:
        applicationId: string ->
        instanceId: string ->
        Task<ActivityInstance>

type ApplicationResource (httpClientFactory, token) =
    interface IApplicationResource with
        member _.GetCurrentApplication () =
            req {
                get "applications/@me"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.EditCurrentApplication content =
            req {
                patch "applications/@me"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetApplicationActivityInstance applicationId instanceId =
            req {
                patch $"applications/{applicationId}/activity-instances/{instanceId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
