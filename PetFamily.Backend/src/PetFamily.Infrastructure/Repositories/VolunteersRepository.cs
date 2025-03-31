
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Database;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;


namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IUnitOfWork _unitOfWork;

    public VolunteersRepository(
        ApplicationDbContext applicationDbContext,
        IUnitOfWork unitOfWork)
    {
        _applicationDbContext = applicationDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Add(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        await _applicationDbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return volunteer.Id.Value;
    }
    
    public Guid Save(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        _applicationDbContext.Volunteers.Attach(volunteer);
        
        return volunteer.Id.Value;
    }

    public Guid Delete(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        _applicationDbContext.Volunteers.Remove(volunteer);
        
        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId, 
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _applicationDbContext.Volunteers
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
        var volunteer = await _applicationDbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Email == email, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound();

        return volunteer;
    }

}
