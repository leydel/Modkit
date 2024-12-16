namespace Discordfs

open Discordfs.Rest

module ActivityInstance =
    let get applicationId instanceId client = task {
        let! res = Rest.getApplicationActivityInstance applicationId instanceId client
        return DiscordResponse.toOption res
    }
    