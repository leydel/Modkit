namespace Modkit.Roles.Infrastructure.Mappers

open Modkit.Roles.Domain.Types

module MetadataTypeMapper =
    let fromDomain (metadataType: MetadataType): string =
        match metadataType with
        | MetadataType.RANGE_MAX -> "RANGE_MAX"
        | MetadataType.RANGE_MIN -> "RANGE_MIN"
        | MetadataType.PICK -> "PICK"
        | MetadataType.PICK_NOT -> "PICK_NOT"
        | MetadataType.IS -> "IS"
        | MetadataType.IS_NOT -> "IS_NOT"
        | _ -> failwith "Unexpected MetadataType provided"

    let toDomain (str: string): MetadataType =
        match str with
        | "RANGE_MAX" -> MetadataType.RANGE_MAX
        | "RANGE_MIN" -> MetadataType.RANGE_MIN
        | "PICK" -> MetadataType.PICK
        | "PICK_NOT" -> MetadataType.PICK_NOT
        | "IS" -> MetadataType.IS
        | "IS_NOT" -> MetadataType.IS_NOT
        | _ -> failwith "Unexpected MetadataType provided"
