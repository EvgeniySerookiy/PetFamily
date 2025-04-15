using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.Species.GetSpeciesWithPagination;

public record GetSpeciesWithPaginationQuery(
    int Page,
    int PageSize) : IQuery;