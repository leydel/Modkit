namespace Modkit.Discordfs.Resources

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System.Net.Http
open System.Threading.Tasks

type IEntitlementResource =
    // https://discord.com/developers/docs/resources/entitlement#list-entitlements
    abstract member ListEntitlements:
        applicationId: string ->
        userId: string option ->
        skuIds: string list option ->
        before: string option ->
        after: string option ->
        limit: int option ->
        guildId: string option ->
        excludeEnded: bool option ->
        Task<Entitlement list>

    // https://discord.com/developers/docs/resources/entitlement#consume-an-entitlement
    abstract member ConsumeEntitlement:
        applicationId: string ->
        entitlementId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/entitlement#create-test-entitlement
    abstract member CreateTestEntitlement:
        applicationId: string ->
        skuId: string ->
        ownerId: string ->
        ownerType: EntitlementOwnerType ->
        Task<Entitlement>

    // https://discord.com/developers/docs/resources/entitlement#delete-test-entitlement
    abstract member DeleteTestEntitlement:
        applicationId: string ->
        entitlementId: string ->
        Task<unit>

type EntitlementResource (httpClientFactory: IHttpClientFactory, token: string) =
    interface IEntitlementResource with
        member _.ListEntitlements
            applicationId userId skuIds before after limit guildId excludeEnded =
                Req.create
                    HttpMethod.Get
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/entitlements"
                |> Req.bot token
                |> Req.queryOpt "user_id" userId
                |> Req.queryOpt "sku_ids" (Option.map (String.concat ",") skuIds)
                |> Req.queryOpt "before" before
                |> Req.queryOpt "after" after
                |> Req.queryOpt "limit" (Option.map (_.ToString()) limit)
                |> Req.queryOpt "guild_id" guildId
                |> Req.queryOpt "exclude_ended" (Option.map (_.ToString()) excludeEnded)
                |> Req.send httpClientFactory
                |> Res.json

        member _.ConsumeEntitlement
            applicationId entitlementId =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/entitlements/{entitlementId}/consume"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore

        member _.CreateTestEntitlement
            applicationId skuId ownerId ownerType =
                Req.create
                    HttpMethod.Post
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/entitlements"
                |> Req.bot token
                |> Req.json (
                    Dto()
                    |> Dto.property "sku_id" skuId
                    |> Dto.property "owner_id" ownerId
                    |> Dto.property "owner_type" ownerType
                    |> Dto.json
                )
                |> Req.send httpClientFactory
                |> Res.json

        member _.DeleteTestEntitlement
            applicationId entitlementId =
                Req.create
                    HttpMethod.Delete
                    Constants.DISCORD_API_URL
                    $"applications/{applicationId}/entitlements/{entitlementId}"
                |> Req.bot token
                |> Req.send httpClientFactory
                |> Res.ignore
