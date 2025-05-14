using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.Infrastructure.Repositories.Write;

public class VolunteersWriteRepository : IVolunteersWriteRepository
{
    private readonly WriteDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<VolunteersWriteRepository> _logger;

    public VolunteersWriteRepository(
        WriteDbContext dbContext,
        IUnitOfWork unitOfWork,
        ILogger<VolunteersWriteRepository> logger)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Guid> Add(
        Volunteer volunteer,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer with id: {VolunteerId} was successfully added", volunteer.Id.Value);

        return volunteer.Id.Value;
    }

    public async Task<Guid> Delete(
        Volunteer volunteer, 
        CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Remove(volunteer);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer with id: {VolunteerId} it was successfully deleted", volunteer.Id.Value);
        
        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId, 
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer == null)
        {
            _logger.LogWarning("Attempted to delete non-existent volunteer with id: {VolunteerId}", volunteerId);
            return Errors.Volunteer.NotFound(volunteerId.Value);
        }
            
        _logger.LogInformation("Successfully retrieved volunteer with id: {VolunteerId}", volunteerId);

        return volunteer;
    }
    
    // public async Task<Result<Volunteer, Error>> GetByEmail(
    //     Email email, 
    //     CancellationToken cancellationToken = default)
    // {
    //     var volunteer = await _dbContext.Volunteers
    //         .FirstOrDefaultAsync(v => v.Email == email, cancellationToken);
    //
    //     if (volunteer != null)
    //     {
    //         _logger.LogWarning("Volunteer with email: {Email} already exists", volunteer.Email.Value);
    //         return Errors.Volunteer.AlreadyExist();
    //     }
    //     
    //     return volunteer;
    // }
}
