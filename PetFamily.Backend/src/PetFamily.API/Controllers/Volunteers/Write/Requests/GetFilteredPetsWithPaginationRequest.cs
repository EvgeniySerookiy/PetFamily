using PetFamily.Application.PetManagement.Queries.Volunteers.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Write.Requests;

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