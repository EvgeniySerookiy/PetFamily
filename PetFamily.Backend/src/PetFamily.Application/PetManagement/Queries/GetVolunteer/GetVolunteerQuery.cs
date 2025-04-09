using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.GetVolunteer;

public record GetVolunteerQuery(
    Guid Id) : IQuery;