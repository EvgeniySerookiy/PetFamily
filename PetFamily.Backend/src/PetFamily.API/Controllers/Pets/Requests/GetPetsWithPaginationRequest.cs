using PetFamily.Application.PetManagement.Queries;

namespace PetFamily.API.Controllers.Pets.Requests;

public record GetPetsWithPaginationRequest(
    int Page,
    int PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() =>
        new(Page, PageSize);
}