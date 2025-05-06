using System.Data;
using PetFamily.Application.Database;

namespace PetFamily.Infrastructure.Repositories.Read;

public class SpeciesReadRepository : ISpeciesReadRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    
    public SpeciesReadRepository(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
}