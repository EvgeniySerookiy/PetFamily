namespace PetFamily.Application.Volunteers.DTOs.Collections;

public record CollectionRequisitesForHelpDto(
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps);