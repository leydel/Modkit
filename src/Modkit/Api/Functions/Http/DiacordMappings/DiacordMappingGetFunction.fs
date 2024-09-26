namespace Modkit.Api.Functions

open Discordfs.Types.Utils
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Api.Actions
open Modkit.Api.DTOs
open System.Net
open System.Net.Http

type DiacordMappingGetFunction (diacordMappingGetAction: IDiacordMappingGetAction) =
    [<Function(nameof DiacordMappingGetFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "diacord/mapping/{guildId}")>] req: HttpRequestMessage,
        log: ILogger,
        guildId: string
    ) = task {
        let! mapping = diacordMappingGetAction.run guildId

        match mapping with
        | Error _ ->
            log.LogInformation($"Failed to call diacord mapping get action for guild {guildId}")
            return new HttpResponseMessage(HttpStatusCode.NotFound)
        | Ok mapping ->
            let payload = DiacordMappingDto.from mapping

            let res = new HttpResponseMessage(HttpStatusCode.OK)
            res.Content <- new StringContent(FsJson.serialize payload)
            res.Headers.Add("Content-Type", "application/json")

            log.LogInformation($"Successfully called diacord mapping get function for guild {guildId}")
            return res
    }
