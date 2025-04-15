using PetFamily.Application.PetManagement.Queries.Species.GetSpeciesWithPagination;

namespace PetFamily.API.Controllers.Species.Read.Request;

public record GetSpeciesWithPaginationRequest(
    int Page,
    int PageSize)
{
    public GetSpeciesWithPaginationQuery ToQuery() =>
        new GetSpeciesWithPaginationQuery(Page, PageSize);
}