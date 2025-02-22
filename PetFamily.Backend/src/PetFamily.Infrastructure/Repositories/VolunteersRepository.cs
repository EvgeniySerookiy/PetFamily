using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Volunteers;
using PetFamily.Domain.VolunteerContext;
using PetFamily.Domain.VolunteerContext.VolunteerVO;

namespace PetFamily.Infrastructure.Repositories;

// Этот слой работает с БД, он 
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

    public async Task<Result<Volunteer>> GetById(VolunteerId volunteerId)
    {
        var volunteer = await _applicationDbContex.Volunteers
            .Include(v => v.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId);

        if (volunteer is null)
            return Result.Failure<Volunteer> ("Volunteer not found");

        return volunteer;
    }

}