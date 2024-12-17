namespace Discordfs

open Discordfs.Rest
open Discordfs.Types

module Application =
    [<Literal>]
    let MAX_ROLE_CONNNECTION_METADATA = 5

    let getActivityInstance applicationId instanceId client = task {
        let! res = Rest.getApplicationActivityInstance applicationId instanceId client
        return DiscordResponse.toOption res
    }
    
    let getRoleConnectionMetadata applicationId client = task {
        let! res = Rest.getApplicationRoleConnectionMetadataRecords applicationId client
        return DiscordResponse.toOption res
    }

    let updateRoleConnectionMetadata applicationId (metadata: ApplicationRoleConnectionMetadata list) client = task {
        let payload = UpdateApplicationRoleConnectionMetadataRecordsPayload(metadata)

        let! res = Rest.updateApplicationRoleConnectionMetadataRecords applicationId payload client
        return DiscordResponse.toOption res
    }
