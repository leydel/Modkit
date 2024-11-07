namespace Discordfs.Types

open System
open System.Text.Json.Serialization

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-entity-metadata
type EntityMetadata = {
    [<JsonPropertyName "location">] Location: string option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object-guild-scheduled-event-recurrence-rule-nweekday-structure
type RecurrenceRuleNWeekday = {
    [<JsonPropertyName "n">] N: int
    [<JsonPropertyName "day">] Day: RecurrenceRuleWeekdayType
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-recurrence-rule-object
type RecurrenceRule = {
    Start: string
    End: string option
    Frequency: RecurrenceRuleFrequencyType
    Interval: int
    ByWeekday: RecurrenceRuleWeekdayType list option
    ByWeekend: RecurrenceRuleNWeekday list option
    ByMonth: RecurrenceRuleMonthType list option
    ByMonthDay: int list option
    ByYearDay: int list option
    Count: int option

}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-object-guild-scheduled-event-structure
type GuildScheduledEvent = {
    [<JsonPropertyName "id">] Id: string
    [<JsonPropertyName "id">] GuildId: string
    [<JsonPropertyName "id">] ChannelId: string option
    [<JsonPropertyName "id">] CreatorId: string option
    [<JsonPropertyName "id">] Name: string
    [<JsonPropertyName "id">] Description: string option
    [<JsonPropertyName "id">] ScheduledStartTime: DateTime option
    [<JsonPropertyName "id">] ScheduledEndTime: DateTime option
    [<JsonPropertyName "id">] PrivacyLevel: PrivacyLevelType
    [<JsonPropertyName "id">] EventStatus: EventStatusType
    [<JsonPropertyName "id">] EntityType: ScheduledEntityType
    [<JsonPropertyName "id">] EntityId: string option
    [<JsonPropertyName "id">] EntityMetadata: EntityMetadata option
    [<JsonPropertyName "id">] Creator: User option
    [<JsonPropertyName "id">] UserCount: int option
    [<JsonPropertyName "id">] Image: string option
    [<JsonPropertyName "id">] RecurrenceRule: RecurrenceRule option
}

// https://discord.com/developers/docs/resources/guild-scheduled-event#guild-scheduled-event-user-object-guild-scheduled-event-user-structure
type GuildScheduledEventUser = {
    [<JsonPropertyName "guild_scheduled_event_id">] GuildScheduledEventId: string
    [<JsonPropertyName "user">] User: User
    [<JsonPropertyName "member">] Member: GuildMember option
}
