namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Threading.Tasks

type CreateTestEntitlement (
    sku_id:     string,
    owner_id:   string,
    owner_type: EntitlementOwnerType
) =
    inherit Payload() with
        override _.Content = json {
            required "sku_id" sku_id
            required "owner_id" owner_id
            required "owner_type" owner_type
        }

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
        content: CreateTestEntitlement ->
        Task<Entitlement> // TODO: Partial (does not contain subscription_id, starts_at, ends_at)

    // https://discord.com/developers/docs/resources/entitlement#delete-test-entitlement
    abstract member DeleteTestEntitlement:
        applicationId: string ->
        entitlementId: string ->
        Task<unit>

type EntitlementResource (httpClientFactory, token) =
    interface IEntitlementResource with
        member _.ListEntitlements applicationId userId skuIds before after limit guildId excludeEnded =
            req {
                get $"applications/{applicationId}/entitlements"
                bot token
                query "user_id" userId
                query "sku_ids" (skuIds >>. String.concat ",")
                query "before" before
                query "after" after
                query "limit" (limit >>. _.ToString())
                query "guild_id" guildId
                query "exclude_ended" (excludeEnded >>. _.ToString())
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.ConsumeEntitlement applicationId entitlementId =
            req {
                post $"applications/{applicationId}/entitlements/{entitlementId}/consume"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait

        member _.CreateTestEntitlement applicationId content =
            req {
                post $"applications/{applicationId}/entitlements"
                bot token
                payload content
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.DeleteTestEntitlement applicationId entitlementId =
            req {
                delete $"applications/{applicationId}/entitlements/{entitlementId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.wait
            