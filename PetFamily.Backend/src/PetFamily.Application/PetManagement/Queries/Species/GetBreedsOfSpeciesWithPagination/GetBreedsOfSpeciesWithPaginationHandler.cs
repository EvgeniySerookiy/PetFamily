using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.PetManagement.Queries.Species.GetBreedsOfSpeciesWithPagination;

public class GetBreedsOfSpeciesWithPaginationHandler : IQueryHandlerBreedsOfSpecies<PagedList<BreedDto>, GetBreedsOfSpeciesWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetBreedsOfSpeciesWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<PagedList<BreedDto>> Handle(
        GetBreedsOfSpeciesWithPaginationQuery query, 
        CancellationToken cancellationToken = default)
    {
        var breedsQuery = _readDbContext.Species
            .Where(s => s.Id == query.SpeciesId)
            .SelectMany(s => s.Breeds)
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
        
        return await breedsQuery;
    }
}