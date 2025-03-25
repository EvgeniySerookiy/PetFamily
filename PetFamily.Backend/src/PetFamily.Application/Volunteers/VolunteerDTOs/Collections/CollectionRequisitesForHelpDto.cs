namespace PetFamily.Application.Volunteers.VolunteerDTOs.Collections;

public record CollectionRequisitesForHelpDto(
    IEnumerable<RequisitesForHelpDto> RequisitesForHelps);