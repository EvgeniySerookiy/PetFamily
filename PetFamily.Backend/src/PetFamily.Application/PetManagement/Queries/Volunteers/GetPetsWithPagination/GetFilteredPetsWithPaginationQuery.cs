using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
    SortBy SortBy,
    SortDirection SortDirection,
    Guid? VolunteerId,
    string? PetName,
    int? MinAge,
    int? MaxAge,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Color,
    string? Region,
    string? City,
    int? PositionFrom,
    int? PositionTo,
    int Page,
    int PageSize) : IQuery;