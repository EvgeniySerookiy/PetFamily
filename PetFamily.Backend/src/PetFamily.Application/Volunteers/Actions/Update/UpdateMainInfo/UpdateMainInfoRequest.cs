using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Application.Volunteers.Actions.Update.UpdateMainInfo;

public record UpdateMainInfoRequest(
    Guid VolunteerId,
    MainInfoDto MainInfo);