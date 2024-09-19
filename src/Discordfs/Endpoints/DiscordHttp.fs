namespace Modkit.Discordfs.Endpoints

type DiscordHttpApplicationIdCommands (httpClientFactory, token, applicationId) =
    member _.CreateGlobalApplicationCommand payload =
        (CreateGlobalApplicationCommand(httpClientFactory, token) :> ICreateGlobalApplicationCommand).Run applicationId payload

type DiscordHttpApplicationId (httpClientFactory, token, applicationId) =
    member val Commands = DiscordHttpApplicationIdCommands(httpClientFactory, token, applicationId)
    
type DiscordHttpApplications (httpClientFactory, token) =
    member val Todo = ()

type DiscordHttp (http, token) =
    member val Applications = DiscordHttpApplications(http, token)
    member _.Application (applicationId: string) = DiscordHttpApplicationId(http, token, applicationId)

    member _.ExampleUsage (api: DiscordHttp) =
        let payload = obj() :?> CreateGlobalApplicationCommandPayload
        let a = api.Application("1223512").Commands.CreateGlobalApplicationCommand(payload)
        let b = api.Applications.Todo
        ()
