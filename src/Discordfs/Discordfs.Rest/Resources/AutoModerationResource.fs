namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Rest.Types
open Discordfs.Types
open System.Net
open System.Net.Http

type GetAutoModerationRuleResponse =
    | Ok              of AutoModerationRule
    | NotFound        of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other           of HttpStatusCode

type CreateAutoModerationRulePayload (
    name:              string,
    event_type:        AutoModerationEventType,
    trigger_type:      AutoModerationTriggerType,
    actions:           AutoModerationAction list,
    ?trigger_metadata: AutoModerationTriggerMetadata,
    ?enabled:          bool,
    ?exempt_roles:     string list,
    ?exempt_channels:  string list
) =
    inherit Payload() with
        override _.Content = json {
            required "name" name
            required "event_type" event_type
            required "trigger_type" trigger_type
            optional "trigger_metadata" trigger_metadata
            required "actions" actions
            optional "enabled" enabled
            optional "exempt_roles" exempt_roles
            optional "exempt_channels" exempt_channels
        }

type CreateAutoModerationRuleResponse =
    | Ok              of AutoModerationRule
    | Created         of AutoModerationRule
    | BadRequest      of ErrorResponse
    | NotFound        of ErrorResponse
    | Conflict        of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other           of HttpStatusCode

type ModifyAutoModerationRulePayload (
    ?name:             string,
    ?event_type:       AutoModerationEventType,
    ?trigger_metadata: AutoModerationTriggerMetadata,
    ?actions:          AutoModerationAction list,
    ?enabled:          bool,
    ?exempt_roles:     string list,
    ?exempt_channels:  string list
) =
    inherit Payload() with
        override _.Content = json {
            optional "name" name
            optional "event_type" event_type
            optional "trigger_metadata" trigger_metadata
            optional "actions" actions
            optional "enabled" enabled
            optional "exempt_roles" exempt_roles
            optional "exempt_channels" exempt_channels
        }

type ModifyAutoModerationRuleResponse =
    | Ok              of AutoModerationRule
    | BadRequest      of ErrorResponse
    | NotFound        of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other           of HttpStatusCode

type DeleteAutoModerationRuleResponse =
    | NoContent
    | NotFound        of ErrorResponse
    | TooManyRequests of RateLimitResponse
    | Other           of HttpStatusCode

module AutoModeration =
    let getAutoModerationRule
        (guildId: string)
        (autoModerationRuleId: string)
        botToken
        (httpClient: HttpClient) =
            req {
                get $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
                bot botToken
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map GetAutoModerationRuleResponse.Ok (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map GetAutoModerationRuleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map GetAutoModerationRuleResponse.TooManyRequests (Http.toJson res)
                | status -> return GetAutoModerationRuleResponse.Other status
            })

    let createAutoModerationRule
        (guildId: string)
        (auditLogReason: string option)
        (content: CreateAutoModerationRulePayload)
        botToken
        (httpClient: HttpClient) =
            req {
                post $"guilds/{guildId}/auto-moderation/rules"
                bot botToken
                audit auditLogReason
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map CreateAutoModerationRuleResponse.Ok (Http.toJson res)
                | HttpStatusCode.Created -> return! Task.map CreateAutoModerationRuleResponse.Created (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map CreateAutoModerationRuleResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map CreateAutoModerationRuleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.Conflict -> return! Task.map CreateAutoModerationRuleResponse.Conflict (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map CreateAutoModerationRuleResponse.TooManyRequests (Http.toJson res)
                | status -> return CreateAutoModerationRuleResponse.Other status
            })

    let modifyAutoModerationRule
        (guildId: string)
        (autoModerationRuleId: string)
        (auditLogReason: string option)
        (content: CreateAutoModerationRulePayload)
        botToken
        (httpClient: HttpClient) =
            req {
                patch $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
                bot botToken
                audit auditLogReason
                payload content
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.OK -> return! Task.map ModifyAutoModerationRuleResponse.Ok (Http.toJson res)
                | HttpStatusCode.BadRequest -> return! Task.map ModifyAutoModerationRuleResponse.BadRequest (Http.toJson res)
                | HttpStatusCode.NotFound -> return! Task.map ModifyAutoModerationRuleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map ModifyAutoModerationRuleResponse.TooManyRequests (Http.toJson res)
                | status -> return ModifyAutoModerationRuleResponse.Other status
            })

    let deleteAutoModerationRule
        (guildId: string)
        (autoModerationRuleId: string)
        (auditLogReason: string option)
        botToken
        (httpClient: HttpClient) =
            req {
                delete $"guilds/{guildId}/auto-moderation/rules/{autoModerationRuleId}"
                bot botToken
                audit auditLogReason
            }
            |> httpClient.SendAsync
            |> Task.mapT (fun res -> task {
                match res.StatusCode with
                | HttpStatusCode.NoContent -> return DeleteAutoModerationRuleResponse.NoContent
                | HttpStatusCode.NotFound -> return! Task.map DeleteAutoModerationRuleResponse.NotFound (Http.toJson res)
                | HttpStatusCode.TooManyRequests -> return! Task.map DeleteAutoModerationRuleResponse.TooManyRequests (Http.toJson res)
                | status -> return DeleteAutoModerationRuleResponse.Other status
            })
