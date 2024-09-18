namespace Modkit.Api.Functions

open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Modkit.Api.Actions
open Modkit.Api.DTOs
open Modkit.Discordfs.Utils
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Text.Json.Serialization

type DiacordMappingPutFunctionPayload = {
    [<JsonName "mappings">] Mappings: IDictionary<string, string>
}

type DiacordMappingPutFunction (diacordMappingPutAction: IDiacordMappingPutAction) =
    [<Function(nameof DiacordMappingPutFunction)>]
    member _.Run (
        [<HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "diacord/mapping/{guildId}")>] req: HttpRequestMessage,
        log: ILogger,
        guildId: string
    ) = task {
        let! json = req.Content.ReadAsStringAsync()
        let body = FsJson.deserialize<DiacordMappingPutFunctionPayload> json

        let! mapping = diacordMappingPutAction.run guildId body.Mappings

        match mapping with
        | Error _ ->
            log.LogInformation($"Failed to call diacord mapping put action for guild {guildId}")
            return new HttpResponseMessage(HttpStatusCode.NotFound)
        | Ok mapping ->
            let payload = DiacordMappingDto.from mapping

            let res = new HttpResponseMessage(HttpStatusCode.OK)
            res.Content <- new StringContent(FsJson.serialize payload)
            res.Headers.Add("Content-Type", "application/json")

            log.LogInformation($"Successfully called diacord mapping put function for guild {guildId}")
            return res
    }
