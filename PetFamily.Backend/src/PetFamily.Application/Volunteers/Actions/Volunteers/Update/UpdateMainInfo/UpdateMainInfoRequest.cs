using PetFamily.Application.Volunteers.VolunteerDTOs;

namespace PetFamily.Application.Volunteers.Actions.Volunteers.Update.UpdateMainInfo;

public record UpdateMainInfoRequest(
    Guid VolunteerId,
    MainInfoDto MainInfo);