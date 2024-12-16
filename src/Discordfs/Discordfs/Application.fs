namespace Discordfs

open Discordfs.Rest

module Application =
    let getActivityInstance applicationId instanceId client = task {
        let! res = Rest.getApplicationActivityInstance applicationId instanceId client
        return DiscordResponse.toOption res
    }
    