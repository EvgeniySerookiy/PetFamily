using PetFamily.Application.Volunteers.DTOs;

namespace PetFamily.Application.Volunteers.Requests;

public record UpdateMainInfoRequest(
    Guid VolunteerId,
    MainInfoDto MainInfo);