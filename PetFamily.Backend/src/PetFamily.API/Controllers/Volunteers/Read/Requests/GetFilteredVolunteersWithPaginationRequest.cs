using PetFamily.Application.PetManagement.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Read.Requests;

public record GetFilteredVolunteersWithPaginationRequest(
    int Page,
    int PageSize)
{
    public GetFilteredVolunteersWithPaginationQuery ToQuery() =>
        new (Page, PageSize);
}