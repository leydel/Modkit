namespace Discordfs.Utils

type TimestampStyle =
    | SHORT_TIME
    | LONG_TIME
    | SHORT_DATE
    | LONG_DATE
    | SHORT_DATE_TIME
    | LONG_DATE_TIME
    | RELATIVE_TIME
with
    override this.ToString () =
        match this with
        | SHORT_TIME -> "t"
        | LONG_TIME -> "T"
        | SHORT_DATE -> "d"
        | LONG_DATE -> "D"
        | SHORT_DATE_TIME -> "f"
        | LONG_DATE_TIME -> "F"
        | RELATIVE_TIME -> "R"

    /// Default timestamp style, to use instead of format without style <t:TIMESTAMP>
    static member zero () =
        TimestampStyle.SHORT_DATE_TIME

type GuildNavigationType =
    | CUSTOMIZE
    | BROWSE
    | GUIDE
    | LINKED_ROLES
    | LINKED_ROLE of roleId: string
with
    override this.ToString () =
        match this with
        | CUSTOMIZE -> "customize"
        | BROWSE -> "browse"
        | GUIDE -> "guide"
        | LINKED_ROLES -> "linked-roles"
        | LINKED_ROLE roleId -> $"linked-roles:{roleId}"

module MessageFormat =
    let user (id: string) = $"<@{id}>"

    let channel (id: string) = $"<#{id}>"

    let role (id: string) = $"<@&{id}>"

    /// Commands with subcommands and subcommand groups are separated by spaces in name. E.g.: "command subcommandgroup subcommand"
    let slashCommand (name: string) (id: string) = $"</{name}:{id}>"

    let customEmoji (name: string) (id: string) = $"<:{name}:{id}>"

    let customAnimatedEmoji (name: string) (id: string) = $"<a:{name}:{id}>"

    /// Default style can be used through `TimestampStyle.zero()`
    let timestamp (timestamp: int64) (style: TimestampStyle) = $"<t:{timestamp}:{style}>"

    let guildNavigation (id: string) (``type``: GuildNavigationType) = $"<{id}:{``type``}>"
