module Discordfs.Rest.Rest

open Discordfs.Rest.Common
open Discordfs.Types
open System.Net.Http

// ----- Interaction -----

let createInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (withResponse: bool option)
    (content: CreateInteractionResponsePayload<'a>)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"interactions/{interactionId}/{interactionToken}/callback"
            bot botToken
            query "with_response" (withResponse >>. _.ToString())
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asOptionalJson<InteractionCallbackResponse>
            
let getOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"webhooks/{interactionId}/{interactionToken}/messages/@original"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
let editOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    (content: EditOriginalInteractionResponsePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"webhooks/{interactionId}/{interactionToken}/messages/@original"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
let deleteOriginalInteractionResponse
    (interactionId: string)
    (interactionToken: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"webhooks/{interactionId}/{interactionToken}/messages/@original"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty
            
let createFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (content: CreateFollowUpMessagePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"webhooks/{applicationId}/{interactionToken}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty
            
let getFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
let editFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    (content: EditFollowUpMessagePayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<Message>
            
let deleteFollowUpMessage
    (applicationId: string)
    (interactionToken: string)
    (messageId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"webhooks/{applicationId}/{interactionToken}/messages/{messageId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty

// ----- Application Command -----

let getGlobalApplicationCommands
    (applicationId: string)
    (withLocalizations: bool option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/commands"
            bot botToken
            query "with_localizations" (withLocalizations >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let createGlobalApplicationCommand
    (applicationId: string)
    (content: CreateGlobalApplicationCommandPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"applications/{applicationId}/commands"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let getGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/commands/{commandId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let editGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    (content: EditGlobalApplicationCommandPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"applications/{applicationId}/commands/{commandId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let deleteGlobalApplicationCommand
    (applicationId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"applications/{applicationId}/commands/{commandId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty
            
let bulkOverwriteGlobalApplicationCommands
    (applicationId: string)
    (content: BulkOverwriteGlobalApplicationCommandsPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"applications/{applicationId}/commands"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let getGuildApplicationCommands
    (applicationId: string)
    (guildId: string)
    (withLocalizations: bool option)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands"
            bot botToken
            query "with_localizations" (withLocalizations >>. _.ToString())
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let createGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (content: CreateGuildApplicationCommandPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            post $"applications/{applicationId}/guilds/{guildId}/commands"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let getGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let editGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (content: EditGuildApplicationCommandPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            patch $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand>
            
let deleteGuildApplicationCommand
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            delete $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asEmpty
            
let bulkOverwriteGuildApplicationCommands
    (applicationId: string)
    (guildId: string)
    (content: BulkOverwriteGlobalApplicationCommandsPayload)
    botToken
    (httpClient: HttpClient) =
        req {
            put $"applications/{applicationId}/guilds/{guildId}/commands"
            bot botToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<ApplicationCommand list>
            
let getGuildApplicationCommandsPermissions
    (applicationId: string)
    (guildId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/permissions"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildApplicationCommandPermissions list>
            
let getGuildApplicationCommandPermissions
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    botToken
    (httpClient: HttpClient) =
        req {
            get $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
            bot botToken
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildApplicationCommandPermissions>
            
let editApplicationCommandPermissions
    (applicationId: string)
    (guildId: string)
    (commandId: string)
    (content: EditApplicationCommandPermissions)
    oauthToken
    (httpClient: HttpClient) =
        req {
            put $"applications/{applicationId}/guilds/{guildId}/commands/{commandId}/permissions"
            oauth oauthToken
            payload content
        }
        |> httpClient.SendAsync
        ?>> DiscordResponse.asJson<GuildApplicationCommandPermissions>

// ----- Application -----

// TODO: Implement

// ----- Audit Log -----

// TODO: Implement

// ----- Auto Moderation -----

// TODO: Implement

// ----- Channel -----

// TODO: Implement

// ----- Emoji -----

// TODO: Implement

// ----- Entitlement -----

// TODO: Implement

// ----- Guild -----

// TODO: Implement

// ----- Guild Scheduled Event -----

// TODO: Implement

// ----- Guild Template -----

// TODO: Implement

// ----- Invite -----

// TODO: Implement

// ----- Message -----

// TODO: Implement

// ----- Poll -----

// TODO: Implement

// ----- Role Connection -----

// TODO: Implement

// ----- Sku -----

// TODO: Implement

// ----- Soundboard -----

// TODO: Implement

// ----- Stage Instance -----

// TODO: Implement

// ----- Sticker -----

// TODO: Implement

// ----- Subscription -----

// TODO: Implement

// ----- User -----

// TODO: Implement

// ----- Voice -----

// TODO: Implement

// ----- Webhook -----

// TODO: Implement

// ----- Gateway -----

// TODO: Implement

// ----- OAuth2 -----

// TODO: Implement
