
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Infrastructure.DbContexts;


namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly WriteDbContext _DbContext;
    private readonly IUnitOfWork _unitOfWork;

    public VolunteersRepository(
        WriteDbContext dbContext,
        IUnitOfWork unitOfWork)
    {
        _DbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Add(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        await _DbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return volunteer.Id.Value;
    }
    
    public Guid Save(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        _DbContext.Volunteers.Attach(volunteer);
        
        return volunteer.Id.Value;
    }

    public Guid Delete(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        _DbContext.Volunteers.Remove(volunteer);
        
        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId, 
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _DbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound(volunteerId.Value);

        return volunteer;
    }
    
    public async Task<Result<Volunteer, Error>> GetByEmail(
        Email email, 
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _DbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Email == email, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound();

        return volunteer;
    }

}
