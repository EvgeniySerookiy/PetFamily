using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Application.Database;

namespace PetFamily.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContex _dbContext;
    
    public UnitOfWork(ApplicationDbContex dbContext)
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
}