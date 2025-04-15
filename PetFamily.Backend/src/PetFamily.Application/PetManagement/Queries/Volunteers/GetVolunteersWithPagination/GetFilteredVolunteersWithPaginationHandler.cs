using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.PetManagement.Queries.Volunteers.GetVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationHandler : IQueryHandlerVolunteers<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetFilteredVolunteersWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _context.Volunteers;

        return await volunteersQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}