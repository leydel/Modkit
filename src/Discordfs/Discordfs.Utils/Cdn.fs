namespace Discordfs.Utils

// TODO: Support different file types
// TODO: Add a_ prefix when animated asset
// TODO: Allow image size query params to be specified
// TODO: Figure out other cdn query params and if/where they are necessary

module Cdn =
    let customEmoji (emojiId: string) =
        IMAGE_BASE_URL + $"emojis/{emojiId}.png"

    let guildIcon (guildId: string) (guildIconHash: string) =
        IMAGE_BASE_URL + $"icons/{guildId}/{guildIconHash}.png"

    let guildSplash (guildId: string) (guildSplashHash: string) =
        IMAGE_BASE_URL + $"splashes/{guildId}/{guildSplashHash}.png"

    let guildDiscoverySplash (guildId: string) (guildDiscoverySplashHash: string) =
        IMAGE_BASE_URL + $"discovery-splashes/{guildId}/{guildDiscoverySplashHash}.png"

    let guildBanner (guildId: string) (guildBannerHash: string) =
        IMAGE_BASE_URL + $"banners/{guildId}/{guildBannerHash}.png"

    let userBanner (userId: string) (userBannerHash: string) =
        IMAGE_BASE_URL + $"banners/{userId}/{userBannerHash}.png"

    let userDefaultAvatar (index: int) =
        IMAGE_BASE_URL + $"embed/avatars/{index}.png"

    // TODO: Make function to nicely find a user's index (User.getAvatarIndex ?)

    let userAvatar (userId: string) (userAvatarHash: string) =
        IMAGE_BASE_URL + $"avatars/{userId}/{userAvatarHash}.png"

    let guildMemberAvatar (guildId: string) (userId: string) (guildMemberAvatarHash: string) =
        IMAGE_BASE_URL + $"guilds/{guildId}/users/{userId}/avatars/{guildMemberAvatarHash}.png"

    let avatarDecoration (assetHash: string) =
        IMAGE_BASE_URL + $"avatar-decoration-presets/{assetHash}.png"

    let applicationIcon (applicationId: string) (applicationIconHash: string) =
        IMAGE_BASE_URL + $"app-icons/{applicationId}/{applicationIconHash}.png"

    let applicationCover (applicationId: string) (applicationCoverHash: string) =
        IMAGE_BASE_URL + $"app-icons/{applicationId}/{applicationCoverHash}.png"

    let applicationAsset (applicationId: string) (applicationAssetId: string) =
        IMAGE_BASE_URL + $"app-assets/{applicationId}/{applicationAssetId}.png"

    let achievementIcon (applicationId: string) (achievementId: string) (achievementIconHash: string) =
        IMAGE_BASE_URL + $"app-assets/{applicationId}/achievements/{achievementId}/icons/{achievementIconHash}.png"

    let storePageAsset (applicationId: string) (assetId: string) =
        IMAGE_BASE_URL + $"app-assets/{applicationId}/store/{assetId}.png"

    let stickerPackBanner (stickerPackBannerAssetId: string) =
        IMAGE_BASE_URL + $"app-assets/710982414301790216/store/{stickerPackBannerAssetId}.png"

    let teamIcon (teamId: string) (teamIconHash: string) =
        IMAGE_BASE_URL + $"team-icons/{teamId}/{teamIconHash}.png"

    let sticker (stickerId: string) =
        IMAGE_BASE_URL + $"stickers/{stickerId}.png" // file type must match sticker type (current implementation will fail)

    let roleIcon (roleId: string) (roleIconHash: string) =
        IMAGE_BASE_URL + $"role-icons/{roleId}/{roleIconHash}.png"

    let guildScheduledEventCover (scheduledEventId: string) (scheduledEventCoverImageHash: string) =
        IMAGE_BASE_URL + $"guild-events/{scheduledEventId}/{scheduledEventCoverImageHash}.png"

    let guildMemberBanner (guildId: string) (userId: string) (guildMemberBannerHash: string) =
        IMAGE_BASE_URL + $"guilds/{guildId}/users/{userId}/banners/{guildMemberBannerHash}.png"
