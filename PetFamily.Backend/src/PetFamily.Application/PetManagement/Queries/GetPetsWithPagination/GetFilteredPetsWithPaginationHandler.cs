using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.PetManagement.Queries.GetPetsWithPagination;

public class GetFilteredPetsWithPaginationHandler : IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly IReadDbContext _context;

    public GetFilteredPetsWithPaginationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<PetDto>> Handle(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var petsQuery = _context.Pets;

        petsQuery = petsQuery.WhereIf(!string.IsNullOrWhiteSpace(query.PetName),
            p => p.PetName.Contains(query.PetName!));
        
        petsQuery = petsQuery.WhereIf(query.PositionTo != null,
            p => p.Position <= query.PositionTo!.Value);
        
        petsQuery = petsQuery.WhereIf(query.PositionFrom != null,
            p => p.Position >= query.PositionFrom!.Value);
        
        return await petsQuery
            .OrderBy(p => p.Position)
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}