using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.Species.GetBreedsOfSpeciesWithPagination;

public record GetBreedsOfSpeciesWithPaginationQuery(
    Guid SpeciesId,
    int Page,
    int PageSize) : IQuery;