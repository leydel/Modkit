namespace Modkit.Roles.Domain.Types

type MetadataType =
    /// The metadata value is less than or equal to this, E.g. "Low rank only (value = threshold)" 
    | RANGE_MAX = 1

    /// The metadata value is greater than or equal to this, E.g. "High rank only (value = threshold)"
    | RANGE_MIN = 2

    /// The metadata value is this option of a selection of options, E.g. Mutually exclusive roles
    | PICK = 3

    /// The metadata value is any option but this from a selection of options, E.g. "Any of these mutually exlusive roles EXCEPT x"
    | PICK_NOT = 4

    // NOTE: DateTime metadata types are not relevant for this project and therefore are not implemented.

    /// The metadata value is equal to this value, represented with 1 or 0, E.g. "User is moderator", "User is whitelisted"
    | IS = 7

    /// The metadata value is not equal to this value, represented with 1 or 0, E.g. "User is not blacklisted"
    | IS_NOT = 8
