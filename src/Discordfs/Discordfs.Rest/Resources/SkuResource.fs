namespace Discordfs.Rest.Resources

open Discordfs.Rest.Common
open Discordfs.Types
open System.Threading.Tasks

type ISkuResource =
    abstract member ListSkus:
        applicationId: string ->
        Task<Sku list>

type SkuResource (httpClientFactory, token) =
    interface ISkuResource with
        member _.ListSkus applicationId =
            req {
                get $"applications/{applicationId}/skus"
                bot token
            }
            |> Http.send httpClientFactory
            |> Task.mapT Http.toJson
