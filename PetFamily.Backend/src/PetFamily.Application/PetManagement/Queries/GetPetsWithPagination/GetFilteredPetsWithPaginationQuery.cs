using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

public record GetFilteredPetsWithPaginationQuery(
    string? PetName,
    int? PositionFrom,
    int? PositionTo,
    int Page,
    int PageSize) : IQuery;