namespace Discordfs

open Discordfs.Rest

type GroupDmRecipient = {
    Id: string
    AccessToken: string
    Nick: string
}

module User =
    let get userId client = task {
        let! res = Rest.getUser userId client
        return DiscordResponse.toOption res
    }

    let createDm recipientId client = task {
        let payload = CreateDmPayload(recipientId)

        let! res = Rest.createDm payload client
        return DiscordResponse.toOption res
    }

    let createGroupDm (recipients: GroupDmRecipient list) client = task {
        let accessTokens = recipients |> List.map _.AccessToken
        let nicks = recipients |> List.map (fun r -> (r.Id, r.Nick)) |> dict

        let payload = CreateGroupDmPayload(accessTokens, nicks)
        
        let! res = Rest.createGroupDm payload client
        return DiscordResponse.toOption res
    }
