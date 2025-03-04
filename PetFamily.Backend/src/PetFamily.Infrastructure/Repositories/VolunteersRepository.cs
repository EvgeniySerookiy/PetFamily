using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.VolunteerVO;
using PetFamily.Domain.Shared.ErrorContext;


namespace PetFamily.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly ApplicationDbContex _applicationDbContex;

    public VolunteersRepository(ApplicationDbContex applicationDbContex)
    {
        _applicationDbContex = applicationDbContex;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _applicationDbContex.Volunteers.AddAsync(volunteer, cancellationToken);
        await _applicationDbContex.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId)
    {
        var volunteer = await _applicationDbContex.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        if (volunteer is null)
            return Errors.General.NotFound(volunteerId.Value);

        return volunteer;
    }
    
    public async Task<Result<Volunteer, Error>> GetByEmail(Email email)
    {
        var volunteer = await _applicationDbContex.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Email == email);

        if (volunteer is null)
            return Errors.General.NotFound();

        return volunteer;
    }

}