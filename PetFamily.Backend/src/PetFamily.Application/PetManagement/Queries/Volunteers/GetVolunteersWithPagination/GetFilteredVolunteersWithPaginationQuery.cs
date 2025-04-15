using PetFamily.Application.Abstractions;

namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetVolunteersWithPagination;

public record GetFilteredVolunteersWithPaginationQuery(
    int Page,
    int PageSize) : IQuery;