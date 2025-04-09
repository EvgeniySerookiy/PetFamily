using PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Pets.Requests;

public record GetFilteredPetsWithPaginationRequest(
    string? PetName,
    int? PositionFrom,
    int? PositionTo,
    int Page,
    int PageSize)
{
    public GetFilteredPetsWithPaginationQuery ToQuery() =>
        new(PetName, PositionFrom, PositionTo, Page, PageSize);
}