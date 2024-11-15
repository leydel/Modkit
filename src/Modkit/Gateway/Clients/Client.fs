namespace Modkit.Gateway.Clients

open Azure.Messaging.ServiceBus
open Discordfs.Gateway.Clients
open Discordfs.Gateway.Types
open Discordfs.Rest
open Discordfs.Types
open Modkit.Gateway.Configuration
open Modkit.Gateway.Factories
open Microsoft.Extensions.Options
open System.Net.Http
open System.Text.Json
open System.Threading.Tasks

type Client (
    httpClientFactory: IHttpClientFactory,
    serviceBusClientFactory: IServiceBusClientFactory,
    discordOptions: IOptions<DiscordOptions>,
    serviceBusOptions: IOptions<ServiceBusOptions>
) =    
    let handler =
        match serviceBusOptions.Value.Enabled with
        | false ->
            fun _ -> Task.FromResult ()
        | true ->
            let client = serviceBusClientFactory.CreateClient serviceBusOptions.Value.ConnectionString
            let sender = client.CreateSender serviceBusOptions.Value.GatewayEventQueueName

            fun (event: GatewayReceiveEvent) -> task {
                let json = Json.serializeF event
                do! sender.SendMessageAsync <| ServiceBusMessage json
            }

    let identify = IdentifySendEvent.build(
        Token = discordOptions.Value.BotToken,
        Intents = GatewayIntent.ALL,
        Properties = ConnectionProperties.build(),
        Shard = (0, 1),
        Presence = UpdatePresenceSendEvent.build(
            Status = StatusType.ONLINE,
            Activities = [
                Activity.build(
                    Name = "Modkit",
                    Type = ActivityType.CUSTOM,
                    State = "🛠️ modkit.org | 1 server" // TODO: Get actual server count
                )
            ] // TODO: Move to READY event received by API and send presence update back to gateway
        )
    )

    member _.StartAsync () = task {
        let httpClient = httpClientFactory.CreateClient()

        let! gatewayUrl =
            httpClient |> Rest.getGateway "10" GatewayEncoding.JSON None
            ?> Result.map _.Data.Url
            ?> Result.toOption

        try
            let gatewayClient: IGatewayClient = GatewayClient()

            match gatewayUrl with
            | Some url -> do! gatewayClient.Connect url identify handler
            | None -> failwith "Unexpectedly unable to get gateway URL"
                
        with | exn ->
            System.Console.WriteLine exn
            !System.Console.ReadKey()
    }
