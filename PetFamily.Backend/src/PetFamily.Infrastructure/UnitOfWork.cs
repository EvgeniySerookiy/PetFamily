using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Application.Database;

namespace PetFamily.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
 
        return transaction.GetDbTransaction();
    }
 
    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    // public async Task ShowListPets()
    // {
    //     var petsId = await _dbContext.Pets.Select(p => p.Id).ToListAsync();
    //
    //     foreach (var petId in petsId)
    //     {
    //         Console.WriteLine(petId);
    //     }
    // }
}