namespace Modkit.Api.Functions

open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Api.Actions
open System.Net
open System.Net.Http

type DiacordMappingDeleteFunction (diacordMappingDeleteAction: IDiacordMappingDeleteAction) =
    [<Function(nameof DiacordMappingDeleteFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "diacord/mapping/{guildId}")>] req: HttpRequestMessage,
        log: ILogger,
        guildId: string
    ) = task {
        let! action = diacordMappingDeleteAction.run guildId

        match action with
        | Error _ ->
            log.LogInformation($"Failed to call diacord mapping delete action for guild {guildId}")
            return new HttpResponseMessage(HttpStatusCode.NotFound)
        | Ok _ ->
            log.LogInformation($"Successfully called diacord mapping delete function for guild {guildId}")
            return new HttpResponseMessage(HttpStatusCode.NoContent)
    }
