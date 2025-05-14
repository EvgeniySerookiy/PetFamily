using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetPet;

public record GetPetQuery(
    Guid VolunteerId,
    Guid PetId) : IQuery;