using PetFamily.Application.Volunteers.DTOs.Collections;

namespace PetFamily.Application.Volunteers.Actions.Update.UpdateRequisitesForHelp;

public record UpdateCollectionRequisitesForHelpRequest(
    Guid VolunteerId,
    CollectionRequisitesForHelpDto CollectionRequisitesForHelp);