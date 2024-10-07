namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Threading.Tasks

type ISubscriptionResource =
    // https://discord.com/developers/docs/resources/subscription#list-sku-subscriptions
    abstract member ListSkuSubscriptions:
        skuId: string ->
        before: string option ->
        after: string option ->
        limit: int option ->
        userId: string option ->
        Task<Subscription list>

    // https://discord.com/developers/docs/resources/subscription#get-sku-subscription
    abstract member GetSkuSubscription:
        skuId: string ->
        subscriptionId: string ->
        Task<Subscription>

type SubscriptionResource (httpClientFactory, token) =
    interface ISubscriptionResource with
        member _.ListSkuSubscriptions skuId before after limit userId =
            req {
                get $"skus/{skuId}/subscriptions"
                bot token
                query "before" before
                query "after" after
                query "limit" (limit >>. _.ToString())
                query "userId" userId
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson

        member _.GetSkuSubscription skuId subscriptionId =
            req {
                get $"skus/{skuId}/subscriptions/{subscriptionId}"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
