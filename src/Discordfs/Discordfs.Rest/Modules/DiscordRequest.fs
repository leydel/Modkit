module Discordfs.Rest.Modules.DiscordRequest

open System.Net.Http

let withAuditLogReason (auditLogReason: string option) (req: HttpRequestMessage) =
    match auditLogReason with
    | Some a -> req.Headers.Add("X-Audit-Log-Reason", a); req
    | None -> req
