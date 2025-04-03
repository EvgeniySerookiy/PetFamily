using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

public class GetPetsWithPaginationHandler
{
    private readonly IReadDbContext _context;

    public GetPetsWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var petsQuery = _context.Pets.AsQueryable();
        
        return await petsQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}