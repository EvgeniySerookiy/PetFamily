using PetFamily.Application.Volunteers.DTOs.Collections;

namespace PetFamily.Application.Volunteers.Requests;

public record UpdateCollectionRequisitesForHelpRequest(
    Guid VolunteerId,
    CollectionRequisitesForHelpDto CollectionRequisitesForHelp);