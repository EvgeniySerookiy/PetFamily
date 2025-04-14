using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetVolunteer;

public record GetVolunteerQuery(
    Guid Id) : IQuery;