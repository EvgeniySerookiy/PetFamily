using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.PetManagement.Queries.Species.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationHandler : 
    IQueryHandlerSpecies<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetSpeciesWithPaginationHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    public Task<PagedList<SpeciesDto>> Handle(
        GetSpeciesWithPaginationQuery query, 
        CancellationToken cancellationToken = default)
    {
        var speciesQuery = _readDbContext.Species;
        
        return speciesQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}