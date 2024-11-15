namespace Modkit.Gateway.Configuration

type DiscordOptions () =
    static member Key = "Discord"

    member val BotToken: string

