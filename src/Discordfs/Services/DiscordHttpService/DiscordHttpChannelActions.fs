namespace Modkit.Discordfs.Services

open Modkit.Discordfs.Common
open Modkit.Discordfs.Types
open System
open System.Net.Http
open System.Threading.Tasks

// https://discord.com/developers/docs/resources/channel
type IDiscordHttpChannelActions =
    // https://discord.com/developers/docs/resources/channel#get-channel
    abstract member GetChannel:
        channelId: string ->
        Task<Channel>
    
    // https://discord.com/developers/docs/resources/channel#modify-channel
    abstract member ModifyChannel:
        unit -> unit // TODO: Implement

    // https://discord.com/developers/docs/resources/channel#deleteclose-channel
    abstract member DeleteChannel:
        channelId: string ->
        auditLogReason: string option ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/channel#edit-channel-permissions
    abstract member EditChannelPermissions:
        channelId: string ->
        overwriteId: string ->
        auditLogReason: string option ->
        payload: EditChannelPermissions ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#get-channel-invites
    abstract member GetChannelInvites:
        channelId: string ->
        Task<Invite list> // TODO: Figure out how to combine the records `Invite` and `InviteMetadata` for this return type

    // https://discord.com/developers/docs/resources/channel#create-channel-invite
    abstract member CreateChannelInvite:
        channelId: string ->
        auditLogReason: string option ->
        payload: CreateChannelInvite ->
        Task<Invite>

    // https://discord.com/developers/docs/resources/channel#delete-channel-permission
    abstract member DeleteChannelPermission:
        channelId: string ->
        overwriteId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#follow-announcement-channel
    abstract member FollowAnnouncementChannel:
        channelId: string ->
        auditLogReason: string option ->
        payload: FollowAnnouncementChannel ->
        Task<FollowedChannel>

    // https://discord.com/developers/docs/resources/channel#trigger-typing-indicator
    abstract member TriggerTypingIndicator:
        channelId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#get-pinned-messages
    abstract member GetPinnedMessages:
        channelId: string ->
        Task<Message list>

    // https://discord.com/developers/docs/resources/channel#pin-message
    abstract member PinMessage:
        channelId: string ->
        messageId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#unpin-message
    abstract member UnpinMessage:
        channelId: string ->
        messageId: string ->
        auditLogReason: string option ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#group-dm-add-recipient
    abstract member GroupDmAddRecipient:
        channelId: string ->
        recipientId: string ->
        payload: GroupDmAddRecipient ->
        Task<unit> // TODO: Test return type, currently undocumented

    // https://discord.com/developers/docs/resources/channel#group-dm-remove-recipient
    abstract member GroupDmRemoveRecipient:
        channelId: string ->
        recipientId: string ->
        Task<unit> // TODO: Test return type, currently undocumented

    // https://discord.com/developers/docs/resources/channel#start-thread-from-message
    abstract member StartThreadFromMessage:
        channelId: string ->
        messageId: string ->
        auditLogReason: string option ->
        payload: StartThreadFromMessage ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/channel#start-thread-without-message
    abstract member StartThreadWithoutMessage:
        channelId: string ->
        auditLogReason: string option ->
        payload: StartThreadWithoutMessage ->
        Task<Channel>

    // https://discord.com/developers/docs/resources/channel#start-thread-in-forum-or-media-channel
    abstract member StartThreadInForumOrMediaChannel:
        unit -> unit // TODO: Implement

    // https://discord.com/developers/docs/resources/channel#join-thread
    abstract member JoinThread:
        channelId: string ->
        // TODO: Check if oauth token can be used here
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#add-thread-member
    abstract member AddThreadMember:
        channelId: string ->
        userId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#leave-thread
    abstract member LeaveThread:
        channelId: string ->
        // TODO: Check if oauth token can be used here
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#remove-thread-member
    abstract member RemoveThreadMember:
        channelId: string ->
        userId: string ->
        Task<unit>

    // https://discord.com/developers/docs/resources/channel#get-thread-member
    abstract member GetThreadMember:
        channelId: string ->
        userId: string ->
        withMember: bool ->
        Task<ThreadMember>

    // https://discord.com/developers/docs/resources/channel#list-thread-members
    abstract member ListThreadMembers:
        unit -> unit // TODO: Implement

    // https://discord.com/developers/docs/resources/channel#list-public-archived-threads
    abstract member ListPublicArchivedThreads:
        channelId: string ->
        before: DateTime option ->
        limit: int option ->
        Task<ListPublicArchivedThreadsResponse>

    // https://discord.com/developers/docs/resources/channel#list-private-archived-threads
    abstract member ListPrivateArchivedThreads:
        channelId: string ->
        before: DateTime option ->
        limit: int option ->
        Task<ListPrivateArchivedThreadsResponse>

    // https://discord.com/developers/docs/resources/channel#list-joined-private-archived-threads
    abstract member ListJoinedPrivateArchivedThreads:
        channelId: string ->
        // TODO: Check if oauth token can be used here
        before: DateTime option ->
        limit: int option ->
        Task<ListJoinedPrivateArchivedThreadsResponse>

type DiscordHttpChannelActions (httpClientFactory: IHttpClientFactory, token: string) =
    interface IDiscordHttpChannelActions with
        member _.GetChannel channelId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"channels/{channelId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.ModifyChannel () =
            raise (NotImplementedException())

        member _.DeleteChannel channelId auditLogReason =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"channels/{channelId}"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.send httpClientFactory
            |> Res.body

        member _.EditChannelPermissions channelId overwriteId auditLogReason payload =
            Req.create
                HttpMethod.Put
                Constants.DISCORD_API_URL
                $"channels/{channelId}/permissions/{overwriteId}"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.GetChannelInvites channelId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"channels/{channelId}/invites"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.CreateChannelInvite channelId auditLogReason payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"channels/{channelId}/invites"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.DeleteChannelPermission channelId overwriteId auditLogReason =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"channels/{channelId}/permissions/{overwriteId}"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.FollowAnnouncementChannel channelId auditLogReason payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"channels/{channelId}/followers"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.TriggerTypingIndicator channelId =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"channels/{channelId}/typing"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.GetPinnedMessages channelId =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"channels/{channelId}/pins"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.body

        member _.PinMessage channelId messageId auditLogReason =
            Req.create
                HttpMethod.Put
                Constants.DISCORD_API_URL
                $"channels/{channelId}/pins/{messageId}"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.UnpinMessage channelId messageId auditLogReason =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"channels/{channelId}/pins/{messageId}"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.GroupDmAddRecipient channelId userId payload =
            Req.create
                HttpMethod.Put
                Constants.DISCORD_API_URL
                $"channels/{channelId}/recipients/{userId}"
            |> Req.bot token
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.GroupDmRemoveRecipient channelId userId =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"channels/{channelId}/recipients/{userId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.StartThreadFromMessage channelId messageId auditLogReason payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"channels/{channelId}/messages/{messageId}/threads"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.StartThreadWithoutMessage channelId auditLogReason payload =
            Req.create
                HttpMethod.Post
                Constants.DISCORD_API_URL
                $"channels/{channelId}/threads"
            |> Req.bot token
            |> Req.audit auditLogReason
            |> Req.body payload
            |> Req.send httpClientFactory
            |> Res.body

        member _.StartThreadInForumOrMediaChannel () =
            raise (NotImplementedException())

        member _.JoinThread channelId =
            Req.create
                HttpMethod.Put
                Constants.DISCORD_API_URL
                $"channels/{channelId}/thread-members/@me"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.AddThreadMember channelId userId =
            Req.create
                HttpMethod.Put
                Constants.DISCORD_API_URL
                $"channels/{channelId}/thread-members/{userId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.LeaveThread channelId =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"channels/{channelId}/thread-members/@me"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore
            
        member _.RemoveThreadMember channelId userId =
            Req.create
                HttpMethod.Delete
                Constants.DISCORD_API_URL
                $"channels/{channelId}/thread-members/{userId}"
            |> Req.bot token
            |> Req.send httpClientFactory
            |> Res.ignore

        member _.GetThreadMember channelId userId withMember =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"channels/{channelId}/thread-members/{userId}"
            |> Req.bot token
            |> Req.queryOpt "with_member" (if withMember then Some "true" else None)
            |> Req.send httpClientFactory
            |> Res.body

        member _.ListThreadMembers () =
            raise (NotImplementedException())

        member _.ListPublicArchivedThreads channelId before limit =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"channels/{channelId}/threads/archived/public"
            |> Req.bot token
            |> Req.queryOpt "before" (match before with Some b -> Some (b.ToString()) | None -> None)
            |> Req.queryOpt "limit" (match limit with Some l -> Some (l.ToString()) | None -> None)
            |> Req.send httpClientFactory
            |> Res.body

        member _.ListPrivateArchivedThreads channelId before limit =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"channels/{channelId}/threads/archived/private"
            |> Req.bot token
            |> Req.queryOpt "before" (match before with Some b -> Some (b.ToString()) | None -> None)
            |> Req.queryOpt "limit" (match limit with Some l -> Some (l.ToString()) | None -> None)
            |> Req.send httpClientFactory
            |> Res.body

        member _.ListJoinedPrivateArchivedThreads channelId before limit =
            Req.create
                HttpMethod.Get
                Constants.DISCORD_API_URL
                $"channels/{channelId}/users/@me/threads/archived/private"
            |> Req.bot token
            |> Req.queryOpt "before" (match before with Some b -> Some (b.ToString()) | None -> None)
            |> Req.queryOpt "limit" (match limit with Some l -> Some (l.ToString()) | None -> None)
            |> Req.send httpClientFactory
            |> Res.body
