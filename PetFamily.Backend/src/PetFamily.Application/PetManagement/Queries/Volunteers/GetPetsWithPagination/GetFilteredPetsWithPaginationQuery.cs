using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
    string? PetName,
    int? PositionFrom,
    int? PositionTo,
    int Page,
    int PageSize) : IQuery;