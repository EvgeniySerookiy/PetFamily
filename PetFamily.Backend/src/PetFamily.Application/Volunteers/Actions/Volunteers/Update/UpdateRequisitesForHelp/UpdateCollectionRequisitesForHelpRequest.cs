using PetFamily.Application.Volunteers.VolunteerDTOs.Collections;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateRequisitesForHelp;

public record UpdateCollectionRequisitesForHelpRequest(
    Guid VolunteerId,
    CollectionRequisitesForHelpDto CollectionRequisitesForHelp);