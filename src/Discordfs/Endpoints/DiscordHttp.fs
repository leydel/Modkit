namespace Modkit.Discordfs.Endpoints

type DiscordHttpApplications (?applicationId: string) =
    member _.Commands () = 0

type DiscordHttp (httpClientFactory, token) =

    member _.Applications (?applicationId) = DiscordHttpApplications(?applicationId = applicationId)
